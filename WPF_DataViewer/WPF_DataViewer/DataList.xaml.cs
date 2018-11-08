using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private List<Character> _characters;
        private IDataService _dataService;
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        public MainWindow()
        {
            InitializeComponent();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen<Help>())
            {
                Help help = new Help();
                help.Visibility = Visibility.Visible;
            }
        }

        private void OpenDetails(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen<Details>())
            {
                Details detail = new Details();
                detail.Visibility = Visibility.Visible;
            }
        }

        private void DeleteRecord(object sender, RoutedEventArgs e)
        {

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
    }
}
