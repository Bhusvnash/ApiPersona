using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace ApiPersonas.services
{
		public class MysqlConnection
		{
				public static readonly string connectionString = "Server=localhost;Database=db_personas;User=root;Password=;";
				public MySqlConnection conn;

				public MysqlConnection()
				{
						conn = new MySqlConnection(connectionString);
				}

				public async Task<MySqlConnection> OpenAsync()
				{
						if (conn.State != System.Data.ConnectionState.Open)
						{
								await conn.OpenAsync();
						}
						return conn;
				}

				public async Task CloseAsync()
				{
						if (conn.State != System.Data.ConnectionState.Closed)
						{
								await conn.CloseAsync();
						}
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