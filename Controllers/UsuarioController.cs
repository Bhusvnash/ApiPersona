using Microsoft.AspNetCore.Http;
using ApiPersonas;
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
						if (data is null) return BadRequest("Bad Request");

						return Ok(await _usuarioRepository.CreateAsync(data));
				}

				[HttpPut] // PUT Usuario
				public async Task<IActionResult> Update([FromBody] Usuario data)
				{
						if (data is null) return BadRequest("Bad Request");
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