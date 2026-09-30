using ApiPersonas.Models;
using ApiPersonas.services;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace ApiPersonas.Repositories
{
		public class SqlServerUsuarioRepository : IUsuarioRepository
		{
				public async Task<List<Usuario>> GetAllAsync()
				{
						var usuarios = new List<Usuario>();
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "SELECT id , nombre , pass FROM usuario";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{
														usuarios.Add(new Usuario(
															 reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre")),
																reader.GetString(reader.GetOrdinal("pass"))
														));
												}
												return usuarios;
										}
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}

				public async Task<Usuario?> GetByIdAsync(long id)
				{
						Usuario? user = null;
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "SELECT id , nombre , pass FROM usuario WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														user = new Usuario(
															 reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre")),
																reader.GetString(reader.GetOrdinal("pass")
													));
												}
												return user;
										}
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}

				public async Task<bool> CreateAsync(DtoUsuario usuario)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "INSERT INTO  usuario(nombre, pass) 	VALUES(@nombre, @pass)";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
										cmd.Parameters.AddWithValue("@pass", usuario.Pass);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}			
				public async Task<bool> UpdateAsync(Usuario usuario)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "UPDATE usuario SET nombre = @nombre, pass = @pass WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", usuario.Id);
										cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
										cmd.Parameters.AddWithValue("@pass", usuario.Pass);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}

				public async Task<bool> DeleteAsync(long id)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "DELETE FROM usuario WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}
		}
}