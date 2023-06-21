using AutoMapper;
using MassTransit;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.OrderRR;
using Simpra.Service.Messages;

namespace Simpra.Api.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public CreateOrderMessageCommandConsumer(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var message = context.Message;

            var orderCreaterequest = new OrderCreateRequest()
            {
                TotalPrice = message.TotalPrice,
                CouponCode = message.CouponCode,
                CreditCard=message.CreditCard,
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

            var order = _mapper.Map<Order>(orderCreaterequest);

            await _orderService.CreateOrderAsync(order,message.UserId);
        }
    }
}
