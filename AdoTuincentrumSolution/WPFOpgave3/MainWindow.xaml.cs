using ConnectionManagerDb;
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

namespace WPFOpgave3
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

        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuincentrumManager();
                if (manager.InsertLeverancier(TextBoxNaam.Text, TextBoxAdres.Text, TextBoxPostcode.Text, TextBoxPlaats.Text))
                {
                    LabelContent.Content = "OK";
                }
                else
                {
                    LabelContent.Content = "leveranciersDb bestaat niet";
                }
            }
            catch (Exception ex)
            {
                LabelContent.Content = "Voer alle velden in";
            }
        }

        public static int teller = 1;
        private void buttonKorting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuincentrumManager();
                LabelContent.Content = manager.KortingOpPlantPrijzen(0.25m) + " prijzen gewijzigt, " + teller + " keer geclickt";
                teller++;
            }
            catch (Exception ex)
            {
                LabelContent.Content = ex.Message;
            }
        }

        private void VervangLeverancierButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuincentrumManager();
                manager.VervangLeverancier(20, 5);
                LabelContent.Content = "Leveranciers gewijzigd";
            }
            catch (Exception ex)
            {
                LabelContent.Content = ex.Message;
            }
        }
    }
}
