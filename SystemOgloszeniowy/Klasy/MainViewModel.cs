using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOgloszeniowyWpf.Klasy
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<Ogloszenie>? _ogloszenia;

        public List<Ogloszenie> Ogloszenia
        {
            get { return _ogloszenia; }
            set
            {
                if (_ogloszenia != value)
                {
                    _ogloszenia = value;
                    OnPropertyChanged(nameof(Ogloszenia));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
