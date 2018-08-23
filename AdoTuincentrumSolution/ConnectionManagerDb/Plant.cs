using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionManagerDb
{
    public class Plant
    {
        public Plant(Int32 plantNr, string plantNaam, string soort, Int32 levNr, string kleur, decimal verkoopPrijs)
        {
            plantNrValue = plantNr;
            PlantNaam = plantNaam;
            Soort = soort;
            LevNrValue = levNr;
            Kleur = kleur;
            VerkoopPrijs = verkoopPrijs;
            Changed = false;
        }

        public bool Changed { get; set; }

        private Int32 plantNrValue;
        public Int32 PlantNr
        {
            get { return plantNrValue; }
        }

        public string PlantNaam { get; set; }

        public string Soort { get; set; }


        private Int32 LevNrValue;
        public Int32 LevNr
        {
            get { return LevNrValue; }
        }

        private string kleurValue;

        public string Kleur
        {
            get { return kleurValue; }
            set { kleurValue = value;
                Changed = true;
            }
        }

        private Decimal verkoopPrijsValue;

        public Decimal VerkoopPrijs
        {
            get { return verkoopPrijsValue; }
            set { verkoopPrijsValue = value;
                Changed = true;
            }
        }

    }
}
