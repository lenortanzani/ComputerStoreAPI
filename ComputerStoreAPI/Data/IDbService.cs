using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI.Data
{
    /// <summary>
    /// DbServiсe декларируется как IDbService для Dependency Injection, соответственно объект будет видеть 
    /// методы которые видит этот интерфейс, интерфейс должен отражать все публичные методы класса DBService
    /// Методы из DbServiсe должны быть задекларированы в интерфейсе IDbService соответственно
    /// </summary>
    public interface IDbService
    {
        IEnumerable<Item> GetItems();
        IEnumerable<Item> GetVideoCards();
        IEnumerable<Item> GetProcessors();
        IEnumerable<Item> GetRAMs();
        IEnumerable<Item> GetMotherboards();
        IEnumerable<Item> GetHardDisks();
        Item GetItem(int id);

        int CreateItem(Item item);

        bool UpdateItem(Item item);

        bool DeleteItem(int id);
        IEnumerable<Order> GetOrders();

        Order GetOrder(int id);

        IEnumerable<Order> GetOrdersForDelivery();
        IEnumerable<Order> GetOutdatedOrdersForDelivery();

        IEnumerable<Order> GetOrdersByCourier(string courierId);
        IEnumerable<Order> GetOrdersByUser(string userId);

        int CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool DeleteOrder(int id);
    }
}
