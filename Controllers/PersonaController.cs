using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiPersonas.Models;
using ApiPersonas.Repositories;
using ApiPersonas;
using System.IO;
using System;

namespace ApiPersonas.Controllers
{
		[ApiController]
		[Route("[controller]")]
		public class PersonaController : ControllerBase
		{
				//	private static SqlServerPersonaRepository repository = new SqlServerPersonaRepository();
				private static MysqlPersonaRepository repository = new MysqlPersonaRepository();

				[HttpGet("/")] // GET /
				public IActionResult index()
				{
						var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
						if (!System.IO.File.Exists(ruta))
						{
								return NotFound();
						}
						return PhysicalFile(ruta, "text/html");
				}

				[HttpGet] // GET /persona/
				public IActionResult GetAll()
				{
						return Ok(repository.GetAll());
				}

				[HttpGet("{id}")] // GET /persona/{id}
				public IActionResult GetById(long id)
				{
						var persona = repository.GetById(id);
						if (persona is null)
						{
								return NotFound();
						}
						return Ok(persona);
				}

				[HttpPost] //post /persona/
				public IActionResult Create([FromBody] Persona persona)
				{
						var (success, id) = repository.Create(persona);
						if (!success)
						{
								return BadRequest();
						}
						persona.Id = id;
						return CreatedAtAction(nameof(GetById), new { id = id }, persona);
				}

				[HttpPut("{id}")] // PUT /persona/{id}
				public IActionResult Update(long id, [FromBody] Persona persona)
				{
						if (id != persona.Id)
						{
								return BadRequest();
						}
						var result = repository.Update(persona);
						if (!result)
						{
								return NotFound();
						}
						return NoContent();
				}
		}
}