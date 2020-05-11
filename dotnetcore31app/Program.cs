using CsvHelper;
using HubspotMailingListExportCombiner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Drawing;
using Console = Colorful.Console;

namespace HubspotMailingListExportCombiner
{
    class Program
    {
        private static bool DEBUG = false;
        private static List<string> KEEP_EVENT_TYPES = new List<string>() { EventType.DELIVERED.Value, EventType.OPEN.Value, EventType.CLICK.Value, EventType.BOUNCE.Value, EventType.UNSUBSCRIBE.Value };
        private static List<List<HubspotMailingListExportEntry>> lists;
        private static List<CombinedDataEntry> combinedList;

        static void Main(string[] args)
        {
            var files = new List<string>();

            Console.WriteLine("Starting Hubspot Mailinglist combiner.", Color.Green);

            if (File.Exists("output.csv"))
            {
                Console.WriteLine("An output.csv file already exists, please rename, move or delete and try again.", Color.Red);
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

            Console.WriteLine($"Generating output.csv file with {combinedList.Count} entries.", Color.Yellow);
            using (var writer = new StreamWriter("output.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(combinedList);
            }

            Console.WriteLine("Done, press enter to close", Color.Green);
            Console.ReadLine();
        }

        static CombinedDataEntry BuildCombinedDataEntry(HubspotMailingListExportEntry entry)
        {
            var domainParts = entry.Recipient.Split("@")[1].Split(".");
            var urlParts = entry.ClickURL.ToLower().Replace("http://", "").Replace("https://", "").Split("/");
            var urlType =
                urlParts.Count() < 2 ?
                    "" :
                    urlParts[1].Contains("?") ?
                        "" :
                        urlParts[1];
            return
                new CombinedDataEntry()
                {
                    BounceMessage = entry.BounceMessage,
                    BounceReason = entry.BounceReason,
                    BounceStatus = entry.BounceStatus,
                    UrlType = urlType,
                    ClickURL = entry.ClickURL,
                    Company = String.Join(".", domainParts.Take(domainParts.Length - 1)),
                    EventType = entry.EventType,
                    OpenDurationSeconds = CleanOpenDurationTime(entry.OpenDuration),
                    Recipient = entry.Recipient,
                    EventCreatedDate = entry.EventCreatedDate.Split(" ")[0],
                    EventCreatedTime = entry.EventCreatedDate.Split(" ")[1]
                };
        }

        static string CleanOpenDurationTime(string time)
        {
            if (string.IsNullOrWhiteSpace(time) || time == "null" || time == "0")
            {
                return "";
            }
            else
            {
                try
                {
                    var timeInMS = int.Parse(time);
                    return (timeInMS / 1000).ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        static List<HubspotMailingListExportEntry> GetCSVData(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
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
