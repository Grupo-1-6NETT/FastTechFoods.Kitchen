namespace Kitchen.Domain.Repositories;
public interface IUnitOfWork
{
    Task CommitAsync();
}
