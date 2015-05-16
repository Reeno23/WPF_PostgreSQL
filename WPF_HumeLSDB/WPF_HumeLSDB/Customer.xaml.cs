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
    public partial class Customer : Window
    {
        /* custGrid is here rather than in setSomeDimensions() as use of Owner (MainWindow) properties are required. 
        * Owner is set to null right after window initilization, so we can't get its properties later. */
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
            setupStackPanel();
            custGrid.Width = width * .9;
            custGrid.Height = height * .45;
            custGrid.SetValue(Canvas.LeftProperty, width * .05);
            custGrid.SetValue(Canvas.TopProperty, height * .4);
            makeCustomerGrid();
        }

        /* Sets majority of dimensions and positioning of elements inside canvas. 
        * Any element that doesn't require use of owner window's properties will be in here or setupStackPanel(); */
        private void setSomeDimensions()
        {
            // Home button. Returns user to main page.
            custHomeBtn.Width = 75; custHomeBtn.Height = 25;
            custHomeBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .05);
            custHomeBtn.SetValue(Canvas.TopProperty, custWindow.Height * .08);
            // Refresh button. Updates grid data.
            custRefreshBtn.Width = 75; custRefreshBtn.Height = 25;
            custRefreshBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .05);
            custRefreshBtn.SetValue(Canvas.TopProperty, custWindow.Height * .35);
            // Insert button. Inserts row into database via textbox fields in stack panel.
            custInsertBtn.Width = 75;
            custInsertBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .8);
            custInsertBtn.SetValue(Canvas.TopProperty, custWindow.Height * .07);
            // Delete button and corresponding textbox. Will delete row in database based on given custCode.
            custDeleteBtn.Width = 75;
            custDeleteBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .88);
            custDeleteBtn.SetValue(Canvas.TopProperty, custWindow.Height * .35);
            custDeleteTextBox.Width = custStackPanel.Width * .4;
            custDeleteTextBox.SetValue(Canvas.LeftProperty, custWindow.Width * .79);
            custDeleteTextBox.SetValue(Canvas.TopProperty, custWindow.Height * .35);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            custStackPanel.Width = 400; custStackPanel.Height = 250;
            custStackPanel.SetValue(Canvas.LeftProperty, custWindow.Width * .4);
            custStackPanel.SetValue(Canvas.TopProperty, custWindow.Height * .02);
            custEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            custEnterDataLabel.Margin = new Thickness(0, 0, 0, 10);

            // All textbox fields used to insert new row into database. 
            custID.Width = custStackPanel.Width * .4;
            custID.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custFName.Width = custStackPanel.Width * .4;
            custFName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custFName.Margin = new Thickness(0, -21, 0, 5);
            custLName.Width = custStackPanel.Width * .4;
            custLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custInitial.Width = custStackPanel.Width * .4;
            custInitial.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custInitial.Margin = new Thickness(0, -21, 0, 5);
            custAreaCode.Width = custStackPanel.Width * .4;
            custAreaCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custPhone.Width = custStackPanel.Width * .4;
            custPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custPhone.Margin = new Thickness(0, -21, 0, 5);
            custAddress.Width = custStackPanel.Width * .4;
            custAddress.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custEmail.Width = custStackPanel.Width * .4;
            custEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custEmail.Margin = new Thickness(0, -21, 0, 5);
            custCity.Width = custStackPanel.Width * .4;
            custCity.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custState.Width = custStackPanel.Width * .4;
            custState.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custState.Margin = new Thickness(0, -21, 0, 5);
            custZip.Width = custStackPanel.Width * .4;
            custZip.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

        }

        // Fills datagrid with all data in specified table (Customer).
        private void makeCustomerGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from Customer";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            custGrid.DataContext = dt.DefaultView;

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
            makeCustomerGrid();
        }




    }
}
