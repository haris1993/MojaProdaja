using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MojaProdaja.Models
{
    public class OglasiViewModel
    {
        public int proizvod_Id { get; set; }
        public string ImeProizvoda { get; set; }
        public string Slika_proizvoda { get; set; }
        public string Opis { get; set; }
        public Nullable<int> Cijena { get; set; }
        public Nullable<int> KategorijaId { get; set; }
        public Nullable<int> KorisnikId { get; set; }
        public int kategorija_Id { get; set; }
        public string ImeKategorije { get; set; }
        public string ImeIPrezime { get; set; }
        public string Slika_korisnika { get; set; }
        public string Kontakt { get; set; }

    }
}