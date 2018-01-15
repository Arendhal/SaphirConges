using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaphirConges.Controllers
{
    public class CongesDescriptionController : Controller
    {

        private SaphirCongesDB db = new SaphirCongesDB();

        //Get: /CongesDescription/
        public ActionResult Index()
        {
            return View(db.CongesDescription.ToList());
        }

        //
        //GET: /CongesDescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesDescription congesDesc = db.CongesDescription.Find(id);
            if (congesDesc == null)
            {
                return HttpNotFound();
            }
            return View(congesDesc);

        }

        //
        //GET: /CongesDesc/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        //POST: /CongesDesc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CongesDescriptionID,TypeConge,TypePour,CongesColor")]CongesDescription congesDesc)
        {

            if (ModelState.IsValid)
            {
                congesDesc.CongesColor = "#" + congesDesc.CongesColor;
                db.CongesDescription.Add(congesDesc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(congesDesc);

        }

        //
        //GET: /CongesDesc/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesDescription congesDesc = db.CongesDescription.Find(id);
            if (congesDesc == null)
            {
                return HttpNotFound();
            }
            return View(congesDesc);

        }

        //
        //POST: /CongesDesc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CongesDescriptionID,TypeConge,TypePour,CongesColor")] CongesDescription congesDesc)

        {
            if (ModelState.IsValid)
            {
                congesDesc.CongesColor = "#" + congesDesc.CongesColor;
                db.Entry(congesDesc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(congesDesc);
        }

        //
        //GET: /CongesDesc/Delete/1
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CongesDescription congesDesc = db.CongesDescription.Find(id);
            if (congesDesc == null)
            {
                return HttpNotFound();
            }
            return View(congesDesc);

        }

        //
        //POST: /CongesDesc/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CongesDescription congesDesc = db.CongesDescription.Find(id);
            db.CongesDescription.Remove(congesDesc);
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