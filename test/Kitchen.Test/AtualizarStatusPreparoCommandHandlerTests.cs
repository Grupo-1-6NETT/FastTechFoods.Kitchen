using Kitchen.Application.Commands;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;
using Kitchen.Domain.Events;
using Kitchen.Domain.Repositories;
using MassTransit;
using Moq;

namespace Kitchen.Application.Test;
public class AtualizarStatusPreparoCommandHandlerTests
{
    private readonly Mock<IPedidoPreparoRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _unitMock = new();
    private readonly Mock<IPublishEndpoint> _publishMock = new();

    private readonly AtualizarStatusPreparoCommandHandler _handler;

    public AtualizarStatusPreparoCommandHandlerTests()
    {
        _handler = new AtualizarStatusPreparoCommandHandler(
            _repoMock.Object,
            _unitMock.Object,
            _publishMock.Object
        );
    }

    [Fact]
    public async Task Handle_StatusFinalizado_DeveAlterarStatusFinalizado()
    {
        // Arrange
        var pedido = new PedidoEmPreparo(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, []);        
        pedido.AtualizarStatus(StatusPreparo.EmPreparacao);
        _repoMock
            .Setup(r => r.ObterPorIdAsync(pedido.Id))
            .ReturnsAsync(pedido);

        var command = new AtualizarStatusPreparoCommand(pedido.Id, StatusPreparo.Finalizado);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result);
        Assert.Equal(StatusPreparo.Finalizado, pedido.Status);
        _unitMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_StatusCancelado_DeveAlterarStatusCancelado()
    {
        // Arrange
        var pedido = new PedidoEmPreparo(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, new List<ItemPreparo>());
        _repoMock.Setup(r => r.ObterPorIdAsync(pedido.Id))
                 .ReturnsAsync(pedido);

        var command = new AtualizarStatusPreparoCommand(pedido.Id, StatusPreparo.Cancelado);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result);
        Assert.Equal(StatusPreparo.Cancelado, pedido.Status);
        _unitMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_StatusEmPreparo_NaoPublicaEvento()
    {
        // Arrange
        var pedido = new PedidoEmPreparo(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, new List<ItemPreparo>());
        _repoMock.Setup(r => r.ObterPorIdAsync(pedido.Id))
                 .ReturnsAsync(pedido);

        var command = new AtualizarStatusPreparoCommand(pedido.Id, StatusPreparo.EmPreparacao);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result);
        Assert.Equal(StatusPreparo.EmPreparacao, pedido.Status);
        _publishMock.Verify(p => p.Publish<IPedidoFinalizadoEvent>(It.IsAny<object>(), default), Times.Never);
        _publishMock.Verify(p => p.Publish<IPedidoRejeitadoEvent>(It.IsAny<object>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_PedidoNaoEncontrado_DeveRetornarFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((PedidoEmPreparo?)null);

        var command = new AtualizarStatusPreparoCommand(id, StatusPreparo.EmPreparacao);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result);
        _unitMock.Verify(u => u.CommitAsync(), Times.Never);
        _publishMock.Verify(p => p.Publish<IPedidoFinalizadoEvent>(It.IsAny<object>(), default), Times.Never);
    }
}
