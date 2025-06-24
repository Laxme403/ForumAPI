using Microsoft.EntityFrameworkCore;
using dev_forum_api.Data;
using Microsoft.OpenApi.Models;
using dev_forum_api.Interfaces;
using dev_forum_api.Repositories;

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
});

// CORS policy for Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
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
app.UseAuthorization();

app.MapControllers();

app.Run();
