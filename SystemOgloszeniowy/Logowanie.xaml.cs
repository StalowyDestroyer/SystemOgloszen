using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        public Logowanie()
        {
            InitializeComponent();

        }

        void Zarejestruj_klik(object sender,EventArgs e)
        {
            this.Close();
            Rejestracja rejestracja = new Rejestracja();
            rejestracja.ShowDialog();
        }



        private void Rejestracja(object sender, RoutedEventArgs e)
        {

            Rejestracja rejestracja = new Rejestracja();
            rejestracja.Show();
            this.Close();
        }

        private void Zaloguj_klik(object sender, RoutedEventArgs e)
        {
            string username = textbox_login.Text;
            string password = textbox_haslo.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a username and password.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (IsUsernameExistsInDatabase(username) == true)
            {
                if (IsPasswordCorrect(password) == true)
                {
                    var log = true;
                    var adm = 0;
                    string user = username;

                    if (IsAdmin(username) == true)
                    {

                        adm = 1;
                        MainWindow mainwindow = new MainWindow(adm, log, user);
                        mainwindow.Show();
                        this.Close();
                    }
                    else
                    {

                        MainWindow mainwindow = new MainWindow(adm, log, user);
                        mainwindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Podano niepoprawne haslo", "Loginb Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Podano niepoprawny login", "Loginb Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        private bool IsUsernameExistsInDatabase(string username)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");
            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT COUNT() FROM uzytkownicy WHERE nick = @Username;";
                selectCommand.Parameters.AddWithValue("@Username", username);

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                return count > 0;
            }
        }


        private bool IsPasswordCorrect(string password)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");
            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT COUNT() FROM uzytkownicy WHERE haslo = @Password;";
                selectCommand.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                return count > 0;
            }
        }


        private bool IsAdmin(string username)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");
            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();


                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT administrator FROM uzytkownicy WHERE Nick=@Nick";
                selectCommand.Parameters.AddWithValue("@Nick", username);



                int admin = Convert.ToInt32(selectCommand.ExecuteScalar());

                return admin > 0;
            }
        }


        private void StronaGlowna(object sender, RoutedEventArgs e)
        {
            int adm = 0;
            bool log = false;
            string user = "";
            MainWindow mainwindow = new MainWindow(adm, log, user);
            mainwindow.Show();
            this.Close();
        }
    }
}

