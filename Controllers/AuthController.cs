using ApiPersonas.Models;
using ApiPersonas.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiPersonas.Controllers
{
		[Route("[controller]")]
		[ApiController]
		public class AuthController : ControllerBase
		{
				private readonly IUsuarioRepository _repositories;

				public AuthController(IUsuarioRepository repositories)
				{
						_repositories = repositories;
				}

				[HttpPost("login")]
				public async Task<IActionResult> Login([FromBody] GetUsuario user)
				{
						//validar data
						if (user is null ||
						string.IsNullOrEmpty(user.Nombre) ||
						string.IsNullOrEmpty(user.Pass) )
								return BadRequest(new { Mensaje = "Por favor, ingrese todos los datos" });
						long? idUser= await _repositories.GetByNombreAsync(user.Nombre);
						if (idUser is null || idUser==0)
						{
								return BadRequest(new { Mensaje = "Nombre o contrasena incorrectos" });
						}
						string hashPass = await _repositories.GetPassAsync(idUser.Value);
						if (hashPass is null)
						{
								return BadRequest(new { Mensaje = "Nombre o contrasena incorrectos" });
						}
						if (!services.Encoder.VerifyPassword(user.Pass, hashPass))
						{
							return BadRequest(new { Mensaje = "Nombre o contrasena incorrectos" });
						}
						return Ok(new { Mensaje = "Login exitoso" });
				}
				//mandar login
				[HttpGet("/")]
				public IActionResult SendLogin()
				{
						//send static files se engarga de wwwroot/auth
						return Redirect("/auth/login.html");
				}
		}
}