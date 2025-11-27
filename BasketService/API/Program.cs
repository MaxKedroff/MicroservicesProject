using Application;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CoreLib.HttpLogic.Services;
using TraceLib;
using MassTransit;

using Infrastructure.Sagas;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogic();
builder.Services.TryAddTraceId();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<BasketCheckoutSaga, BasketCheckoutState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.AddDbContext<DbContext, BasketSagaDbContext>((provider, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        });

    x.AddRequestClient<IGetBasketCheckoutStatus>();


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ReceiveEndpoint("basket-checkout-saga", e =>
        {
            e.ConfigureSaga<BasketCheckoutState>(context);
        });

        cfg.ReceiveEndpoint("basket-inventory-responses", e =>
        {
            e.Bind("inventory-reserved");
            e.Bind("inventory-reservation-failed");
        });
    });

});


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


