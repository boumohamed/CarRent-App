﻿using ProjectGestionLocationVoiture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGestionLocationVoiture.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.Voitures = db.Voitures.Count();
            ViewBag.Reservations = db.Reservations.Count();
            ViewBag.Clients = db.Clients.Count();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string categorie, string carburant, DateTime? datedebut, DateTime? datefin)
        {
            ViewBag.Error = 0;
            ViewBag.Voitures = db.Voitures.Count();
            ViewBag.Reservations = db.Reservations.Count();
            ViewBag.Clients = db.Clients.Count();
            if (datedebut == null || datefin == null || datedebut > datefin || datefin == DateTime.Now || datefin == datedebut)
            {
                ViewBag.Error = 1;
                return View();
            }
            return RedirectToAction("Voitures", "Clients", new { Categorie = categorie, Carburant = carburant, DateDebut = datedebut, DateFin = datefin });
        }


        public ActionResult About()
        {
            
            return View();
        }

        public ActionResult NotFound()
        {

            return View();
        }
    }
}