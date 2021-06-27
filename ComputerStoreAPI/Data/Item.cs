using System;
using System.Collections.Generic;

#nullable disable

namespace ComputerStoreAPI
{
    /// <summary>
    /// Модель отвечающая за таблицу Item, сформирован автоматически Entity Frameworke Core
    /// </summary>
    public partial class Item
    {
        public Item()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Price { get; set; }
        public string Characteristics { get; set; }
        public string PictureUrl { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
