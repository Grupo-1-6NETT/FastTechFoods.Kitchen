using Kitchen.Domain.Entities;
using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Domain.Events;

namespace Kitchen.Application.Consumer;
public class PedidoCriadoConsumer : IConsumer<IPedidoCriadoEvent>
{
    private readonly ILogger<PedidoCriadoConsumer> _logger;
    private readonly IPedidoPreparoRepository _repository;
    private readonly KitchenDbContext _dbContext;

    public PedidoCriadoConsumer(ILogger<PedidoCriadoConsumer> logger, IPedidoPreparoRepository repository, KitchenDbContext dbContext)
    {
        _logger = logger;
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<IPedidoCriadoEvent> context)
    {
        var pedidoEvent = context.Message;

        _logger.LogInformation("PedidoCriadoEvent recebido: {Id}", pedidoEvent.PedidoId);

        var itens = pedidoEvent.Itens
            .Select(i => new ItemPreparo(i.ProdutoId, i.NomeProduto, i.Quantidade))
            .ToList();

        var pedido = new PedidoEmPreparo(pedidoEvent.PedidoId, pedidoEvent.ClienteId, pedidoEvent.DataCriacao, itens);
        pedido.AtualizarStatus(Domain.Enums.StatusPreparo.Recebido);
        await _repository.AdicionarAsync(pedido);
        await _dbContext.SaveChangesAsync();
    }
}
