using MojaProdaja.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojaProdaja.Controllers
{
    public class AdminController : Controller
    {
        E_MarketingEntities db = new E_MarketingEntities();
        // GET: Admin

      //[HttpGet]
      //public  ActionResult Login()
      //  {
      //      return View();
      //  }

      //  [HttpPost]
      //  public ActionResult login(Admin adm)
      //  {
      //      Admin ad = db.Admin.Where(x => x.Username == adm.Username && x.Password == adm.Password).SingleOrDefault();
      //      if (ad != null)
      //      {

      //          Session["Id"] = ad.Id.ToString();
      //          return RedirectToAction("KreirajKategoriju");

      //      }
      //      else
      //      {
      //          ViewBag.error = "Pogrešno korisničko ime ili lozinka";

      //      }

      //      return View();
      //  }

        [HttpGet]
        public ActionResult KreirajKategoriju()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("PregledKateogrija");
            }
            return View();
        }

        [HttpPost]
        public ActionResult KreirajKategoriju(Kategorija kategorija, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Slika se ne može postavit....";
            }
            else
            {
                Kategorija kat = new Kategorija();
                kat.Naziv = kategorija.Naziv;
                kat.Slika = path;
                kat.Status = 1;
                kat.AdminId = Convert.ToInt32(Session["Id"].ToString());
                db.Kategorija.Add(kat);
                db.SaveChanges();
                return RedirectToAction("PregledKategorija");
            }

            return View();
        }

        public string uploadimgfile(HttpPostedFileBase fajl)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (fajl != null && fajl.ContentLength > 0)
            {
                string extension = Path.GetExtension(fajl.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(fajl.FileName));
                        fajl.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(fajl.FileName);

                        ViewBag.Message = "Fajl je uspješno dodat";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Samo jpg,jpeg ili png formati....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Molimo selektujte fajl'); </script>");
                path = "-1";
            }



            return path;
        }
        public ActionResult PregledKategorija(int? stranica)
        {
            int velicinaStranice = 9, indexStranice = 1;
            indexStranice = stranica.HasValue ? Convert.ToInt32(stranica) : 1;
            var list = db.Kategorija.Where(x => x.Status == 1).OrderByDescending(x => x.Id).ToList();
            IPagedList<Kategorija> str = list.ToPagedList(indexStranice, velicinaStranice);


            return View(str);




        }

        public ActionResult ObrisiKategoriju(int? id)
        {

            Kategorija k = db.Kategorija.Where(x => x.Id == id).SingleOrDefault();
            db.Kategorija.Remove(k);
            db.SaveChanges();

            return RedirectToAction("PregledKategorija");
        }

        [HttpGet]
        public ActionResult EditujKategoriju(int? id)
        {
            Kategorija k = db.Kategorija.Where(i => i.Id == id).SingleOrDefault();

            return View(k);
        }
        [HttpPost]
        public ActionResult EditujKategoriju(Kategorija k, HttpPostedFileBase imgfile)
        {



            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Slika ne može biti dodata....";
            }
            else
            {

                k.Naziv = k.Naziv;
                k.Slika = path;
                db.SaveChanges();
                Response.Redirect("PregledKategorija");
            }
            return View("PregledKategorija");
        }

    }
}