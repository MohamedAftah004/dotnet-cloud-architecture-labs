using MiniOrderApi.Models;
using MiniOrderApi.Messaging;

namespace MiniOrderApi.Services;

public class OrderService
{
    private static readonly List<Order> _orders = new();
    private readonly SqsPublisher _publisher;

    public OrderService(SqsPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<Order> CreateOrder(decimal amount)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            TotalAmount = amount
        };

        _orders.Add(order);

        await _publisher.PublishAsync(new OrderCreatedEvent
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount
        });

        return order;
    }

    public Order? GetOrder(Guid orderId)
    {
        return _orders.FirstOrDefault(x => x.Id == orderId);
    }

    public void MarkAsPaid(Guid orderId)
    {
        var order = _orders.FirstOrDefault(x => x.Id == orderId);
        if (order != null)
            order.Status = "Paid";
    }
}