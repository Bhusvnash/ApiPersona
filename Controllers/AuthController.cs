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
				[HttpGet("/")]
				public IActionResult SendLogin()
				{
						return Redirect("/auth/login.html");
				}
		}
}