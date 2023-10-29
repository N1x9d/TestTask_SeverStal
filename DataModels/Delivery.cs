using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Testovoe.DataModels
{
    /// <summary>
    /// Модель поставки от поставщика
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Ид.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата доставки.
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// Заказ.
        /// </summary>
        public List<OrderItem> OrderedItems { get; set; }

        /// <summary>
        /// Ид поставщика.
        /// </summary>
        public int ProviderId { get; set; }
    }
}