using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubspotMailingListExportCombiner.Models
{
    class CombinedDataEntry
    {
        [Name("Company")]
        public string Company { get; set; } = string.Empty;
        [Name("Recipient")]
        public string Recipient { get; set; } = string.Empty;
        [Name("Event Created Date")]
        public string EventCreatedDate { get; set; } = string.Empty;
        [Name("Event Created Time")]
        public string EventCreatedTime { get; set; } = string.Empty;
        [Name("Event Type")]
        public string EventType { get; set; } = string.Empty;
        [Name("Open duration (seconds)")]
        public string OpenDurationSeconds { get; set; } = string.Empty;
        [Name("Url Type")]
        public string UrlType { get; set; } = string.Empty;
        [Name("Click URL")]
        public string ClickURL { get; set; } = string.Empty;
        [Name("Bounce status")]
        public string BounceStatus { get; set; } = string.Empty;
        [Name("Bounce reason")]
        public string BounceReason { get; set; } = string.Empty;
        [Name("Bounce message")]
        public string BounceMessage { get; set; } = string.Empty;
    }
}


