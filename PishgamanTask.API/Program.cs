using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using PishgamanTask.API.Middleware;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Application.Services.Database;
using PishgamanTask.Domain.Entities;
using PishgamanTask.Infrastructure.Database;
using PishgamanTask.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adding Swagger UI

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); //Added Authentication to SwaggerUI

// ConnectionString
builder.Services.AddDbContext<PishgamanContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adding Identity and JWT
// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //Less securiy for now
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 2;
    options.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<PishgamanContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Swagger with authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// ************ DI ************ //
builder.Services.AddScoped<IPersonService, PersonService>();

// Infrastructure DI
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Adding AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Adding Account DI
builder.Services.AddScoped<IUserAccountService, AccountRepository>();

var app = builder.Build();

//Associate middlewares

app.UseMiddleware<IPBlockerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    //policy.WithOrigins("http://localhost:7067", "https://localhost:7067")
    policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithHeaders(HeaderNames.ContentType);
});

app.UseHttpsRedirection();

//First Authenticate then Authorize!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
if (scope != null)
    await InitAccountAndRoles(scope.ServiceProvider);

app.Run();


async Task InitAccountAndRoles(IServiceProvider serviceProvider)
{
	var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var usermanager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

	var roles = new List<string> { "Admin" };
	foreach (var x in roles)
	{
		if ((await rolemanager.RoleExistsAsync(x)) == false)
		{
			var role = new IdentityRole(x);
			await rolemanager.CreateAsync(role);
		}
	}

	var adminuser = await usermanager.FindByNameAsync("admin");
	if (adminuser == null)
	{
		adminuser = new ApplicationUser
		{
			Email = "admin@gmail.com",
			UserName = "admin",
			FullName = "AdminFullNAME",
		};
		usermanager.CreateAsync(adminuser, "password123").Wait();
	}
	if ((await usermanager.IsInRoleAsync(adminuser, "Admin")) == false)
	{
		usermanager.AddToRoleAsync(adminuser, "Admin").Wait();
	}
}