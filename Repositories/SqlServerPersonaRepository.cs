using ApiPersonas.Models;
using ApiPersonas.services;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiPersonas.Repositories
{
		public class SqlServerPersonaRepository : IPersonaRepository
		{
				/// <summary>
				/// Obtiene todas las personas de la base de datos
				/// </summary>
				/// <returns>
				/// lista de objetos Persona
				/// </returns>
				public List<Persona> GetAll()
				{
						var sqlServer = new sqlServerConnection();
						var personas = new List<Persona>();
						try
						{
								sqlServer.ConnectionOpen();
								string query = "select * from personas";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										var reader = cmd.ExecuteReader();
										while (reader.Read())
										{
												personas.Add(new Persona(
														reader.GetInt64("id"),
														reader.GetString("nombre"),
														reader.GetString("telefono")
												));
										}
								}
						}
						finally
						{
								sqlServer.ConnectionClose();
						}
						return personas;
				}

				/// <summary>
				/// Obtiene una persona por si ID en la base de datos
				/// </summary>
				/// <param name="id"></param>
				/// <returns> Persona where ID or Null</returns>
				public Persona? GetById(long id)
				{
						Persona? persona = null;
						var sqlServer = new sqlServerConnection();
						try
						{
								string query = "Select * from personas where id= @id";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);

										var reader = cmd.ExecuteReader();
										while (reader.Read())
										{
												
												persona = new Persona(

														reader.GetInt64("id"),
														reader.GetString("nombre"),
														reader.GetString("telefono")
												);
										}
								}
						}
						finally
						{
								sqlServer.ConnectionClose();
						}
						return persona;
				}
				/// <summary>
				/// inserta una Persona  en la db
				/// </summary>
				/// <param name="persona"></param>
				/// <returns>bool segun resultado del insert </returns>
				public (bool, long) Create(Persona persona)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								var query = "INSERT INTO personas(nombre, telefono) OUTPUT INSERTED.id VALUES(@nombre, @telefono)";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
										var idGenerado = Convert.ToInt64(cmd.ExecuteScalar());
										return (idGenerado > 0, idGenerado);
								}
						}
						finally
						{
								sqlServer.ConnectionClose();
						}
				}
				public bool Update(Persona persona)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								string query = "UPDATE personas SET nombre = @nombre , telefono =@telefono  WHERE od = @id;";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
										cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
										return cmd.ExecuteNonQuery() > 0;
								}
						}
						finally
						{
								sqlServer.ConnectionClose();
						}
				}
				public bool DeleteById(long id)
				{
						var sqlServer = new sqlServerConnection();
						try
						{
								var query = "DELETE  personas  where id = @id;";
								using (var cmd = new SqlCommand(query, sqlServer.conn))
								{
										cmd.Parameters.AddWithValue("@id", id);
										return cmd.ExecuteNonQuery() > 0;
								}
						}
						finally
						{
								sqlServer.ConnectionClose();
						}
				}
		}
}