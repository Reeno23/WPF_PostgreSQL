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
    /// Interaction logic for Vendor.xaml
    /// </summary>
    public partial class Vendor : Window
    {
        public Vendor()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            double height = Owner.ActualHeight;
            double width = Owner.ActualWidth;
            Width = width;
            Height = height;
            InitializeComponent();
            setSomeDimensions();
            vendGrid.Width = width * .9;
            vendGrid.Height = height * .45;
            vendGrid.SetValue(Canvas.LeftProperty, width * .05);
            vendGrid.SetValue(Canvas.TopProperty, height * .4);
            makeVendorGrid();
        }

        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeVendorGrid();
        }

        private void setSomeDimensions()
        {
            vendHomeBtn.Width = 75;
            vendHomeBtn.Height = 25;
            vendHomeBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .05);
            vendHomeBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .08);
            vendRefreshBtn.Width = 75;
            vendRefreshBtn.Height = 25;
            vendRefreshBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .05);
            vendRefreshBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .35);
            vendCode.Width = vendStackPanel.Width * .4;
            vendName.Width = vendStackPanel.Width * .4;
            vendArea.Width = vendStackPanel.Width * .4;
            vendPhone.Width = vendStackPanel.Width * .4;
            vendAddress.Width = vendStackPanel.Width * .4;
            vendEmail.Width = vendStackPanel.Width * .4;
            vendCity.Width = vendStackPanel.Width * .4;
            vendState.Width = vendStackPanel.Width * .4;
            vendZip.Width = vendStackPanel.Width * .4;
        }

        private void makeVendorGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from vendor";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            vendGrid.DataContext = dt.DefaultView;

            App.closeConn(conn);
        }
    }
}
