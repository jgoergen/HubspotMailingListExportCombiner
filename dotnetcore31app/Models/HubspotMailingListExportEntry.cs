using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubspotMailingListExportCombiner.Models
{
    class HubspotMailingListExportEntry
    {
        [Name("Recipient")]
        public string Recipient { get; set; }
        [Name("Hub ID")]
        public string HubID { get; set; }
        [Name("Email Campaign ID")]
        public string EmailCampaignID { get; set; }
        [Name("Subject")]
        public string Subject { get; set; }
        [Name("From")]
        public string From { get; set; }
        [Name("Reply-To")]
        public string ReplyTo { get; set; }
        [Name("CC")]
        public string CC { get; set; }
        [Name("BCC")]
        public string BCC { get; set; }
        [Name("Sent Date (Your time zone)")]
        public string SentDate { get; set; }
        [Name("Sent By Event ID")]
        public string SentByEventID { get; set; }
        [Name("Event Type")]
        public string EventType { get; set; }
        [Name("Event ID")]
        public string EventID { get; set; }
        [Name("Event Created Date (Your time zone)")]
        public string EventCreatedDate { get; set; }
        [Name("Message ID")]
        public string MessageID { get; set; }
        [Name("Obsoleted by Event ID")]
        public string ObsoletedByEventID { get; set; }
        [Name("Obsoleted by Date (Your time zone)")]
        public string ObsoletedByDate { get; set; }
        [Name("Caused by Event ID")]
        public string CausedByEventID { get; set; }
        [Name("Caused by Date (Your time zone)")]
        public string CausedByDate { get; set; }
        [Name("Not Sent Reason")]
        public string NotSentReason { get; set; }
        [Name("Not Sent Message")]
        public string NotSentMessage { get; set; }
        [Name("App ID")]
        public string AppID{ get; set; }
        [Name("App Name")]
        public string AppName { get; set; }
        [Name("User Agent")]
        public string UserAgent { get; set; }
        [Name("City")]
        public string City { get; set; }
        [Name("State")]
        public string State { get; set; }
        [Name("Country")]
        public string Country { get; set; }
        [Name("CTA ID")]
        public string CTAID { get; set; }
        [Name("CTA Name")]
        public string CTAName { get; set; }
        [Name("Click URL")]
        public string ClickURL { get; set; }
        [Name("Open duration (ms)")]
        public string OpenDuration { get; set; }
        [Name("Bounce status")]
        public string BounceStatus { get; set; }
        [Name("Bounce reason")]
        public string BounceReason { get; set; }
        [Name("Bounce message")]
        public string BounceMessage { get; set; }
        [Name("Subscription Change Requested By")]
        public string SubscriptionChangeRequestedBy { get; set; }
        [Name("Subscription Change Source")]
        public string SubscriptionChangeSource { get; set; }
        [Name("Portal Subscription Status")]
        public string PortalSubscriptionStatus { get; set; }
    }
}


