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
            custInsertBtn.SetValue(Canvas.TopProperty, custWindow.Height * .08);
            // Populate button. Populates all input fields based on input cust_id.
            custPopulateBtn.Width = 75;
            custPopulateBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .88);
            custPopulateBtn.SetValue(Canvas.TopProperty, custWindow.Height * .25);
            // Update button, delete button, and corresponding textbox. Will update or delete row in database based on given cust_id.
            custUpdateBtn.Width = 75;
            custUpdateBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .88);
            custUpdateBtn.SetValue(Canvas.TopProperty, custWindow.Height * .30);
            custDeleteBtn.Width = 75;
            custDeleteBtn.SetValue(Canvas.LeftProperty, custWindow.Width * .88);
            custDeleteBtn.SetValue(Canvas.TopProperty, custWindow.Height * .35);
            custUpdateOrDeleteTextBox.Width = 80;
            custUpdateOrDeleteTextBox.SetValue(Canvas.LeftProperty, custWindow.Width * .79);
            custUpdateOrDeleteTextBox.SetValue(Canvas.TopProperty, custWindow.Height * .3);
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
            custEnterDataLabel.Margin = new Thickness(-7, 0, 0, 10);
            custEnterDataLabel.SetValue(FontWeightProperty, FontWeights.Bold);
            custEnterDataLabel.FontSize = 16;

            // All textbox fields used to insert new row into database. 
            custFName.Width = custStackPanel.Width * .4;
            custFName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custLName.Width = custStackPanel.Width * .4;
            custLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custLName.Margin = new Thickness(0, -21, 0, 5);
            custInitial.Width = custStackPanel.Width * .4;
            custInitial.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custArea.Width = custStackPanel.Width * .4;
            custArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custArea.Margin = new Thickness(0, -21, 0, 5);
            custPhone.Width = custStackPanel.Width * .4;
            custPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custAddress.Width = custStackPanel.Width * .4;
            custAddress.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custAddress.Margin = new Thickness(0, -21, 0, 5);
            custEmail.Width = custStackPanel.Width * .4;
            custEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custCity.Width = custStackPanel.Width * .4;
            custCity.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custCity.Margin = new Thickness(0, -21, 0, 5);
            custState.Width = custStackPanel.Width * .4;
            custState.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            custZip.Width = custStackPanel.Width * .4;
            custZip.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            custZip.Margin = new Thickness(0, -21, 0, 5);
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
            makeCustomerGrid();
        }

        // Populate button will populate all fields based on input cust_id.
        private void populateClick(object sender, RoutedEventArgs e)
        {
            string currentCodeInfo = custUpdateOrDeleteTextBox.Text;
            string sql = "select * from customer where cust_id = " + currentCodeInfo;
            NpgsqlConnection conn = App.openConn();
            NpgsqlCommand selectQuery = new NpgsqlCommand(sql, conn);

            try
            {

                using (NpgsqlDataReader dr = selectQuery.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        custFName.Text = (dr["cust_fname"].ToString());
                        custLName.Text = (dr["cust_lname"].ToString());
                        custInitial.Text = (dr["cust_initial"].ToString());
                        custArea.Text = (dr["cust_areacode"].ToString());
                        custPhone.Text = (dr["cust_phone"].ToString());
                        custAddress.Text = (dr["cust_address"].ToString());
                        custEmail.Text = (dr["cust_email"].ToString());
                        custCity.Text = (dr["cust_city"].ToString());
                        custState.Text = (dr["cust_state"].ToString());
                        custZip.Text = (dr["cust_zipcode"].ToString());

                        // Removing automatic text removal upon click in all textboxes, as we just populated them. 
                        // If the user is tabbing through, it'd be silly to have them all clear out.
                        custFName.GotFocus -= textBox_gotFocus;
                        custLName.GotFocus -= textBox_gotFocus;
                        custInitial.GotFocus -= textBox_gotFocus;
                        custArea.GotFocus -= textBox_gotFocus;
                        custPhone.GotFocus -= textBox_gotFocus;
                        custAddress.GotFocus -= textBox_gotFocus;
                        custEmail.GotFocus -= textBox_gotFocus;
                        custCity.GotFocus -= textBox_gotFocus;
                        custState.GotFocus -= textBox_gotFocus;
                        custZip.GotFocus -= textBox_gotFocus;
                    }
                }

            }
            finally
            {
                App.closeConn(conn);
            }
        }

        // Insert button will insert row into database based on input information.
        // Areacode & zip are string rather than ints for ease of home database use.
        private void insertClick(object sender, RoutedEventArgs e)
        {
            string useFName = custFName.Text;
            string useLName = custLName.Text;
            string useInitial = custInitial.Text;
            string useArea = custArea.Text;
            string usePhone = custPhone.Text;
            string useAddress = custAddress.Text;
            string useEmail = custEmail.Text;
            string useCity = custCity.Text;
            string useState = custState.Text;
            string useZip = custZip.Text;

            using (NpgsqlConnection conn = App.openConn())
            {

                using (NpgsqlCommand insertQuery = new NpgsqlCommand("insert into Customer ( "
                            + "cust_fname, cust_lname, cust_initial, cust_areacode, "
                            + "cust_phone, cust_address, cust_email, cust_city, cust_state, cust_zipcode) VALUES ("
                            + ":FName, :LName, :Initial, :Area, :Phone, :Address, :Email, :City, :State, :Zip)", conn))
                {

                    insertQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["FName"].Value = useArea;
                    insertQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["LName"].Value = usePhone;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["Initial"].Value = useInitial;
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

                    try
                    {
                        int rowsAffected = insertQuery.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected.ToString());
                    }
                    catch (NpgsqlException q)
                    {
                        Console.Write(q);
                    }
                    finally
                    {
                        App.closeConn(conn);
                    }
                }

            }
        }

        // Update button will update row based on input cust_code with input fields.
        // Areacode & zip are string rather than ints for ease of home database use.
        private void updateClick(object sender, RoutedEventArgs e)
        {
            int useCode = Int32.Parse(custUpdateOrDeleteTextBox.Text);
            string useFName = custFName.Text;
            string useLName = custLName.Text;
            string useInitial = custInitial.Text;
            string useArea = custArea.Text;
            string usePhone = custPhone.Text;
            string useAddress = custAddress.Text;
            string useEmail = custEmail.Text;
            string useCity = custCity.Text;
            string useState = custState.Text;
            string useZip = custZip.Text;

            NpgsqlConnection conn = App.openConn();
            string updatingID = custUpdateOrDeleteTextBox.Text;
            string sql = "update Customer set cust_fname = :FName, cust_lname = :LName, "
                       + "cust_initial = :Initial, cust_areacode = :Area, cust_phone = :Phone, cust_address = :Address, "
                       + "cust_email = :Email, cust_city = :City, cust_state = :State, cust_zipcode = :Zip where cust_id = :Code";
            NpgsqlCommand updateQuery = new NpgsqlCommand(sql, conn);

            updateQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["FName"].Value = useArea;
            updateQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["LName"].Value = usePhone;
            updateQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["Initial"].Value = useInitial;
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

            try
            {
                int rowsAffected = updateQuery.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with cust_id of " + updatingID + " has been updated in Customer table.");
                MessageBox.Show(dialog);
            }
            catch (NpgsqlException q)
            {
                Console.Write(q);
            }
            finally
            {
                App.closeConn(conn);
            }
        }

        // Delete button will delete row based on input cust_code.
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            string deletingID = custUpdateOrDeleteTextBox.Text;
            string sql = ("delete from Customer where cust_id = " + deletingID + ";");
            NpgsqlCommand da = new NpgsqlCommand(sql, conn);
            int rowsAffected;

            try
            {
                rowsAffected = da.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with cust_id of " + deletingID + " has been deleted from Customer table.");
                MessageBox.Show(dialog);

            }
            finally
            {
                App.closeConn(conn);
            }
        }
    }
}
