using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
using SalesFirst.Core.Service;
using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaphirConges.Controllers
{
    [Authorize]
    public class CongesController : Controller
    {

        private readonly SaphirCongesDB db = new SaphirCongesDB();
        private readonly ClientDb salesFirstDb = new ClientDb();

        public enum typesConges { personnel, Mensuel, Conges_Maladie, Annuel };

        private void SetViewBagTypesConges(typesConges typeChoisi)
        {
            IEnumerable<typesConges> vals = Enum.GetValues(typeof(typesConges)).Cast<typesConges>();
            IEnumerable<SelectListItem> items = from val in vals
                                                select new SelectListItem
                                                {
                                                    Text = val.ToString(),
                                                    Value = val.ToString(),
                                                    Selected = val == typeChoisi,

                                                };
            ViewBag.typesConges = items;

        }

        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;

        public CongesController()
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        public ActionResult Index()
        {
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe == null || db.GetCongesByEmploye(employe).FirstOrDefault() == null)
            {
                ViewBag.Message = "Pas de congés prévus";
                return View();
            }
           
            return View(db.GetCongesByEmploye(employe).ToList());
        }

        //
        //GET: /Conges/Creer
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Personnel", Value = "Personnel", Selected = true });
            items.Add(new SelectListItem { Text = "Mensuel", Value = "Mensuel", });
            items.Add(new SelectListItem { Text = "Annuel", Value = "Annuel", });
            items.Add(new SelectListItem { Text = "Conges Maladie", Value = "Conges Maladie", });

            ViewBag.TypeConges = new SelectList(db.GetAllEmployeCongesDescriptions,"TypeConges","TypeConges");
            ViewBag.TypeConges = items;
            return View();
        }

        //	
        //POST: /Conges/Creer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CongesID,StartDate,EndDate,NoOfDays,Employe,BookingDate,BookedBy,TypeConges,CongesDescription,HalfDay")] Conges conges)
        {

            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (ModelState.IsValid)
            {
                conges.Employe = employe;
                conges.BookingDate = DateTime.Today;
                conges.BookedBy = User.Identity.GetUserName();
                db.Conges.Add(conges);
                db.SaveChanges();

                //Code envoi de mail si besoin
                return RedirectToAction("Index");

            }
            return View(conges);
        }

        public ActionResult Details(int? id)
        {
            Conges conges = db.Conges.Find(id);
            if (conges == null)
            {
                return HttpNotFound();
            }
            return View(conges);
        }

        //	
        //GET: /Conges/Delete/5
        public ActionResult Delete(int? id)
        {
            Conges conges = db.Conges.Find(id);
            if (conges == null)
            {
                return HttpNotFound();
            }
            if (id == null || conges.Statut != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(conges);

        }

        //
        //POST: /Conges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conges conges = db.Conges.Find(id);
            db.Conges.Remove(conges);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        //
        //GET: Conges/Edit/5
        public ActionResult Edit(int id)
        {
            Conges conges = db.Conges.Find(id);
            if (conges == null)
            {
                return HttpNotFound();
            }
            if ( conges.Statut != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            ViewBag.EmployeID = employe.EmployeeId;


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Personnel", Value = "Personnel", Selected = true });
            items.Add(new SelectListItem { Text = "Mensuel", Value = "Mensuel", });
            items.Add(new SelectListItem { Text = "Annuel", Value = "Annuel", });
            items.Add(new SelectListItem { Text = "Conges Maladie", Value = "Conges Maladie", });

            ViewBag.HolidayType = items;
            return View(conges);
        }

        //
        //Post: /Conges/Edit/5
        [HttpPost, ActionName("Editer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CongesID,StartDate,EndDate,NoOfDays,Employe,HalfDay,TypeConges,CongesDescription")] Conges conges)
        {

            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (ModelState.IsValid)
            {
                conges.Employe = employe;
                db.Entry(conges).State = EntityState.Modified;
                conges.BookingDate = DateTime.Today;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conges);
        }

        //Generation des rapports
        //GET: /Holiday/Report
        public ActionResult Report(int id)
        {
            return View();
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