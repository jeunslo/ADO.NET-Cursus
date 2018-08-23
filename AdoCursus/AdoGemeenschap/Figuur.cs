using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class Figuur
    {
        public Int32 ID { get; set; }
        private String naamValue;

        public String Naam
        {
            get { return naamValue; }
            set { naamValue = value;
                Changed = true; }
        }

        public Object Versie { get; set; }
        public bool Changed { get; set; }
        public Figuur(Int32 id, string naam, Object versie)
        {
            ID = id;
            Naam = naam;
            Versie = versie;
            Changed = false;
        }

        public Figuur() { }
    }
}
