using Kitchen.Domain.Entities;

namespace Kitchen.Domain.Repositories;
public interface IPedidoPreparoRepository
{
    Task<PedidoEmPreparo?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<PedidoEmPreparo>> ObterTodosAsync();
    Task AdicionarAsync(PedidoEmPreparo pedido);
    Task AtualizarAsync(PedidoEmPreparo pedido);
}
