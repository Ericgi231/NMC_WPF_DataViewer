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
        public List<Character> _characters;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dg_CharacterGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // read data
                //
                IDataService dataService = new MongoDataService();
                _characters = dataService.ReadAll();

                // bind data
                //
                dg_CharacterGrid.ItemsSource = _characters;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error");
            }
        }
    }
}
