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
        void Zaloguj_klik(object sender,EventArgs e)
        {
            string username = textbox_haslo.Text; 
            string password = textbox_haslo.Text;
            Login(username, password);
        }
        public bool Login(string username, string password)
        {
            //logowanie
            return false;
        }
        void Zarejestruj_klik(object sender,EventArgs e)
        {
            this.Close();
            Rejestracja rejestracja = new Rejestracja();
            rejestracja.ShowDialog();
        }
    }
}
