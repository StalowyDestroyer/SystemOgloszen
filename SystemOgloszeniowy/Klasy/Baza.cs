using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOgloszeniowyWpf.Klasy
{
    public class Baza
    {
        public static void TabelaUzytkownikow()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS uzytkownicy (uzytkownik_id INTEGER PRIMARY KEY AUTOINCREMENT, nick TEXT, haslo TEXT, email TEXT, administrator BOOLEAN)";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void UtworzUzytkownika(Uzytkownik uzytkownik)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();
                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT INTO uzytkownicy VALUES(NULL, @Nick, @Haslo, @Email, @Admin);";
                insertCommand.Parameters.AddWithValue("@Nick", uzytkownik.Nick);
                insertCommand.Parameters.AddWithValue("@Email", uzytkownik.Email);
                insertCommand.Parameters.AddWithValue("@Haslo", uzytkownik.Haslo);
                insertCommand.Parameters.AddWithValue("@Admin", uzytkownik.Administrator);
                insertCommand.ExecuteReader();
            }
        }

        public static Uzytkownik PobierzUzytkownika(string nick)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                string query = "SELECT * FROM uzytkownicy WHERE nick = @Nick";
                var command = new SqliteCommand(query, db);
                command.Parameters.AddWithValue("@Nick", nick);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var uzytkownik = new Uzytkownik
                        {
                            ID = reader.GetInt32(0),
                            Nick = reader.GetString(1),
                            Haslo = reader.GetString(2),
                            Email = reader.GetString(3),
                            Administrator = reader.GetBoolean(4)
                        };
                        return uzytkownik;
                    }
                }
            }

            return null; 
        }

        public static List<Uzytkownik> GetAllUsers()
        {
            List<Uzytkownik> users = new List<Uzytkownik>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();
                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "SELECT * FROM uzytkownicy";

                using (SqliteDataReader reader = insertCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int uzytkownik_id = reader.GetInt32(0);
                        string nick = reader.GetString(1);
                        string haslo = reader.GetString(2);
                        string email = reader.GetString(3);
                        bool administrator = reader.GetBoolean(4);

                        Uzytkownik user = new Uzytkownik(uzytkownik_id, nick, haslo, email, administrator);
                        users.Add(user);
                    }
                }
                return users;
            }
        }

        public static void GiveUserAdmin(Uzytkownik uzytkownik)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();
                var updateCommand = new SqliteCommand();
                updateCommand.Connection = db;
                updateCommand.CommandText = "UPDATE uzytkownicy SET administrator = @Administrator WHERE uzytkownik_id = @ID;";
                updateCommand.Parameters.AddWithValue("@ID", uzytkownik.ID);
                updateCommand.Parameters.AddWithValue("@Administrator", uzytkownik.Administrator);

                updateCommand.ExecuteNonQuery();
            }
        }

        public static void TabelaProfile(string user)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectUserCommand = new SqliteCommand();
                selectUserCommand.Connection = db;
                selectUserCommand.CommandText = "SELECT uzytkownik_id FROM uzytkownicy WHERE nick = @uzytkownik";
                selectUserCommand.Parameters.AddWithValue("@uzytkownik", user);

                int uzytkownikId = -1;

                using (SqliteDataReader userReader = selectUserCommand.ExecuteReader())
                {
                    if (userReader.Read())
                    {
                        uzytkownikId = userReader.GetInt32(0);
                    }
                }

                string createTablesCommand = @"
            CREATE TABLE IF NOT EXISTS profile(
                profil_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                uzytkownik_id INTEGER, 
                imie TEXT, 
                nazwisko TEXT, 
                miasto TEXT, 
                numer_telefonu INTEGER, 
                zdjecie_profilowe TEXT, 
                stanowisko TEXT, 
                podsumowanie_zawodowe TEXT, 
                FOREIGN KEY(uzytkownik_id) REFERENCES uzytkownicy(uzytkownik_id)
            );
            INSERT INTO profile (uzytkownik_id) SELECT uzytkownik_id FROM uzytkownicy WHERE nick = @uzytkownik;

            CREATE TABLE IF NOT EXISTS doswiadczenie_zawodowe(
                doswiadczenie_zawodowe_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                stanowisko TEXT, 
                nazwa_firmy TEXT, 
                okres_zatrudnienia TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );

            CREATE TABLE IF NOT EXISTS wyksztalcenie(
                wyksztalcenie_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                miejsce_edukacji TEXT, 
                poziom_wykszalcenia TEXT, 
                kierunek TEXT, 
                okres_wyksztalcenia TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );

            CREATE TABLE IF NOT EXISTS jezyki(
                jezyk_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                nazwa_jezyka TEXT, 
                poziom_jezyka TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );

            CREATE TABLE IF NOT EXISTS umiejetnosci(
                umiejetnosc_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                nazwa_umiejetnosci TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );

            CREATE TABLE IF NOT EXISTS dodatkowe_szkolenia(
                szkolenie_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                nazwa_szkolenia TEXT, 
                organizator TEXT, 
                okres_szkolenia TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );

            CREATE TABLE IF NOT EXISTS linki(
                link_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                profil_id INTEGER, 
                link TEXT, 
                FOREIGN KEY(profil_id) REFERENCES profile(profil_id)
            );
        ";

                var createTables = new SqliteCommand(createTablesCommand, db);
                createTables.Parameters.AddWithValue("@uzytkownik", user);
                createTables.ExecuteNonQuery();
            }
        }

        public static List<Profil> CzytajProfil(string user)
        {
            List<Profil> profile = new List<Profil>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM profile p INNER JOIN uzytkownicy u ON p.uzytkownik_id = u.uzytkownik_id WHERE u.nick = @uzytkownik";
                selectCommand.Parameters.AddWithValue("@uzytkownik", user);
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int profilId = reader.GetInt32(0);
                        int uzytkownikId = reader.GetInt32(1);
                        string? imie = reader.IsDBNull(2) ? null : reader.GetString(2);
                        string? nazwisko = reader.IsDBNull(3) ? null : reader.GetString(3);
                        string? miasto = reader.IsDBNull(4) ? null : reader.GetString(4);
                        int? numerTelefonu = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5);
                        string? zdjecieProfilowe = reader.IsDBNull(6) ? null : reader.GetString(6);
                        string? stanowisko = reader.IsDBNull(7) ? null : reader.GetString(7);
                        string? podsuowanieZawodowe = reader.IsDBNull(8) ? null : reader.GetString(8);

                        Profil prof = new Profil(profilId, uzytkownikId, imie, nazwisko, miasto, numerTelefonu, zdjecieProfilowe, stanowisko, podsuowanieZawodowe);

                        profile.Add(prof);
                    }
                }
            }

            return profile;
        }

        public static void TabelaOgloszenia()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT EXISTS kategorie(kategoria_id INTEGER PRIMARY KEY AUTOINCREMENT, nazwa TEXT);" +
                "CREATE TABLE IF NOT EXISTS firmy(firma_id INTEGER PRIMARY KEY AUTOINCREMENT, nazwa TEXT);" +
                "CREATE TABLE IF NOT EXISTS ogloszenia (ogloszenie_id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "kategoria_id INTEGER,firma_id INTEGER,tytul TEXT,nazwa_stanowiska TEXT," +
                "poziom_stanowiska TEXT,rodzaj_pracy TEXT,wymiar_zatrudnienia TEXT,rodzaj_umowy TEXT,najmniejsze_wynagrodzenie DECIMAL(5,2)," +
                "najwieksze_wynagrodzenie DECIMAL(5,2),dni_pracy TEXT,godziny_pracy TEXT,data_waznosci DATE,obowiazki TEXT," +
                "wymagania TEXT,benefity TEXT,informacje TEXT,data_utworzenia DATE, zdjecie TEXT," +
                "FOREIGN KEY(kategoria_id) REFERENCES kategorie(kategoria_id),FOREIGN KEY(firma_id) REFERENCES firmy(firma_id));";
                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteNonQuery();
            }
        }
        
        public static void UtworzOgloszenie(Ogloszenie ogloszenie)
        {            
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();
                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT INTO ogloszenia VALUES(NULL, @KategoriaId, @FirmaId, @Tytul, @NazwaStanowiska, @PoziomStanowiska, @RodzajPracy, @WymiarZatrudnienia, @RodzajUmowy," +
                    "@NajmniejszeWynagrodzenie, @NajwiekszeWynagrodzenie, @DniPracy, @GodzinyPracy, @DataWaznosci, @Obowiazki, @Wymagania, @Benefity, @Informacje, @DataUtworzenia, @Zdjecie);";
                insertCommand.Parameters.AddWithValue("@KategoriaId", ogloszenie.KategoriaId);
                insertCommand.Parameters.AddWithValue("@FirmaId", ogloszenie.FirmaId);
                insertCommand.Parameters.AddWithValue("@Tytul", ogloszenie.Tytul);
                insertCommand.Parameters.AddWithValue("@NazwaStanowiska", ogloszenie.NazwaStanowiska);
                insertCommand.Parameters.AddWithValue("@PoziomStanowiska", ogloszenie.PoziomStanowiska);
                insertCommand.Parameters.AddWithValue("@RodzajPracy", ogloszenie.RodzajPracy);
                insertCommand.Parameters.AddWithValue("@WymiarZatrudnienia", ogloszenie.WymiarZatrudnienia);
                insertCommand.Parameters.AddWithValue("@RodzajUmowy", ogloszenie.RodzajUmowy);
                insertCommand.Parameters.AddWithValue("@NajmniejszeWynagrodzenie", ogloszenie.NajmniejszeWynagrodzenie);
                insertCommand.Parameters.AddWithValue("@NajwiekszeWynagrodzenie", ogloszenie.NajwiekszeWynagrodzenie);
                insertCommand.Parameters.AddWithValue("@DniPracy", ogloszenie.DniPracy);
                insertCommand.Parameters.AddWithValue("@GodzinyPracy", ogloszenie.GodzinyPracy);
                insertCommand.Parameters.AddWithValue("@DataWaznosci", ogloszenie.DataWaznosci);
                insertCommand.Parameters.AddWithValue("@Obowiazki", ogloszenie.Obowiazki);
                insertCommand.Parameters.AddWithValue("@Wymagania", ogloszenie.Wymagania);
                insertCommand.Parameters.AddWithValue("@Benefity", ogloszenie.Benefity);
                insertCommand.Parameters.AddWithValue("@Informacje", ogloszenie.Informacje);
                insertCommand.Parameters.AddWithValue("@DataUtworzenia", ogloszenie.DataUtworzenia);
                insertCommand.Parameters.AddWithValue("@Zdjecie", ogloszenie.Zdjecie);                
                insertCommand.ExecuteReader();
            }
        }

        public static void EdycjaOgloszenia(Ogloszenie ogloszenie)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();
                var updateCommand = new SqliteCommand();
                updateCommand.Connection = db;
                updateCommand.CommandText = "UPDATE ogloszenia SET kategoria_id=@KategoriaId, firma_id=@FirmaId, Tytul=@Tytul, nazwa_stanowiska=@NazwaStanowiska, poziom_stanowiska=@PoziomStanowiska ,rodzaj_pracy=@RodzajPracy,wymiar_zatrudnienia=@WymiarZatrudnienia,rodzaj_umowy=@RodzajUmowy" +
                    ",najmniejsze_wynagrodzenie=@NajmniejszeWynagrodzenie,najwieksze_wynagrodzenie=@NajwiekszeWynagrodzenie,dni_pracy=@DniPracy,godziny_pracy=@GodzinyPracy,data_waznosci=@DataWaznosci,obowiazki=@Obowiazki,wymagania=@Wymagania," +
                    "benefity=@Benefity,informacje=@Informacje, zdjecie=@Zdjecie  WHERE ogloszenie_id = @Id;";
                updateCommand.Parameters.AddWithValue("@Id", ogloszenie.OgloszenieId);
                updateCommand.Parameters.AddWithValue("@KategoriaId", ogloszenie.KategoriaId);
                updateCommand.Parameters.AddWithValue("@FirmaId", ogloszenie.FirmaId);
                updateCommand.Parameters.AddWithValue("@Tytul", ogloszenie.Tytul);
                updateCommand.Parameters.AddWithValue("@NazwaStanowiska", ogloszenie.NazwaStanowiska);
                updateCommand.Parameters.AddWithValue("@PoziomStanowiska", ogloszenie.PoziomStanowiska);
                updateCommand.Parameters.AddWithValue("@RodzajPracy", ogloszenie.RodzajPracy);
                updateCommand.Parameters.AddWithValue("@WymiarZatrudnienia", ogloszenie.WymiarZatrudnienia);
                updateCommand.Parameters.AddWithValue("@RodzajUmowy", ogloszenie.RodzajUmowy);
                updateCommand.Parameters.AddWithValue("@NajmniejszeWynagrodzenie", ogloszenie.NajmniejszeWynagrodzenie);
                updateCommand.Parameters.AddWithValue("@NajwiekszeWynagrodzenie", ogloszenie.NajwiekszeWynagrodzenie);
                updateCommand.Parameters.AddWithValue("@DniPracy", ogloszenie.DniPracy);
                updateCommand.Parameters.AddWithValue("@GodzinyPracy", ogloszenie.GodzinyPracy);
                updateCommand.Parameters.AddWithValue("@DataWaznosci", ogloszenie.DataWaznosci);
                updateCommand.Parameters.AddWithValue("@Obowiazki", ogloszenie.Obowiazki);
                updateCommand.Parameters.AddWithValue("@Wymagania", ogloszenie.Wymagania);
                updateCommand.Parameters.AddWithValue("@Benefity", ogloszenie.Benefity);
                updateCommand.Parameters.AddWithValue("@Informacje", ogloszenie.Informacje);                
                updateCommand.Parameters.AddWithValue("@Zdjecie", ogloszenie.Zdjecie);
                updateCommand.ExecuteReader();
            }
        }

        public static List<Ogloszenie> CzytajOgloszenia(int kategoriaId, int firmaId)
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM ogloszenia o INNER JOIN kategorie k ON o.kategoria_id = k.kategoria_id WHERE k.kategoria_id = @kategoria";
                selectCommand.Parameters.AddWithValue("@kategoria", kategoriaId);
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firma = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);  
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firma, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }

        public static List<Ogloszenie> CzytajOgloszeniaSzczegoly(int IdWybranego)
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM ogloszenia WHERE ogloszenie_id = @IdWybranego";
                selectCommand.Parameters.AddWithValue("@IdWybranego", IdWybranego);
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firma = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firma, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }
        public static List<Ogloszenie> CzytajWszystkieOgloszenia()
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM ogloszenia ORDER BY data_utworzenia DESC ";
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firmaId = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firmaId, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }

        public static List<Firma> CzytajFirmy()
        {
            List<Firma> firmy = new List<Firma>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM firmy;";

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nazwa = reader.GetString(1);

                        Firma fir = new Firma(id, nazwa);
                        firmy.Add(fir);
                    }
                }
            }
            return firmy;
        }

        public static List<Ogloszenie> CzytajWyszukaneOgloszeniaKategoria(int wybranaKategoria)
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;                
                selectCommand.CommandText = "SELECT * FROM ogloszenia WHERE kategoria_id = @wybranaKategoria ORDER BY data_utworzenia DESC;";
                
                selectCommand.Parameters.AddWithValue("@wybranaKategoria", wybranaKategoria);
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firmaId = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firmaId, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }
       
        public static List<Ogloszenie> CzytajWyszukaneOgloszenia(string szukana, int wybranaKategoria)
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM ogloszenia " +
                "WHERE (@szukana IS NULL OR tytul LIKE @szukana " +
                "OR nazwa_stanowiska LIKE @szukana " +
                "OR firma_id IN (SELECT firma_id FROM firmy WHERE nazwa LIKE @szukana)) " +
                "AND (@wybranaKategoria <= 0 OR kategoria_id = @wybranaKategoria) " +
                "AND ((@szukana IS NOT NULL AND @wybranaKategoria <= 0) " +
                "OR (tytul LIKE @szukana OR nazwa_stanowiska LIKE @szukana OR firma_id IN (SELECT firma_id FROM firmy WHERE nazwa LIKE @szukana) AND kategoria_id = @wybranaKategoria)) " +
                "ORDER BY data_utworzenia DESC";

                selectCommand.Parameters.AddWithValue("@szukana", szukana != null ? $"%{szukana}%" : (object)DBNull.Value);
                selectCommand.Parameters.AddWithValue("@wybranaKategoria", wybranaKategoria);
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firmaId = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firmaId, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }

        public static List<Ogloszenie> CzytajWyszukaneOgloszeniaSzukana(string szukana)
        {
            List<Ogloszenie> ogloszenia = new List<Ogloszenie>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM ogloszenia WHERE (@szukana IS NULL OR (tytul LIKE '%' || @szukana || '%' OR nazwa_stanowiska LIKE '%' || @szukana || '%' OR firma_id IN (SELECT firma_id FROM firmy WHERE nazwa LIKE '%' || @szukana || '%') OR instr(tytul, @szukana) > 0 OR instr(nazwa_stanowiska, @szukana) > 0 OR firma_id IN (SELECT firma_id FROM firmy WHERE instr(nazwa, @szukana) > 0))) ORDER BY CASE WHEN @szukana IS NOT NULL THEN 1 ELSE 0 END, data_utworzenia DESC;";

                selectCommand.Parameters.AddWithValue("@szukana", szukana != null ? $"%{szukana}%" : (object)DBNull.Value);                
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ogloszenie = reader.GetInt32(0);
                        int kategoria = reader.GetInt32(1);
                        int firmaId = reader.GetInt32(2);
                        string tytul = reader.GetString(3);
                        string nazwaStanowiska = reader.GetString(4);
                        string poziomStanowiska = reader.GetString(5);
                        string rodzajPracy = reader.GetString(6);
                        string wymiarZatrudnienia = reader.GetString(7);
                        string rodzajUmowy = reader.GetString(8);
                        decimal? najmniejszeWynagrodzenie = reader.GetDecimal(9);
                        decimal? najwiekszeWynagrodzenie = reader.GetDecimal(10);
                        string dniPracy = reader.GetString(11);
                        string godzinyPracy = reader.GetString(12);
                        DateTime dataWaznosci = reader.GetDateTime(13);
                        string obowiazki = reader.GetString(14);
                        string wymagania = reader.GetString(15);
                        string benefity = reader.GetString(16);
                        string informacje = reader.GetString(17);
                        DateTime dataUtworzenia = reader.GetDateTime(18);
                        string zdjecie = reader.GetString(19);

                        Ogloszenie ogl = new Ogloszenie(ogloszenie, kategoria, firmaId, tytul, nazwaStanowiska, poziomStanowiska, rodzajPracy, wymiarZatrudnienia, rodzajUmowy, najmniejszeWynagrodzenie, najwiekszeWynagrodzenie,
                            dniPracy, godzinyPracy, dataWaznosci, obowiazki, wymagania, benefity, informacje, dataUtworzenia, zdjecie);

                        ogloszenia.Add(ogl);
                    }
                }
            }

            return ogloszenia;
        }

        public static List<Kategoria> CzytajKategorie()
        {
            List<Kategoria> kategorie = new List<Kategoria>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM kategorie;";

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nazwa = reader.GetString(1);

                        Kategoria kat = new Kategoria(id, nazwa);
                        kategorie.Add(kat);
                    }
                }
            }

            return kategorie;
        }



    }    
}
