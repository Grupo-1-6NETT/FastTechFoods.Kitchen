using Kitchen.Domain.Enums;
using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kitchen.Application.Commands;

public record AtualizarStatusPreparoCommand(Guid PedidoId, StatusPreparo NovoStatus) : IRequest<bool>;
public class AtualizarStatusPreparoCommandHandler : IRequestHandler<AtualizarStatusPreparoCommand, bool>
{
    private readonly IPedidoPreparoRepository _repository;
    private readonly IUnitOfWork _unit;

    public AtualizarStatusPreparoCommandHandler(IPedidoPreparoRepository repository, IUnitOfWork unit)
    {
        _repository = repository;
        _unit = unit;
    }

    public async Task<bool> Handle(AtualizarStatusPreparoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _repository.ObterPorIdAsync(request.PedidoId);
        if (pedido is null)
            return false;

        pedido.AtualizarStatus(request.NovoStatus);
        await _unit.CommitAsync();
        return true;
    }
}
