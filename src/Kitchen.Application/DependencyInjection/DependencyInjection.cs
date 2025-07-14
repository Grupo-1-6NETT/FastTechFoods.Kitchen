using Kitchen.Application.Consumer;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Kitchen.Application.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        AddMassTransit(services);
        return services;
    }
    private static void AddMassTransit(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PedidoCriadoConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("kitchen-pedido-criado", e =>
                {
                    e.ConfigureConsumer<PedidoCriadoConsumer>(ctx);
                });
            });
        });
    }
}
