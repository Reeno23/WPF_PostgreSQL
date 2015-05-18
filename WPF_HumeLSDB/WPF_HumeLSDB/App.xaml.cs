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
using System.Resources;
using Npgsql;

namespace WPF_HumeLSDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static NpgsqlConnection openConn()
        {
            string connString = ("Server=192.168.1.105; Port=5432; User Id=postgres; Password=chum2087$; Database=hume;");
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            return conn;
        }

        public static void closeConn(NpgsqlConnection conn)
        {
            conn.Close();
        }

    }
}
