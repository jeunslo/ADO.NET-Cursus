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
using ConnectionManagerDb;

namespace WpfOpgave5
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var manager = new TuincentrumManager();
            try
            {
                prijsLabel.Content = "Gemiddelde prijs: " + manager.BerekenGemiddeldePrijs(textBoxSoort.Text).ToString("C");
            }
            catch(Exception ex)
            {
                prijsLabel.Content = ex.Message;
            }
        }
    }
}
