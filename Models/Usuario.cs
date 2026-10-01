namespace ApiPersonas.Models
{	
		public record GetUsuario(string Nombre, string Pass);
		public record SendUsuario(long Id, string Nombre);
}
