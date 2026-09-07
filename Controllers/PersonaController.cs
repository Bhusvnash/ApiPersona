using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiPersonas.Models;
using ApiPersonas;
using System;

namespace ApiPersonas.Controllers
{
		[ApiController]
		[Route("[controller]")]
		public class PersonaController : ControllerBase
		{
				private static Repositories.SqlServerPersonaRepository repository = new Repositories.SqlServerPersonaRepository();

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
		}
}