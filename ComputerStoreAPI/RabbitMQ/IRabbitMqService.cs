using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI.RabbitMQ
{
    public interface IRabbitMqService
    {
        void SendMessage(string message);
    }
}
