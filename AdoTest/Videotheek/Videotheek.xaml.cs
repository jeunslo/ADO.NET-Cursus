using AdoGemeenschap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
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

namespace Videotheek
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

        public ObservableCollection<Film> filmOb = new ObservableCollection<Film>();
        public List<Film> toegvoegdeFilmsList = new List<Film>();
        public List<Film> verwijderdeFilmsList = new List<Film>();
        public List<Film> gewijzigdeFilmsList = new List<Film>();
        public CollectionViewSource filmViewSource;
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var manager = new VideotheekManager();
           
            filmOb = manager.GetFilmsList();

            List<String> genreList = manager.getGenreList();
            //ComboBox opvullen met genreNamen
            genreList.Insert(0, "");
            GenreCB.ItemsSource = genreList;

            filmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("filmViewSource")));
            filmViewSource.Source = filmOb;
            FilmLB.SelectedIndex = 0;

            filmOb.CollectionChanged += this.OnCollectionChanged;
            // Load data by setting the CollectionViewSource.Source property:
            // filmViewSource.Source = [generic data source]
        }

        //****************zelfgemaakte methods***********************************

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(Film newFilm in e.NewItems)
                {
                    toegvoegdeFilmsList.Add(newFilm);
                }
            }

            if(e.OldItems != null)
            {
                foreach (Film oldFilm in e.OldItems)
                {
                    if (oldFilm.BandNr != 0)
                        verwijderdeFilmsList.Add(oldFilm);
                    else
                        toegvoegdeFilmsList.Remove(oldFilm);
                }
            }
        }

        private void availabilityMethod()
        {
            BevestigenButton.Visibility = BevestigenButton.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            AnnulerenButton.Visibility = AnnulerenButton.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            OpslaanButton.IsEnabled = OpslaanButton.IsEnabled == true ? false : true;
            VerhuurButton.IsEnabled = VerhuurButton.IsEnabled == true ? false : true;
            FilmLB.IsEnabled = FilmLB.IsEnabled == true ? false : true;

            //grid1 children
            titelTextBox.IsReadOnly = titelTextBox.IsReadOnly == true ? false : true;
            GenreCB.IsEnabled = GenreCB.IsEnabled == true ? false : true;
            inVoorraadTextBox.IsReadOnly = inVoorraadTextBox.IsReadOnly == true ? false : true;
            uitVoorraadTextBox.IsReadOnly = uitVoorraadTextBox.IsReadOnly == true ? false : true;
            totaalVerhuurdTextBox.IsReadOnly = totaalVerhuurdTextBox.IsReadOnly == true ? false : true;
            prijsTextBox.IsReadOnly = prijsTextBox.IsReadOnly == true ? false : true;         
        }


        //************************Button click events********************************
        private void ToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            availabilityMethod();
            
            //nieuw object aanmaken dat gewijzigd mag worden
            Film resetFilm = new Film(0, "", "", 0, 0, 0m, 0);
            filmOb.Add(resetFilm);
            FilmLB.SelectedIndex = filmOb.IndexOf(resetFilm);
        }

        private void AnnulerenButton_Click(object sender, RoutedEventArgs e)
        {
            availabilityMethod();
            FilmLB.SelectedIndex = 0;
            filmOb.RemoveAt(filmOb.Count() - 1);
        }

        private void BevestigenButton_Click(object sender, RoutedEventArgs e)
        {
            //forceer validatie op textbox waar nodig
            titelTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            inVoorraadTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            prijsTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            GenreCB.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            //checken naar errors
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
            }
            
            if (foutGevonden)
            {
                MessageBox.Show("U heeft ergens een fout gemaakt!", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                availabilityMethod();
                FilmLB.SelectedIndex = 0;
            }
        }

        private void VerwijderenButton_Click(object sender, RoutedEventArgs e)
        {
            if(FilmLB.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker dat je deze film wil verwijderen?", "Verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    filmOb.RemoveAt(FilmLB.SelectedIndex);
                }
            }
        }

        private void OpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wilt u alles wegschrijven naar de database?", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                foreach(Film f in filmOb)
                {
                    if ((f.Changed == true) && (f.BandNr != 0))
                    {
                        gewijzigdeFilmsList.Add(f);
                        f.Changed = false;
                    }
                }

                var manager = new VideotheekManager();

                StringBuilder message = new StringBuilder();
                if (toegvoegdeFilmsList.Count != 0)
                {
                    if ((manager.SchrijfToevoegingen(toegvoegdeFilmsList)).Count == 0)
                    {
                        message.Append("Toevoeging ok \n");
                        toegvoegdeFilmsList.Clear();
                    }
                    else
                        message.Append("Toevoeging niet ok \n");
                }
                if (verwijderdeFilmsList.Count != 0)
                {
                    if ((manager.SchrijfVerwijderingen(verwijderdeFilmsList)).Count == 0)
                    {
                        message.Append("Verwijdering ok \n");
                        verwijderdeFilmsList.Clear();
                    }
                    else
                        message.Append("Verwijdering niet ok \n");
                }
                if (gewijzigdeFilmsList.Count != 0)
                {
                    if ((manager.SchrijfWijzigingen(gewijzigdeFilmsList)).Count == 0)
                    {
                        message.Append("Wijziging ok");
                        gewijzigdeFilmsList.Clear();
                    }
                    else
                        message.Append("Wijziging niet ok");
                }
                if (!string.IsNullOrEmpty(message.ToString()))
                    MessageBox.Show(message.ToString(), "Info", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                else
                    MessageBox.Show("Niets aangepast", "Info", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

            }
        }

        private void VerhuurButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilmLB.SelectedItem != null)
            {
                Film selectedFilm = FilmLB.SelectedItem as Film;
                if (selectedFilm.InVoorraad <= 0)
                    MessageBox.Show("Alle films zijn verhuurd!", "Verhuur", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                else
                {                 
                    selectedFilm.InVoorraad -= 1;
                    selectedFilm.UitVoorraad += 1;
                    selectedFilm.TotaalVerhuurd += 1;
                }
            }
        }

    }
}
