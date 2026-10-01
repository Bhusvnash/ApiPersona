using ApiPersonas.Models;
using ApiPersonas.Repositories;
using ApiPersonas.services;
using Microsoft.AspNetCore.Mvc.Routing;
using MySql.Data.MySqlClient;

namespace ApiPersonas.Repositories
{


		internal class MysqlUsuarioRepository : IUsuarioRepository
		{
				public async Task<List<SendUsuario>> GetAllAsync()
				{
						var usuarios = new List<SendUsuario>();
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "SELECT id, nombre FROM usuario";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{
														usuarios.Add(
																new SendUsuario
																(
																		reader.GetInt64(reader.GetOrdinal("id")),
																		reader.GetString(reader.GetOrdinal("nombre"))
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

				public async Task<SendUsuario?> GetByIdAsync(long id)
				{
						SendUsuario? user = null;
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								var query = "SELECT id , nombre FROM usuario WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("id", @id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														user = new SendUsuario(
																reader.GetInt64(reader.GetOrdinal("id")),
																reader.GetString(reader.GetOrdinal("nombre"))
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

				public async Task<(bool, long?)> CreateAsync(GetUsuario user)
				{
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "INSERT INTO usuario (nombre, pass) VALUES (@nombre, @pass)";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", user.Nombre);
										cmd.Parameters.AddWithValue("@pass", user.Pass);
										int rowsAffected = await cmd.ExecuteNonQueryAsync();
										return (rowsAffected > 0, cmd.LastInsertedId);
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}

				public async Task<bool> UpdateAsync(long id, GetUsuario usuario)
				{
						var mysql = new MysqlConnection();
						try 
						{
								await mysql.OpenAsync();
								var query = "UPDATE usuario SET nombre = @nombre, pass = @pass WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
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
				public async Task<bool> DeleteAsync(long id)
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