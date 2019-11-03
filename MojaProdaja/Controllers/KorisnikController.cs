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
    public class KorisnikController : Controller
    {
        E_MarketingEntities db = new E_MarketingEntities();
        // GET: Korisnik
        public ActionResult Početna(int? stranica)
        {
            int velicinaStranice = 9, indexStranice = 1;
            indexStranice = stranica.HasValue ? Convert.ToInt32(stranica) : 1;
            var list = db.Kategorija.Where(x => x.Status == 1).OrderByDescending(x => x.Status).ToList();
            IPagedList<Kategorija> str = list.ToPagedList(indexStranice, velicinaStranice);


            return View(str);
        }

        [HttpGet]
        public ActionResult KreiranjeKorisnika()
        {

            return View();
        }

        [HttpPost]
        public ActionResult KreiranjeKorisnika(Korisnik korisnik, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("1"))
            {
                ViewBag.error = "Slika ne može biti dodata....";
            }
            else
            {
                Korisnik k = new Korisnik();
                k.ImeIPrezime = korisnik.ImeIPrezime;
                k.Email = korisnik.Email;
                k.Username = korisnik.Username;
                k.Password = korisnik.Password;
                k.Slika = path;
                k.BrojTelefona = korisnik.BrojTelefona;
                db.Korisnik.Add(k);
                db.SaveChanges();
                return RedirectToAction("Početna");

            }
            return View();
        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(Korisnik kor)
        //{
        //    Korisnik korisnik = db.Korisnik.Where(z => z.Username == kor.Username && z.Password == kor.Password).SingleOrDefault();
        //    if (korisnik != null)
        //    {

        //        Session["Id"] = korisnik.Id.ToString();
        //        return RedirectToAction("Početna");

        //    }
        //    else
        //    {
        //        ViewBag.error = "Pogrešno korisničko ime ili lozinka";

        //    }

        //    return View();
        //}

        [HttpGet]
        public ActionResult KreirajProizvod()
        {
            List<Kategorija> lista = db.Kategorija.ToList();
            ViewBag.categorylist = new SelectList(lista, "Id", "Naziv");

            return View();
        }

        [HttpPost]
        public ActionResult KreirajProizvod(Proizvod proizvod, HttpPostedFileBase imgfile)
        {
            List<Kategorija> lista = db.Kategorija.ToList();
            ViewBag.categorylist = new SelectList(lista, "Id", "Naziv");


            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Slika ne može biti dodata....";
            }
            else
            {
                Proizvod p = new Proizvod();
                p.Naziv = proizvod.Naziv;
                p.Cijena = proizvod.Cijena;
                p.Slika = path;
                p.KategorijaId = proizvod.KategorijaId;
                p.DetaljanOpis = proizvod.DetaljanOpis;
                p.KorisnikId = Convert.ToInt32(Session["Id"].ToString());
                db.Proizvod.Add(p);
                db.SaveChanges();
                Response.Redirect("Početna");

            }

            return View();
        }

        [HttpGet]
        public ActionResult Oglasi(int? id, int? stranica)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = stranica.HasValue ? Convert.ToInt32(stranica) : 1;
            var list = db.Proizvod.Where(x => x.KategorijaId == id).OrderByDescending(x => x.Id).ToList();
            IPagedList<Proizvod> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }

        [HttpPost]
        public ActionResult Oglasi(int? id, int? stranica, string pretraga)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = stranica.HasValue ? Convert.ToInt32(stranica) : 1;
            var list = db.Proizvod.Where(x => x.Naziv.Contains(pretraga)).OrderByDescending(x => x.Id).ToList();
            IPagedList<Proizvod> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }

        public ActionResult PogledNaOglas(int? id)
        {
            OglasiViewModel ad = new OglasiViewModel();
            Proizvod p = db.Proizvod.Where(x => x.Id == id).SingleOrDefault();
            ad.proizvod_Id = p.Id;
            ad.ImeProizvoda = p.Naziv;
            ad.Slika_proizvoda = p.Slika;
            ad.Cijena = Convert.ToInt32(p.Cijena);
            ad.Opis = p.DetaljanOpis;
            Kategorija cat = db.Kategorija.Where(x => x.Id == p.KategorijaId).SingleOrDefault();
            ad.ImeKategorije = cat.Naziv;
            Korisnik u = db.Korisnik.Where(x => x.Id == p.KorisnikId).SingleOrDefault();
            ad.ImeIPrezime = u.ImeIPrezime;
            ad.Slika_korisnika = u.Slika;
            ad.Kontakt = u.BrojTelefona;
            ad.KorisnikId = u.Id;




            return View(ad);
        }


        //public ActionResult Odjava()
        //{
        //    Session.RemoveAll();
        //    Session.Abandon();

        //    return RedirectToAction("Početna");
        //}


        public ActionResult ObrisiOglas(int? id)
        {

            Proizvod p = db.Proizvod.Where(x => x.Id == id).SingleOrDefault();
            db.Proizvod.Remove(p);
            db.SaveChanges();

            return RedirectToAction("Početna");
        }


        [HttpGet]
        public ActionResult EditujOglas(int? id)
        {
            Proizvod p = db.Proizvod.Where(i => i.Id == id).SingleOrDefault();

            List<Kategorija> lista = db.Kategorija.ToList();
            ViewBag.categorylist = new SelectList(lista, "Id", "Naziv");

            return View(p);
        }
        [HttpPost]
        public ActionResult EditujOglas(Proizvod p, HttpPostedFileBase imgfile)
        {


            List<Kategorija> lista = db.Kategorija.ToList();
            ViewBag.categorylist = new SelectList(lista, "Id", "Naziv");

            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Slika ne može biti dodata....";
            }
            else
            {

                p.Naziv = p.Naziv;
                p.Cijena = p.Cijena;
                p.Slika = path;
                p.KategorijaId = p.KategorijaId;
                p.DetaljanOpis = p.DetaljanOpis;
                p.KorisnikId = Convert.ToInt32(Session["Id"].ToString());
                db.SaveChanges();
                Response.Redirect("PogledNaOglas");
            }
            return View("Početna");
        }


    }
}
