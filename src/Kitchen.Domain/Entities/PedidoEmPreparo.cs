using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;
public class PedidoEmPreparo
{
    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public StatusPreparo Status { get; private set; }
    public List<ItemPreparo> Itens { get; private set; } = new();

    protected PedidoEmPreparo() { }

    public PedidoEmPreparo(Guid id, Guid clienteId, DateTime dataCriacao, List<ItemPreparo> itens)
    {
        Id = id;
        ClienteId = clienteId;
        DataCriacao = dataCriacao;
        Status = StatusPreparo.Recebido;
        Itens = itens;
    }

    public void AtualizarStatus(StatusPreparo novoStatus)
    {        
        Status = novoStatus;
    }
}
