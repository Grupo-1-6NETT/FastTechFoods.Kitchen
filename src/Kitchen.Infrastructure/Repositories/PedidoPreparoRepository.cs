using Kitchen.Domain.Entities;
using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kitchen.Infrastructure.Repositories;
internal class PedidoPreparoRepository : IPedidoPreparoRepository
{
    private readonly KitchenDbContext _dbContext;

    public PedidoPreparoRepository(KitchenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(PedidoEmPreparo pedido)
    {
        await _dbContext.Pedidos.AddAsync(pedido);
    }

    public Task AtualizarAsync(PedidoEmPreparo pedido)
    {
        _dbContext.Pedidos.Update(pedido);
        return Task.CompletedTask;
    }

    public async Task<PedidoEmPreparo?> ObterPorIdAsync(Guid id)
    {
        return await _dbContext.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PedidoEmPreparo>> ObterTodosAsync()
    {
        return await _dbContext.Pedidos
            .AsNoTracking()
            .Include(p => p.Itens)
            .Take(100)
            .ToListAsync();
    }
}
