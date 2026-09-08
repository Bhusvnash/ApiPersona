using ApiPersonas.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
//aqui se decide si usar sqlServer o mysql
builder.Services.AddScoped<IPersonaRepository, MysqlPersonaRepository>();
builder.Services.AddCors(options =>
{
		options.AddPolicy("dev", policity =>
		{
				policity.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
		});
});
Console.ForegroundColor = ConsoleColor.White;
var app = builder.Build();
app.UseCors("dev");//any origins to fecht
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
//app usa async callback (httpContext,next= "ya termine continua con el siguiente middleware")
app.Use(async (context, next) =>
{
		var inicio = DateTime.Now;

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("\n======================================================");
		Console.WriteLine($"||  {context.Request.Method,-6} {context.Request.Path,-27} ");
		Console.WriteLine($"||{inicio:HH:mm:ss.fff}");
		Console.ResetColor();
		await next();
		var duracion = DateTime.Now - inicio;
		Console.ForegroundColor = context.Response.StatusCode switch
		{
				>= 200 and < 300 => ConsoleColor.Green,
				>= 300 and < 400 => ConsoleColor.Yellow,
				>= 400 => ConsoleColor.Red,
				_ => ConsoleColor.White
		};

		Console.WriteLine(
				$" └── [{context.Response.StatusCode}] " +$"{context.Request.Method} {context.Request.Path} " +$"({duracion.TotalMilliseconds:0.0} ms)"
		);
		Console.ResetColor();
});

app.Run();