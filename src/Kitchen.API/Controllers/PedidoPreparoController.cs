using Kitchen.Application.Commands;
using Kitchen.Application.Queries;
using Kitchen.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.API.Controllers;
[Route("[controller]")]
[ApiController]
public class PedidoPreparoController(IMediator _mediator) : ControllerBase
{
    [HttpPut("{id}/preparar")]
    [Authorize(Roles = "atendente")]
    public async Task<IActionResult> IniciarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.EmPreparo));
        return result ? Ok("Pedido em preparo") : NotFound("Pedido não encontrado");
    }

    [HttpPut("{id}/finalizar")]
    [Authorize(Roles = "atendente")]
    public async Task<IActionResult> FinalizarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Finalizado));
        return result ? Ok("Pedido finalizado") : NotFound("Pedido não encontrado");
    }

    [HttpPut("{id}/rejeitar")]
    [Authorize(Roles = "gerente,atendente")]
    public async Task<IActionResult> RejeitarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Cancelado));
        return result ? Ok("Pedido rejeitado pela cozinha") : NotFound("Pedido não encontrado");
    }
    [HttpGet]
    [Authorize(Roles = "gerente,atendente")]
    public async Task<IActionResult> ObterTodos()
    {
        var pedidos = await _mediator.Send(new ObterPedidosEmPreparoQuery());
        return Ok(pedidos);
    }
}
