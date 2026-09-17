using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ApiPersonas.Models;
using ApiPersonas.services;

namespace ApiPersonas.Repositories
{
		public class SqlServerPersonaRepository : IPersonaRepository
		{
				/// <summary>
				/// Obtiene todas las personas de la base de datos
				/// </summary>
				public async Task<List<Persona>> GetAllAsync()
				{
						var sqlServer = new sqlServerConnection();
						var personas = new List<Persona>();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								string query = "SELECT id, nombre, telefono FROM personas";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												while (await reader.ReadAsync())
												{
														personas.Add(new Persona(
																reader.GetInt64("id"),
																reader.GetString("nombre"),
																reader.GetString("telefono")
														));
												}
										}
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
						return personas;
				}

				/// <summary>
				/// Obtiene una persona por su ID en la base de datos
				/// </summary>
				public async Task<Persona?> GetByIdAsync(long id)
				{
						Persona? persona = null;
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								string query = "SELECT id, nombre, telefono FROM personas WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										using (var reader = await cmd.ExecuteReaderAsync())
										{
												if (await reader.ReadAsync())
												{
														persona = new Persona(
																reader.GetInt64("id"),
																reader.GetString("nombre"),
																reader.GetString("telefono")
														);
												}
										}
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
						return persona;
				}

				/// <summary>
				/// Inserta una Persona en la db
				/// </summary>
				public async Task<(bool, long)> CreateAsync(Persona persona)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "INSERT INTO personas (nombre, telefono) OUTPUT INSERTED.id VALUES (@nombre, @telefono)";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
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

				/// <summary>
				/// Actualiza una persona en la base de datos
				/// </summary>
				public async Task<bool> UpdateAsync(Persona persona)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								string query = "UPDATE personas SET nombre = @nombre, telefono = @telefono WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
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
								await sqlServer.ConnectionCloseAsync();
						}
				}

				/// <summary>
				/// Elimina una persona por ID
				/// </summary>
				public async Task<bool> DeleteByIdAsync(long id)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								await sqlServer.ConnectionOpenAsync();
								var query = "DELETE FROM personas WHERE id = @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										int rowsAffected = await cmd.ExecuteNonQueryAsync();
										return rowsAffected > 0;
								}
						}
						finally
						{
								await sqlServer.ConnectionCloseAsync();
						}
				}
		}
}