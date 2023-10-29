using System;

namespace Testovoe.DataModels
{
    /// <summary>
    /// Цена типа продукта у поставщика в указанный перид времени.
    /// </summary>
    public class PriceBySeller
    {
        /// <summary>
        /// Ид поставщика.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Ид типа продукта.
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Начало действия цены.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Конец действия цены.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Цена на период.
        /// </summary>
        public int Price { get; set; }
    }
}