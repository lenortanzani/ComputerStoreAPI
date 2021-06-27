using ComputerStoreAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerStoreController : ControllerBase
    {
        private readonly IDbService _dbService;

        // Dependency Injection контейнер передает экземпляр класса IDbService,
        // инстанциирует экземпляр DbService, и передаст его в конструктор
        public ComputerStoreController(IDbService dbService)
        {
            _dbService = dbService;
        }

        #region ------------------ Items ------------------
        // Метод действия
        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetItems()
        {
            return _dbService.GetItems().ToList();
        }

        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetVideoCards()
        {
            return _dbService.GetVideoCards().ToList();
        }

        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetProcessors()
        {
            return _dbService.GetProcessors().ToList();
        }

        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetRAMs()
        {
            return _dbService.GetRAMs().ToList();
        }

        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetMotherboards()
        {
            return _dbService.GetMotherboards().ToList();
        }

        [HttpGet("[action]")]
        public ActionResult<List<Item>> GetHardDisks()
        {
            return _dbService.GetHardDisks().ToList();
        }
        /// <summary>
        /// Сюда приходит Http запрос, какой то из клиентов запросил товар с определенным id,
        /// метод действий Контроллера обращается к DbService 
        /// 
        /// Чтобы получить параметр (int id например) нужно прописать Route["action"]
        /// </summary>
        [HttpGet("[action]/{id}")] 
        public Item GetItem(int id)
        {
            return _dbService.GetItem(id);
        }

        /// <summary>
        /// Ждем запросHttpPost и предполагаем что в теле запроса нам будет отправлен JSON строка с 
        /// объектом которая будет по структуре совпадать с Item
        /// </summary>
        [HttpPost("[action]")]
        public int CreateItem([FromBody] Item item)
        {
            return _dbService.CreateItem(item);
        }

        [HttpPut("[action]")]
        public ActionResult<bool> UpdateItem([FromBody] Item item)
        {
            try
            {
                return _dbService.UpdateItem(item);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // В фигурных скобках параметр ("[action]/{id}") который должен смапиться с нашим принимаемым
        // параметром id
        [HttpDelete("[action]/{id}")]
        public bool DeleteItem(int id)
        {
            return _dbService.DeleteItem(id);
        }

        #endregion ------------------ Items ------------------


        #region ------------------ Orders ------------------


        [HttpGet("[action]")]
        public List<Order> GetOrders()
        {
            return _dbService.GetOrders().ToList();
        }


        [HttpGet("[action]/{id}")]
        public Order GetOrder(int id)
        {
            return _dbService.GetOrder(id);
        }

        [HttpGet("[action]")]
        public List<Order> GetOrdersForDelivery()
        {
            return _dbService.GetOrdersForDelivery().ToList();
        }

        [HttpGet("[action]")]
        public List<Order> GetOutdatedOrdersForDelivery()
        {
            return _dbService.GetOutdatedOrdersForDelivery().ToList();
        }

        [HttpGet("[action]/{id}")]
        public List<Order> GetOrdersByCourier(string courierId)
        {
            return _dbService.GetOrdersByCourier(courierId).ToList();
        }

        [HttpGet("[action]/{userId}")]
        public List<Order> GetOrdersByUser(string userId)
        {
            return _dbService.GetOrdersByUser(userId).ToList();
        }

        [HttpPost("[action]")]
        public int CreateOrder([FromBody] Order order)
        {
            return _dbService.CreateOrder(order);
        }

        [HttpPut("[action]")]
        public bool UpdateOrder([FromBody] Order order) // добавить try/catch 
        {
            return _dbService.UpdateOrder(order);
        }

        [HttpDelete("[action]/{id}")]
        public bool DeleteOrder(int id)
        {
            return _dbService.DeleteOrder(id);
        }



        #endregion ------------------ Orders ------------------
    }
}
