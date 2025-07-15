using Kitchen.Application.Consumer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kitchen.Application.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration config)
    {
        AddMassTransit(services, config);
        return services;
    }
    private static void AddMassTransit(IServiceCollection services,IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PedidoCriadoConsumer>();

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
            });
        });
    }
}
