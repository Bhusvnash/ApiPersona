using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ApiPersonas.Models;
using ApiPersonas.services;

namespace ApiPersonas.Repositories
{
		/// <summary>
		/// Repository class para manejar Persona entity en la base de datos
		/// </summary>
		public class MysqlPersonaRepository : IPersonaRepository
		{
				/// <summary>
				/// Obtiene todas las personas de la base de datos
				/// </summary>
				public async Task<List<Persona>> GetAllAsync()
				{
						var personas = new List<Persona>();
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "SELECT id, nombre, telefono FROM personas";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{	personas.Add(
														new Persona(
															reader.GetInt64(reader.GetOrdinal("id")),
															reader.GetString(reader.GetOrdinal("nombre")),
															reader.GetString(reader.GetOrdinal("telefono"))
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
						return personas;
				}

				/// <summary>
				/// Obtiene una persona por su ID de la base de datos
				/// </summary>
				public async Task<Persona?> GetByIdAsync(long id)
				{
						Persona? persona = null;
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "SELECT id, nombre, telefono FROM personas WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														persona = new Persona(
															reader.GetInt64(reader.GetOrdinal("id")),
															reader.GetString(reader.GetOrdinal("nombre")),
															reader.GetString(reader.GetOrdinal("telefono"))
														);
												}
										}
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
						return persona;
				}

				/// <summary>
				/// Inserta una Persona en la db 
				/// </summary>
				public async Task<(bool, long)> CreateAsync(Persona persona)
				{
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "INSERT INTO personas (nombre, telefono) VALUES (@nombre, @telefono)";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
										int rowsAffected = await cmd.ExecuteNonQueryAsync();
										return (rowsAffected > 0, cmd.LastInsertedId);
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}

				/// <summary>
				/// Actualiza una persona en la base de datos
				/// </summary>
				public async Task<bool> UpdateAsync(Persona persona)
				{
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "UPDATE personas SET nombre = @nombre, telefono = @telefono WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", persona.Id);
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);

										int rowsAffected = await cmd.ExecuteNonQueryAsync();
										return rowsAffected > 0;
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}

				/// <summary>
				/// Elimina una persona de la base de datos por su ID
				/// </summary>
				public async Task<bool> DeleteByIdAsync(long id)
				{
						var mysql = new MysqlConnection();
						try
						{
								await mysql.OpenAsync();
								string query = "DELETE FROM personas WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);

										int rowsAffected = await cmd.ExecuteNonQueryAsync();
										return rowsAffected > 0;
								}
						}
						finally
						{
								await mysql.CloseAsync();
						}
				}
		}
}

