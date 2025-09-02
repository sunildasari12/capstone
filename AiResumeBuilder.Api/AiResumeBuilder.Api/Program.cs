

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AiResumeBuilder.Api.Data;
using AiResumeBuilder.Api.Repositories;
using AiResumeBuilder.Api.Services;
using Microsoft.OpenApi.Models;
using System.Text;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

// 1️⃣ FIXED: Configure CORS for React frontend (typically runs on port 3000 or 5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000", "http://localhost:3002"


            //"http://localhost:3000",    // HTTP - Create React App default
            //"https://localhost:3000",   // HTTPS - Create React App with HTTPS
            //"http://localhost:5173",    // HTTP - Vite default
            //"https://localhost:5173",   // HTTPS - Vite with HTTPS
            //"http://127.0.0.1:3000",    // HTTP - IP address
            //"https://127.0.0.1:3000",   // HTTPS - IP address
            //"http://127.0.0.1:5173",    // HTTP - IP address
            //"https://127.0.0.1:5173"    // HTTPS - IP address
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Add this if using cookies/auth
    });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AiResumeBuilder.Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\nExample: 'Bearer abc123def456'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IAiService, AiService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ReplaceThisWithAStrongSecretKeyOfAtLeast32Chars";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

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
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ FIXED: CORS middleware placement
app.UseCors("AllowReactFrontend"); // This must come before UseAuthentication/UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
// Add this at the bottom
//public partial class Program { }


