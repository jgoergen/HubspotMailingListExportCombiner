using CsvHelper;
using HubspotMailingListExportCombiner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Drawing;
using Console = Colorful.Console;
using CsvHelper.Configuration;

namespace HubspotMailingListExportCombiner
{
    class Program
    {
        private static readonly bool DEBUG = false;
        private static readonly List<string> KEEP_EVENT_TYPES = new List<string>() { EventType.DELIVERED.Value, EventType.OPEN.Value, EventType.CLICK.Value, EventType.BOUNCE.Value, EventType.UNSUBSCRIBE.Value };
        private static List<List<HubspotMailingListExportEntry>> lists = new List<List<HubspotMailingListExportEntry>>();
        private static List<CombinedDataEntry> combinedList = new List<CombinedDataEntry>();

        static void Main(string[] args)
        {
            var files = new List<string>();
            var outputFileName = $"output-{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}.csv";

            Console.WriteLine("Starting Hubspot Mailinglist combiner.", Color.Green);

            if (File.Exists(outputFileName))
            {
                Console.WriteLine($"An {outputFileName} file already exists, please rename, move or delete and try again.", Color.Red);
                Console.ReadLine();
                return;
            }

            if (args.Length == 0)
            {
                if (DEBUG == false)
                {
                    Console.WriteLine("No files provided, please drag some onto the icon!", Color.Red);
                    Console.ReadLine();
                    return;
                }
                else
                {
                    files.Add("test1.csv");
                    files.Add("test2.csv");
                }
            }
            else
            {
                for (var i = 0; i < args.Length; i++)
                {
                    files.Add(args[i]);
                }
            }

            Console.WriteLine("Reading data from files", Color.Yellow);
            lists = new List<List<HubspotMailingListExportEntry>>();
            var index = 0;

            foreach (var path in files)
            {
                Console.WriteLine($"    Getting Data from {index}:{path}", Color.Yellow);
                var listData = GetCSVData(path);
                Console.WriteLine($"    Entries found: {listData.Count}", Color.Yellow);
                lists.Add(listData);
                index++;
            }

            Console.WriteLine("Combining data", Color.Yellow);
            combineLists();

            Console.WriteLine($"Generating {outputFileName} file with {combinedList.Count} entries.", Color.Yellow);
            using (var writer = new StreamWriter(outputFileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(combinedList);
            }

            Console.WriteLine("Done, press enter to close", Color.Green);
            Console.ReadLine();
        }

        static CombinedDataEntry BuildCombinedDataEntry(HubspotMailingListExportEntry entry)
        {
            var eventCreatedDateParts = entry.EventCreatedDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var urlType =
                string.IsNullOrWhiteSpace(entry.ClickURL) ?
                    "" :
                    GetUrlType(entry.ClickURL);

            return new CombinedDataEntry()
            {
                BounceMessage = entry.BounceMessage,
                BounceReason = entry.BounceReason,
                BounceStatus = entry.BounceStatus,
                UrlType = urlType,
                ClickURL = entry.ClickURL,
                Company = GetCompanyName(entry.Recipient),
                EventType = entry.EventType,
                OpenDurationSeconds = CleanOpenDurationTime(entry.OpenDuration),
                Recipient = entry.Recipient,
                EventCreatedDate = eventCreatedDateParts.Length > 0 ? eventCreatedDateParts[0] : "",
                EventCreatedTime = eventCreatedDateParts.Length > 1 ? eventCreatedDateParts[1] : ""
            };
        }

        static string GetCompanyName(string recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient) || !recipient.Contains("@"))
            {
                return "";
            }

            var domainParts = recipient.Split("@")[1].Split(".");
            return domainParts.Length > 1 ? String.Join(".", domainParts.Take(domainParts.Length - 1)) : domainParts[0];
        }

        static string GetUrlType(string clickUrl)
        {
            var urlParts = clickUrl.ToLowerInvariant().Replace("http://", "").Replace("https://", "").Split("/");
            return urlParts.Count() < 2 ?
                    "" :
                    urlParts[1].Contains("?") ?
                        "" :
                        urlParts[1];
        }

        static string CleanOpenDurationTime(string time)
        {
            if (string.IsNullOrWhiteSpace(time) || time == "null" || time == "0")
            {
                return "";
            }
            else
            {
                if (int.TryParse(time, out var timeInMS))
                {
                    return (timeInMS / 1000).ToString();
                }

                return "";
            }
        }

        static List<HubspotMailingListExportEntry> GetCSVData(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
            };

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<HubspotMailingListExportEntry>().ToList();
            }
        }
    
        static void combineLists()
        {
            combinedList = new List<CombinedDataEntry>();
            var listIndex = 0;
            foreach (var listData in lists)
            {
                Console.WriteLine($"    Processing list {listIndex}", Color.Yellow);

                foreach (var entry in listData)
                {
                    if (KEEP_EVENT_TYPES.Contains(entry.EventType))
                    {
                        // if this entries recipient already exists then...
                        var recipientExists = combinedList.Where(e => e.Recipient == entry.Recipient).FirstOrDefault();
                        if (recipientExists != null)
                        {
                            if (entry.EventType == EventType.BOUNCE.Value)
                            {
                                Console.Write($".", Color.Yellow);
                                combinedList.Add(BuildCombinedDataEntry(entry));
                            }
                            else if (entry.EventType == EventType.UNSUBSCRIBE.Value)
                            {
                                Console.Write($".", Color.Yellow);
                                combinedList.Add(BuildCombinedDataEntry(entry));
                            }
                            else if (entry.EventType == EventType.CLICK.Value)
                            {
                                Console.Write($"+", Color.Blue);
                                combinedList.RemoveAll(e => e.Recipient == entry.Recipient && e.EventType == EventType.OPEN.Value);
                                combinedList.RemoveAll(e => e.Recipient == entry.Recipient && e.EventType == EventType.DELIVERED.Value);
                                combinedList.Add(BuildCombinedDataEntry(entry));
                            }
                            else if (entry.EventType == EventType.OPEN.Value)
                            {
                                var higherExists = combinedList.Where(e => e.Recipient == entry.Recipient && e.EventType == EventType.CLICK.Value).FirstOrDefault();
                                if (higherExists == null)
                                {
                                    Console.Write($"+", Color.Blue);
                                    combinedList.RemoveAll(e => e.Recipient == entry.Recipient && (e.EventType == EventType.DELIVERED.Value));
                                    combinedList.Add(BuildCombinedDataEntry(entry));
                                }
                                else
                                {
                                    Console.Write("x", Color.Yellow);
                                }
                            }
                            else if (entry.EventType == EventType.DELIVERED.Value)
                            {
                                var higherExists = combinedList.Where(e => e.Recipient == entry.Recipient && (e.EventType == EventType.CLICK.Value || e.EventType == EventType.OPEN.Value)).FirstOrDefault();
                                if (higherExists == null)
                                {
                                    Console.Write($"+", Color.Blue);
                                    combinedList.Add(BuildCombinedDataEntry(entry));
                                }
                                else
                                {
                                    Console.Write("x", Color.Yellow);
                                }
                            }
                        }
                        else
                        {
                            Console.Write($".", Color.Yellow);
                            combinedList.Add(BuildCombinedDataEntry(entry));
                        }
                    }
                    else
                    {
                        Console.Write($"X", Color.Yellow);
                    }
                }
                Console.WriteLine($" Done.", Color.Yellow);
                listIndex++;
            }
        }
    }
}
