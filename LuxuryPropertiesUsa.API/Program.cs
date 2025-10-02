using LuxuryPropertiesUsa.Application.Features.Properties.Queries;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure;
using LuxuryPropertiesUsa.Infrastructure.Data;
using LuxuryPropertiesUsa.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("SecretsValues:ConnectionStrings").Value));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPropertiesQuery).Assembly));

// Repositorios y UoW
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Luxury Properties USA API",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer",
      new OpenApiSecurityScheme
      {
          Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
      });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
});

//Coors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builderCoors =>
                      {
                          builderCoors.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Value ?? string.Empty).
                                              AllowAnyHeader().
                                              AllowAnyMethod();
                      });
});

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("SecretsValues:AuthIssuer").Value,
        ValidAudience = builder.Configuration.GetSection("SecretsValues:AuthAudience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SecretsValues:AuthKey").Value ?? string.Empty))
    };
});

// For addd comments - swagger.
builder.Services.AddSwaggerGen(o => AddSwaggerDocumentation(o));

static void AddSwaggerDocumentation(SwaggerGenOptions o)
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();