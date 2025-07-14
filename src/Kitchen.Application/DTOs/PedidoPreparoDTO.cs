namespace Kitchen.Application.DTOs;
public record ItemPreparoDTO(Guid ProdutoId, string NomeProduto, int Quantidade);
public record PedidoPreparoDTO(Guid Id, Guid ClienteId, DateTime DataCriacao, string Status, List<ItemPreparoDTO> Itens);