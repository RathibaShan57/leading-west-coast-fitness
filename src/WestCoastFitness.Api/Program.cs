using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WestCoastFitness.Api;
using WestCoastFitness.Application;
using WestCoastFitness.Application.Bookings;
using WestCoastFitness.Application.Members;
using WestCoastFitness.Infrastructure;
using WestCoastFitness.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterMemberValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddCheck<PostgresReadyCheck>("postgres");
builder.Services.AddCors(options =>
{
    options.AddPolicy("spa", policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("spa");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.EnsureSeededAsync(db, CancellationToken.None);
}

app.Run();

public partial class Program;
