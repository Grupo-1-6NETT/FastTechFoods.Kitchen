namespace Kitchen.Domain.Entities;
public class ItemPreparo
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }

    protected ItemPreparo() { }

    public ItemPreparo(Guid produtoId, string nomeProduto, int quantidade)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
    }
}
