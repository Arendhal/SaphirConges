using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
using SalesFirst.Core.Service;
using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using System;
using SaphirCongesCore.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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

        private void MakeViewBag()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Personnel", Value = "Personnel", Selected = true });
            items.Add(new SelectListItem { Text = "Mensuel", Value = "Mensuel", });
            items.Add(new SelectListItem { Text = "Annuel", Value = "Annuel", });
            items.Add(new SelectListItem { Text = "Conges Maladie", Value = "Conges Maladie", });

            ViewBag.TypeConges = new SelectList(db.GetAllEmployeCongesDescriptions, "TypeConges", "TypeConges");
            ViewBag.TypeConges = items;
        }

        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;

        public CongesController()
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        public ActionResult FullYearCalendar()
        {
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
           // Utils.GetPublicHoliday();
            if (employe == null || db.GetCongesNonRefuseByEmploye(employe).FirstOrDefault() == null)
            {
                ViewBag.CongesGeneral = db.CongesGeneral.ToList();
                ViewBag.CongesDescription = db.GetAllCongesDescriptions.ToList();
                ViewBag.Conges = null;
                return View();
            }

            ViewBag.CongesGeneral = Utils.GetPublicHoliday();
            //ViewBag.CongesGeneral = db.CongesGeneral.ToList();
            ViewBag.Conges = db.GetCongesNonRefuseByEmploye(employe).ToList();

            return View();
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
            MakeViewBag();
            return View();
        }

        //	
        //POST: /Conges/Creer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CongesID,StartDate,EndDate,NoOfDays,Employe,BookingDate,BookedBy,TypeConges,CongesDescription")] Conges conges)
        {
            MakeViewBag();
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            var cult = System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var NoOfDaysDecimal = Request.Form["NoOfDays"];
           
            if (ModelState.IsValid || conges.NoOfDays==0)
            {
                if(conges.CongesDescription == null)
                {
                    conges.CongesDescription = "Congés '" + conges.TypeConges + " 'de " + employe.FirstName + " " + employe.LastName;
                }
                conges.Employe = employe;
                conges.BookingDate = DateTime.Today;
                conges.BookedBy = User.Identity.GetUserName();
                conges.NoOfDays = Single.Parse(NoOfDaysDecimal, cult);
                db.Conges.Add(conges);
                db.SaveChanges();

                //Code envoi de mail si besoin
                return RedirectToAction("FullYearCalendar");

            }
            return View(conges);
        }

        //
        //GET: Conges/Edit/5
        public ActionResult Edit(int id)
        {
            MakeViewBag();
            Conges conges = db.Conges.Find(id);
            if (conges == null)
            {
                return HttpNotFound();
            }
            if (conges.Statut != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            ViewBag.EmployeID = employe.EmployeeId;


            /*List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Personnel", Value = "Personnel", Selected = true });
            items.Add(new SelectListItem { Text = "Mensuel", Value = "Mensuel", });
            items.Add(new SelectListItem { Text = "Annuel", Value = "Annuel", });
            items.Add(new SelectListItem { Text = "Conges Maladie", Value = "Conges Maladie", });

            ViewBag.HolidayType = items;*/
            return View(conges);
        }

        //
        //Post: /Conges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CongesID,StartDate,EndDate,NoOfDays,Employe,TypeConges,")] Conges conges)
        {
            MakeViewBag();
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            var cult = System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var NoOfDaysDecimal = Request.Form["NoOfDays"];
            if (ModelState.IsValid == false)
            {
                if (conges.CongesDescription == null)
                {
                    conges.CongesDescription = "Congés '" + conges.TypeConges + " 'de " + employe.FirstName + " " + employe.LastName;
                }
                conges.Employe = employe;
                conges.BookingDate = DateTime.Today;
                conges.NoOfDays = Single.Parse(NoOfDaysDecimal, cult);
                db.Entry(conges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FullYearCalendar");
  
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