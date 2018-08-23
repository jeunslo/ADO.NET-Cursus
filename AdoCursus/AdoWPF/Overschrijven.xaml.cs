using AdoGemeenschap;
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
using System.Windows.Shapes;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for Overschrijven.xaml
    /// </summary>
    public partial class Overschrijven : Window
    {
        public Overschrijven()
        {
            InitializeComponent();
        }

        private void buttonOverschrijven_Click(object sender, RoutedEventArgs e)
        {
            Decimal bedrag;
            if (Decimal.TryParse(textBoxBedrag.Text, out bedrag))
            {
                try
                {
                    var manager = new RekeningManager();
                    manager.Overschrijven(bedrag, textBoxVanRekNr.Text, textBoxNaarRekNr.Text);
                    labelStatus.Content = "OK";
                }
                catch (Exception ex)
                {
                    labelStatus.Content = ex.Message;
                }
            }
            else
                labelStatus.Content = "bedrag bevat geen getal";
        }

        private void buttonSaldo_Click(object sender, RoutedEventArgs e)
        {
            var manager = new RekeningManager();
            try
            {
                labelStatus.Content = manager.SaldoRekeningRaadplegen(textBoxVanRekNr.Text).ToString("C");
            }
            catch(Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }

        private void buttonInfo_Click(object sender, RoutedEventArgs e)
        {
            var manager = new RekeningManager();
            try
            {
                var info = manager.RekeningInfoRaadplegen(textBoxVanRekNr.Text);
                labelStatus.Content = info.Klantnaam + " / " + info.Saldo.ToString("C");
            }
            catch(Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }
    }
}
