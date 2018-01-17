using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using SalesFirst.Core.Data;
using SalesFirst.Core.Model;
using SaphirCongesCore.Data;
using SalesFirst.Core.Service;
using System.Collections.Generic;
using System.Data.Entity;

namespace SaphirConges.Controllers
{

    public class EmployeController : Controller
    {

        private ClientDb db = new ClientDb();
        private SaphirCongesDB SaphirDb = new SaphirCongesDB();
        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;

        public enum JobTitle
        {
            Directeur=1,
            Responsable=2,
            Employe=3,
        };

        public void SetViewBagJobTitle(JobTitle job)
        {
            IEnumerable<JobTitle> vals = Enum.GetValues(typeof(JobTitle)).Cast<JobTitle>();
            IEnumerable<SelectListItem> items = from val in vals
                                               select new SelectListItem
                                               {
                                                   Text = val.ToString(),
                                                   Value = val.ToString(),
                                                   Selected = val == job,
                                               };
            ViewBag.JobTitle = items;
        }


        public EmployeController()
        {

            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        //
        //GET: /Employe
        public ActionResult Index()
        {
            return View(employeService.GetAll().OrderBy(s=>s.JobTitle));
        }

        //
        //GET: /Employe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employe = employeService.Get(id);
            if (employe == null)
            {
                return HttpNotFound();
            }
            return View(employe);
        }

        //
        //GET: /Employe/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Directeur", Value = "Directeur", Selected = true });
            items.Add(new SelectListItem { Text = "Responsable", Value = "Responsable", });
            items.Add(new SelectListItem { Text = "Employe", Value = "Employe", });
            
            ViewBag.JobTitle = new SelectList(employeService.GetAll(), "JobTitle", "JobTitle");
            ViewBag.JobTitle = items;

            return View();
        }

        //
        //POST: /Employe/Create		
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,BirthDate,HireDate,PersonalEmail,JobTitle")] Employee employe)
        {
           
            if (ModelState.IsValid)
            {
                employe.Username = "" + employe.FirstName +"." + employe.LastName + "@apexure.com";
                employe.Username = employe.Username.ToLower();
                employeService.Create(employe);
                return RedirectToAction("Index");
            }
            return View(employe);
        }

        //
        //GET: /Employe/Edit/
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Directeur", Value = "Directeur", Selected = true });
            items.Add(new SelectListItem { Text = "Responsable", Value = "Responsable", });
            items.Add(new SelectListItem { Text = "Employe", Value = "Employe", });

            ViewBag.JobTitle = new SelectList(employeService.GetAll(), "JobTitle", "JobTitle");
            ViewBag.JobTitle = items;

           var loggedInUser = User.Identity.Name;
           Employee employe = employeService.Get(id);
           Employee me = employeService.GetEmployeeByUsername(loggedInUser);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(id != me.EmployeeId && me.JobTitle=="Employe" || id != me.EmployeeId && me.JobTitle == "Responsable")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (employe == null)
            {
                return HttpNotFound();
            }
            return View(employe);
        }

        //
        //POST: /Employe/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,PersonalEmail,HireDate,BirthDate,JobTitle")]Employee employe)
        {
             

            if (ModelState.IsValid)
            {
                employeService.Update(employe);
                SaphirDb.Entry(employe).State = EntityState.Modified;
                SaphirDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employe);
        }

        //
        //GET: /Employe/Delete/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employe = employeService.Get(id);
            if (employe == null)
            {
                return HttpNotFound();
            }
            return View(employe);
        }

        //
        //POST: /Employe/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            Employee employe = employeRepo.Get(id);
            employeRepo.Delete(employe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
            {
                //employeService.Dispose();
            }
            base.Dispose(dispose);
        }
    }
}
