using ApiPersonas.Models;

namespace ApiPersonas.Repositories
{

		public interface IUsuario
		{
				//promete crud sobre usuario {id,nombre,pass}
				public Task<List<Usuario>> GetAllAsync();
				public Task<Usuario?> GetByIdAsync(int id);

				public Task<bool> CreateAsync(Usuario usuario);
				public Task<bool> UpdateAsync(Usuario usuario);
				public Task<bool> DeleteAsync(int id);



		}
}