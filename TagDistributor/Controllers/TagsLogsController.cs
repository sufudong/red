using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TagDistributor.DAL;
using TagDistributor.Models;

namespace TagDistributor.Controllers
{
    public class TagsLogsController : Controller
    {
        private TDContext db = new TDContext();

        // GET: TagsLogs
        public ActionResult Index()
        {
            return View(db.TagsLogs.ToList());
        }

        // GET: TagsLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagsLog tagsLog = db.TagsLogs.Find(id);
            if (tagsLog == null)
            {
                return HttpNotFound();
            }
            return View(tagsLog);
        }

        // GET: TagsLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagsLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Info,Username,DistributDate")] TagsLog tagsLog)
        {
            if (ModelState.IsValid)
            {
                db.TagsLogs.Add(tagsLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tagsLog);
        }

        // GET: TagsLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagsLog tagsLog = db.TagsLogs.Find(id);
            if (tagsLog == null)
            {
                return HttpNotFound();
            }
            return View(tagsLog);
        }

        // POST: TagsLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Info,Username,DistributDate")] TagsLog tagsLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tagsLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tagsLog);
        }

        // GET: TagsLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagsLog tagsLog = db.TagsLogs.Find(id);
            if (tagsLog == null)
            {
                return HttpNotFound();
            }
            return View(tagsLog);
        }

        // POST: TagsLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TagsLog tagsLog = db.TagsLogs.Find(id);
            db.TagsLogs.Remove(tagsLog);
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
