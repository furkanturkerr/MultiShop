using System.Text.Json.Serialization;
using MultiShop.Order.Domain.Entities;
using MediatR;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;

public class CreateOrderingCommand : IRequest<int>
{
    [JsonIgnore]
    public List<OrderDetail> OrderDetails { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public decimal TotalPrice { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}
