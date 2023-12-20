using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SystemOgloszeniowy.Entity
{

    public class User
    {
        public int? Id { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public int? Data_Urodzenia { get; set; }
        public int? Numer_Telefonu { get; set; }
        public string? Miejsce_Zamieszkania { get; set; }
        public string? Aktualne_Stanowisko_Pracy { get; set; }
        public string? Aktualne_Stanowisko_Pracy_Opis { get; set; }
        public string? Opis_Stanowiska { get; set; }
        public User(int? id, string? imie, string? nazwisko, int? data_Urodzenia, int? numer_Telefonu, string? miejsce_Zamieszkania, string? aktualne_Stanowisko_Pracy, string? aktualne_Stanowisko_Pracy_Opis, string? opis_Stanowiska)
        {
            Id = id;
            Imie = imie;
            Nazwisko = nazwisko;
            Data_Urodzenia = data_Urodzenia;
            Numer_Telefonu = numer_Telefonu;
            Miejsce_Zamieszkania = miejsce_Zamieszkania;
            Aktualne_Stanowisko_Pracy = aktualne_Stanowisko_Pracy;
            Aktualne_Stanowisko_Pracy_Opis = aktualne_Stanowisko_Pracy_Opis;
            Opis_Stanowiska = opis_Stanowiska;
        }

        public User(string? imie, string? nazwisko, int? data_Urodzenia, int? numer_Telefonu, string? miejsce_Zamieszkania, string? aktualne_Stanowisko_Pracy, string? aktualne_Stanowisko_Pracy_Opis, string? opis_Stanowiska)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Data_Urodzenia = data_Urodzenia;
            Numer_Telefonu = numer_Telefonu;
            Miejsce_Zamieszkania = miejsce_Zamieszkania;
            Aktualne_Stanowisko_Pracy = aktualne_Stanowisko_Pracy;
            Aktualne_Stanowisko_Pracy_Opis = aktualne_Stanowisko_Pracy_Opis;
            Opis_Stanowiska = opis_Stanowiska;
        }
    }



}
