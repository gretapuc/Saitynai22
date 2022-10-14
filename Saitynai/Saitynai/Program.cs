using Saitynai.Data;
using Saitynai.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<IsmDbContext>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<ICompetitionsRepository, CompetitionsRepository>();
builder.Services.AddScoped<IRegistrationsRepository, RegistrationsRepository>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();


app.Run();
