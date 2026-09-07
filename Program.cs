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

var app = builder.Build();
app.UseCors("dev");//any origins to fecht
app.UseAuthorization();
app.MapControllers();
app.Run();