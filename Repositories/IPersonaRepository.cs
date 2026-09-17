using System.Collections.Generic;
using System.Threading.Tasks;
using ApiPersonas.Models;

namespace ApiPersonas.Repositories
{
		public interface IPersonaRepository
		{
				public Task<List<Persona>> GetAllAsync();

				public Task<Persona?> GetByIdAsync(long id);

				public Task<(bool, long)> CreateAsync(Persona persona);

				public Task<bool> UpdateAsync(Persona persona);
				
				public Task<bool> DeleteByIdAsync(long id);
		}
}