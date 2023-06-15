using MassTransit;
using Simpra.Api.Consumer;

namespace Simpra.Api.Extensions
{
    public static class RabbitMQExtension
    {
        public static void AddRabbitMQExtension(this IServiceCollection services, IConfiguration configuration)
        {
            //MassTransit => RabbitMQ ile command göndermek için
            services.AddMassTransit(x =>
            {
                // Default port :5672
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["RabbitMQUrl"], "/", host =>
                    {
                        //Default olarak username ve password guest olarak gelmektedir.
                        host.Username("guest");
                        host.Password("guest");
                    });

                    //Command i okumak için Basket.Api tarafında oluşturduğumuz kuyruğu dinliyoruz.
                    config.ReceiveEndpoint("create-order-service", e =>
                    {
                        e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
                    });
                });

                // Message-Event yakalamak için
                x.AddConsumer<CreateOrderMessageCommandConsumer>();
            });

        }

    }
}
