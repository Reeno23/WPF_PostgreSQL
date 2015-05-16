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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        public Product()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            double height = Owner.ActualHeight;
            double width = Owner.ActualWidth;
            Width = width;
            Height = height;
            InitializeComponent();
            setSomeDimensions();
            prodGrid.Width = width * .9;
            prodGrid.Height = height * .45;
            prodGrid.SetValue(Canvas.LeftProperty, width * .05);
            prodGrid.SetValue(Canvas.TopProperty, height * .4);
            makeProductGrid();
        }

        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeProductGrid();
        }

        private void setSomeDimensions()
        {
            prodHomeBtn.Width = 75;
            prodHomeBtn.Height = 25;
            prodHomeBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .05);
            prodHomeBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .08);
            prodRefreshBtn.Width = 75;
            prodRefreshBtn.Height = 25;
            prodRefreshBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .05);
            prodRefreshBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .35);
        }

        private void makeProductGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from product";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            prodGrid.DataContext = dt.DefaultView;

            App.closeConn(conn);
        }
    }
}
