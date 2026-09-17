using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiPersonas.Models;
using ApiPersonas.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace ApiPersonas.Controllers
{
		[ApiController]
		[Route("[controller]")]
		public class PersonaController : ControllerBase
		{
				private readonly IPersonaRepository _repository;

				public PersonaController(IPersonaRepository repository)
				{
						_repository = repository;
				}

				[HttpGet("/")] // GET /
				public IActionResult Index()
				{
						var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
						if (!System.IO.File.Exists(ruta))
						{
								return NotFound();
						}
						return PhysicalFile(ruta, "text/html");
				}

				[HttpGet] // GET /persona 
				public async Task<IActionResult> GetAll()
				{
						var personas = await _repository.GetAllAsync();
						return Ok(personas);
				}

				[HttpGet("{id}")] // GET /persona/{id}
				public async Task<IActionResult> GetById(long id)
				{
						var persona = await _repository.GetByIdAsync(id);
						if (persona is null)
						{
								return NotFound();
						}
						return Ok(persona);
				}

				[HttpPost] // POST /persona
				public async Task<IActionResult> Create([FromBody] Persona persona)
				{
						var (success, id) = await _repository.CreateAsync(persona);
						if (!success)
						{
								return BadRequest();
						}
						persona.Id = id;
						return CreatedAtAction(nameof(GetById), new { id = id }, persona);
				}

				[HttpPut("{id}")] // PUT /persona/{id}
				public async Task<IActionResult> Update(long id, [FromBody] Persona persona)
				{
						if (id != persona.Id)
						{
								return BadRequest();
						}
						var result = await _repository.UpdateAsync(persona);
						if (!result)
						{
								return NotFound();
						}
						return NoContent();
				}

				[HttpDelete("{id}")] // DELETE /persona/{id}
				public async Task<IActionResult> Delete(long id)
				{
						var result = await _repository.DeleteByIdAsync(id);
						if (!result)
						{
								return NotFound();
						}
						return NoContent();
				}
		}
}