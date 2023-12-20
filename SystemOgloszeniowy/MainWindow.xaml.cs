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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool UzytkownikZalogowany=false;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void label_profil_MouseEnter(object sender, MouseEventArgs e)
        {
            label_profil.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
        }
        private void label_profil_MouseLeave(object sender, MouseEventArgs e)
        {
            label_profil.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        }
        private void Profil_klik(object sender,EventArgs e)
        {
            if (UzytkownikZalogowany== false)
            {
                Logowanie logowanie = new Logowanie();
                logowanie.ShowDialog();
            }
            else
            {
                Profil profil = new Profil();
                profil.ShowDialog();
            }
        }
    }
}
