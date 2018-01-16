using SalesFirst.Core.Data;
using SalesFirst.Core.Service;
using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaphirConges.Controllers
{
    [Authorize]
    public class CongesGeneralController : Controller
    {
        private SaphirCongesDB db = new SaphirCongesDB();
        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;

        public CongesGeneralController()
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }


        //
        //GET: /General/
        public ActionResult Index()
        {
            return View(db.CongesGeneral.ToList());
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View(db.CongesGeneral.ToList());
        }

        public ActionResult MonthCalendar()
        {

            ClientDb clientDb = new ClientDb();
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe == null || db.GetCongesNonRefuseByEmploye(employe).FirstOrDefault() == null)
            {
                ViewBag.GeneralHolidays = db.CongesGeneral.ToList();
                ViewBag.Holidays = null;
                return View();
            }

            ViewBag.CongesGeneral = db.CongesGeneral.ToList();
            ViewBag.Conges = db.GetCongesNonRefuseByEmploye(employe).ToList();
            return View();
        }


        public ActionResult FullYearCalendar()
        {
            ClientDb clientDb = new ClientDb();
            var loggedInUser = User.Identity.Name;
            var employe = employeService.GetEmployeeByUsername(loggedInUser);
            if (employe == null || db.GetCongesNonRefuseByEmploye(employe).FirstOrDefault() == null)
            {
                ViewBag.GeneralHolidays = db.CongesGeneral.ToList();
                ViewBag.HolidaysDesc = db.GetAllCongesDescriptions.ToList();
                ViewBag.Holidays = null;
                return View();
            }

            ViewBag.CongesGeneral = db.CongesGeneral.ToList();
            ViewBag.Conges = db.GetCongesAccepteByEmploye(employe).ToList();

            return View();
        }


        //
        //GET: /General/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesGeneral congesGen = db.CongesGeneral.Find(id);
            if (congesGen == null)
            {
                return HttpNotFound();
            }
            return View(congesGen);
        }

        //
        //GET: /General/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        //POST: /General/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CongesGeneralID,Nom,Description,Type,StartDate,EndDate,Frequency")] CongesGeneral congesGen)
        {
            congesGen.Type = "JoursFeries";
            if (ModelState.IsValid)
            {
                db.CongesGeneral.Add(congesGen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(congesGen);

        }

        //
        //GET: /General/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesGeneral congesGen = db.CongesGeneral.Find(id);
            if (congesGen == null)
            {
                return HttpNotFound();
            }
            return View(congesGen);

        }

        //
        //POST: /General/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CongesGeneralID,Nom,Description,StartDate,EndDate,Frequency")] CongesGeneral congesGen)
        {

            if (ModelState.IsValid)
            {
                db.Entry(congesGen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(congesGen);
        }

        //
        //GET: /General/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesGeneral congesGen = db.CongesGeneral.Find(id);
            if (congesGen == null)
            {
                return HttpNotFound();
            }
            return View(congesGen);
        }

        //
        //POST: /General/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            CongesGeneral congesGen = db.CongesGeneral.Find(id);
            db.CongesGeneral.Remove(congesGen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
            {
                db.Dispose();
            }
            base.Dispose(dispose);
        }
    }
}