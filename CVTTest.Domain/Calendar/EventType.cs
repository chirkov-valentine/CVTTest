using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVTTest.Domain.Calendar
{
    public class EventType 
    {
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
