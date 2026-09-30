namespace ApiPersonas.Models
{	
		public class Usuario
		{
			public long Id { get; set; } 
			public string Nombre  { get; set; } 
			public string Pass { get; set; } 

			public Usuario(long id, string nombre, string pass)
			{
				this.Id = id;
				this.Nombre = nombre;
				this.Pass = pass;
			}
		}
		public record DtoUsuario(string Nombre, string Pass);
}
