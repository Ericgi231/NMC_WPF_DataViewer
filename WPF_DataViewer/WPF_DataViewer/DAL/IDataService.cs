using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_DataViewer.Models;

namespace WPF_DataViewer.DAL
{
    interface IDataService
    {
        List<Character> ReadAll();
        void WriteAll(List<Character> characters);
    }
}
