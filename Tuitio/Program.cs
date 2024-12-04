using Microsoft.EntityFrameworkCore;
using Tuitio.Models;
using AutoMapper;
using Tuitio.Extensions;
using Tuitio.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using TCGCardCapital.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Set Kestrel's MaxRequestBodySize
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1_073_741_824; // 1 GB in bytes
});

// Add FormOptions to increase MultipartBodyLengthLimit
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_073_741_824; // 1 GB in bytes
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tuitio API", Version = "v1" });

    // Configure Swagger to use JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into the text box below. Example: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

//configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddDbContext<TutoringSchoolContext>(
 o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddProjectServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

// Call the seeding method during startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TutoringSchoolContext>();
    RolesSeeder.SeedRolesAndAdmin(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/uploads"
});

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
