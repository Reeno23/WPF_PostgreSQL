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
            empInsertBtn.SetValue(Canvas.TopProperty, empWindow.Height * .08);
            // Update button, delete button, and corresponding textbox. Will update or delete row in database based on given empCode.
            empUpdateBtn.Width = 75;
            empUpdateBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empUpdateBtn.SetValue(Canvas.TopProperty, empWindow.Height * .30);
            empDeleteBtn.Width = 75;
            empDeleteBtn.SetValue(Canvas.LeftProperty, empWindow.Width * .88);
            empDeleteBtn.SetValue(Canvas.TopProperty, empWindow.Height * .35);
            empUpdateOrDeleteTextBox.Width = 80;
            empUpdateOrDeleteTextBox.SetValue(Canvas.LeftProperty, empWindow.Width * .79);
            empUpdateOrDeleteTextBox.SetValue(Canvas.TopProperty, empWindow.Height * .325);
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
            empEnterDataLabel.Margin = new Thickness(-7, 0, 0, 10);
            empEnterDataLabel.SetValue(FontWeightProperty, FontWeights.Bold);
            empEnterDataLabel.FontSize = 16;

            // All textbox fields used to insert new row into database. 
            empTitle.Width = empStackPanel.Width * .4;
            empTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empFName.Width = empStackPanel.Width * .4;
            empFName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empFName.Margin = new Thickness(0, -21, 0, 5);
            empLName.Width = empStackPanel.Width * .4;
            empLName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empInitial.Width = empStackPanel.Width * .4;
            empInitial.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empInitial.Margin = new Thickness(0, -21, 0, 5);
            empArea.Width = empStackPanel.Width * .4;
            empArea.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empPhone.Width = empStackPanel.Width * .4;
            empPhone.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            empPhone.Margin = new Thickness(0, -21, 0, 5);
            empEmail.Width = empStackPanel.Width * .4;
            empEmail.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            empRate.Width = empStackPanel.Width * .4;
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

        // Insert button will insert row into database based on input information.
        private void insertClick(object sender, RoutedEventArgs e)
        {
            string useTitle = empTitle.Text;
            string useFName = empFName.Text;
            string useLName = empLName.Text;
            string useInitial = empInitial.Text;
            int useArea = Int32.Parse(empArea.Text);
            string usePhone = empPhone.Text;
            string useEmail = empEmail.Text;
            double useRate = Double.Parse(empRate.Text);


            using (NpgsqlConnection conn = App.openConn())
            {

                using (NpgsqlCommand insertQuery = new NpgsqlCommand("insert into Employee ( "
                            + "emp_title, emp_fname, emp_lname, emp_initial, "
                            + "emp_areacode, emp_phone, emp_email, emp_rate) VALUES ("
                            + ":Title, :FName, :LName, :Initial, :Area, :Phone, :Email, :Rate)", conn))
                {

                    insertQuery.Parameters.Add(new NpgsqlParameter("Title", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["Title"].Value = useTitle;
                    insertQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["FName"].Value = useArea;
                    insertQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["LName"].Value = usePhone;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["Initial"].Value = useInitial;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Integer));
                    insertQuery.Parameters["Area"].Value = useArea;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["Phone"].Value = usePhone;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
                    insertQuery.Parameters["Email"].Value = useEmail;
                    insertQuery.Parameters.Add(new NpgsqlParameter("Rate", NpgsqlTypes.NpgsqlDbType.Double));
                    insertQuery.Parameters["Rate"].Value = useRate;

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

        // Update button will update row based on input emp_code with input fields.
        private void updateClick(object sender, RoutedEventArgs e)
        {
            int useCode = Int32.Parse(empUpdateOrDeleteTextBox.Text);
            string useTitle = empTitle.Text;
            string useFName = empFName.Text;
            string useLName = empLName.Text;
            string useInitial = empInitial.Text;
            int useArea = Int32.Parse(empArea.Text);
            string usePhone = empPhone.Text;
            string useEmail = empEmail.Text;
            double useRate = Double.Parse(empRate.Text);

            NpgsqlConnection conn = App.openConn();
            string updatingID = empUpdateOrDeleteTextBox.Text;
            string sql = "update Employee set emp_title = :Title, emp_fname = :FName, emp_lname = :LName, "
                       + "emp_initial = :Initial, emp_areacode = :Area, emp_phone = :Phone, emp_email = :Email, "
                       + "emp_rate = :Rate where emp_id = :Code";
            NpgsqlCommand updateQuery = new NpgsqlCommand(sql, conn);

            updateQuery.Parameters.Add(new NpgsqlParameter("Title", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["Title"].Value = useTitle;
            updateQuery.Parameters.Add(new NpgsqlParameter("FName", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["FName"].Value = useArea;
            updateQuery.Parameters.Add(new NpgsqlParameter("LName", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["LName"].Value = usePhone;
            updateQuery.Parameters.Add(new NpgsqlParameter("Initial", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["Initial"].Value = useInitial;
            updateQuery.Parameters.Add(new NpgsqlParameter("Area", NpgsqlTypes.NpgsqlDbType.Integer));
            updateQuery.Parameters["Area"].Value = useArea;
            updateQuery.Parameters.Add(new NpgsqlParameter("Phone", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["Phone"].Value = usePhone;
            updateQuery.Parameters.Add(new NpgsqlParameter("Email", NpgsqlTypes.NpgsqlDbType.Varchar));
            updateQuery.Parameters["Email"].Value = useEmail;
            updateQuery.Parameters.Add(new NpgsqlParameter("Rate", NpgsqlTypes.NpgsqlDbType.Double));
            updateQuery.Parameters["Rate"].Value = useRate;
            updateQuery.Parameters.Add(new NpgsqlParameter("Code", NpgsqlTypes.NpgsqlDbType.Integer));
            updateQuery.Parameters["Code"].Value = useCode;

            try
            {
                int rowsAffected = updateQuery.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with emp_id of " + updatingID + " has been updated in Employee table.");
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

        // Delete button will delete row based on input emp_code.
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = App.openConn();
            string deletingID = empUpdateOrDeleteTextBox.Text;
            string sql = ("delete from Employee where emp_id = " + deletingID + ";");
            NpgsqlCommand da = new NpgsqlCommand(sql, conn);
            int rowsAffected;

            try
            {
                rowsAffected = da.ExecuteNonQuery();
                string dialog = (rowsAffected + " record(s) with emp_id of " + deletingID + " has been deleted from Employee table.");
                MessageBox.Show(dialog);

            }
            finally
            {
                App.closeConn(conn);
            }
        }
    }
}
