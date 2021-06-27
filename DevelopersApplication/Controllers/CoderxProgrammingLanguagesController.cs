using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevelopersApplication.Models;

namespace DevelopersApplication.Controllers
{
    public class CoderxProgrammingLanguagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CoderxProgrammingLanguages
        public ActionResult Index()
        {
            var codersxProgrammingLanguages = db.CodersxProgrammingLanguages.Include(c => c.Coder).Include(c => c.ProgrammingLanguage);
            return View(codersxProgrammingLanguages.ToList());
        }

        // GET: CoderxProgrammingLanguages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             CoderxProgrammingLanguage coderxProgrammingLanguage = db.CodersxProgrammingLanguages.Find(id);
            if (coderxProgrammingLanguage == null)
            {
                return HttpNotFound();
            }
            return View(coderxProgrammingLanguage);
        }

        // GET: CoderxProgrammingLanguages/Create
        public ActionResult Create()
        {
            ViewBag.CoderId = new SelectList(db.Coders, "CoderId", "Name");
            ViewBag.LanguageId = new SelectList(db.ProgrammingLanguages, "LanguageId", "Language");
            return View();
        }

        // POST: CoderxProgrammingLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoderxPLId,CoderId,LanguageId,FavLang")] CoderxProgrammingLanguage coderxProgrammingLanguage)
        {
            if (ModelState.IsValid)
            {
                db.CodersxProgrammingLanguages.Add(coderxProgrammingLanguage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CoderId = new SelectList(db.Coders, "CoderId", "Name", coderxProgrammingLanguage.CoderId);
            ViewBag.LanguageId = new SelectList(db.ProgrammingLanguages, "LanguageId", "Language", coderxProgrammingLanguage.LanguageId);
            return View(coderxProgrammingLanguage);
        }

        // GET: CoderxProgrammingLanguages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoderxProgrammingLanguage coderxProgrammingLanguage = db.CodersxProgrammingLanguages.Find(id);
            if (coderxProgrammingLanguage == null)
            {
                return HttpNotFound();
            }
            ViewBag.CoderId = new SelectList(db.Coders, "CoderId", "Name", coderxProgrammingLanguage.CoderId);
            ViewBag.LanguageId = new SelectList(db.ProgrammingLanguages, "LanguageId", "Language", coderxProgrammingLanguage.LanguageId);
            return View(coderxProgrammingLanguage);
        }

        // POST: CoderxProgrammingLanguages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoderxPLId,CoderId,LanguageId,FavLang")] CoderxProgrammingLanguage coderxProgrammingLanguage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coderxProgrammingLanguage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CoderId = new SelectList(db.Coders, "CoderId", "Name", coderxProgrammingLanguage.CoderId);
            ViewBag.LanguageId = new SelectList(db.ProgrammingLanguages, "LanguageId", "Language", coderxProgrammingLanguage.LanguageId);
            return View(coderxProgrammingLanguage);
        }

        // GET: CoderxProgrammingLanguages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoderxProgrammingLanguage coderxProgrammingLanguage = db.CodersxProgrammingLanguages.Find(id);
            if (coderxProgrammingLanguage == null)
            {
                return HttpNotFound();
            }
            return View(coderxProgrammingLanguage);
        }

        // POST: CoderxProgrammingLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoderxProgrammingLanguage coderxProgrammingLanguage = db.CodersxProgrammingLanguages.Find(id);
            db.CodersxProgrammingLanguages.Remove(coderxProgrammingLanguage);
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
