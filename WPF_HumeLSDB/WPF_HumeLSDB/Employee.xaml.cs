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
    public partial class Employee : Window
    {
        /* empGrid is here rather than in setSomeDimensions() as use of Owner (MainWindow) properties are required. 
        * Owner is set to null right after window initilization, so we can't get its properties later. */
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
            setupStackPanel();
            empGrid.Width = width * .9;
            empGrid.Height = height * .45;
            empGrid.SetValue(Canvas.LeftProperty, width * .05);
            empGrid.SetValue(Canvas.TopProperty, height * .4);
            makeEmployeeGrid();
        }

        /* Sets majority of dimensions and positioning of elements inside canvas. 
        * Any element that doesn't require use of owner window's properties will be in here or setupStackPanel(); */
        private void setSomeDimensions()
        {
            // Home button. Returns user to main page.
            empHomeBtn.Width = 75; empHomeBtn.Height = 25;
            empHomeBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .05);
            empHomeBtn.SetValue(Canvas.TopProperty, empWindow.Height * .08);
            // Refresh button. Updates grid data.
            empRefreshBtn.Width = 75; empRefreshBtn.Height = 25;
            empRefreshBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .05);
            empRefreshBtn.SetValue(Canvas.TopProperty, empWindow.Height * .35);
            // Insert button. Inserts row into database via textbox fields in stack panel.
            empInsertBtn.Width = 75;
            empInsertBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .8);
            empInsertBtn.SetValue(Canvas.TopProperty, empWindow.Height * .07);
            // Delete button and corresponding textbox. Will delete row in database based on given empCode.
            empDeleteBtn.Width = 75;
            empDeleteBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empDeleteBtn.SetValue(Canvas.TopProperty, empWindow.Height * .35);
            empDeleteTextBox.Width = empStackPanel.Width * .4;
            empDeleteTextBox.SetValue(Canvas.LeftProperty, empWindow.Width * .79);
            empDeleteTextBox.SetValue(Canvas.TopProperty, empWindow.Height * .35);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            empStackPanel.Width = 400; empStackPanel.Height = 250;
            empStackPanel.SetValue(Canvas.LeftProperty, empWindow.Width * .4);
            empStackPanel.SetValue(Canvas.TopProperty, empWindow.Height * .02);
            empEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            empEnterDataLabel.Margin = new Thickness(0, 0, 0, 10);

            // All textbox fields used to insert new row into database. 
            empID.Width = empStackPanel.Width * .4;
            empID.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empTitle.Width = empStackPanel.Width * .4;
            empTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empTitle.Margin = new Thickness(0, -21, 0, 5);
            empFName.Width = empStackPanel.Width * .4;
            empFName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empLName.Width = empStackPanel.Width * .4;
            empLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empLName.Margin = new Thickness(0, -21, 0, 5);
            empInitial.Width = empStackPanel.Width * .4;
            empInitial.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empAreaCode.Width = empStackPanel.Width * .4;
            empAreaCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empAreaCode.Margin = new Thickness(0, -21, 0, 5);
            empPhone.Width = empStackPanel.Width * .4;
            empPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empEmail.Width = empStackPanel.Width * .4;
            empEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empEmail.Margin = new Thickness(0, -21, 0, 5);
            empRate.Width = empStackPanel.Width * .4;
            empRate.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        }

        // Fills datagrid with all data in specified table (Employee).
        private void makeEmployeeGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from Employee";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            empGrid.DataContext = dt.DefaultView;

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
            makeEmployeeGrid();
        }




    }
}
