using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            string connString = ("Server=localhost; Port=5432; User Id=postgres; Password=chum2087$; Database=hume;");
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
