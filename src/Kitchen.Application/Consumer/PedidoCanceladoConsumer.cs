using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Domain.Events;

namespace Kitchen.Application.Consumer;
public class PedidoCanceladoConsumer : IConsumer<IPedidoCanceladoEvent>
{
    private readonly ILogger<PedidoCanceladoConsumer> _logger;
    private readonly IPedidoPreparoRepository _repository;
    private readonly IUnitOfWork _unit;
    private readonly KitchenDbContext _dbContext;

    public PedidoCanceladoConsumer(ILogger<PedidoCanceladoConsumer> logger, IPedidoPreparoRepository repository, KitchenDbContext dbContext, IUnitOfWork unit)
    {
        _logger = logger;
        _repository = repository;
        _dbContext = dbContext;
        _unit = unit;
    }

    public async Task Consume(ConsumeContext<IPedidoCanceladoEvent> context)
    {
        var evento = context.Message;

        var pedido = await _repository.ObterPorIdAsync(evento.PedidoId);
        if (pedido is null)
        {
            _logger.LogWarning("Pedido não encontrado: {Id}", evento.PedidoId);
            return;
        }
        
        pedido.AtualizarStatus(Domain.Enums.StatusPreparo.Cancelado);
        pedido.JustificarCancelamento(evento.Justificativa);
        await _unit.CommitAsync();

        _logger.LogInformation("Pedido cancelado via evento: {Id}| Motivo: {Justificativa}", evento.PedidoId, evento.Justificativa);
    }
}
