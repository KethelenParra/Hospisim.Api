using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using System.Text.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HospisimDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSwaggerGen(options =>
{
    // Habilita os comentários XML para serem exibidos no Swagger UI
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta linha faz a mágica: converte Enums para strings no JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
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
