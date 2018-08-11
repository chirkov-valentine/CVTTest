using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CVTTest.DAL;
using CVTTest.Domain.ContactList;

namespace MVCEventCalendar.Controllers
{
    public class PeopleController : Controller
    {
        private RepositoryContext db = new RepositoryContext();

        // GET: People
        public ActionResult Index(string searchString)
        {
            var contacts = from c in db.Persons select c;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchString)
                                               || c.LastName.Contains(searchString)
                                               || c.MiddleName.Contains(searchString)
                                               || c.Organization.Contains(searchString)
                                               || c.Position.Contains(searchString));
                DateTime birthDate;
                if (DateTime.TryParse(searchString, out birthDate))
                    contacts = contacts.Where(c => c.DateOfBirth == birthDate);
                var info = from ci in db.ContactInfos select ci;
                info = info.Where(i => i.AdditinalInfo.Contains(searchString)
                                       || i.Email.Contains(searchString)
                                       || i.PhoneNumber.Contains(searchString)
                                       || i.Skype.Contains(searchString));
                var contactsInfo = from i in info
                    join allcontacts in db.Persons on i.PersonId equals allcontacts.PersonId
                    select allcontacts;
                contacts = contacts.Union(contactsInfo).Distinct();

            }
            return View(contacts.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,LastName,FirstName,MiddleName,Organization,Position,DateOfBirth")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,LastName,FirstName,MiddleName,Organization,Position,DateOfBirth")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
