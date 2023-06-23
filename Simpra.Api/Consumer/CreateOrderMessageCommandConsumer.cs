using AutoMapper;
using MassTransit;
using Serilog;
using Simpra.Api.Helper;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Service.Messages;

namespace Simpra.Api.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public CreateOrderMessageCommandConsumer(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var message = context.Message;

            var order = new Order
            {
                CouponCode = message.CouponCode,
            };

            message.OrderItems.ForEach(x =>
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    ProductId = x.ProductId,
                });
            });

            // Kredi kartı bilgilerini hashleme => Gerçek proejelerde Stripe.net gibi araçlar kullanılabilir.
            string hashedCreditCard = CreditCardHashHelper.HashCreditCardInfo(message.CreditCard);

            var response = await _orderService.CreateOrderAsync(order, message.UserId, hashedCreditCard);

            Log.Information($"Order ({response.OrderNumber}) is successfully created with RabbitMQ!");
        }
    }
}
