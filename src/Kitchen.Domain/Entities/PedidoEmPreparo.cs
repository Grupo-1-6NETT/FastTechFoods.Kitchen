using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;
public class PedidoEmPreparo
{
    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public StatusPreparo Status { get; private set; }
    public List<ItemPreparo> Itens { get; private set; } = new();
    public string FormaDeEntrega { get; private set; } = string.Empty;
    public string? Justificativa { get; private set; }

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

        switch (novoStatus)
        {
            case StatusPreparo.Rejeitado:
                if (Status != StatusPreparo.Recebido && Status != StatusPreparo.EmPreparacao)
                    throw new InvalidOperationException("Pedido não pode ser rejeitado nesse estado.");
                Status = StatusPreparo.Rejeitado;
                break;

            case StatusPreparo.Cancelado:
                if (Status != StatusPreparo.Recebido)
                    throw new InvalidOperationException("Pedido não pode ser cancelado nesse estado.");
                Status = StatusPreparo.Cancelado;
                break;

            case StatusPreparo.EmPreparacao:
                if (Status != StatusPreparo.Recebido)
                    throw new InvalidOperationException("Pedido só pode ir para preparo se estiver confirmado.");
                Status = StatusPreparo.EmPreparacao;
                break;

            case StatusPreparo.Finalizado:
                if (Status != StatusPreparo.EmPreparacao)
                    throw new InvalidOperationException("Pedido só pode ser finalizado após o preparo.");
                Status = StatusPreparo.Finalizado;
                break;

            default:
                throw new InvalidOperationException("Status inválido para transição.");
        }
    }
    public void JustificarCancelamento(string motivo)
    {
        Justificativa = motivo;
    }
}
