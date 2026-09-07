namespace ApiPersonas.Models
{
    public class Persona
    {
        public long? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }

        public Persona() { }

        public Persona(long? id, string? nombre, string? telefono)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Telefono = telefono;
        }
    }
}
