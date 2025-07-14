using Kitchen.Application.Commands;
using Kitchen.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.API.Controllers;
[Route("[controller]")]
[ApiController]
public class PedidoPreparoController(IMediator _mediator) : ControllerBase
{
    [HttpPut("{id}/preparar")]
    public async Task<IActionResult> IniciarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.EmPreparo));
        return result ? Ok("Pedido em preparo") : NotFound("Pedido não encontrado");
    }

    [HttpPut("{id}/finalizar")]
    public async Task<IActionResult> FinalizarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Finalizado));
        return result ? Ok("Pedido finalizado") : NotFound("Pedido não encontrado");
    }

    [HttpPut("{id}/rejeitar")]
    public async Task<IActionResult> RejeitarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Cancelado));
        return result ? Ok("Pedido rejeitado pela cozinha") : NotFound("Pedido não encontrado");
    }
}
