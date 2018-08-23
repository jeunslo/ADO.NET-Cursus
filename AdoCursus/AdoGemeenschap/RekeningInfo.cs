using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class RekeningInfo
    {
        private Decimal saldoValue;

        public Decimal Saldo
        {
            get { return saldoValue; }
            set { saldoValue = value; }
        }

        private String klantNaamValue;

        public String Klantnaam
        {
            get { return klantNaamValue; }
            set { klantNaamValue = value; }
        }

        public RekeningInfo(Decimal saldo, string klantNaam)
        {
            saldoValue = saldo;
            klantNaamValue = klantNaam;
        }
    }
}
