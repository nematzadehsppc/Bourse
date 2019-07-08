using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Back.DAL.Models
{
    [DataContract]
    public class EventLog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }
        
        [DataMember]
        public EventLogType EventId { get; set; }

        [DataMember]
        public string EDesc { get; set; }
        
        [DataMember]
        public object UserName { get; set; }
        
        //فیلدهای زیر در جیسون نادیده گرفته می شوند. 
        public DateTime EventDate { get; set; }

        public DateTime EventTime { get; set; }
    }
}
