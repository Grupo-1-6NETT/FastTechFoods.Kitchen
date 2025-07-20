using Kitchen.Application.Commands;
using Kitchen.Application.Consumer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kitchen.Application.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration config)
    {
        AddMediatr(services);
        AddMassTransit(services, config);
        return services;
    }
    private static void AddMediatr(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AtualizarStatusPreparoCommand).Assembly);
        });

    }
    private static void AddMassTransit(IServiceCollection services,IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PedidoCriadoConsumer>();
            x.AddConsumer<PedidoCanceladoConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username(config["RabbitMQSettings:Username"]!);
                    h.Password(config["RabbitMQSettings:Password"]!);
                });

                cfg.ReceiveEndpoint("kitchen-pedido-criado", e =>
                {
                    e.ConfigureConsumer<PedidoCriadoConsumer>(ctx);
                });
                cfg.ReceiveEndpoint("kitchen-pedido-cancelado", e =>
                {
                    e.ConfigureConsumer<PedidoCanceladoConsumer>(ctx);
                });
            });
        });
    }
}
