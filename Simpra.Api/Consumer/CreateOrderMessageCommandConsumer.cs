using MassTransit;
using Simpra.Api.Messages;
using Simpra.Core.Service;
using Simpra.Schema.OrderRR;

namespace Simpra.Api.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly IOrderService _orderService;

        public CreateOrderMessageCommandConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var message = context.Message;

            var orderCreaterequest = new OrderCreateRequest()
            {
                TotalPrice = message.TotalPrice,
                UserId = message.UserId,
                CouponCode = message.CouponCode,
            };

            message.OrderItems.ForEach(x =>
            {
                orderCreaterequest.OrderDetails.Add(new OrderDetailRequest
                {
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    ProductId = x.ProductId,
                });
            });

            await _orderService.CreateOrderFromMessage(orderCreaterequest);
        }
    }
}
