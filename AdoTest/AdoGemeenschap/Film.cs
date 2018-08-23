using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;

namespace AdoGemeenschap
{
    public class Film : INotifyPropertyChanged
    {
        public Film(Int32 bandNr, string titel, string genre, Int32 inVoorraad, Int32 uitVoorraad, decimal prijs, Int32 totaalVerhuurd)
        {
            BandNr = bandNr;
            Titel = titel;
            Genre = genre;
            InVoorraad = inVoorraad;
            UitVoorraad = uitVoorraad;
            Prijs = prijs;
            TotaalVerhuurd = totaalVerhuurd;
            Changed = false;
        }

        public bool Changed { get; set; }

        public Int32 BandNr { get; set; }

        private string titelValue;

        public string Titel
        {
            get { return titelValue; }
            set { titelValue = value;
                RaisePropertyChanged("Titel");
            }
        }

        private string genreValue;

        public string Genre
        {
            get { return genreValue; }
            set { genreValue = value;
                RaisePropertyChanged("Genre");
            }
        }

        private Int32 inVoorraadValue;

        public Int32 InVoorraad
        {
            get { return inVoorraadValue; }
            set { inVoorraadValue = value;
                Changed = true;
                RaisePropertyChanged("InVoorraad");
            }
        }

        private Int32 uitVoorraadValue;

        public Int32 UitVoorraad
        {
            get { return uitVoorraadValue; }
            set { uitVoorraadValue = value;
                Changed = true;
                RaisePropertyChanged("UitVoorraad");
            }
        }

        private decimal prijsValue;

        public decimal Prijs
        {
            get { return prijsValue; }
            set { prijsValue = value; }
        }

        private Int32 totaalVerhuurdValue;

        public Int32 TotaalVerhuurd
        {
            get { return totaalVerhuurdValue; }
            set { totaalVerhuurdValue = value;
                Changed = true;
                RaisePropertyChanged("TotaalVerhuurd");
            }
        }

        private void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
