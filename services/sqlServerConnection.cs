using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ApiPersonas.services
{
    public class sqlServerConnection
    {
        private static readonly string connectionString = "Server=NOMBRE_SERVIDOR_SOMEE;Database=NOMBRE_BD_SOMEE;User Id=TU_USUARIO_SOMEE;Password=TU_CONTRASEÑA_SOMEE;TrustServerCertificate=True;";
        public SqlConnection conn;

        public sqlServerConnection()
        {
            conn = new SqlConnection(connectionString);
        }

        public async Task<SqlConnection> ConnectionOpenAsync()
        {
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            return conn;
        }

        public async Task ConnectionCloseAsync()
        {
            if (conn.State != ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }
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