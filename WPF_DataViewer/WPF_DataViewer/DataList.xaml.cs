using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
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
using WPF_DataViewer.DAL;
using WPF_DataViewer.Models;

namespace WPF_DataViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Character> _characters;
        private IDataService _dataService;

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        private bool _changesMade;
        public bool ChangesMade
        {
            get { return _changesMade; }
            set
            {
                _changesMade = value;
                if (_changesMade)
                {
                    butt_Save.Background = Brushes.DarkGreen;
                } else {
                    butt_Save.Background = Brushes.LightGray;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ChangesMade = false;

            //set data source
            //
            try
            {
                _dataService = new MongoDataService();
                _characters = _dataService.ReadAll();

                dg_CharacterGrid.ItemsSource = _characters;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

            cb_Filter.Items.Add("All");
            //set combo boxes
            //
            foreach (PropertyInfo prop in typeof(Character).GetProperties())
            {
                cb_Filter.Items.Add(prop.Name);
                cb_Sort.Items.Add(prop.Name);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.WriteAll(_characters);
                ChangesMade = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            if (IsWindowOpen<Help>())
            {
                foreach (Window window in App.Current.Windows.OfType<Help>())
                {
                    window.Close();
                }
            }
            Help help = new Help();
            help.Visibility = Visibility.Visible;
        }

        private void OpenDetails(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsWindowOpen<Details>())
                {
                    foreach (Window window in App.Current.Windows.OfType<Details>())
                    {
                        window.Close();
                    }
                }
                Details detail = new Details((Character)dg_CharacterGrid.SelectedItem);
                detail.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a a full row.","Error");
            }
        }

        private void DeleteRecord(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = dg_CharacterGrid.SelectedIndex;
                
                _characters.RemoveAt(index);
                dg_CharacterGrid.Items.Refresh();
                ChangesMade = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please select a row to delete.\n{ex}","Error");
            }
            
        }

        private void SetFilter(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataGridColumn column in dg_CharacterGrid.Columns)
            {
                if (cb_Filter.SelectedValue.ToString() == "All")
                {
                    dg_CharacterGrid.Columns[column.DisplayIndex].Visibility = Visibility.Visible;
                }
                else if (column.Header.ToString() == cb_Filter.SelectedValue.ToString())
                {
                    dg_CharacterGrid.Columns[column.DisplayIndex].Visibility = Visibility.Visible;
                }
                else
                {
                    if (column.Header.ToString() == "_id")
                    {
                        dg_CharacterGrid.Columns[column.DisplayIndex].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        dg_CharacterGrid.Columns[column.DisplayIndex].Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void SearchValue(object sender, TextChangedEventArgs e)
        {
            dg_CharacterGrid.SelectedCells.Clear();

            dg_CharacterGrid.SelectedItem = FindCharcater();
        }

        private Character FindCharcater()
        {
            foreach (Character c in _characters)
            {
                foreach (var prop in _characters.First().GetType().GetProperties())
                {
                    if (prop.GetValue(c) != null)
                    {
                        if (tb_Search.Text.ToLower() == prop.GetValue(c).ToString().ToLower())
                        {
                            return c;
                        }
                    }
                }
            }
            return new Character();
        }

        private void CellChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            ChangesMade = true;
        }

        private void ChangeSortOrder(object sender, SelectionChangedEventArgs e)
        {
            SetSort();
        }

        private void SetSort()
        {
            try
            {
                dg_CharacterGrid.Items.SortDescriptions.Clear();
                dg_CharacterGrid.Items.SortDescriptions.Add(new SortDescription(cb_Sort.SelectedValue.ToString(), GetDirection()));
                dg_CharacterGrid.Items.Refresh();
            }
            catch (Exception)
            {

            }
        }

        private ListSortDirection GetDirection()
        {
            ListSortDirection direction = ListSortDirection.Ascending;

            if ((bool)rb_Desc.IsChecked)
            {
                direction = ListSortDirection.Descending;
            }

            return direction;
        }

        private void DirectionChanged(object sender, RoutedEventArgs e)
        {
            SetSort();
        }

        private void EnterSearch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OpenDetails(null,null);
            }
        }
    }
}
