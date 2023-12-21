using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemOgloszeniowy;
using SystemOgloszeniowyWpf.Klasy;

namespace SystemOgloszeniowy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Kategoria> kategorie;



        private int admin = 0;
        private bool logged = false;
        private string usermn = "";


        public MainWindow()
        {
            InitializeComponent();

            Baza.TabelaUzytkownikow();
            Baza.TabelaOgloszenia();

            MainViewModel viewModel = new MainViewModel();
            DataContext = viewModel;

            kategorie = Baza.CzytajKategorie();

            Kategoria specjalnaKategoria = new Kategoria { KategoriaId = 0, KategoriaNazwa = "Wybierz kategorie:" };
            kategorie.Insert(0, specjalnaKategoria);


            // Wczytaj ogłoszenia
            viewModel.Ogloszenia = Baza.CzytajWszystkieOgloszenia();


        }

        public MainWindow(int adm, bool log, string user)
        {
            InitializeComponent();

            usermn = user;
            admin = adm;
            logged = log;


            if (logged == true)
            {
                uzytkownik.Content = user;
            }
            else
            {
                uzytkownik.Content = "PROFIL";
            }

            Baza.TabelaUzytkownikow();
            Baza.TabelaOgloszenia();

            kategorie = Baza.CzytajKategorie();
            Kategoria specjalnaKategoria = new Kategoria { KategoriaId = 0, KategoriaNazwa = "Wybierz kategorie:" };
            kategorie.Insert(0, specjalnaKategoria);


            MainViewModel viewModel = new MainViewModel();
            DataContext = viewModel;

            // Wczytaj ogłoszenia
            viewModel.Ogloszenia = Baza.CzytajWszystkieOgloszenia();
        }


        private void label_profil_MouseEnter(object sender, MouseEventArgs e)
        {
            uzytkownik.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
        }
        private void label_profil_MouseLeave(object sender, MouseEventArgs e)
        {
            uzytkownik.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        }
        private void label_ogloszenie_MouseEnter(object sender, MouseEventArgs e)
        {
            ogloszenie.Foreground = new SolidColorBrush(Colors.LightSkyBlue);
        }
        private void label_ogloszenie_MouseLeave(object sender, MouseEventArgs e)
        {
            ogloszenie.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        }
        private void Profil_klik(object sender, EventArgs e)
        {
            if (logged == false)
            {
                Logowanie logowanie = new Logowanie();
                logowanie.ShowDialog();
                this.Close();
            }
            else
            {
                Profil profil = new Profil(admin,logged,usermn);
                profil.ShowDialog();
                this.Close();
            }
        }
        private void Dodaj(object sender, RoutedEventArgs e)
        {
            if (logged == true)
            {
                DodajOgloszenie dodajogloszenie = new DodajOgloszenie();
                dodajogloszenie.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Najpierw musisz się zalogować.", "Bląd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}



