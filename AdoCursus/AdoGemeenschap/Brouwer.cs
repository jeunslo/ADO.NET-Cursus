using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class Brouwer
    {
        public Brouwer()
        {

        }

        public Brouwer(Int32 brNr, string brNaam, string adres, Int16 postcode, string gemeente, Int32? omzet)
        {
            brouwersNrValue = brNr;
            Naam = brNaam;
            Adres = adres;
            Postcode = postcode;
            Gemeente = gemeente;
            Omzet = omzet;
            Changed = false;
        }


        private Int32 brouwersNrValue;

        public Int32 BrouwerNr
        {
            get { return brouwersNrValue; }
        }
        public bool Changed { get; set; }

        private String naamValue;
        public String Naam
        {
            get { return naamValue; }
            set { naamValue = value;
                Changed = true;
            }
        }

        private String adresValue;
        public String Adres
        {
            get { return adresValue; }
            set
            {
                adresValue = value;
                Changed = true;
            }
        }

        private Int16 postcodeValue;
        public Int16 Postcode
        {
            get { return postcodeValue; }
            set
            {
                postcodeValue = value;
                Changed = true;
            }
        }

        private String gemeenteValue;
        public String Gemeente
        {
            get { return gemeenteValue; }
            set
            {
                gemeenteValue = value;
                Changed = true;
            }
        }

        private Int32? omzetValue { get; set; }

        
        public Int32? Omzet
        {
            get { return omzetValue; }
            set
            {
                if (value.HasValue & Convert.ToInt32(value) < 0)
                    throw new Exception("Omzet moet positief zijn");
                else
                {
                    omzetValue = value;
                    Changed = true;
                }
            }
        }
    }
}
