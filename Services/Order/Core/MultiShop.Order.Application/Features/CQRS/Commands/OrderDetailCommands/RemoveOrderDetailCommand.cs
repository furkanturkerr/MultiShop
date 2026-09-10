namespace MultiShop.Order.Application.Features.Commands.OrderDetailCommands;

public class RemoveOrderDetailCommand
{
    public int Id { get; set; }
    public RemoveOrderDetailCommand(int id)
    {
        Id = id;
    }
}