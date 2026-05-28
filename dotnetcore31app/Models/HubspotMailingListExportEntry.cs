using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubspotMailingListExportCombiner.Models
{
    class HubspotMailingListExportEntry
    {
        [Name("Recipient")]
        public string Recipient { get; set; } = string.Empty;
        [Name("Hub ID")]
        public string HubID { get; set; } = string.Empty;
        [Name("Email Campaign ID")]
        public string EmailCampaignID { get; set; } = string.Empty;
        [Name("Subject")]
        public string Subject { get; set; } = string.Empty;
        [Name("From")]
        public string From { get; set; } = string.Empty;
        [Name("Reply-To")]
        public string ReplyTo { get; set; } = string.Empty;
        [Name("CC")]
        public string CC { get; set; } = string.Empty;
        [Name("BCC")]
        public string BCC { get; set; } = string.Empty;
        [Name("Sent Date (Your time zone)")]
        public string SentDate { get; set; } = string.Empty;
        [Name("Sent By Event ID")]
        public string SentByEventID { get; set; } = string.Empty;
        [Name("Event Type")]
        public string EventType { get; set; } = string.Empty;
        [Name("Event ID")]
        public string EventID { get; set; } = string.Empty;
        [Name("Event Created Date (Your time zone)")]
        public string EventCreatedDate { get; set; } = string.Empty;
        [Name("Message ID")]
        public string MessageID { get; set; } = string.Empty;
        [Name("Obsoleted by Event ID")]
        public string ObsoletedByEventID { get; set; } = string.Empty;
        [Name("Obsoleted by Date (Your time zone)")]
        public string ObsoletedByDate { get; set; } = string.Empty;
        [Name("Caused by Event ID")]
        public string CausedByEventID { get; set; } = string.Empty;
        [Name("Caused by Date (Your time zone)")]
        public string CausedByDate { get; set; } = string.Empty;
        [Name("Not Sent Reason")]
        public string NotSentReason { get; set; } = string.Empty;
        [Name("Not Sent Message")]
        public string NotSentMessage { get; set; } = string.Empty;
        [Name("App ID")]
        public string AppID { get; set; } = string.Empty;
        [Name("App Name")]
        public string AppName { get; set; } = string.Empty;
        [Name("User Agent")]
        public string UserAgent { get; set; } = string.Empty;
        [Name("City")]
        public string City { get; set; } = string.Empty;
        [Name("State")]
        public string State { get; set; } = string.Empty;
        [Name("Country")]
        public string Country { get; set; } = string.Empty;
        [Name("CTA ID")]
        public string CTAID { get; set; } = string.Empty;
        [Name("CTA Name")]
        public string CTAName { get; set; } = string.Empty;
        [Name("Click URL")]
        public string ClickURL { get; set; } = string.Empty;
        [Name("Open duration (ms)")]
        public string OpenDuration { get; set; } = string.Empty;
        [Name("Bounce status")]
        public string BounceStatus { get; set; } = string.Empty;
        [Name("Bounce reason")]
        public string BounceReason { get; set; } = string.Empty;
        [Name("Bounce message")]
        public string BounceMessage { get; set; } = string.Empty;
        [Name("Subscription Change Requested By")]
        public string SubscriptionChangeRequestedBy { get; set; } = string.Empty;
        [Name("Subscription Change Source")]
        public string SubscriptionChangeSource { get; set; } = string.Empty;
        [Name("Portal Subscription Status")]
        public string PortalSubscriptionStatus { get; set; } = string.Empty;
    }
}


