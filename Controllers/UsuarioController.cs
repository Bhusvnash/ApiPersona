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
						try
						{
								return Ok(await _usuarioRepository.GetAllAsync());
						}
						catch (Exception ex)
						{
								return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
						}
				}

				[HttpGet("{id}")] // GET Usuario/{id}
				public async Task<IActionResult> GetById(long id)
				{
						try
						{
								if (id <= 0) return BadRequest("Bad Request");
								var result = await _usuarioRepository.GetByIdAsync(id);
								if (result == null) return NotFound();
								return Ok(result);
						}
						catch (Exception ex)
						{
								return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
						}
				}

				[HttpPost] // POST Usuario/   
				public async Task<IActionResult> Create([FromBody] GetUsuario data)
				{
						try
						{
								if (data is null) return BadRequest(new { Error = "data is null" });
								
								data = new GetUsuario(data.Nombre, Encoder.HashPassword(data.Pass));
								var result = await _usuarioRepository.CreateAsync(data);
								
								if (!result.Item1)
								{
										return StatusCode(500, new { Error = "can't create user" });
								}
								
								return CreatedAtAction(nameof(GetById), new { id = result.Item2 }, new { Id = result.Item2, Nombre = data.Nombre });
						}
						catch (Exception ex)
						{
								return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
						}
				}

				[HttpPut("{id}")] // PUT Usuario/{id}
				public async Task<IActionResult> Update(long id, [FromBody] GetUsuario data)
				{
						try
						{
								if (id <= 0 || data is null) return BadRequest("Bad Request");
								
								data = new GetUsuario(data.Nombre, Encoder.HashPassword(data.Pass));
								
								if (!await _usuarioRepository.UpdateAsync(id, data)) 
								{
										return StatusCode(400, new { Message = "Error updating user or user not found" });
								}
								
								return Ok(new { Message = "User updated successfully" });
						}
						catch (Exception ex)
						{
								return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
						}
				}

				[HttpDelete("{id}")] // DELETE Usuario/{id}
				public async Task<IActionResult> Delete(long id)
				{
						try
						{
								if (id <= 0) return BadRequest("Bad Request");
								
								if (!await _usuarioRepository.DeleteAsync(id))
								{
										return StatusCode(400, new { Message = "Error deleting user or user not found" });
								}
								
								return NoContent();
						}
						catch (Exception ex)
						{
								return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
						}
				}
		}
}