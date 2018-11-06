using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_DataViewer.Models
{
    public class Character
    {
        public int _id { get; set; }
        public string name { get; set; }
        public string weapon { get; set; }
        public string gender { get; set; }
        public string img_path { get; set; }
        public string description { get; set; }
    }
}
