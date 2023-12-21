using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOgloszeniowyWpf.Klasy
{
    public class Profil
    {

        public int ProfilId { get; set; }

        public int UzytkownikId { get; set; }

        public string? Imie {  get; set; }

        public string? Nazwisko { get; set; }

        public string? Miasto { get; set; }

        public int? NumerTelefonu { get; set; }

        public string? ZdjecieProfilowe { get; set; }

        public string? Stanowisko { get; set; }

        public string? PodsumowanieZawodowe {  get; set; }

        public Profil(int profilId, int uzytkownikId, string? imie, string? nazwisko, string? miasto, int? numerTelefonu, string? zdjecieProfilowe, string? stanowisko
            ,string? podsumowanieZawodowe)
        {
            ProfilId = profilId;

            UzytkownikId = uzytkownikId;

            Imie = imie;

            Nazwisko = nazwisko;

            Miasto = miasto;

            NumerTelefonu = numerTelefonu;

            ZdjecieProfilowe = zdjecieProfilowe;

            Stanowisko = stanowisko;

            PodsumowanieZawodowe = podsumowanieZawodowe;
        }


    }
}
