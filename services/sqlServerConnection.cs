using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Mysqlx.Connection;

namespace ApiPersonas.services
{
    public class sqlServerConnection
    {
        private static readonly string connectionString = "Server=localhost,1433;Database=db_personas;User Id=sa;Password=Dunamer5684857.com;TrustServerCertificate=True;";
        public SqlConnection conn;

        public sqlServerConnection()
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        public SqlConnection ConnectionOpen()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }
        public void ConnectionClose()
        {
            if(conn.State != ConnectionState.Closed)
                conn.Close();
        }
    }
}