using Serilog;

namespace Microservices.Common.Helpers
{
    public static class GlobalLogger
    {
        public static void ConfigureLog()
        {
            //var config = new RabbitMQConfiguration
            //{
            //    Username = "guest",
            //    Password = "guest",
            //    Exchange = "direct_logs",
            //    ExchangeType = ExchangeType.Direct,
            //    DeliveryMode = RabbitMQDeliveryMode.Durable,
            //    RouteKey = "",
            //    Port = 15672,
            //    Hostnames = { "localhost" }
            //};

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                //.RabbitMQ((clientConfiguration, sinkConfiguration) => {
                //    clientConfiguration.From(config);
                //    sinkConfiguration.TextFormatter = new JsonFormatter();
                //})
                .Seq("http://localhost:5341")
                .MinimumLevel
                .Warning()
                .Enrich
                .FromLogContext()
                .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo
            //    //.Seq("http://localhost:5341")
            //    //.RabbitMQ(config, new RabbitMQSinkConfiguration())
            //    .RabbitMQ(config, new RabbitMQSinkConfiguration
            //    {

            //    })
            //    .MinimumLevel
            //    .Warning()
            //    .Enrich
            //    .FromLogContext()
            //    .CreateLogger();
        }
    }


}
