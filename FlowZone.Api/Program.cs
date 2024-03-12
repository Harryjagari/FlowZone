using FlowZone.Api.Data;
using FlowZone.Api.Endpoints;
using FlowZone.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddAuthentication(Options =>
{
	Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(JwtOptions =>
		JwtOptions.TokenValidationParameters = TokenService.GetTokenValidationParameters(builder.Configuration)
	 );
builder.Services.AddAuthorization();

builder.Services.AddTransient<TokenService>()
	.AddTransient<PasswordService>()
	.AddTransient<AuthService>()
	.AddTransient<ToDoService>()
	.AddTransient<AvatarService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();


app.Run();

