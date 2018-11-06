using System;
using System.Collections.Generic;
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

            //set combo boxes
            //
            foreach (var col in dg_CharacterGrid.Columns)
            {
                cb_Filter.Items.Add(col.Header);
                cb_Sort.Items.Add(col.Header);
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

        }
    }
}
