using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IAuthentication, AuthenticationImplementation>();

builder.Services.Configure<AuthConfiguration>(options =>
{
    options.clientId = builder.Configuration["Auth0:ClientId"];
    options.connection = builder.Configuration["Auth0:Connection"]; 
    options.Domain = builder.Configuration["Auth0:Domain"];
});


var app = builder.Build();

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
