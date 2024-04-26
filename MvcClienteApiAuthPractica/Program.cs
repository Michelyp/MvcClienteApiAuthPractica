
using Microsoft.EntityFrameworkCore;
using MvcClienteApiAuthPractica.Data;
using MvcClienteApiAuthPractica.Repositories;
using MvcClienteApiAuthPractica.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
HelperOAuthServices helper =
    new HelperOAuthServices(builder.Configuration);
//ESTA INSTANCIA DEL HELPER DEBEMOS INCLUIRLA DENTRO 
//DE NUESTRA APLICACION SOLAMENTE UNA VEZ, PARA QUE 
//TODO LO QUE HEMOS CREADO DENTRO NO SE GENERE DE NUEVO
builder.Services.AddSingleton<HelperOAuthServices>(helper);

//HABILITAMOS LOS SERVICIOS DE AUTHENTICATION QUE HEMOS 
//CREADO EN EL HELPER CON Action<>
builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//importar bien el modelo 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Api OAuth Empleados", Description = "Api con Token de seguridad" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{

    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api OAuth Empleados");
    options.RoutePrefix = "";
});
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
