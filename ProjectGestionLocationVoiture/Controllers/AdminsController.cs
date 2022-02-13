using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectGestionLocationVoiture.Models;

namespace ProjectGestionLocationVoiture.Controllers
{
    [Autorization]
    public class AdminsController : Controller
    {
        private readonly Context db = new Context();

        // GET: Admins
        public ActionResult Voitures()
        {
            var voitures = db.Voitures.Include(v => v.Modele);
            return View(voitures.ToList());
        }
        /* ------------------------------------------- Client --------------------------------------------*/
        public ActionResult Clients()
        {
            var Clients = db.Clients;
            return View(Clients.ToList());
        }
        public ActionResult DetailsClient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        /* ------------------------------------------ Admin ---------------------------------------------- */

        public ActionResult Admins()
        {
            var admins = db.Admins.ToList();

            return View(admins);
        }

        public ActionResult ModifierAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword.GetMd5Hash(admin.MotDePasse);
                admin.MotDePasse = hashedPassword;
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admins");
            }
            return View(admin);
        }


        public ActionResult AjouterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.TypeUser = "admin";
                string hashedPassword = HashPassword.GetMd5Hash(admin.MotDePasse);
                admin.MotDePasse = hashedPassword;
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Admins");
            }

            return View(admin);
        }

        /* ------------------------------------------- Modeles --------------------------------------------*/
        public ActionResult Modeles()
        {
            return View(db.Modeles.ToList());
        }

        public ActionResult ModeleDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modele modele = db.Modeles.Find(id);
            if (modele == null)
            {
                return HttpNotFound();
            }
            return View(modele);
        }

        public ActionResult CreateModele()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModele(Modele modele)
        {
            if (ModelState.IsValid)
            {
                db.Modeles.Add(modele);
                db.SaveChanges();
                return RedirectToAction("Modeles");
            }

            return View(modele);
        }

        public ActionResult ModifierModele(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modele modele = db.Modeles.Find(id);
            if (modele == null)
            {
                return HttpNotFound();
            }
            
            return View(modele);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierModele(Modele modele)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modele).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Modeles");
            }
            return View(modele);
        }

        /* ---------------------------------------- Reservations -----------------------------------------------*/

        public ActionResult Reservations()
        {
            var reservations = db.Reservations.Include(r => r.Voiture);
            reservations = reservations.Include(r => r.Client);
            return View(reservations.ToList());
        }

        public ActionResult DetailsReservation(DateTime date, int client, int voiture)
        {
            var reservations = db.Reservations.Include(r => r.Client);
            reservations = reservations.Include(r => r.Voiture);
            reservations = reservations.Include(r => r.Voiture.Modele);

            try
            {
                var reservation = reservations.Where(r => r.VID == voiture && r.CID == client && r.DateDebutLocation == date).FirstOrDefault();
                ViewBag.dateDebut = reservation.DateDebutLocation;
                ViewBag.dateFin   = reservation.DateFinLocation;

                int jours = (reservation.DateFinLocation - reservation.DateDebutLocation).Days;
                ViewBag.jours = jours;
                ViewBag.Total = jours * reservation.Voiture.Prixournalier + 150.00;
                return View(reservation);
            }
            catch(InvalidCastException e)
            {
                return RedirectToAction("Reservations");
            }     
        }

        /* ------------------------------------------- Voitures -----------------------------------------*/

        public ActionResult VoitureDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // GET: Admins/Create
        public ActionResult AjouterVoiture()
        {
            List<object> newList = new List<object>();
            foreach (var member in db.Modeles)
                newList.Add(new
                {
                    MID = member.MID,
                    Name = member.Marque + " " + member.Serie
                });
            ViewBag.MID = new SelectList(newList, "MID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterVoiture([Bind(Include = "VID,MID,Immatriculation,DateCirculation,Carburant,Prixournalier,ImagePrincipale,ImagePrincipaleFile,ImageSecondaire,ImageSecondaireFile,Dsiponible,Categorie")] Voiture voiture)
        {
            /*
             var errors = ModelState.Values.SelectMany(v => v.Errors);  
            */

            if (ModelState.IsValid)
            {

                voiture.ImagePrincipale = "~/Assets/Voitures/" + voiture.ImagePrincipaleFile.FileName;
                string path = Path.Combine(Server.MapPath("~/Assets/Voitures/"), voiture.ImagePrincipaleFile.FileName);
                voiture.ImagePrincipaleFile.SaveAs(path);

                voiture.ImageSecondaire = "~/Assets/Voitures/" + voiture.ImageSecondaireFile.FileName;
                path = Path.Combine(Server.MapPath("~/Assets/Voitures/"), voiture.ImageSecondaireFile.FileName);
                voiture.ImageSecondaireFile.SaveAs(path);

                db.Voitures.Add(voiture);
                db.SaveChanges();
                return RedirectToAction("Voitures");
            }



            List<object> newList = new List<object>();
            foreach (var member in db.Modeles)
                newList.Add(new
                {
                    MID = member.MID,
                    Name = member.Marque + " " + member.Serie
                });
            ViewBag.MID = new SelectList(newList, "MID", "Name");
            return View(voiture);
        }

        // GET: Admins/Edit/5

        
      
        public ActionResult ModifierVoiture(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            ViewBag.MID = new SelectList(db.Modeles, "MID", "Marque", voiture.MID);
            return View(voiture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierVoiture(Voiture voiture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voiture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Voitures");
            }
            ViewBag.MID = new SelectList(db.Modeles, "MID", "Marque", voiture.MID);
            return View(voiture);
        }

        // GET: Admins/Delete/5
        public ActionResult SupprimerVoiture(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            return View(voiture);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("SupprimerVoiture")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voiture voiture = db.Voitures.Find(id);
            db.Voitures.Remove(voiture);
            db.SaveChanges();
            return RedirectToAction("Voitures");
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
