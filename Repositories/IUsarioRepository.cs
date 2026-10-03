using ApiPersonas.Models;

namespace ApiPersonas.Repositories
{

		public interface IUsuarioRepository
		{
				//promete crud sobre SendUsuario {id,nombre} y GetUsuario {nombre,pass}
				public Task<List<SendUsuario>> GetAllAsync();
				public Task<SendUsuario?> GetByIdAsync(long id);

				public Task<(bool,long?)> CreateAsync(GetUsuario usuario);
				public Task<bool> UpdateAsync(long id, GetUsuario usuario);
				public Task<bool> DeleteAsync(long id);

				//for login 
				public Task<long?> GetByNombreAsync(string nombre);
				public Task<string> GetPassAsync(long id);


		}
}