using MojaProdaja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojaProdaja.Controllers
{
    public class AutentifikacijaController : Controller
    {
        // GET: Autentifikacija

        E_MarketingEntities db = new E_MarketingEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel ad)
        {
            Admin admin = db.Admin.Where(x => x.Username == ad.Username && x.Password == ad.Password).SingleOrDefault();
            Korisnik korisnik = db.Korisnik.Where(z => z.Username == ad.Username && z.Password == ad.Password).SingleOrDefault();
            if (admin != null)
            {
                Session["Id"] = admin.Id.ToString();
                return RedirectToAction("KreirajKategoriju", "Admin");
            }
            if (korisnik != null)
            {
                Session["Id"] = korisnik.Id.ToString();
                return RedirectToAction("Početna", "Korisnik");
            }
            else
            {
                ViewBag.error = "Pogrešan username ili password";
            }
            return View();
        }

        public ActionResult Odjava()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Početna","Korisnik");
        }
    }
}