using Microsoft.EntityFrameworkCore;
using UltraPlay.Constants;
using UltraPlay.Data;
using UltraPlay.Data.Interfaces;
using UltraPlay.Data.Repositories;
using UltraPlay.Services.BLL;
using UltraPlay.Services.BLL.Interfaces;
using UltraPlay.Services.Clients;
using UltraPlay.Services.Clients.Interfaces;
using UltraPlay.Services.Matches;
using UltraPlay.Services.Matches.Interfaces;
using UltraPlay.Services.Utils.XMLReader;
using UltraPlay.Services.Utils.XMLReader.Interfaces;
using UltraPlay.Services.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISportsClient, SportsClient>();
builder.Services.AddScoped<IXMLReader, XMLReader>();
builder.Services.AddScoped<IMatchEngine, MatchEngine>();
builder.Services.AddScoped<IDatabaseEngine, DatabaseEngine>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddHostedService<SyncWorker>();

// Clients
builder.Services.AddHttpClient<ISportsClient, SportsClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(ConfigurationKeys.UltraPlayDBConnectionStringKey)));

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

