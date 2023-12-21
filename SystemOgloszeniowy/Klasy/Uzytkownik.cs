using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOgloszeniowyWpf.Klasy
{
    public class Uzytkownik
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Haslo { get; set; }
        public bool Administrator { get; set; }

        public Uzytkownik(int id, string nick, string haslo, string email, bool administrator)
        {
            ID = id;
            Email = email;
            Nick = nick;
            Haslo = haslo;
            Administrator = administrator;
        }

        public Uzytkownik(string nick, string haslo, string email, bool admin)
        {
            Nick = nick;
            Haslo = haslo;
            Administrator = admin;
            Email = email;
        }

        public Uzytkownik() { }
       
    }
}
