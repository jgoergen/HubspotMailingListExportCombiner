using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubspotMailingListExportCombiner.Models
{
    class CombinedDataEntry
    {
        [Name("Company")]
        public string Company { get; set; }
        [Name("Recipient")]
        public string Recipient { get; set; }
        [Name("Event Created Date")]
        public string EventCreatedDate { get; set; }
        [Name("Event Created Time")]
        public string EventCreatedTime { get; set; }
        [Name("Event Type")]
        public string EventType { get; set; }
        [Name("Open duration (seconds)")]
        public string OpenDurationSeconds { get; set; }
        [Name("Url Type")]
        public string UrlType { get; set; }
        [Name("Click URL")]
        public string ClickURL { get; set; }
        [Name("Bounce status")]
        public string BounceStatus { get; set; }
        [Name("Bounce reason")]
        public string BounceReason { get; set; }
        [Name("Bounce message")]
        public string BounceMessage { get; set; }
    }
}


