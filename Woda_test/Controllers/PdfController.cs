﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Woda_test.Models;

namespace Woda_test.Controllers
{
    public class PdfController : Controller
    {
        private woda_testEntities db = new woda_testEntities();

        // GET: Pdf
        public ActionResult Index()
        {
            return View(db.pdffiles.ToList());
        }
        public ActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files, pdffile filepdf)
        {

            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);




                        db.pdffiles.Add(new pdffile
                        {
                            status = false,
                            File = ServerSavePath


                        });
                        db.SaveChanges();

                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            return View();
        }

        // GET: Pdf/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pdffile pdffile = db.pdffiles.Find(id);
            if (pdffile == null)
            {
                return HttpNotFound();
            }
            return View(pdffile);
        }

        // GET: Pdf/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pdf/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PdfID,status,File")] pdffile pdffile)
        {
            if (ModelState.IsValid)
            {
                db.pdffiles.Add(pdffile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pdffile);
        }

        // GET: Pdf/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pdffile pdffile = db.pdffiles.Find(id);
            if (pdffile == null)
            {
                return HttpNotFound();
            }
            return View(pdffile);
        }

        // POST: Pdf/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PdfID,status,File")] pdffile pdffile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pdffile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pdffile);
        }

        // GET: Pdf/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pdffile pdffile = db.pdffiles.Find(id);
            if (pdffile == null)
            {
                return HttpNotFound();
            }
            return View(pdffile);
        }

        // POST: Pdf/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pdffile pdffile = db.pdffiles.Find(id);
            db.pdffiles.Remove(pdffile);
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
