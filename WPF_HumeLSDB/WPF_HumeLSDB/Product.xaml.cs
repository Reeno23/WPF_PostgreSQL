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
            prodInsertBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .79);
            prodInsertBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .08);
            // Populate button. Populates all input fields based on input prod_code.
            prodPopulateBtn.Width = 75;
            prodPopulateBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .88);
            prodPopulateBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .25);
            // Update button, delete button, and corresponding textbox. Will update or delete row in database based on given prodCode.
            prodUpdateBtn.Width = 75;
            prodUpdateBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .88);
            prodUpdateBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .30);
            prodDeleteBtn.Width = 75;
            prodDeleteBtn.SetValue(Canvas.LeftProperty, prodWindow.Width * .88);
            prodDeleteBtn.SetValue(Canvas.TopProperty, prodWindow.Height * .35);
            prodUpdateOrDeleteTextBox.Width = 75;
            prodUpdateOrDeleteTextBox.SetValue(Canvas.LeftProperty, prodWindow.Width * .79);
            prodUpdateOrDeleteTextBox.SetValue(Canvas.TopProperty, prodWindow.Height * .3);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            prodStackPanel.Width = 350; prodStackPanel.Height = 250;
            prodStackPanel.SetValue(Canvas.LeftProperty, prodWindow.Width * .325);
            prodStackPanel.SetValue(Canvas.TopProperty, prodWindow.Height * .02);
            prodEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prodEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            prodEnterDataLabel.Margin = new Thickness(-7, 0, 0, 10);
            prodEnterDataLabel.SetValue(FontWeightProperty, FontWeights.Bold);
            prodEnterDataLabel.FontSize = 16;

            // Stack panels that hold all insert field labels.
            prodLabelPanelLeft.Width = 110; prodLabelPanelLeft.Height = 250;
            prodLabelPanelLeft.SetValue(Canvas.LeftProperty, prodWindow.Width * .217);
            prodLabelPanelLeft.SetValue(Canvas.TopProperty, prodWindow.Height * .1);
            prodLabelPanelRight.Width = 110; prodLabelPanelRight.Height = 250;
            prodLabelPanelRight.SetValue(Canvas.LeftProperty, prodWindow.Width * .65);
            prodLabelPanelRight.SetValue(Canvas.TopProperty, prodWindow.Height * .1);

            // All labels used to describe textbox fields.
            labelVendCode.Margin = new Thickness(0, -20, 0, 20);
            labelPrice.Margin = new Thickness(0, -20, 0, 20);
            labelDescription.Margin = new Thickness(0, -20, 0, 20);

            labelVendCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelPrice.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            labelVendCode.FontSize = 14;
            labelPrice.FontSize = 14;
            labelDescription.FontSize = 14;

            labelVendCode.Foreground = Brushes.Coral;
            labelPrice.Foreground = Brushes.Coral;
            labelDescription.Foreground = Brushes.Coral;

            // All textbox fields used to insert new row into database. 
            vendCode.Width = prodStackPanel.Width * .45;
            vendCode.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prodDescription.Width = prodStackPanel.Width * .45;
            prodDescription.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            prodDescription.Margin = new Thickness(0, -21, 0, 5);
            prodPrice.Width = prodStackPanel.Width * .45;
            prodPrice.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        }

        // Fills datagrid with all data in specified table (Product).
        private void makeProductGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            NpgsqlConnection conn = App.openConn();
            string sql = "select * from Product";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            prodGrid.DataContext = dt.DefaultView;

            App.closeConn(conn);
        }

        // Makes textbox empty when you click inside it, so the default description text is gone.
        private void textBox_gotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= textBox_gotFocus;
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

        // Populate button will populate all fields based on input prod_code.
        private void populateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                string currentCodeInfo = prodUpdateOrDeleteTextBox.Text;
                string sql = "select * from product where prod_code = " + currentCodeInfo;
                NpgsqlCommand selectQuery = new NpgsqlCommand(sql, conn);

                    using (NpgsqlDataReader dr = selectQuery.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            vendCode.Text = (dr["vend_code"].ToString());
                            prodDescription.Text = (dr["prod_description"].ToString());
                            prodPrice.Text = (dr["prod_price"].ToString());


                            // Removing automatic text removal upon click in all textboxes, as we just populated them. 
                            // If the user is tabbing through, it'd be silly to have them all clear out.
                            vendCode.GotFocus -= textBox_gotFocus;
                            prodDescription.GotFocus -= textBox_gotFocus;
                            prodPrice.GotFocus -= textBox_gotFocus;
                        }
                    }
            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            finally
            {
                App.closeConn(conn);
            }
        }

        // Insert button will insert row into database based on input information.
        private void insertClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {
                int useVendCode = Int32.Parse(vendCode.Text);
                string useProdDescription = prodDescription.Text;
                double useProdPrice = Double.Parse(prodPrice.Text);

                using (NpgsqlCommand insertQuery = new NpgsqlCommand("insert into Product ( "
                            + "vend_code, prod_description, prod_price )  VALUES ("
                            + ":VendCode, :ProdDescription, :ProdPrice )", conn))
                {
                    
                    insertQuery.Parameters.Add(new NpgsqlParameter("VendCode", NpgsqlTypes.NpgsqlDbType.Integer));
                    insertQuery.Parameters["VendCode"].Value = useVendCode;
                    insertQuery.Parameters.Add(new NpgsqlParameter("ProdDescription", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["ProdDescription"].Value = useProdDescription;
                    insertQuery.Parameters.Add(new NpgsqlParameter("ProdPrice", NpgsqlTypes.NpgsqlDbType.Double));
                    insertQuery.Parameters["ProdPrice"].Value = useProdPrice;

                    int rowsAffected = insertQuery.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected.ToString());
                }
            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            finally
            {
                App.closeConn(conn);
            }
        }

        // Update button will update row based on input prod_code with input fields.
        private void updateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                int useProdCode = Int32.Parse(prodUpdateOrDeleteTextBox.Text);
                int useVendCode = Int32.Parse(vendCode.Text);
                string useProdDescription = prodDescription.Text;
                double useProdPrice = Double.Parse(prodPrice.Text);

                string updatingID = prodUpdateOrDeleteTextBox.Text;
                string sql = "update Product set vend_code = :VendCode, prod_description = :ProdDescription,"
                           + " prod_price = :ProdPrice where prod_code = :ProdCode";
                NpgsqlCommand updateQuery = new NpgsqlCommand(sql, conn);


                updateQuery.Parameters.Add(new NpgsqlParameter("VendCode", NpgsqlTypes.NpgsqlDbType.Integer));
                updateQuery.Parameters["VendCode"].Value = useVendCode;
                updateQuery.Parameters.Add(new NpgsqlParameter("ProdDescription", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["ProdDescription"].Value = useProdDescription;
                updateQuery.Parameters.Add(new NpgsqlParameter("ProdPrice", NpgsqlTypes.NpgsqlDbType.Double));
                updateQuery.Parameters["ProdPrice"].Value = useProdPrice;
                updateQuery.Parameters.Add(new NpgsqlParameter("ProdCode", NpgsqlTypes.NpgsqlDbType.Integer));
                updateQuery.Parameters["ProdCode"].Value = useProdCode;

                int rowsAffected = updateQuery.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with prod_code of " + updatingID + " has been updated in Product table.");
                MessageBox.Show(dialog);

            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            finally
            {
                App.closeConn(conn);
            }
        }

        // Delete button will delete row based on input prod_code.
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {
                string deletingID = prodUpdateOrDeleteTextBox.Text;
                string sql = ("delete from Product where prod_code = " + deletingID + ";");
                NpgsqlCommand da = new NpgsqlCommand(sql, conn);
                int rowsAffected;

                rowsAffected = da.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with prod_code of " + deletingID + " has been deleted from Product table.");
                MessageBox.Show(dialog);

            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Hey, double check what you entered.");
            }
            finally
            {
                App.closeConn(conn);
            }
        }
    }
}
