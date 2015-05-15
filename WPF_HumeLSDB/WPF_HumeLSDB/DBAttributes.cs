using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_HumeLSDB
{
    class DBAttributes : ObservableCollection<string>
    {
        
        public DBAttributes ()
        {
            Add("Customer");
            Add("Employee");
            Add("Vendor");
            Add("Product");
        }
    }
}
