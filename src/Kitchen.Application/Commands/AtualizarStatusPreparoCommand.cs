using Kitchen.Domain.Enums;
using Kitchen.Domain.Events;
using Kitchen.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Kitchen.Application.Commands;

public record AtualizarStatusPreparoCommand(Guid PedidoId, StatusPreparo NovoStatus) : IRequest<bool>;
public class AtualizarStatusPreparoCommandHandler : IRequestHandler<AtualizarStatusPreparoCommand, bool>
{
    private readonly IPedidoPreparoRepository _repository;
    private readonly IUnitOfWork _unit;
    private readonly IPublishEndpoint _publish;

    public AtualizarStatusPreparoCommandHandler(IPedidoPreparoRepository repository, IUnitOfWork unit, IPublishEndpoint publish)
    {
        _repository = repository;
        _unit = unit;
        _publish = publish;
    }

    public async Task<bool> Handle(AtualizarStatusPreparoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _repository.ObterPorIdAsync(request.PedidoId);
        if (pedido is null)
            return false;

        pedido.AtualizarStatus(request.NovoStatus);
        await _unit.CommitAsync();
        if (request.NovoStatus == StatusPreparo.Finalizado)
        {
            await _publish.Publish<IPedidoFinalizadoEvent>(new
            {
                PedidoId = pedido.Id,
                DataFinalizacao = DateTime.UtcNow
            });
        }

        if (request.NovoStatus == StatusPreparo.Cancelado)
        {
            await _publish.Publish<IPedidoRejeitadoEvent>(new
            {
                PedidoId = pedido.Id,
                Motivo = "Pedido rejeitado na cozinha",
                DataCancelamento = DateTime.UtcNow
            });
        }
        return true;
    }
}
