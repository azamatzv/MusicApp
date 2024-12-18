
using Microsoft.Extensions.Configuration;
using N_Tier.DataAccess;
using N_Tier.DataAccess.Persistence;
using N_Tier.Shared.Services.Impl;
using N_Tier.Shared.Services;
using N_Tier.Application;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDataAccess(builder.Configuration).AddApplication(builder.Environment); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using var scope = app.Services.CreateScope();
builder.Services.AddScoped<IClaimService, ClaimService>();

await AutomatedMigration.MigrateAsync(scope.ServiceProvider);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
namespace MusicApp
{
    public partial class Program { }
}
