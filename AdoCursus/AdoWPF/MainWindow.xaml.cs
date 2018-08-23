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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using AdoGemeenschap;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonBieren_Click(object sender, RoutedEventArgs e)
        { 
            try
            {   
                var manager = new BierenDbManager();
                using (var conBieren = manager.GetConnection())
                {
                    conBieren.Open();
                    labelStatus.Content = "Bieren geopend";
                }
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }

        private void buttonBonus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new RekeningManager();
                labelStatus.Content = manager.SaldoBonus() + " rekeningen aangepast";
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }

        private void buttonStorten_Click(object sender, RoutedEventArgs e)
        {
            Decimal teStorten;
            if (decimal.TryParse(textBoxTeStorten.Text, out teStorten))
            {
                try
                {
                    var manager = new RekeningManager();
                    if (manager.Storten(teStorten, textBoxRekeningNr.Text))
                        labelStatus.Content = "OK";
                    else
                        labelStatus.Content = "Rekening niet gevonden";
                }
                catch (Exception ex)
                {
                    labelStatus.Content = ex.Message;
                }
            }
            else
                labelStatus.Content = "Tik een getal bij het storten";
        }
    }
}
