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
    /// Logika interakcji dla klasy Profil.xaml
    /// </summary>
    public partial class Profil : Window
    {
        public Profil()
        {
            InitializeComponent();
        }

        private void Powrot_klik(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Wyloguj_klik(object sender, RoutedEventArgs e)
        {
            //wyloguj
            this.Close();
        }
    }
}
