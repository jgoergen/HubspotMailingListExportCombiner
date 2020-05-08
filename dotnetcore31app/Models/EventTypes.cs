using System;
using System.Collections.Generic;
using System.Text;

namespace HubspotMailingListExportCombiner.Models
{
    class EventType
    {
        private EventType(string value) { Value = value; }

        public string Value { get; set; }

        public static EventType UNSUBSCRIBE { get { return new EventType("UNSUBSCRIBE"); } }
        public static EventType BOUNCE { get { return new EventType("BOUNCE"); } }
        public static EventType CLICK { get { return new EventType("CLICK"); } }
        public static EventType OPEN { get { return new EventType("OPEN"); } }
        public static EventType DELIVERED { get { return new EventType("DELIVERED"); } }
    }
}