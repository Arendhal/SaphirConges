using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
using SalesFirst.Core.Model;
using SalesFirst.Core.Service;
using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using SaphirCongesCore.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaphirConges.Controllers
{
    [Authorize]
    public class EmployeQuotaController : Controller
    {
        private SaphirCongesDB db = new SaphirCongesDB();
        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;
        

        public EmployeQuotaController()
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        //
        //GET: /EmployeQuota/
        public ActionResult Manage()
        {

            return View(db.EmployeQuota.ToList());
        }


        //
        //GET: /EmployeQuota/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeQuota employeQuota = db.EmployeQuota.Find(id);
            if (employeQuota == null)
            {
                return HttpNotFound();
            }
            return View(employeQuota);

        }

        public ActionResult Index1()
        {
            //Nom de la personne connectée
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if ((employe == null) || db.GetEmployeQuotaByEmploye(employe) == null)
            {
                ViewBag.Message = "Pas d'information de quota disponible";
                return View();
            }
            ViewBag.approved = Utils.ActualCongesPris(employe);
            ViewBag.Username = employe.Username;
            return View(db.GetEmployeQuotaByEmploye(employe));

        }


        public ActionResult Index(int id = -1)
        {
            if (id == -1)
            {
                var loggedInUser = User.Identity.GetUserName();
                id  = employeService.GetEmployeeByUsername(User.Identity.Name).EmployeeId;
            }

            var employe = employeService.GetEmployeeByEmployeeId(id);          
            string Username = employe.Username;
            string result = "";
            if ((employe == null) || db.GetEmployeQuotaByEmploye(employe) == null)
            {
                ViewBag.Message = "Pas d'information de quota.";
                return View();
            }

            if (Username.Contains('@')) // to hide thge @apexure.com part of the username
            {
                int index = Username.IndexOf('@');
                result = Username.Substring(0, index);
                ViewBag.Username = result;
            }
           
            ViewBag.Entitlement = db.GetEmployeQuotaByEmploye(employe).PaidQuota;
            ViewBag.CongesPris = Utils.CongesPris(employe);
            ViewBag.CongesPrisThisYear = Utils.CongesPosesInYear(employe, DateTime.Now.Year);
            ViewBag.Restant = db.GetEmployeQuotaByEmploye(employe).PaidQuota - Utils.CongesPosesInYear(employe, DateTime.Now.Year);
            ViewData["PourcentRestant"] = Utils.CongesPosesInYear(employe, DateTime.Now.Year) / db.GetEmployeQuotaByEmploye(employe).PaidQuota;
            return View(db.GetEmployeQuotaByEmploye(employe));
        }


        //
        //GET: /EmployeQuota/Create
        public ActionResult Create()
        {
            ViewBag.Employe = new SelectList(employeService.GetAll(), "EmployeeId", "Username");
            return View();
        }

        //
        //POST: /EmployeQuota/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeQuotaID,EmployeID,PaidQuota,NonPaidQuota")]EmployeQuota employeQuota)
        {

            if (ModelState.IsValid)
            {
                employeQuota.NonPaidQuota = 22.5;
                employeQuota.PaidQuota = 22.5;
                db.EmployeQuota.Add(employeQuota);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(employeQuota);

        }


        //
        //GET: /EmployeQuota/Edit/5
        public ActionResult Edit(int? id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            ViewBag.Employes = new SelectList(employeService.GetAll(), "EmployeeId", "Username");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(id != employe.EmployeeId && employe.JobTitle =="Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            EmployeQuota employeQuota = db.EmployeQuota.Find(id);
            if (employeQuota == null)
            {
                return HttpNotFound();
            }

            return View(employeQuota);
        }

        //
        //POST: /EmployeQuota/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeQuotaID,EmployeID,PaidQuota,NonPaidQuota")] EmployeQuota employeQuota)
        {
            ViewBag.Employes = new SelectList(employeService.GetAll(), "EmployeeId", "Username");
            if (ModelState.IsValid)
            {
                db.Entry(employeQuota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(employeQuota);
        }


        //
        //GET: /EmployeQuota/Delete/5
        public ActionResult Delete(int? id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeQuota employeQuota = db.EmployeQuota.Find(id);
            if (employeQuota == null)
            {
                return HttpNotFound();
            }

            return View(employeQuota);
        }

        //POST: /EmployeQuota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeQuota employeQuota = db.EmployeQuota.Find(id);
            db.EmployeQuota.Remove(employeQuota);
            db.SaveChanges();
            return RedirectToAction("Manage"); //Change back to index once fixed 

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
