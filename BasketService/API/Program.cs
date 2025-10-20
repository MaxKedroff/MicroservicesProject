using Application.Interfaces;
using Application.Services;
using Application;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Core.HttpLogic;
using CoreLib.HttpLogic.Services;
using TraceLib;
using Microsoft.Extensions.DependencyInjection.Extensions;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogic();
builder.Services.TryAddTraceId();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BasketLogDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


