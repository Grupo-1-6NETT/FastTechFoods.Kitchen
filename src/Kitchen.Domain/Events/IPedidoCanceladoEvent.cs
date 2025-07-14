namespace Kitchen.Domain.Events;
public interface IPedidoCanceladoEvent
{
    Guid PedidoId { get; }
    string Motivo { get; }
    DateTime DataCancelamento { get; }
}
