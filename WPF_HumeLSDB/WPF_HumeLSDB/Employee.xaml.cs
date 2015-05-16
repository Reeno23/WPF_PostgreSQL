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
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public Employee()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            double height = Owner.ActualHeight;
            double width = Owner.ActualWidth;
            Width = width;
            Height = height;
            InitializeComponent();
            setSomeDimensions();
            empGrid.Width = width * .9;
            empGrid.Height = height * .45;
            empGrid.SetValue(Canvas.LeftProperty, width * .05);
            empGrid.SetValue(Canvas.TopProperty, height * .4);
            makeEmployeeGrid();
        }

        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
                new MainWindow().Show();
                this.Close();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeEmployeeGrid();
        }

        private void setSomeDimensions()
        {
            empHomeBtn.Width = 75;
            empHomeBtn.Height = 25;
            empHomeBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .05);
            empHomeBtn.SetValue(Canvas.TopProperty, empWindow.Height * .08);
            empRefreshBtn.Width = 75;
            empRefreshBtn.Height = 25;
            empRefreshBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .05);
            empRefreshBtn.SetValue(Canvas.TopProperty, empWindow.Height * .35);
        }

        private void makeEmployeeGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from employee";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            empGrid.DataContext = dt.DefaultView;

            App.closeConn(conn);
        }

    }
}
