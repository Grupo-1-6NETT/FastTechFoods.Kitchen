using Kitchen.API.Controllers;
using Kitchen.Application.Commands;
using Kitchen.Application.DTOs;
using Kitchen.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Kitchen.API.Test;

public class PedidoPreparoControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PedidoPreparoController _sut;

    private PedidoPreparoDTO _pedidoPreparoDto;

    public PedidoPreparoControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _sut = new(_mediatorMock.Object);

        _pedidoPreparoDto = new(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "Confirmado", []);
    }

    #region ObterTodos
    [Fact]
    public async Task ObterTodos_DeveRetornarPedidos()
    {
        var pedidoId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ObterPedidosEmPreparoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([_pedidoPreparoDto]);

        var result = await _sut.ObterTodos();

        Assert.IsType<OkObjectResult>(result);
    }
    #endregion

    #region IniciarPreparo
    [Fact]
    public async Task IniciarPreparo_PedidoValido_DeveRetornarOk()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.IniciarPreparo(_pedidoPreparoDto.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task IniciarPreparo_PedidoInvalido_DeveRetornarNotFound()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.IniciarPreparo(Guid.Empty);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    #endregion

    #region RejeitarPreparo
    [Fact]
    public async Task RejeitarPreparo_PedidoValido_DeveRetornarOk()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.RejeitarPreparo(_pedidoPreparoDto.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RejeitarPreparo_PedidoInvalido_DeveRetornarNotFound()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.RejeitarPreparo(Guid.Empty);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    #endregion

    #region FinalizarPreparo
    [Fact]
    public async Task FinalizarPreparo_PedidoValido_DeveRetornarOk()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.FinalizarPreparo(_pedidoPreparoDto.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task FinalizarPreparo_PedidoInvalido_DeveRetornarNotFound()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarStatusPreparoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.FinalizarPreparo(Guid.Empty);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    #endregion
}