var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
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
		Console.ForegroundColor = ConsoleColor.Green;
		await next();
		Console.WriteLine($"{context.Request.Method}{context.Request.Path}::[{context.Response.StatusCode}]");
		Console.ResetColor();
		Console.WriteLine("==========================\n");
});

app.Run();