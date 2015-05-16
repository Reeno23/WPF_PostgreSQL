using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using Npgsql;

namespace WPF_HumeLSDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = Combo1.SelectedIndex;
            switch (index)
            {
                case 0:
                    Customer customer = new Customer();
                    customer.Show();
                    customer.Owner = null;
                    Application.Current.MainWindow.Close();
                    break;
                case 1:
                    Employee employee = new Employee();
                    employee.Show();
                    employee.Owner = null;
                    Application.Current.MainWindow.Close();
                    break;
                case 2:
                    Vendor vendor = new Vendor();
                    vendor.Show();
                    vendor.Owner = null;
                    Application.Current.MainWindow.Close();
                    break;
                case 3:
                    Product product = new Product();
                    product.Show();
                    product.Owner = null;
                    Application.Current.MainWindow.Close();
                    break;
            }
        }
    }
}
