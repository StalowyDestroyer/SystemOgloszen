using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy DodajOgloszenie.xaml
    /// </summary>
    public partial class DodajOgloszenie : Window
    {
        private List<Kategoria> kategorie;
        private List<Firma> firmy;
        private int admin = 0;
        private bool logged = false;
        private string usermn = "";

        public DodajOgloszenie()
        {
            InitializeComponent();

            kategorie = Baza.CzytajKategorie();
            KategoriaComboBox.ItemsSource = kategorie;

            firmy = Baza.CzytajFirmy();
            FirmaComboBox.ItemsSource = firmy;

        }

        private void Powrot_klik(object sender,EventArgs e)
        {
            MainWindow main = new MainWindow(admin, logged, usermn);
            main.Show();
            this.Close();
        }
        private void Dodaj_klik(object sender, RoutedEventArgs e)
        {
            var kategoria = KategoriaComboBox.SelectedItem as Kategoria;

            var firma = FirmaComboBox.SelectedItem as Firma;

            int Id = 0;

            int KategoriaId = kategoria.KategoriaId;

            int FirmaId = firma.FirmaId;

            string Tytul = TxBTytul.Text;

            string NazwaStanowiska = TxBNazwaStanowiska.Text;

            string PoziomStanowiska = TxBPoziomStanowiska.Text;

            string RodzajPracy = TxBRodzajPracy.Text;

            string WymiarZatrudnienia = TxBWymiarZatrudnienia.Text;

            string RodzajUmowy = TxBRodzajUmowy.Text;

            string NajmniejszeWynagrodzenieText = TxBNajmniejszeWynagrodzenie.Text;

            string NajwiekszeWynagrodzenieText = TxBNajwiekszeWynagrodzenie.Text;

            string DniPracy = TxBDniPracy.Text;

            string GodzinyPracy = TxBGodzinyPracy.Text;

            DateTime? DataWaznosci = TxBDataWaznosci.SelectedDate;

            string Obowiazki = TxBObowiazki.Text;

            string Wymagania = TxBWymagania.Text;

            string Benefity = TxBBenefity.Text;

            string Informacje = TxBInformacje.Text;

            DateTime DataUtworzenia = DateTime.Now;

            string Zdjecie = TxBZdjecie.Text;

            if (DataWaznosci == null)
            {
                MessageBox.Show("Podaj datę ważności.", "Product Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal NajmniejszeWynagrodzenie;
            decimal NajwiekszeWynagrodzenie;
            if (Regex.IsMatch(NajmniejszeWynagrodzenieText, @"^[-,0-9]+$"))
            {
                NajmniejszeWynagrodzenie = decimal.Parse(NajmniejszeWynagrodzenieText);
            }
            else
            {
                MessageBox.Show("Mozna wprowadzac wartosc dziesietne tylko po przecinku.", "Product Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Regex.IsMatch(NajwiekszeWynagrodzenieText, @"^[-,0-9]+$"))
            {
                NajwiekszeWynagrodzenie = decimal.Parse(NajwiekszeWynagrodzenieText);
            }
            else
            {
                MessageBox.Show("Mozna wprowadzac wartosc dziesietne tylko po przecinku.", "Product Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Ogloszenie ogloszenie = new Ogloszenie(Id, KategoriaId, FirmaId, Tytul, NazwaStanowiska, PoziomStanowiska, RodzajPracy, WymiarZatrudnienia, RodzajUmowy, NajmniejszeWynagrodzenie, NajwiekszeWynagrodzenie, DniPracy, GodzinyPracy, DataWaznosci, Obowiazki, Wymagania, Benefity, Informacje, DataUtworzenia, Zdjecie);

            Baza.UtworzOgloszenie(ogloszenie);

            TxBTytul.Text = string.Empty;

            TxBNazwaStanowiska.Text = string.Empty;

            TxBPoziomStanowiska.Text = string.Empty;

            TxBRodzajPracy.Text = string.Empty;

            TxBWymiarZatrudnienia.Text = string.Empty;

            TxBRodzajUmowy.Text = string.Empty;

            TxBNajmniejszeWynagrodzenie.Text = string.Empty;

            TxBNajwiekszeWynagrodzenie.Text = string.Empty;

            TxBDniPracy.Text = string.Empty;

            TxBGodzinyPracy.Text = string.Empty;

            TxBDataWaznosci.SelectedDate = DateTime.Now;

            TxBObowiazki.Text = string.Empty;

            TxBWymagania.Text = string.Empty;

            TxBBenefity.Text = string.Empty;

            TxBInformacje.Text = string.Empty;

            TxBZdjecie.Text = string.Empty;

            KategoriaComboBox.SelectedIndex = 1;
            FirmaComboBox.SelectedIndex = 1;
            MainWindow main = new MainWindow(admin, logged, usermn);
            main.Show();
            this.Close();

        }
    }
}
