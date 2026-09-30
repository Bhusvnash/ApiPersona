using ApiPersonas.Repositories;
using ApiPersonas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace ApiPersonas.Controllers
{
		[Route("[controller]")]
		[ApiController]
		public class AuthController : ControllerBase
		{
				//clase para procesar un login

				public IPersonaRepository _repositories;

				public AuthController(IPersonaRepository repo)
				{
						this._repositories = repo;
				}

				//mandar login
				[HttpGet("/login")]
				public IActionResult MandarLogin()
				{
						var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "auth", "login.html");
						if (!System.IO.File.Exists(rutaArchivo))
						{
								return NotFound("El archivo no existe.");
						}
						return PhysicalFile(rutaArchivo, "text/html");
				}
				/*
				[HttpPost]
				[Route("/login")]  // POST Auth/login
				public IActionResult LoginValidate([FromBody] DtoUsuario data)
				{
						//valida el pass y usuario;
						return StatusCode(200, new { nombre = data.Nombre, pass = data.Pass});
				}
				*/
		}
}