using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectGestionLocationVoiture.Models;

namespace ProjectGestionLocationVoiture.Controllers
{
    public class ClientsController : Controller
    {
        private readonly Context db = new Context();

        // GET: Clients

        public ActionResult Voitures(string Categorie, string Carburant, DateTime? datedebut, DateTime? datefin)
        {

            
            var voitures = db.Voitures.Include(v => v.Modele);
            if (datedebut == null || datefin == null || datedebut > datefin || datefin <= DateTime.Now || datefin == datedebut || datedebut <= DateTime.Now)
            {
                ViewBag.dateDebut = null;
                ViewBag.dateFin = null;
                return View(voitures.ToList());
            }

            ViewBag.dateDebut = datedebut;
            ViewBag.dateFin = datefin;
            if (string.IsNullOrWhiteSpace(Categorie) && string.IsNullOrWhiteSpace(Categorie))
                return View(voitures.ToList());

            var Filtere = voitures.Where(v => v.Categorie.ToLower() == Categorie.ToLower().Trim() && v.Carburant.ToLower() == Carburant.ToLower().Trim());

            return View(Filtere.ToList());

        }
        public ActionResult DetailsReservation(DateTime date, int voiture, int client)
        {
            var reservations = db.Reservations.Include(r => r.Client);
            reservations = reservations.Include(r => r.Voiture);
            var reservation = reservations.Where(r => r.DateDebutLocation == date && r.CID == client && r.VID == voiture);
            return View(reservation.First());
        }
        // GET: Clients/Details/5
        public ActionResult Reserver(int? id, DateTime? datedebut, DateTime? datefin)
        {
            var voiture = db.Voitures.Include(v => v.Modele);
            DateTime DateDebut = (DateTime)datedebut;
            DateTime DateFin = (DateTime)datefin;
            ViewBag.dateDebut = DateDebut; //
            ViewBag.dateFin = DateFin;     //.ToString("dddd, dd MMMM yyyy");
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("NotFound");
            }
            var Filtered = voiture.Where(v => v.VID == id).First();
            if (Filtered == null)
            {
                return HttpNotFound();
            }
            ViewData["img1"] = Filtered.ImagePrincipale;
            ViewData["img2"] = Filtered.ImageSecondaire;
            int jours = (DateFin - DateDebut).Days;
            ViewBag.jours = jours;
            if(jours > 30)
                ViewBag.Total = jours * Filtered.Prixournalier * 0.6 * 30 + 150.00;
            else
                ViewBag.Total = jours * Filtered.Prixournalier + 150.00;
            return View(Filtered);
        }

        public ActionResult ConfirmerResevation(int idClient, int Voiture, DateTime datedebut, DateTime datefin)
        {
            

            DateTime DD = (DateTime)datedebut;
            DateTime DF = (DateTime)datefin;

            Voiture voiture = db.Voitures.Find(Voiture);
            Client Client  = db.Clients.Find(idClient);
            ViewBag.Voiture = Voiture;
            string type = (DF - DD).Days <= 30 ? "courte" : "Longue";
            Reservation reservation = new Reservation
            {
                Client = Client,
                CID = Client.UID,
                VID = Voiture,
                Voiture = voiture,
                DateDebutLocation = DD,
                DateFinLocation = DF,
                TypeLocation = type
            };
            Reservation check = null;
            if (db.Reservations.Where(r => r.DateDebutLocation == DD && r.CID == idClient && r.VID == Voiture).Count() > 0)
                check = db.Reservations.Where(r => r.DateDebutLocation == DD && r.CID == idClient && r.VID == Voiture).First();

            
            if(check == null)
            {
                db.Reservations.Add(reservation);
                voiture.Dsiponible = false; //wrong
                db.SaveChanges();
                return RedirectToAction("MesReservations");
            }
            return RedirectToAction("voitures");
            //return RedirectToAction("Reserver", new { id = Voiture, DateTime ? datedebut, DateTime ? datefin });



        }
        // GET: Clients/Create
        public ActionResult MesReservations()
        {
            if (Session["CurrentUser"] == null)
                return RedirectToAction("voitures");
            int user = (int)Session["CurrentUser"];
            var reservations = db.Reservations.Include(v => v.Voiture);
            reservations = reservations.Include(v => v.Voiture.Modele);
            var filtered = reservations.Where(r => r.CID == user);
            return View(filtered.ToList());
        }



        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Clients/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,Nom,AdresseMail,MotDePasse,Telephone,DateNaissance,Cin,PermisConduire")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
