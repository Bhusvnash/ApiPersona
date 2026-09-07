using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ApiPersonas.Models;
using ApiPersonas.services;

namespace ApiPersonas.Repositories
{
		/// <summary>
		/// Repository class para manejar Persona entiti en la base de datos
		/// </summary>

		public class MysqlPersonaRepository:IPersonaRepository
		{
				/// <summary>
				/// Obtiene todas las personas de la base de datos
				/// </summary>
				/// <returns>
				/// lista de objetos Persona
				/// </returns>
				public List<Persona> GetAll()
				{
						var personas = new List<Persona>();
						var mysql = new MysqlConnection();
						try
						{
								string query = "SELECT id, nombre, telefono FROM personas";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										using (var reader = cmd.ExecuteReader())
										{
												while (reader.Read())
												{
														long id = reader.GetInt64("id");
														string nombre = reader.GetString("nombre");
														string telefono = reader.GetString("telefono");
														personas.Add(new Persona(id, nombre, telefono));
												}
										}
								}
						}
						finally
						{
								mysql.CloseConnection();
						}
						return personas;
				}
				/// <summary>
				/// Obtiene una persona por su ID de la base de datos
				/// </summary>
				/// <param name="id"></param>
				/// <returns>
				/// persona con el ID especificado, o null si no se encuentra
				/// </returns>
				public Persona? GetById(long id)
				{
						Persona? persona = null;
						var mysql = new MysqlConnection();
						try
						{
								string query = "SELECT id, nombre, telefono FROM personas WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										using (var reader = cmd.ExecuteReader())
										{
												if (reader.Read())
												{
														long personaId = reader.GetInt64("id");
														string nombre = reader.GetString("nombre");
														string telefono = reader.GetString("telefono");
														persona = new Persona(personaId, nombre, telefono);
												}
										}
								}
						}
						finally
						{
								mysql.CloseConnection();
						}
						return persona;
				}


				/// <summary>
				/// inserta una Persona  en la db 
				/// </summary>
				/// <param name="persona"></param>
				/// <returns>bool segun resultado del insert </returns>
				public (bool,long) Create(Persona persona)
				{
						var mysql = new MysqlConnection();
						try
						{
								string query = "INSERT INTO personas (nombre, telefono) VALUES (@nombre, @telefono)";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
										int rowsAffected = cmd.ExecuteNonQuery();
										return (rowsAffected > 0, cmd.LastInsertedId);
								}
						}
						finally
						{
								mysql.CloseConnection();
						}
				}
				/// <summary>
				/// Actualiza una persona en la base de datos
				/// </summary>
				/// <param name="persona"></param>
				/// <returns>
				/// verdadero si la actualizaci�n fue exitosa, falso de lo contrario
				/// </returns>
				public bool Update(Persona persona)
				{
						var mysql = new MysqlConnection();
						try
						{
								string query = "UPDATE personas SET nombre = @nombre, telefono = @telefono WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", persona.Id);
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);

										int rowsAffected = cmd.ExecuteNonQuery();
										return rowsAffected > 0;
								}
						}
						finally
						{
								mysql.CloseConnection();
						}
				}
				/// <summary>
				/// Elimina una persona de la base de datos por su ID
				/// </summary>
				/// <param name="id"></param>
				/// <returns>
				/// verdadero si la eliminaci�n fue exitosa, falso de lo contrario
				/// </returns>
				public bool DeleteById(long id)
				{
						var mysql = new MysqlConnection();
						try
						{
								string query = "DELETE FROM personas WHERE id = @id";
								using (var cmd = new MySqlCommand(query, mysql.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);

										int rowsAffected = cmd.ExecuteNonQuery();
										return rowsAffected > 0;
								}
						}
						finally
						{
								mysql.CloseConnection();
						}
				}
		}
}
