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
    public partial class Product : Window
    {
        /* prodGrid is here rather than in setSomeDimensions() as use of Owner (MainWindow) properties are required. 
        * Owner is set to null right after window initilization, so we can't get its properties later. */
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
            setupStackPanel();
            prodGrid.Width = width * .9;
            prodGrid.Height = height * .45;
            prodGrid.SetValue(Canvas.LeftProperty, width * .05);
            prodGrid.SetValue(Canvas.TopProperty, height * .4);
            makeProductGrid();
        }

        /* Sets majority of dimensions and positioning of elements inside canvas. 
        * Any element that doesn't require use of owner window's properties will be in here or setupStackPanel(); */
        private void setSomeDimensions()
        {
            // Home button. Returns user to main page.
            prodHomeBtn.Width = 75; prodHomeBtn.Height = 25;
            prodHomeBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .05);
            prodHomeBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .08);
            // Refresh button. Updates grid data.
            prodRefreshBtn.Width = 75; prodRefreshBtn.Height = 25;
            prodRefreshBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .05);
            prodRefreshBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .35);
            // Insert button. Inserts row into database via textbox fields in stack panel.
            prodInsertBtn.Width = 75;
            prodInsertBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .8);
            prodInsertBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .07);
            // Delete button and corresponding textbox. Will delete row in database based on given prodCode.
            prodDeleteBtn.Width = 75;
            prodDeleteBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .88);
            prodDeleteBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .35);
            prodDeleteTextBox.Width = prodStackPanel.Width * .4;
            prodDeleteTextBox.SetValue(Canvas.LeftProperty, prodWindow.Width * .79);
            prodDeleteTextBox.SetValue(Canvas.TopProperty, prodWindow.Height * .35);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            prodStackPanel.Width = 400; prodStackPanel.Height = 250;
            prodStackPanel.SetValue(Canvas.LeftProperty, prodWindow.Width * .4);
            prodStackPanel.SetValue(Canvas.TopProperty, prodWindow.Height * .02);
            prodEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prodEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            prodEnterDataLabel.Margin = new Thickness(0, 0, 0, 10);

            // All textbox fields used to insert new row into database. 
            prodCode.Width = prodStackPanel.Width * .4;
            prodCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendCode.Width = prodStackPanel.Width * .4;
            vendCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendCode.Margin = new Thickness(0, -21, 0, 5);
            prodDescription.Width = prodStackPanel.Width * .4;
            prodDescription.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prodPrice.Width = prodStackPanel.Width * .4;
            prodPrice.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            prodPrice.Margin = new Thickness(0, -21, 0, 5);
        }

        // Fills datagrid with all data in specified table (Product).
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

        // Takes user back to main window. 
        private void homeBtnClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        // Updates datagrid. Useful if any insertions or deletions have occured.
        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            makeProductGrid();
        }




    }
}
