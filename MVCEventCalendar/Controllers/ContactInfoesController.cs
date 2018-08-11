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
    public class ContactInfoesController : Controller
    {
        private RepositoryContext db = new RepositoryContext();

        // GET: ContactInfoes
        public ActionResult Index(int id)
        {
            var contactInfos = db.ContactInfos.Where(ci => ci.PersonId == id);
            ViewBag.PersonId = id;
            var person = db.Persons.FirstOrDefault(p => p.PersonId == id);
            ViewBag.FullName = $"{person?.LastName} {person?.FirstName} {person?.MiddleName}";
            return View(contactInfos.ToList());
        }

        // GET: ContactInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfos.Find(id);

            if (contactInfo == null)
            {
                return HttpNotFound();
            }

            var person = db.Persons.FirstOrDefault(p => p.PersonId == contactInfo.PersonId);
            ViewBag.FullName = $"{person?.LastName} {person?.FirstName} {person?.MiddleName}";
            return View(contactInfo);
        }

        // GET: ContactInfoes/Create
        public ActionResult Create(int personId)
        {
            var contactInfo = new ContactInfo {PersonId = personId};
            var person = db.Persons.FirstOrDefault(p => p.PersonId == personId);
            ViewBag.FullName = $"{person?.LastName} {person?.FirstName} {person?.MiddleName}";
            return View(contactInfo);
        }

        // POST: ContactInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactInfoId,PhoneNumber,Email,Skype,AdditinalInfo,PersonId")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                db.ContactInfos.Add(contactInfo);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = contactInfo.PersonId});
            }

            ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "LastName", contactInfo.PersonId);
            return View(contactInfo);
        }

        // GET: ContactInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfos.Find(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            var person = db.Persons.FirstOrDefault(p => p.PersonId == contactInfo.PersonId);
            ViewBag.FullName = $"{person?.LastName} {person?.FirstName} {person?.MiddleName}";
            return View(contactInfo);
        }

        // POST: ContactInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactInfoId,PhoneNumber,Email,Skype,AdditinalInfo,PersonId")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    id = contactInfo.PersonId
                });
            }

            return View(contactInfo);
        }

        // GET: ContactInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = db.ContactInfos.Find(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            var person = db.Persons.FirstOrDefault(p => p.PersonId == contactInfo.PersonId);
            ViewBag.FullName = $"{person?.LastName} {person?.FirstName} {person?.MiddleName}";
            return View(contactInfo);
        }

        // POST: ContactInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactInfo contactInfo = db.ContactInfos.Find(id);
            db.ContactInfos.Remove(contactInfo);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                id = contactInfo.PersonId
            });
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
