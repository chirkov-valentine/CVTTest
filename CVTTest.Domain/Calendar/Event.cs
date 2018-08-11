using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVTTest.Domain.Calendar
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string MeetingPoint { get; set; }
        public bool IsFinished { get; set; }
        public int EventTypeId { get; set; }
        //public virtual EventType EventType { get; set; }
    }
}
