using Microsoft.EntityFrameworkCore;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Application.Services.Database;
using PishgamanTask.Infrastructure.Database;
using PishgamanTask.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adding Swagger UI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ConnectionString
builder.Services.AddDbContextPool<PishgamanContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ************ DI ************ //
builder.Services.AddScoped<IPersonService, PersonService>();

// Infrastructure DI
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Adding AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


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
