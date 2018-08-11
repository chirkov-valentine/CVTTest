using CVTTest.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CVTTest.Domain.Calendar;
using RepositoryContext = CVTTest.DAL.RepositoryContext;

namespace MVCEventCalendar.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents(bool meeting, bool business, bool jotting)
        {
            using (RepositoryContext dc = new RepositoryContext())
            {
                var meetings = from e in dc.Events where e.EventTypeId == 1 select e;
                var businesses = from e in dc.Events where e.EventTypeId == 2 select e;
                var jottings = from e in dc.Events where e.EventTypeId == 3 select e;
                var events = new List<Event>();
                if (meeting)
                    events.AddRange(meetings);
                if (business)
                    events.AddRange(businesses);
                if (jotting)
                    events.AddRange(jottings);
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (RepositoryContext dc = new RepositoryContext())
            {
                if (e.EventId > 0)
                {
                    //Обновление события
                    var v = dc.Events.FirstOrDefault(a => a.EventId == e.EventId);
                    if (v != null)
                    {
                        v.Title = e.Title;
                        v.StartDate = e.StartDate;
                        v.EndDate = e.EndDate;
                        v.MeetingPoint = e.MeetingPoint;
                        v.IsFinished = e.IsFinished;
                        v.EventTypeId = e.EventTypeId;
                    }
                }
                else
                {
                    dc.Events.Add(new Event
                    {
                        EventTypeId = e.EventTypeId,
                        IsFinished = e.IsFinished,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        MeetingPoint = e.MeetingPoint,
                        Title = e.Title,
                    });
                }
                
                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (RepositoryContext dc = new RepositoryContext())
            {
                var v = dc.Events.FirstOrDefault(a => a.EventId == eventID);
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status} };
        }
    }
}