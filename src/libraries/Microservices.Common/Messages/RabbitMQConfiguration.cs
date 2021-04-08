using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace Microservices.Common.Messages
{
    public class RabbitMQConfiguration : RabbitMQClientConfiguration
    {
        public RabbitMQConfiguration()
        {
            Username = "guest";
            Password = "guest";
            Exchange = "Logs";
            ExchangeType = "fanout";
            DeliveryMode = RabbitMQDeliveryMode.Durable;
            //RouteKey = "Logs";
            Port = 5672;
            //Hostnames = new List<string>() {"localhost"};
        }
    }
}
