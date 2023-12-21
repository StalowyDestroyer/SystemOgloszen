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
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using SystemOgloszeniowyWpf.Klasy;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Window
    {
        public Rejestracja()
        {
            InitializeComponent();
        }
        private void Zarejestruj_klik(object sender, RoutedEventArgs e)
        {
            string username = textbox_login.Text;
            string password = textbox_haslo.Password;
            string email = textbox_email.Text;
            bool administrator = false;

            Uzytkownik uzytkownik = new Uzytkownik(username, password, email, administrator);
            string patter = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Wprowadź login i hasło.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Regex.IsMatch(email, patter) == true)
            {
                if (CzyIstniejeEmail(email) == true)
                {
                    MessageBox.Show("Taki email już istnieje. Jeśli masz już konto proszę się zalogować", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (CzyIstniejeUzytkownik(username) == true)
                    {
                        MessageBox.Show("Taki użytkownik już istnieje", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (textbox_haslo.Password == textbox_powtorzHaslo.Password)
                        {
                            Baza.UtworzUzytkownika(uzytkownik);

                            MessageBox.Show("Rejestreacja przebiegła pomyślnie.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);

                            textbox_login.Text = string.Empty;
                            textbox_haslo.Password = string.Empty;
                            textbox_email.Text = string.Empty;

                            Logowanie logowanie = new Logowanie();
                            logowanie.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Hasła nie są identyczne.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę podać poprawny email", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Powrot_klik(object sender,EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool CzyIstniejeEmail(string email)
        {

            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemOgloszeniowy.db");
            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();


                var selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT COUNT() FROM uzytkownicy WHERE email = @Email;";
                selectCommand.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                return count > 0;
            }
        }

        private bool CzyIstniejeUzytkownik(string username)
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
    }
}
