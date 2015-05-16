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

    public partial class Vendor : Window
    {
        /* vendGrid is here rather than in setSomeDimensions() as use of Owner (MainWindow) properties are required. 
         * Owner is set to null right after window initilization, so we can't get its properties later. */
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
            setupStackPanel();
            vendGrid.Width = width * .9;
            vendGrid.Height = height * .45;
            vendGrid.SetValue(Canvas.LeftProperty, width * .05);
            vendGrid.SetValue(Canvas.TopProperty, height * .4);
            makeVendorGrid();
        }

        /* Sets majority of dimensions and positioning of elements inside canvas. 
        * Any element that doesn't require use of owner window's properties will be in here or setupStackPanel(); */
        private void setSomeDimensions()
        {
            // Home button. Returns user to main page.
            vendHomeBtn.Width = 75; vendHomeBtn.Height = 25;
            vendHomeBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .05);
            vendHomeBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .08);
            // Refresh button. Updates grid data.
            vendRefreshBtn.Width = 75; vendRefreshBtn.Height = 25;
            vendRefreshBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .05);
            vendRefreshBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .35);
            // Insert button. Inserts row into database via textbox fields in stack panel.
            vendInsertBtn.Width = 75;
            vendInsertBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .8);
            vendInsertBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .07);
            // Delete button and corresponding textbox. Will delete row in database based on given vendCode.
            vendDeleteBtn.Width = 75;
            vendDeleteBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .88);
            vendDeleteBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .35);
            vendDeleteTextBox.Width = vendStackPanel.Width * .4;
            vendDeleteTextBox.SetValue(Canvas.LeftProperty, vendWindow.Width * .79);
            vendDeleteTextBox.SetValue(Canvas.TopProperty, vendWindow.Height * .35);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            vendStackPanel.Width = 400; vendStackPanel.Height = 250;
            vendStackPanel.SetValue(Canvas.LeftProperty, vendWindow.Width * .4);
            vendStackPanel.SetValue(Canvas.TopProperty, vendWindow.Height * .02);
            vendEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            vendEnterDataLabel.Margin = new Thickness(0, 0, 0, 10);

            // All textbox fields used to insert new row into database. 
            vendCode.Width = vendStackPanel.Width * .4;
            vendCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendName.Width = vendStackPanel.Width * .4;
            vendName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendName.Margin = new Thickness(0, -21, 0, 5);
            vendArea.Width = vendStackPanel.Width * .4;
            vendArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendPhone.Width = vendStackPanel.Width * .4;
            vendPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendPhone.Margin = new Thickness(0, -21, 0, 5);
            vendAddress.Width = vendStackPanel.Width * .4;
            vendAddress.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendEmail.Width = vendStackPanel.Width * .4;
            vendEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendEmail.Margin = new Thickness(0, -21, 0, 5);
            vendCity.Width = vendStackPanel.Width * .4;
            vendCity.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendState.Width = vendStackPanel.Width * .4;
            vendState.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendState.Margin = new Thickness(0, -21, 0, 5);
            vendZip.Width = vendStackPanel.Width * .4;
            vendZip.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        }

        // Fills datagrid with all data in specified table (Vendor).
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

        // Takes user back to main window. 
        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        // Updates datagrid. Useful if any insertions or deletions have occured.
        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeVendorGrid();
        }
    }
}
