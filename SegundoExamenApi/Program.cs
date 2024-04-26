using Microsoft.EntityFrameworkCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using SegundoExamenApi.Helpers;
using SegundoExamenApi.Data;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using SegundoExamenApi.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());



builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient =
builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secret =
    await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value;



//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddDbContext<ExamenContext>(options => options.UseSqlServer(connectionString));




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




//builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api OAuth Examen";
    document.Description = "Api con Token de Seguridad";
    // CONFIGURAMOS LA SEGURIDAD JWT PARA SWAGGER,
    // PERMITE AÑADIR EL TOKEN JWT A LA CABECERA.
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});





var app = builder.Build();
app.UseOpenApi();

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api OAuth Examen");
    options.RoutePrefix = "";
});





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
