using MusicApp;
using MusicApp.Filters;
using MusicApp.Middleware;
using N_Tier.Application;
using N_Tier.DataAccess;
using N_Tier.DataAccess.Authentication;
using N_Tier.DataAccess.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using N_Tier.Application.QuartzConfigPayment;

var builder = WebApplication.CreateBuilder(args);

// Service registrations
builder.Services.AddControllers(
    config => config.Filters.Add(typeof(ValidateModelAttribute))
);

builder.Services.AddSwagger();

builder.Services.AddDataAccess(builder.Configuration)
    .AddApplication(builder.Environment);

builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("JwtOptions"));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy =>
        policy.RequireRole("Role", "User"));
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Role", "Admin"));
});

// JWT Authentication
var jwtOption = builder.Configuration.GetSection("JwtOptions").Get<JwtOption>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOption.Issuer,
        ValidAudience = jwtOption.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOption.SecretKey)),
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = "Role"
    };
});

builder.Services.AddQuartzJobs();

var app = builder.Build();

// Database migration
using var scope = app.Services.CreateScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

// Environment-specific middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "N-Tier V1"); });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

namespace MusicApp
{
    public partial class Program { }
}