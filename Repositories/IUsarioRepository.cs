using ApiPersonas.Models;

namespace ApiPersonas.Repositories
{

		public interface IUsuarioRepository
		{
				//promete crud sobre usuario {id,nombre,pass} y DtoUsuario {nombre,pass}
				public Task<List<Usuario>> GetAllAsync();
				public Task<Usuario?> GetByIdAsync(long id);

				public Task<(bool,long?)> CreateAsync(DtoUsuario usuario);
				public Task<bool> UpdateAsync(Usuario usuario);
				public Task<bool> DeleteAsync(long id);

		}
}