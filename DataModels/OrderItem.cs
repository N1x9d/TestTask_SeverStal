namespace Testovoe.DataModels
{
    public class OrderItem
    {
        /// <summary>
        /// Ид доставки.
        /// </summary>
        public int DeliveryId { get; set; }

        /// <summary>
        /// Ид типа продукта.
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Количество товара в заказе.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена поставки продукта.
        /// </summary>
        public int Price { get; set; }
    }
}