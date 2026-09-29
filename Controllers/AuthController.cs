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

				[HttpPost]
				[Route("/login")]
				[Route("/")]
				public IActionResult LoginValidate([FromBody] DtoLogin data)
				{
						//valida el pass y usuario ;
						return StatusCode(200, new { nombre = data.nombre, pass = data.pass});
				}
		}
}
