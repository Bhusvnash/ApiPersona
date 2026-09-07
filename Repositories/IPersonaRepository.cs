using ApiPersonas.Models;

namespace ApiPersonas.Repositories
{
		public interface IPersonaRepository
		{
				public List<Persona> GetAll();

				public Persona? GetById(long id);

				public (bool, long) Create(Persona persona);

				public bool Update(Persona persona);

				public bool DeleteById(long id);
		}
}