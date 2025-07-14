using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;

namespace Kitchen.Infrastructure.Repositories;
internal class UnitOfwork : IUnitOfWork
{
    private readonly KitchenDbContext _dbContext;

    public UnitOfwork(KitchenDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
