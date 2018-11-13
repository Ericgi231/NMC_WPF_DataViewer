using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_DataViewer.Models;

namespace WPF_DataViewer.DAL
{
    interface IDataService
    {
        ObservableCollection<Character> ReadAll();
        void WriteAll(ObservableCollection<Character> characters);
    }
}
