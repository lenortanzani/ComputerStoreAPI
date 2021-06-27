using ComputerStoreAPI.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI.Data
{
    /// <summary>
    /// Класс DbService должен должен реализовывать CRUD (create, read, update, delete) методы, 
    /// т.е. методы создания, просмотра, редактирования, удаления;
    /// DbService должен работать с классом DbContext, который обеспечивает доступ к БД через DbSet
    /// 
    /// Посредник управляющей БД, единственный класс имеющий доступ к DbContext
    /// </summary>
    public class DbService : IDbService
    {
        // ссылка для работы с DbContext
        private readonly ComputerStoreDbContext _dbContext;
        private readonly IConfiguration _configuration; // Конфигурации из appsettings
        private readonly IRabbitMqService _mqService; // теперь можно из DbService обращаться к MqService

        // Dependency Injection, при создании класса для него будет автоматические создан объект, 
        // передан в конструктор
        public DbService(ComputerStoreDbContext dbContext, IConfiguration configuration, IRabbitMqService mqService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _mqService = mqService;
        }


        #region ------------------ Items ------------------


        public IEnumerable<Item> GetItems() => _dbContext.Items;
        public IEnumerable<Item> GetVideoCards() => _dbContext.Items.Where(i => i.Type == "Видеокарта" || i.Type == "Video Card" || i.Type == "Graphics Card");
        public IEnumerable<Item> GetProcessors() => _dbContext.Items.Where(i => i.Type == "Процессор" || i.Type == "Processor");
        public IEnumerable<Item> GetRAMs() => _dbContext.Items.Where(i => i.Type == "Оперативная память" || i.Type == "RAM");
        public IEnumerable<Item> GetMotherboards() => _dbContext.Items.Where(i => i.Type == "Материнская плата" || i.Type == "Motherboard");
        public IEnumerable<Item> GetHardDisks() => _dbContext.Items.Where(i => i.Type == "Жесткий диск" || i.Type == "Hard Disk");

        public Item GetItem(int id) => _dbContext.Items.FirstOrDefault(i => i.Id == id) ?? new Item();

        public int CreateItem(Item item)
        {
            _dbContext.Items.Add(item); // добавление в БД
            _dbContext.SaveChanges(); // сохранение

            _mqService.SendMessage(_configuration["RabbitMq:OnItemsChanged"]);

            return item.Id; 
        }

        public bool UpdateItem(Item item)
        {
            try
            {
                bool isExists = _dbContext.Items.Any(i => i.Id == item.Id);
                if(isExists)
                {
                    _dbContext.Items.Update(item);
                    _dbContext.SaveChanges();

                    _mqService.SendMessage(_configuration["RabbitMq:OnItemsChanged"]);
                    
                    return true;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteItem(int id)
        {
            Item deletingItem = _dbContext.Items.FirstOrDefault(i => i.Id == id);
            if (deletingItem is not null)
            {
                _dbContext.Items.Remove(deletingItem);
                _dbContext.SaveChanges();

                _mqService.SendMessage(_configuration["RabbitMq:OnItemsChanged"]);

                return true;
            }
            return false;
        }

        #endregion ------------------ Items ------------------

        #region ------------------ Orders ------------------

        public IEnumerable<Order> GetOrders() => _dbContext.Orders.Include(o => o.Item);

        public Order GetOrder(int id) => _dbContext.Orders.Include(o => o.Item).FirstOrDefault(o => o.Id == id) ?? new Order();

        public IEnumerable<Order> GetOrdersForDelivery()
        {
            var deliveryTime = double.Parse(_configuration["OrderLifetime:DeliveryTime"]);
            var orderValidTime = DateTime.Now.AddDays(-1 * deliveryTime);

            return _dbContext.Orders.Include(o => o.Item).Where(o => (o.Datetimeorderdelivered == null ||
                    o.Datetimeorderdelivered == default(DateTime))
                    && orderValidTime < o.Datetimeorderplaced); 
        }

        public IEnumerable<Order> GetOutdatedOrdersForDelivery()
        {
            var deliveryTime = double.Parse(_configuration["OrderLifetime:DeliveryTime"]);
            var orderValidTime = DateTime.Now.AddDays(-1 * deliveryTime);

            return _dbContext.Orders.Include(o => o.Item).Where(o => (o.Datetimeorderdelivered == null ||
                    o.Datetimeorderdelivered == default(DateTime))
                    && orderValidTime >= o.Datetimeorderplaced);
        }

        public IEnumerable<Order> GetOrdersByCourier(string courierId) => _dbContext.Orders
            .Include(o => o.Item).Where(o => o.CourierId.Equals(courierId));
        public IEnumerable<Order> GetOrdersByUser(string userId) => _dbContext.Orders
            .Include(o => o.Item).Include(o => o.Item).Where(o => o.UserId.Equals(userId));

        public int CreateOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            _mqService.SendMessage(_configuration["RabbitMq:OnOrdersChanged"]);

            return order.Id;
        }

        public bool UpdateOrder(Order order)
        {
            bool isExists = _dbContext.Orders.Any(o => o.Id == order.Id);
            if (isExists)
            {
                _dbContext.Orders.Update(order);
                _dbContext.SaveChanges();

                _mqService.SendMessage(_configuration["RabbitMq:OnOrdersChanged"]);

                return true;
            }
            return false;
        }

        public bool DeleteOrder(int id)
        {
            Order orderToDelete = _dbContext.Orders.FirstOrDefault(o => o.Id == id);
            if (orderToDelete is not null)
            {
                _dbContext.Orders.Remove(orderToDelete);
                _dbContext.SaveChanges();

                _mqService.SendMessage(_configuration["RabbitMq:OnOrdersChanged"]);

                return true;
            }
            return false;
        }

        #endregion ------------------ Orders ------------------
    }
}
 