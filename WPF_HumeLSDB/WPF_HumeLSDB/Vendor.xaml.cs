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
            vendInsertBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .79);
            vendInsertBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .08);
            // Populate button. Populates all input fields based on input vend_code.
            vendPopulateBtn.Width = 75;
            vendPopulateBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .88);
            vendPopulateBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .25);
            // Update button, delete button, and corresponding textbox. Will update or delete row in database based on given vendCode.
            vendUpdateBtn.Width = 75;
            vendUpdateBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .88);
            vendUpdateBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .30);
            vendDeleteBtn.Width = 75;
            vendDeleteBtn.SetValue(Canvas.LeftProperty, vendWindow.Width * .88);
            vendDeleteBtn.SetValue(Canvas.TopProperty, vendWindow.Height * .35);
            vendUpdateOrDeleteTextBox.Width = 75;
            vendUpdateOrDeleteTextBox.SetValue(Canvas.LeftProperty, vendWindow.Width * .79);
            vendUpdateOrDeleteTextBox.SetValue(Canvas.TopProperty, vendWindow.Height * .3);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            vendStackPanel.Width = 350; vendStackPanel.Height = 250;
            vendStackPanel.SetValue(Canvas.LeftProperty, vendWindow.Width * .325);
            vendStackPanel.SetValue(Canvas.TopProperty, vendWindow.Height * .02);
            vendEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            vendEnterDataLabel.Margin = new Thickness(-7, 0, 0, 10);
            vendEnterDataLabel.SetValue(FontWeightProperty, FontWeights.Bold);
            vendEnterDataLabel.FontSize = 16;

            // Stack panels that hold all insert field labels.
           vendLabelPanelLeft.Width = 110; vendLabelPanelLeft.Height = 250;
           vendLabelPanelLeft.SetValue(Canvas.LeftProperty, vendWindow.Width * .217);
           vendLabelPanelLeft.SetValue(Canvas.TopProperty, vendWindow.Height * .1);
           vendLabelPanelRight.Width = 110; vendLabelPanelRight.Height = 250;
           vendLabelPanelRight.SetValue(Canvas.LeftProperty, vendWindow.Width * .65);
           vendLabelPanelRight.SetValue(Canvas.TopProperty, vendWindow.Height * .1);

            // All labels used to describe textbox fields.
            labelName.Margin = new Thickness(0, -20, 0, 20);
            labelPhone.Margin = new Thickness(0, -20, 0, 20);
            labelEmail.Margin = new Thickness(0, -20, 0, 20);
            labelState.Margin = new Thickness(0, -20, 0, 20);
            labelArea.Margin = new Thickness(0, -20, 0, 20);
            labelAddress.Margin = new Thickness(0, -20, 0, 20);
            labelCity.Margin = new Thickness(0, -20, 0, 20);
            labelZip.Margin = new Thickness(0, -20, 0, 20);

            labelName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelState.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelName.FontSize = 14;
            labelPhone.FontSize = 14;
            labelEmail.FontSize = 14;
            labelState.FontSize = 14;
            labelArea.FontSize = 14;
            labelAddress.FontSize = 14;
            labelCity.FontSize = 14;
            labelZip.FontSize = 14;

            labelName.Foreground = Brushes.Coral;
            labelPhone.Foreground = Brushes.Coral;
            labelEmail.Foreground = Brushes.Coral;
            labelState.Foreground = Brushes.Coral;
            labelArea.Foreground = Brushes.Coral;
            labelAddress.Foreground = Brushes.Coral;
            labelCity.Foreground = Brushes.Coral;
            labelZip.Foreground = Brushes.Coral;

            // All textbox fields used to insert new row into database. 
            vendName.Width = vendStackPanel.Width * .45;
            vendName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendArea.Width = vendStackPanel.Width * .45;
            vendArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendArea.Margin = new Thickness(0, -21, 0, 5);
            vendPhone.Width = vendStackPanel.Width * .45;
            vendPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendAddress.Width = vendStackPanel.Width * .45;
            vendAddress.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendAddress.Margin = new Thickness(0, -21, 0, 5);
            vendEmail.Width = vendStackPanel.Width * .45;
            vendEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendCity.Width = vendStackPanel.Width * .45;
            vendCity.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendCity.Margin = new Thickness(0, -21, 0, 5);
            vendState.Width = vendStackPanel.Width * .45;
            vendState.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vendZip.Width = vendStackPanel.Width * .45;
            vendZip.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            vendZip.Margin = new Thickness(0, -21, 0, 5);
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
            makeVendorGrid();
        }

        // Populate button will populate all fields based on input vend_code.
        private void populateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {
                string currentCodeInfo = vendUpdateOrDeleteTextBox.Text;
                string sql = "select * from vendor where vend_code = " + currentCodeInfo;
                NpgsqlCommand selectQuery = new NpgsqlCommand(sql, conn);

                using (NpgsqlDataReader dr = selectQuery.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        vendName.Text = (dr["vend_name"].ToString());
                        vendArea.Text = (dr["vend_areacode"].ToString());
                        vendPhone.Text = (dr["vend_phone"].ToString());
                        vendAddress.Text = (dr["vend_address"].ToString());
                        vendEmail.Text = (dr["vend_email"].ToString());
                        vendCity.Text = (dr["vend_city"].ToString());
                        vendState.Text = (dr["vend_state"].ToString());
                        vendZip.Text = (dr["vend_zipcode"].ToString());

                        // Removing automatic text removal upon click in all textboxes, as we just populated them. 
                        // If the user is tabbing through, it'd be silly to have them all clear out.
                        vendName.GotFocus -= textBox_gotFocus;
                        vendArea.GotFocus -= textBox_gotFocus;
                        vendPhone.GotFocus -= textBox_gotFocus;
                        vendAddress.GotFocus -= textBox_gotFocus;
                        vendEmail.GotFocus -= textBox_gotFocus;
                        vendCity.GotFocus -= textBox_gotFocus;
                        vendState.GotFocus -= textBox_gotFocus;
                        vendZip.GotFocus -= textBox_gotFocus;
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
        // Strings are being used for int values for ease of home database use. These would be properly paramatized otherwise.
        private void insertClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                string useName = vendName.Text;
                string useArea = vendArea.Text;
                string usePhone = vendPhone.Text;
                string useAddress = vendAddress.Text;
                string useEmail = vendEmail.Text;
                string useCity = vendCity.Text;
                string useState = vendState.Text;
                string useZip = vendZip.Text;

                    using (NpgsqlCommand insertQuery = new NpgsqlCommand("insert into vendor ( "
                                + "vend_name, vend_areacode, vend_phone, vend_address, "
                                + "vend_email, vend_city, vend_state, vend_zipcode) VALUES ("
                                + ":Name, :Area, :Phone, :Address, :Email, :City, :State, :Zip)", conn))
                    {

                        insertQuery.Parameters.Add(new NpgsqlParameter("Name", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Name"].Value = useName;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Area"].Value = useArea;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Phone"].Value = usePhone;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Address", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Address"].Value = useAddress;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Email"].Value = useEmail;
                        insertQuery.Parameters.Add(new NpgsqlParameter("City", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["City"].Value = useCity;
                        insertQuery.Parameters.Add(new NpgsqlParameter("State", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["State"].Value = useState;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Zip", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Zip"].Value = useZip;

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

        // Update button will update row based on input vend_code with input fields.
        // Strings are being used for int values for ease of home database use. These would be properly paramatized otherwise.
        private void updateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                int useCode = Int32.Parse(vendUpdateOrDeleteTextBox.Text);
                string useName = vendName.Text;
                string useArea = vendArea.Text;
                string usePhone = vendPhone.Text;
                string useAddress = vendAddress.Text;
                string useEmail = vendEmail.Text;
                string useCity = vendCity.Text;
                string useState = vendState.Text;
                string useZip = vendZip.Text;

                string updatingID = vendUpdateOrDeleteTextBox.Text;
                string sql = "update vendor set vend_name = :Name, vend_areacode = :Area, vend_phone = :Phone, "
                           + "vend_address = :Address, vend_email = :Email, vend_city = :City, vend_state = :State, "
                           + "vend_zipcode = :Zip where vend_code = :Code";
                NpgsqlCommand updateQuery = new NpgsqlCommand(sql, conn);

                updateQuery.Parameters.Add(new NpgsqlParameter("Name", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Name"].Value = useName;
                updateQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Area"].Value = useArea;
                updateQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Phone"].Value = usePhone;
                updateQuery.Parameters.Add(new NpgsqlParameter("Address", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Address"].Value = useAddress;
                updateQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Email"].Value = useEmail;
                updateQuery.Parameters.Add(new NpgsqlParameter("City", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["City"].Value = useCity;
                updateQuery.Parameters.Add(new NpgsqlParameter("State", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["State"].Value = useState;
                updateQuery.Parameters.Add(new NpgsqlParameter("Zip", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Zip"].Value = useZip;
                updateQuery.Parameters.Add(new NpgsqlParameter("Code", NpgsqlTypes.NpgsqlDbType.Integer));
                updateQuery.Parameters["Code"].Value = useCode;

                int rowsAffected = updateQuery.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with vend_code of " + updatingID + " has been updated in Vendor table.");
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

        // Delete button will delete row based on input vend_code.
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                string deletingID = vendUpdateOrDeleteTextBox.Text;
                string sql = ("delete from vendor where vend_code = " + deletingID + ";");
                NpgsqlCommand da = new NpgsqlCommand(sql, conn);
                int rowsAffected;

                    rowsAffected = da.ExecuteNonQuery();
                    string dialog = (rowsAffected + " record(s) with vend_code of " + deletingID + " has been deleted from Vendor table.");
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
