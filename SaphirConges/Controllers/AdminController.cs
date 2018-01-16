using SalesFirst.Core.Data;
using SalesFirst.Core.Model;
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
    public class AdminController : Controller
    {
        private readonly SaphirCongesDB db = new SaphirCongesDB();
        private readonly ClientDb salesFirstDb = new ClientDb();
        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;
       

        public AdminController()
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        //Liste des types de congés
        public enum typeConges { personnel, Mensuel, Conge_Maladie, Annuel };

        private void SetEnsembleViewTypeConge(typeConges typeChoisi)
        {
            IEnumerable<typeConges> values = Enum.GetValues(typeof(typeConges)).Cast<typeConges>();
            IEnumerable<SelectListItem> items = from val in values
                                                select new SelectListItem
                                                {
                                                    Text = val.ToString(),
                                                    Value = val.ToString(),
                                                    Selected = val == typeChoisi,
                                                };

            ViewBag.typeConges = items;
        }

        //
        //GET: /Admin/
        public ActionResult Index()
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if(employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            return View(db.GetCongesEnAttente().ToList());
        }

        //
        //GET: /Admin/Requests
        public ActionResult Requests()
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            return View(db.GetCongesAcceptes().ToList());
        }

        //
        //GET: /Admin/RejectedRequests
        public ActionResult RejectedRequests()
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            return View(db.GetCongesRefuse().ToList());
        }

        //
        //GET: /Admin/Edit/1
        public ActionResult Edit(int id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }

            Conges conges = db.Conges.Find(id);
            if (conges == null)
            {
                return HttpNotFound();
            }
            return View(conges);
        }

        //
        //POST: /Holiday/Edit/1
        [HttpPost, ActionName("Editer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CongesID,StartDate,EndDate,NoOfDays,Employe,BookingDate,HalfDay,TypeConges,CongesDescription,Statut")] Conges conges)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            
            if (ModelState.IsValid)
            {
                db.Entry(conges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Requests");
            }
            return View(conges);
        }

        //		
        //GET: /Admin/Create
        [Authorize]
        public ActionResult Create()
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Personnel", Value = "Personnel", Selected = true });
            items.Add(new SelectListItem { Text = "Mensuel", Value = "Mensuel" });
            items.Add(new SelectListItem { Text = "Annuel", Value = "Annuel" });
            items.Add(new SelectListItem { Text = "Conge Maladie", Value = "Conge Maladie" });

            ViewBag.TypeConges = items;

            var employes = employeService.GetAll();
            List<SelectListItem> employeList = new List<SelectListItem>();
            foreach (var nom in employes)
            {
                employeList.Add(new SelectListItem { Text = nom.Username, Value = nom.Username });
            }
            ViewBag.EmployeName = employeList;
            return View();
        }

        //
        //POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NomsEmployes,StartDate,EndDate,NoOfDays,TypeConges,CongesDescription,HalfDay")] Conges conges)
        {
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(Request.Form["NomsEmployes"]);
           
            if (ModelState.IsValid)
            {
                conges.Employe = employe;
                conges.BookingDate = DateTime.Today;
                conges.BookedBy = employe.Username;
                conges.Statut = "Approuve";
                db.Conges.Add(conges);
                db.SaveChanges();

                //Code envoi de mail si besoin		
                return RedirectToAction("Index");
            }
            return View(conges);
        }

        //
        //GET: /Admin/Accept/1
        public ActionResult Accept(int? id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }

            if (id == null)
            {
                return HttpNotFound();
            }
            Conges conges = db.Conges.Find(id);
            if (conges == null || conges.StartDate < DateTime.Today)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            conges.Statut = "Accepte";
            var date = conges.StartDate;
            db.Entry(conges).State = EntityState.Modified;
            db.SaveChanges();

            //Code envoi de mail si besoin
            return RedirectToAction("Index");
        }

        //
        //GET: /Admin/Reject/1
        public ActionResult Reject(int id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            Conges conges = db.Conges.Find(id);
            if (conges == null || conges.StartDate < DateTime.Today)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//TODO Faire une vue adaptée
            }
           
            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            conges.Statut = "Rejete";
            var date = conges.StartDate;
            db.Entry(conges).State = EntityState.Modified;
            db.SaveChanges();

            //Code envoi de mail si besoin
            return RedirectToAction("Index");
        }

        //
        //GET: /Admin/Details/1
        public ActionResult HolidayDetails(int? id)
        {
            var loggedInUser = User.Identity.Name;
            Employee employe = employeService.GetEmployeeByUsername(loggedInUser);
            Conges conges = db.Conges.Find(id);

            if (employe.JobTitle == "Employe")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //TODO Faire une vue adaptée
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if (conges == null)
            {
                return HttpNotFound();
            }
            ViewData["Date de début"] = conges.StartDate;
            return View(conges);
        }
    }
}
