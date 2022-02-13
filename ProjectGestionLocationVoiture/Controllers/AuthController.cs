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
    public class AuthController : Controller
    {
        private readonly Context db = new Context();



        // GET: Auth/Create
        public ActionResult CreerCompte()
        {
            return View();
        }

        // POST: Auth/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreerCompte([Bind(Include = "Nom,AdresseMail,MotDePasse,Telephone,DateNaissance, CinFile, PermisConduireFile")] Client client)
        {

            if (ModelState.IsValid)
            {

                client.Cin = "~/Assets/Data/" + client.CinFile.FileName;
                string path = Path.Combine(Server.MapPath("~/Assets/Data/"), client.CinFile.FileName);
                client.CinFile.SaveAs(path);

                client.PermisConduire = "~/Assets/Data/" + client.PermisConduireFile.FileName;
                path = Path.Combine(Server.MapPath("~/Assets/Data/"), client.PermisConduireFile.FileName);
                client.PermisConduireFile.SaveAs(path);

                client.TypeUser = "client";
                string hashedPassword = HashPassword.GetMd5Hash(client.MotDePasse);
                client.MotDePasse = hashedPassword;

                db.Clients.Add(client);
                db.SaveChanges();
                Session["CurrentUser"] = client.UID;
                Session["CurrentUserEmail"] = client.AdresseMail;
                Session["Typeuser"] = client.TypeUser;
                return RedirectToAction("Voitures", "Clients");
            }

            return View(client);
        }




        public ActionResult LogOut()
        {
            Session["CurrentUserEmail"] = null;
            Session["CurrentUser"] = null;
            Session["TypeUser"] = null;
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {

            //return View(client);
            string admin = "admin";
   
            if (email == null || password == null)
                return View();

            User CurrentUser;

            if (db.Users.Where(c => c.AdresseMail == email).Count() > 0)
            {
                string hashedPassword = HashPassword.GetMd5Hash(password);
                CurrentUser = db.Users.Where(c => c.AdresseMail == email).SingleOrDefault();
                if(CurrentUser.MotDePasse == hashedPassword)
                {
                    Session["CurrentUserEmail"] = CurrentUser.AdresseMail;
                    if(CurrentUser.TypeUser == admin)
                    {
                        Session["Typeuser"] = admin;
                        return RedirectToAction("Voitures", "Admins");
                    }
                    else
                    {
                        Client client = db.Clients.Where(c => c.AdresseMail == email).First();
                        Session["Typeuser"] = client.TypeUser;
                        Session["CurrentUser"] = client.UID;
                        return RedirectToAction("Voitures", "Clients");
                    }
                }
            }

            return View();

        }       
    }
}
