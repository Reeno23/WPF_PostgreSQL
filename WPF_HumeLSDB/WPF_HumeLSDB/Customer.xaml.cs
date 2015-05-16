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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {

        public Customer()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            double height = Owner.ActualHeight;
            double width = Owner.ActualWidth;
            Width = width;
            Height = height;
            InitializeComponent();
            setSomeDimensions();
            custGrid.Width = width * .9;
            custGrid.Height = height * .45;
            custGrid.SetValue(Canvas.LeftProperty, width * .05);
            custGrid.SetValue(Canvas.TopProperty, height * .4);
            makeCustomerGrid();
        }

        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeCustomerGrid();
        }

        private void setSomeDimensions()
        {
            custHomeBtn.Width = 75;
            custHomeBtn.Height = 25;
            custHomeBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .05);
            custHomeBtn.SetValue(Canvas.TopProperty, custWindow.Height * .08);
            custRefreshBtn.Width = 75;
            custRefreshBtn.Height = 25;
            custRefreshBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .05);
            custRefreshBtn.SetValue(Canvas.TopProperty, custWindow.Height * .35);
        }

        private void makeCustomerGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from customer";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            custGrid.DataContext = dt.DefaultView;

            App.closeConn(conn);
        }




    }
}
