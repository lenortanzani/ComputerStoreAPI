using System;
using System.Collections.Generic;

#nullable disable

namespace ComputerStoreAPI
{
    /// <summary>
    /// Модель отвечающая за таблицу Order, сформирован автоматически Entity Frameworke Core
    /// </summary>
    public partial class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public string Address { get; set; }
        public string CourierId { get; set; }
        public DateTime? Datetimeorderplaced { get; set; }
        public DateTime? Datetimeorderdelivered { get; set; }

        public virtual Item Item { get; set; }
    }
}
