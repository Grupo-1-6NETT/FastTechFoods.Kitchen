using Kitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kitchen.Infrastructure.Data;
public class KitchenDbContext : DbContext
{
    public KitchenDbContext(DbContextOptions<KitchenDbContext> options) : base(options) { }

    public DbSet<PedidoEmPreparo> Pedidos => Set<PedidoEmPreparo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PedidoEmPreparo>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ClienteId).IsRequired();
            builder.Property(p => p.DataCriacao).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            builder.OwnsMany(p => p.Itens, itens =>
            {
                itens.WithOwner().HasForeignKey("PedidoId");
                itens.HasKey(i => i.Id);
                itens.Property(i => i.ProdutoId).IsRequired();
                itens.Property(i => i.NomeProduto).IsRequired().HasMaxLength(100);
                itens.Property(i => i.Quantidade).IsRequired();
            });
        });
    }
}
