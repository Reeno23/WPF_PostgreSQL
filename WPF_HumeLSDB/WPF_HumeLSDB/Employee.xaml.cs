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
            empInsertBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .79);
            empInsertBtn.SetValue(Canvas.TopProperty, empWindow.Height * .08);
            // Populate button. Populates all input fields based on input emp_id.
            empPopulateBtn.Width = 75;
            empPopulateBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empPopulateBtn.SetValue(Canvas.TopProperty, empWindow.Height * .25);
            // Update button, delete button, and corresponding textbox. Will update or delete row in database based on given empCode.
            empUpdateBtn.Width = 75;
            empUpdateBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empUpdateBtn.SetValue(Canvas.TopProperty, empWindow.Height * .30);
            empDeleteBtn.Width = 75;
            empDeleteBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empDeleteBtn.SetValue(Canvas.TopProperty, empWindow.Height * .35);
            empUpdateOrDeleteTextBox.Width = 75;
            empUpdateOrDeleteTextBox.SetValue(Canvas.LeftProperty, empWindow.Width * .79);
            empUpdateOrDeleteTextBox.SetValue(Canvas.TopProperty, empWindow.Height * .3);
        }

        // Sets properties for all text boxes inside the stack panel & the panel itself.
        private void setupStackPanel()
        {
            // Stack panel that holds all insert fields.
            empStackPanel.Width = 350; empStackPanel.Height = 250;
            empStackPanel.SetValue(Canvas.LeftProperty, empWindow.Width * .325);
            empStackPanel.SetValue(Canvas.TopProperty, empWindow.Height * .02);
            empEnterDataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empEnterDataLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            empEnterDataLabel.Margin = new Thickness(-7, 0, 0, 10);
            empEnterDataLabel.SetValue(FontWeightProperty, FontWeights.Bold);
            empEnterDataLabel.FontSize = 16;

            // Stack panels that hold all insert field labels.
            empLabelPanelLeft.Width = 110; empLabelPanelLeft.Height = 250;
            empLabelPanelLeft.SetValue(Canvas.LeftProperty, empWindow.Width * .217);
            empLabelPanelLeft.SetValue(Canvas.TopProperty, empWindow.Height * .1);
            empLabelPanelRight.Width = 110; empLabelPanelRight.Height = 250;
            empLabelPanelRight.SetValue(Canvas.LeftProperty, empWindow.Width * .65);
            empLabelPanelRight.SetValue(Canvas.TopProperty, empWindow.Height * .1);

            // All labels used to describe textbox fields.
            labelTitle.Margin = new Thickness(0, -20, 0, 20);
            labelLName.Margin = new Thickness(0, -20, 0, 20);
            labelArea.Margin = new Thickness(0, -20, 0, 20);
            labelEmail.Margin = new Thickness(0, -20, 0, 20);
            labelFName.Margin = new Thickness(0, -20, 0, 20);
            labelInitial.Margin = new Thickness(0, -20, 0, 20);
            labelPhone.Margin = new Thickness(0, -20, 0, 20);
            labelRate.Margin = new Thickness(0, -20, 0, 20);
            labelTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            labelTitle.FontSize = 14;
            labelLName.FontSize = 14;
            labelArea.FontSize = 14;
            labelEmail.FontSize = 14;
            labelFName.FontSize = 14;
            labelInitial.FontSize = 14;
            labelPhone.FontSize = 14;
            labelRate.FontSize = 14;

            labelTitle.Foreground = Brushes.Coral;
            labelLName.Foreground = Brushes.Coral;
            labelArea.Foreground = Brushes.Coral;
            labelEmail.Foreground = Brushes.Coral;
            labelFName.Foreground = Brushes.Coral;
            labelInitial.Foreground = Brushes.Coral;
            labelPhone.Foreground = Brushes.Coral;
            labelRate.Foreground = Brushes.Coral;


            // All textbox fields used to insert new row into database. 
            empTitle.Width = empStackPanel.Width * .45;
            empTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empFName.Width = empStackPanel.Width * .45;
            empFName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empFName.Margin = new Thickness(0, -21, 0, 5);
            empLName.Width = empStackPanel.Width * .45;
            empLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empInitial.Width = empStackPanel.Width * .45;
            empInitial.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empInitial.Margin = new Thickness(0, -21, 0, 5);
            empArea.Width = empStackPanel.Width * .45;
            empArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empPhone.Width = empStackPanel.Width * .45;
            empPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empPhone.Margin = new Thickness(0, -21, 0, 5);
            empEmail.Width = empStackPanel.Width * .45;
            empEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empRate.Width = empStackPanel.Width * .45;
            empRate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empRate.Margin = new Thickness(0, -21, 0, 5);
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
            makeEmployeeGrid();
        }

        // Populate button will populate all fields based on input emp_id.
        private void populateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                string currentCodeInfo = empUpdateOrDeleteTextBox.Text;
                string sql = "select * from employee where emp_id = " + currentCodeInfo;
                NpgsqlCommand selectQuery = new NpgsqlCommand(sql, conn);

                    using (NpgsqlDataReader dr = selectQuery.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            empTitle.Text = (dr["emp_title"].ToString());
                            empFName.Text = (dr["emp_fname"].ToString());
                            empLName.Text = (dr["emp_lname"].ToString());
                            empInitial.Text = (dr["emp_initial"].ToString());
                            empArea.Text = (dr["emp_areacode"].ToString());
                            empPhone.Text = (dr["emp_phone"].ToString());
                            empEmail.Text = (dr["emp_email"].ToString());
                            empRate.Text = (dr["emp_rate"].ToString());

                            // Removing automatic text removal upon click in all textboxes, as we just populated them. 
                            // If the user is tabbing through, it'd be silly to have them all clear out.
                            empTitle.GotFocus -= textBox_gotFocus;
                            empFName.GotFocus -= textBox_gotFocus;
                            empLName.GotFocus -= textBox_gotFocus;
                            empInitial.GotFocus -= textBox_gotFocus;
                            empArea.GotFocus -= textBox_gotFocus;
                            empPhone.GotFocus -= textBox_gotFocus;
                            empEmail.GotFocus -= textBox_gotFocus;
                            empRate.GotFocus -= textBox_gotFocus;
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
        // Areacode is a string rather than int for ease of home database use.
        private void insertClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                string useTitle = empTitle.Text;
                string useFName = empFName.Text;
                string useLName = empLName.Text;
                string useInitial = empInitial.Text;
                string useArea = empArea.Text;
                string usePhone = empPhone.Text;
                string useEmail = empEmail.Text;
                double useRate = Double.Parse(empRate.Text);

                    using (NpgsqlCommand insertQuery = new NpgsqlCommand("insert into Employee ( "
                                + "emp_title, emp_fname, emp_lname, emp_initial, "
                                + "emp_areacode, emp_phone, emp_email, emp_rate) VALUES ("
                                + ":Title, :FName, :LName, :Initial, :Area, :Phone, :Email, :Rate)", conn))
                    {

                        insertQuery.Parameters.Add(new NpgsqlParameter("Title", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Title"].Value = useTitle;
                        insertQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["FName"].Value = useFName;
                        insertQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["LName"].Value = useLName;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Initial"].Value = useInitial;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Area"].Value = useArea;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Phone"].Value = usePhone;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
                        insertQuery.Parameters["Email"].Value = useEmail;
                        insertQuery.Parameters.Add(new NpgsqlParameter("Rate", NpgsqlTypes.NpgsqlDbType.Double));
                        insertQuery.Parameters["Rate"].Value = useRate;

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

        // Update button will update row based on input emp_code with input fields.
        // Areacode is a string rather than int for ease of home database use.
        private void updateClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {

                int useCode = Int32.Parse(empUpdateOrDeleteTextBox.Text);
                string useTitle = empTitle.Text;
                string useFName = empFName.Text;
                string useLName = empLName.Text;
                string useInitial = empInitial.Text;
                string useArea = empArea.Text;
                string usePhone = empPhone.Text;
                string useEmail = empEmail.Text;
                double useRate = Double.Parse(empRate.Text);

                string updatingID = empUpdateOrDeleteTextBox.Text;
                string sql = "update Employee set emp_title = :Title, emp_fname = :FName, emp_lname = :LName, "
                           + "emp_initial = :Initial, emp_areacode = :Area, emp_phone = :Phone, emp_email = :Email, "
                           + "emp_rate = :Rate where emp_id = :Code";
                NpgsqlCommand updateQuery = new NpgsqlCommand(sql, conn);

                updateQuery.Parameters.Add(new NpgsqlParameter("Title", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Title"].Value = useTitle;
                updateQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["FName"].Value = useFName;
                updateQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["LName"].Value = useLName;
                updateQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Initial"].Value = useInitial;
                updateQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Area"].Value = useArea;
                updateQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Phone"].Value = usePhone;
                updateQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
                updateQuery.Parameters["Email"].Value = useEmail;
                updateQuery.Parameters.Add(new NpgsqlParameter("Rate", NpgsqlTypes.NpgsqlDbType.Double));
                updateQuery.Parameters["Rate"].Value = useRate;
                updateQuery.Parameters.Add(new NpgsqlParameter("Code", NpgsqlTypes.NpgsqlDbType.Integer));
                updateQuery.Parameters["Code"].Value = useCode;

                int rowsAffected = updateQuery.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with emp_id of " + updatingID + " has been updated in Employee table.");
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

        // Delete button will delete row based on input emp_code.
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            try
            {
                string deletingID = empUpdateOrDeleteTextBox.Text;
                string sql = ("delete from Employee where emp_id = " + deletingID + ";");
                NpgsqlCommand da = new NpgsqlCommand(sql, conn);
                int rowsAffected;

                rowsAffected = da.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with emp_id of " + deletingID + " has been deleted from Employee table.");
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
