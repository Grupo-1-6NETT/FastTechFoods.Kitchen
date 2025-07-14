using Kitchen.Application.DTOs;
using Kitchen.Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace Kitchen.Application.Queries;
public record ObterPedidosEmPreparoQuery() : IRequest<IEnumerable<PedidoPreparoDTO>>;

public class ObterPedidosEmPreparoQueryhandler : IRequestHandler<ObterPedidosEmPreparoQuery, IEnumerable<PedidoPreparoDTO>>
{
    private readonly IPedidoPreparoRepository _repository;

    public ObterPedidosEmPreparoQueryhandler(IPedidoPreparoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PedidoPreparoDTO>> Handle(ObterPedidosEmPreparoQuery request, CancellationToken cancellationToken)
    {
        var pedidos = await _repository.ObterTodosAsync();

        return pedidos.Select(p => new PedidoPreparoDTO(
            p.Id,
            p.ClienteId,
            p.DataCriacao,
            p.Status.ToString(),
            p.Itens.Select(i => new ItemPreparoDTO(i.ProdutoId, i.NomeProduto, i.Quantidade)).ToList()
        ));
    }
}