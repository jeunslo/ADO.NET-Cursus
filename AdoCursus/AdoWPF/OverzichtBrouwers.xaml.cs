using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using AdoGemeenschap;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for OverzichtBrouwers.xaml
    /// </summary>
    public partial class OverzichtBrouwers : Window
    {
        private CollectionViewSource brouwerViewSource;
        public OverzichtBrouwers()
        {
            InitializeComponent();
        }

        public ObservableCollection<Brouwer> brouwersOb = new ObservableCollection<Brouwer>();
        public List<Brouwer> oudeBrouwers = new List<Brouwer>();
        public List<Brouwer> nieuweBrouwers = new List<Brouwer>();
        public List<Brouwer> GewijzigdeBrouwers = new List<Brouwer>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            VulDeGrid();
            textBoxZoeken.Focus();
            var nummers = (from b in brouwersOb orderby b.Postcode select b.Postcode.ToString()).Distinct().ToList();
            nummers.Insert(0, "alles");
            comboBoxPostcode.ItemsSource = nummers;
            comboBoxPostcode.SelectedIndex = 0;
        }


        private void VulDeGrid()
        {
            brouwerViewSource = ((CollectionViewSource)(this.FindResource("brouwerViewSource")));
            var manager = new BrouwerManager();
            brouwersOb = manager.GetBrouwersBeginNaam(textBoxZoeken.Text);
            brouwerViewSource.Source = brouwersOb;
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count;
            brouwersOb.CollectionChanged += this.OnCollectionChanged;
            goUpdate();
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Brouwer oudeBrouwer in e.OldItems)
                    oudeBrouwers.Add(oudeBrouwer);
            }
            if (e.NewItems != null)
            {
                foreach (Brouwer nieuweBrouwer in e.NewItems)
                {
                    nieuweBrouwers.Add(nieuweBrouwer);
                }
            }
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count;
        }

        private void buttonZoeken_Click(object sender, RoutedEventArgs e)
        {
            VulDeGrid();
        }

        private void textBoxZoeken_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                VulDeGrid();
        }


        private void goToFirstButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToFirst();
            goUpdate();
        }

        private void goToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToPrevious();
            goUpdate();
        }

        private void goToNextButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToNext();
            goUpdate();
        }

        private void goToLastButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToLast();
            goUpdate();
        }

        private void goUpdate()
        {
            goToPreviousButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == 0);
            goToNextButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == brouwerDataGrid.Items.Count - 2);
            goToFirstButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == 0);
            goToLastButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == brouwerDataGrid.Items.Count - 2);

            if (brouwerDataGrid.Items.Count != 0)
            {
                if (brouwerDataGrid.SelectedItem != null)
                {
                    brouwerDataGrid.ScrollIntoView(brouwerDataGrid.SelectedItem);
                    listBoxBrouwers.ScrollIntoView(brouwerDataGrid.SelectedItem);
                }

            }
            textBoxGo.Text = (brouwerViewSource.View.CurrentPosition + 1).ToString();
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            int position;
            int.TryParse(textBoxGo.Text, out position);
            if (position > 0 && position <= brouwerDataGrid.Items.Count)
                brouwerViewSource.View.MoveCurrentToPosition(position - 1);
            else
                MessageBox.Show("The input value is not valid.");
            goUpdate();
        }

        private void brouwerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            goUpdate();
        }

        private void checkBoxPostcode0_Click(object sender, RoutedEventArgs e)
        {
            Binding binding1 = BindingOperations.GetBinding(postcodeTextBox, TextBox.TextProperty);
            binding1.ValidationRules.Clear();

            var binding2 = (postcodeColumn as DataGridBoundColumn).Binding as Binding;
            binding2.ValidationRules.Clear();

            brouwerDataGrid.RowValidationRules.Clear();

            //PostcodeRangeRule0 rule0 = new PostcodeRangeRule0();
            //rule0.ValidationStep = ValidationStep.UpdatedValue;

            PostcodeRangeRule0 rule01 = new PostcodeRangeRule0();
            //rule01.ValidationStep = ValidationStep.UpdatedValue;

            //PostcodeRangeRule rule = new PostcodeRangeRule();
            //rule.ValidationStep = ValidationStep.UpdatedValue;

            //PostcodeRangeRule rule1 = new PostcodeRangeRule();
            //rule1.ValidationStep = ValidationStep.UpdatedValue;

            switch (checkBoxPostcode0.IsChecked)
            {
                case true:
                    binding1.ValidationRules.Add(new PostcodeRangeRule0());
                    binding2.ValidationRules.Add(rule01);
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule0());
                    break;
                case false:
                    binding1.ValidationRules.Add(new PostcodeRangeRule());
                    binding2.ValidationRules.Add(new PostcodeRangeRule());
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule());
                    break;
                default:
                    binding1.ValidationRules.Add(new PostcodeRangeRule());
                    binding2.ValidationRules.Add(new PostcodeRangeRule());
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule());
                    break;
            }
        }

        private void testOpFouten_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            bool foutGevonden = false;
            foreach (var c in gridDetail.Children)
            {
                if (c is AdornerDecorator)
                {
                    if (Validation.GetHasError(((AdornerDecorator)c).Child))
                        foutGevonden = true;
                }
                if (Validation.GetHasError((DependencyObject)c))
                    foutGevonden = true;
            }

            foreach (var c in brouwerDataGrid.ItemsSource)
            {
                var d = brouwerDataGrid.ItemContainerGenerator.ContainerFromItem(c);
                if (d is DataGridRow)
                {
                    if (Validation.GetHasError(d))
                        foutGevonden = true;
                }
            }
            if (foutGevonden)
                e.Handled = true;
        }

        private void comboBoxPostcode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPostcode.SelectedIndex == 0)
                brouwerDataGrid.Items.Filter = null;
            else
                brouwerDataGrid.Items.Filter = new Predicate<object>(PostCodeFilter);
            goUpdate();
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count - 1;
        }

        public bool PostCodeFilter(object br)
        {
            Brouwer b = br as Brouwer;
            bool result = (b.Postcode == Convert.ToInt16(comboBoxPostcode.SelectedValue));
            return result;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            brouwerDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            CheckWhatup(oudeBrouwers);

            CheckWhatup(nieuweBrouwers);

            foreach (Brouwer b in brouwersOb)
            {
                if ((b.Changed == true) && (b.BrouwerNr != 0))
                    GewijzigdeBrouwers.Add(b);
                b.Changed = false;
            }
            CheckWhatup(GewijzigdeBrouwers);

            VulDeGrid();

            oudeBrouwers.Clear();
            nieuweBrouwers.Clear();
            GewijzigdeBrouwers.Clear();
        }

        public void CheckWhatup(List<Brouwer> List)
        {
            List<Brouwer> resultaatBrouwers = new List<Brouwer>();
            var manager = new BrouwerManager();
            string tekst = "";
            if (List == oudeBrouwers)
            {
                if (List.Count() != 0)
                    resultaatBrouwers = manager.SchrijfVerwijderingen(List);
                tekst = "verwijderd";
            }
            if (List == nieuweBrouwers)
            {
                if (List.Count() != 0)
                    resultaatBrouwers = manager.SchrijfToevoegingen(List);
                tekst = "toegevoegd";
            }
            if (List == GewijzigdeBrouwers)
            {
                if (List.Count() != 0)
                    resultaatBrouwers = manager.SchrijfWijzigingen(List);
                tekst = "gewijzigd";
            }
            if (resultaatBrouwers.Count > 0)
            {
                StringBuilder boodschap = new StringBuilder();
                boodschap.Append("Niet " + tekst + ": \n");
                foreach (var b in resultaatBrouwers)
                {
                    boodschap.Append("Nummer: " + b.BrouwerNr + " : " + b.Naam + " niet\n");
                }
                MessageBox.Show(boodschap.ToString());
            }
            MessageBox.Show(List.Count - resultaatBrouwers.Count + " brouwer(s) "+tekst+" aan de database", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            resultaatBrouwers.Clear();
        }

    }
}
