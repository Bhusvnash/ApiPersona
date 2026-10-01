 using Microsoft.AspNetCore.Http;
using ApiPersonas.services;
using Microsoft.AspNetCore.Mvc;
using ApiPersonas.Repositories;
using ApiPersonas.Models;

namespace ApiPersonas.Controllers
{
		[Route("[controller]")]
		[ApiController]
		public class UsuarioController : ControllerBase
		{
				private readonly IUsuarioRepository _usuarioRepository;

				public UsuarioController(IUsuarioRepository usuarioRepository)
				{
						_usuarioRepository = usuarioRepository;
				}

				[HttpGet] // GET Usuario/
				public async Task<IActionResult> GetAll()
				{
						return Ok(await _usuarioRepository.GetAllAsync());
				}

				[HttpGet("{id}")] // GET Usuario/{id}
				public async Task<IActionResult> GetById(long id)
				{
						if (id <= 0) return BadRequest("Bad Request");
						return Ok(await _usuarioRepository.GetByIdAsync(id));
				}


				
				[HttpPost] // POST Usuario/   
				public async Task<IActionResult> Create([FromBody] DtoUsuario data)
				{
						if (data is null) return NotFound(new { Error = "data is null" });
						data = new DtoUsuario(data.Nombre, Encoder.HashPassword(data.Pass));
						var result = await _usuarioRepository.CreateAsync(data);
						if (!result.Item1)
						{
								return StatusCode(500,new {Error="cant'n create user" });
						}
						return Created($"/Usuarios/{result.Item2}", new { Id = result.Item2, Nombre = data.Nombre, Pass = data.Pass });
				}

				[HttpPut] // PUT Usuario
				public async Task<IActionResult> Update([FromBody] Usuario data)
				{
						if (data is null) return BadRequest("Bad Request");
						data.Pass = Encoder.HashPassword(data.Pass);
						if (!await _usuarioRepository.UpdateAsync(data)) return StatusCode(400, new { Message = "Error updating user" });
						return Created($"/Usuarios/{data.Id}", data);
				}

				[HttpDelete("{id}")] // DELETE Usuario/{id}
				public async Task<IActionResult> Delete(long id)
				{
						if (id <= 0) return BadRequest("Bad Request");
						if (!await _usuarioRepository.DeleteAsync(id))
						{
								return StatusCode(400, new { Message = "Error deleting user" });
						}
						return NoContent();
				}
		}
}