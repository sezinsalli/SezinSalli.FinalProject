using Simpra.Schema.CreditCardRR;

namespace Simpra.Service.Messages;

public class CreateOrderMessageCommand
{
    public CreateOrderMessageCommand()
    {
        OrderItems = new List<OrderItemDto>();
    }
    public string CouponCode { get; set; }
    public string UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public CreditCardRequest CreditCard { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
