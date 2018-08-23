using ConnectionManagerDb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfOpgave8
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

        public ObservableCollection<Plant> plantenOb = new ObservableCollection<Plant>();
        public List<Plant> gewijzigdePlantList = new List<Plant>();
        public CollectionViewSource plantViewSource;
        public int selectedIndex = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var manager = new PlantenManager();
            plantenOb = manager.GetPlantenList();
            var CBList = (plantenOb.OrderBy(x=>x.Soort).Select(x => x.Soort)).Distinct().ToList();
            CBList.Insert(0, "-alles-");
            soortCB.ItemsSource = CBList;
            soortCB.SelectedIndex = selectedIndex;
            plantViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("plantViewSource")));
            plantViewSource.Source = plantenOb;
            // Load data by setting the CollectionViewSource.Source property:
            // plantViewSource.Source = [generic data source]
        }

        private void soortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = soortCB.SelectedIndex;
            if (selectedIndex != 0)
                plantNaamLB.Items.Filter = new Predicate<object>(SoortFilter);
            else
                plantNaamLB.Items.Filter = null;
        }

        public bool SoortFilter(object plant)
        {
            Plant p = plant as Plant;
            bool result = (p.Soort == soortCB.SelectedValue.ToString());
            return result;
        }

        private void opslaanButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Gewijzigde planten van soort '" + soortCB.SelectedValue.ToString() + "' opslaan?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                List<Plant> nietGewijzigdePlanten = new List<Plant>();
                var manager = new PlantenManager();
                foreach (Plant p in plantenOb)
                {
                    if (p.Changed == true)
                        gewijzigdePlantList.Add(p);
                    p.Changed = false;
                }

                if (gewijzigdePlantList.Count != 0)
                {
                    nietGewijzigdePlanten = manager.SchrijfGewijzigdePlanten(gewijzigdePlantList);
                    if (nietGewijzigdePlanten.Count > 0)
                    {
                        MessageBox.Show("Niet gewijzigd", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Gewijzigd", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                gewijzigdePlantList.Clear();
            }
        }

        private void soortCB_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            foreach (Plant p in plantenOb)
            {
                if (p.Changed == true)
                {
                    MessageBox.Show("Slaag eerst op voordat u van soort verandert", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void testOpFouten_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            bool foutGevonden = false;
            foreach (var c in grid1.Children)
            {
                if (c is AdornerDecorator)
                {
                    if (Validation.GetHasError(((AdornerDecorator)c).Child))
                        foutGevonden = true;
                }
                else
                {
                    if (Validation.GetHasError((DependencyObject)c))
                        foutGevonden = true;
                }
                if (foutGevonden)
                    e.Handled = true;
            }
        }
    }
}
