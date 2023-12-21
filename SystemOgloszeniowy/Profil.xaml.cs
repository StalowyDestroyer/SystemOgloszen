using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
using SystemOgloszeniowyWpf.Klasy;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Profil.xaml
    /// </summary>
    public partial class Profil : Window
    {
        private int admin = 0;
        private bool logged = false;
        private string usermn = "";
        public Profil(int adm, bool log, string user)
        {
            usermn = user;
            admin = adm;
            logged = log;

            InitializeComponent();
        }

        private void Powrot_klik(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(admin,logged,usermn);
            mainWindow.Show();
            this.Close();
        }
        private void Wyloguj_klik(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }


    }
}
