using ApiPersonas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace ApiPersonas.Controllers
{
		[Route("[controller]")]
		[ApiController]
		public class AuthController : ControllerBase
		{
				public IPersonaRepository _repositories;
				public AuthController(IPersonaRepository repo)
				{
						this._repositories = repo;
				}
		}
}
