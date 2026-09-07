using MySql.Data.MySqlClient;
using System.Configuration;

namespace ApiPersonas.services
{
		public class MysqlConnection
		{
				public static readonly string connectionString = "Server=localhost;Database=db_personas;User=root;Password=;";
				public MySqlConnection conn;

				public MysqlConnection()
				{
						conn = new MySqlConnection(connectionString);

						conn.Open();
				}

				public MySqlConnection GetConnection()
				{
						if (conn.State != System.Data.ConnectionState.Open)
						{
								conn.Open();
						}
						return conn;
				}

				public void CloseConnection()
				{
						conn.Close();
				}
		}
}