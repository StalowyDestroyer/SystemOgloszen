using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOgloszeniowyWpf.Klasy
{
    public class MainViewProfile : INotifyPropertyChanged
    {

        private List<Profil> _profile;


        public List<Profil> Profile
        {
            get { return _profile; }
            set
            {
                if (_profile != value)
                {
                    _profile = value;
                    OnPropertyChanged(nameof(Profile));
                }
            }
        }

        public MainViewProfile(string usermn)
        {
            Profile = Baza.CzytajProfil(usermn);
        }

        // Implementacja interfejsu INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
