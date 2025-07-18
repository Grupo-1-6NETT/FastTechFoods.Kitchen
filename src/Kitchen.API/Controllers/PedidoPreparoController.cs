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
    /// <summary>
    /// Obtem pedido na cozinha
    /// </summary>
    /// <returns>Lista de pedidos</returns>
    /// <response code="200">Lista de pedidos</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet]
    [Authorize(Roles = "gerente,atendente")]
    public async Task<IActionResult> ObterTodos()
    {
        var pedidos = await _mediator.Send(new ObterPedidosEmPreparoQuery());
        return Ok(pedidos);
    }

    /// <summary>
    /// Inicia preparo do pedido
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Status do processo</returns>
    /// <response code="200">Pedido em preparo</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPut("{id}/preparar")]
    [Authorize(Roles = "atendente")]
    public async Task<IActionResult> IniciarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.EmPreparacao));
        return result ? Ok("Pedido em preparo") : NotFound("Pedido não encontrado");
    }


    /// <summary>
    /// Rejeita preparo do pedido
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Status do processo</returns>
    /// <response code="200">Pedido rejeitado com sucesso</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPut("{id}/rejeitar")]
    [Authorize(Roles = "gerente,atendente")]
    public async Task<IActionResult> RejeitarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Rejeitado));
        return result ? Ok("Pedido rejeitado pela cozinha") : NotFound("Pedido não encontrado");
    }

    /// <summary>
    /// Finaliza preparo do pedido
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Status do processo</returns>
    /// <response code="200">Pedido finalizado com sucesso</response>
    /// <response code="404">Não encontrado</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPut("{id}/finalizar")]
    [Authorize(Roles = "atendente")]
    public async Task<IActionResult> FinalizarPreparo(Guid id)
    {
        var result = await _mediator.Send(new AtualizarStatusPreparoCommand(id, StatusPreparo.Finalizado));
        return result ? Ok("Pedido finalizado") : NotFound("Pedido não encontrado");
    }

}
