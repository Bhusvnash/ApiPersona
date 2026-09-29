using ApiPersonas.Models;
using ApiPersonas.Repositories;
using ApiPersonas.services;
using Microsoft.AspNetCore.Mvc.Routing;
using MySql.Data.MySqlClient;

namespace ApiPersonas.Repositories
{


		internal class MysqlUsuarioRepository : IUsuario
		{
				public async Task<List<Usuario>> GetAllAsync()
				{
						var usuarios = new List<Usuario>();
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "SELECT id, nombre, pass FROM usuarios";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{
														usuarios.Add(
																new
																(
																		reader.GetInt64(reader.GetOrdinal("id")),
																		reader.GetString(reader.GetOrdinal("nombre")),
																		reader.GetString(reader.GetOrdinal("pass"))
																)
														);
												}
										}
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
						return usuarios;
				}

				public async Task<Usuario?> GetByIdAsync(int id)
				{
						Usuario? user = null;
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								var query = "SELECT id , nombre , pass FROM usuario WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("id", @id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														user = new Usuario(
																reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre")),
																reader.GetString(reader.GetOrdinal("pass"))
														);
												}
										}
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
						return user;
				}
				public async Task<bool> CreateAsync(Usuario usuario)
				{
						var mysql = new MysqlConnection();

						try
						{
								await mysql.CloseAsync();
								var query = "INSERT INTO usuario (nombre,pass) Values (@nombre,@pass)";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("nombre", usuario.Nombre);
										cmd.Parameters.AddWithValue("pass", usuario.Pass);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}
				public async Task<bool> UpdateAsync(Usuario usuario)
				{
						var mysql = new MysqlConnection();
						try
						{

								await mysql.OpenAsync();
								var query = "UPDATE usuario SET nombre = @nombre, pass = @pass WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", usuario.Id);
										cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
										cmd.Parameters.AddWithValue("@pass", usuario.Pass);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}
				public async Task<bool> DeleteAsync(int id)
				{
						var mysql = new MysqlConnection();
						try
						{
							await mysql.OpenAsync();

								var query = "DELETE FROM usuario WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										return await cmd.ExecuteNonQueryAsync() > 0;
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}
		}
}