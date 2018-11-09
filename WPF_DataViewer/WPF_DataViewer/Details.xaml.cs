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
using System.Windows.Shapes;
using WPF_DataViewer.Models;

namespace WPF_DataViewer
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        public Details(Character character)
        {
            InitializeComponent();

            try
            {
                var uri = new Uri("pack://application:,,,/Resources/" + character.img_path);
                var bitmap = new BitmapImage(uri);
                img_Character.Source = bitmap;
            }
            catch (Exception)
            {
                var uri = new Uri("pack://application:,,,/Resources/ErrorImage.png");
                var bitmap = new BitmapImage(uri);
                img_Character.Source = bitmap;
            }

            tb_Info.Text = $"Id: {character._id}\n" +
                $"Name: {character.name}\n" +
                $"Weapon: {character.weapon}\n" +
                $"Gender: {character.gender}\n" +
                $"Image Path: {character.img_path}\n" +
                $"Description: {character.description}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
