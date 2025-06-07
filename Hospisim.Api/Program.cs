using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HospisimDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// servir arquivos estáticos (wwwroot/css/site.css)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// rota padrão para o nosso MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pacientes}/{action=Index}/{id?}"
);

app.UseAuthorization();

app.MapControllers();

app.Run();
