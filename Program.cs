using Microsoft.EntityFrameworkCore;
using dev_forum_api.Data;
using Microsoft.OpenApi.Models;
using dev_forum_api.Interfaces;
using dev_forum_api.Repositories;
using dev_forum_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dev Forum API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

// CORS policy for Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
builder.Services.AddScoped<IReplyRepository, ReplyRepository>();

// Register JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();

// Register Admin Service
builder.Services.AddScoped<AdminService>();

// JWT Authentication Configuration
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey not found");
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uncomment if you want to use HTTPS redirection
// app.UseHttpsRedirection();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
