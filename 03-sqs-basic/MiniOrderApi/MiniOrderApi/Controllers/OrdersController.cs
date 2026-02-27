using Microsoft.AspNetCore.Mvc;
using MiniOrderApi.Services;

namespace MiniOrderApi.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(decimal amount)
    {
        var order = await _service.CreateOrder(amount);
        return Ok(order);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var order = _service.GetOrder(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }
}   