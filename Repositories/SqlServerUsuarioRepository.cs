using ApiPersonas.Models;
using ApiPersonas.services;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace ApiPersonas.Repositories
{
		public class SqlServerUsuarioRepository : IUsuarioRepository
		{
				public async Task<List<SendUsuario>> GetAllAsync()
				{
						var usuarios = new List<SendUsuario>();
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "SELECT id , nombre FROM usuario";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{
														usuarios.Add(new SendUsuario(
															 reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre"))
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

				public async Task<SendUsuario?> GetByIdAsync(long id)
				{
						SendUsuario? user = null;
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "SELECT id , nombre FROM usuario WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														user = new SendUsuario(
															 reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre"))
													);
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

				public async Task<(bool, long?)> CreateAsync(GetUsuario user)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "INSERT INTO usuario (nombre, pass) OUTPUT INSERTED.id VALUES (@nombre, @pass)";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", user.Nombre);
										cmd.Parameters.AddWithValue("@pass", user.Pass);
										var result = await cmd.ExecuteScalarAsync();
										long idGenerado = result != null ? Convert.ToInt64(result) : 0;
										return (idGenerado > 0, idGenerado);
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}

				public async Task<bool> UpdateAsync(long id, GetUsuario usuario)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "UPDATE usuario SET nombre = @nombre, pass = @pass WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
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