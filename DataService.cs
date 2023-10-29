using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using MySqlConnector;
using Testovoe.DataModels;

namespace Testovoe
{
    public class DataService
    {
        private readonly string _conString;

        public DataService(string conString)
        {
            _conString = conString;
        }

        /// <summary>
        /// получить соединение с БД.
        /// </summary>
        /// <returns>Соединение с БД.</returns>
        protected IDbConnection GetConnection()
        {
            var con = new MySqlConnection(_conString);
            return con;
        }

        /// <summary>
        /// Получить заказу у конкретного поставщика за указанный период времени.
        /// </summary>
        /// <param name="start">Начало диапазона времени.</param>
        /// <param name="end">Конец диапазона времени.</param>
        /// <param name="providerId">Поставщик.</param>
        /// <returns>Список поставок за период.</returns>
        public List<Delivery> GetDeliveriesFromProviderForPeriod(DateTime start, DateTime end, int providerId)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string reqString = @"SELECT 
                Id,
                Date as deliveryDate,
                ProviderId as providerId
                FROM Delivery d
                WHERE d.Date > @startDate 
                and d.Date < @endDate
                and d.ProviderId = @pId";

                var deliveries = connection.Query<Delivery>(
                    reqString, new
                    {
                        startDate = start,
                        endDate = end,
                        pId = providerId
                    });
                foreach (var delivery in deliveries)
                {
                    delivery.OrderedItems = GetOrderedItemsForDelivery(delivery.Id);
                }
                return deliveries.ToList();
            }
        }

        /// <summary>
        /// Получить список заказов в поставке.
        /// </summary>
        /// <param name="deliveryId">Ид поставки.</param>
        /// <returns>Список позиций в поставке.</returns>
        private List<OrderItem> GetOrderedItemsForDelivery(int deliveryId)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string reqString = @"SELECT 
                DeliveryId as deliveryId,
                ProductType_ProductId as productTypeId,
                ProductCount as count
                FROM Delivery_has_ProductType d
                WHERE d.DeliveryId = @dId";

                var orderedItems = connection.Query<OrderItem>(
                    reqString, new
                    {
                        dId = deliveryId
                    });

                return orderedItems.ToList();
            }
        }

        public void GetPriceForOrderedItem(List<OrderItem> items,int providerId, DateTime date)
        {
            foreach (var item in items)
            {

                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    string reqString = @"SELECT 
                    Price
                    FROM PriceByDate p
                    WHERE p.StartOfPricingDate <= @date
                    and p.EndOfPricingDate >= @date
                    and p.providers_ProviderNumber = @pId
                    and p.ProductType_ProductId = @prId";

                    var price = connection.QueryFirst<int>(
                        reqString, new
                        {
                            date = date.ToString("yyyy-MM-dd"),
                            pId = providerId,
                            prId = item.ProductTypeId
                        });
                    item.Price = price;
                }
            }
        }

        /// <summary>
        /// Добавить новую поставку.
        /// </summary>
        /// <param name="orderItems">Состав поставки.</param>
        /// <param name="providerId">Поставщик.</param>
        /// <param name="deliveryDate">Дата поставки.</param>
        /// <param name="id">ИД поставки.</param>
        public void AddNewDelivery(List<OrderItem> orderItems, int providerId, DateTime deliveryDate, int id)
        {
            using (IDbConnection connection = GetConnection())
            {
                var sql = "INSERT INTO Delivery (Id, Date, ProviderId) VALUES (@Id, @DeliveryDate, @ProviderId)";
                var delivery = new Delivery() { Id = id, ProviderId = providerId, DeliveryDate = deliveryDate.ToString("yyyy-MM-dd") };
                connection.Execute(sql, delivery);
                foreach (var orderItem in orderItems)
                {
                    AddOrderItem(orderItem);
                }
                MessageBox.Show("Информация сохранена.");
            }
        }

        /// <summary>
        /// Добавить продукт поставки.
        /// </summary>
        /// <param name="item">Продукт поставки.</param>
        private void AddOrderItem(OrderItem item)
        {
            using (IDbConnection connection = GetConnection())
            {
                var sql = "INSERT INTO Delivery_has_ProductType (DeliveryId, ProductType_ProductId, ProductCount) VALUES (@DeliveryId, @ProductTypeId, @Count)";
                connection.Execute(sql, item);
            }
        }

        /// <summary>
        /// Получить последний ИД в истории поставок.
        /// </summary>
        /// <returns>ИД в истории поставок.</returns>
        public int GetOrdersLastId()
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string reqString = @"SELECT 
                Id
                FROM Delivery d";

                var ids = connection.Query<int>(reqString);

                return ids.OrderBy(x => x).LastOrDefault();
            }
        }

        /// <summary>
        /// Получить список типов товаров.
        /// </summary>
        /// <returns></returns>
        public List<ProductType> GetProductsTypeData()
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string reqString = @"SELECT 
                ProductId as id,
                ProductName as Name
                FROM ProductType";

                var productTypes = connection.Query<ProductType>(reqString);
                return productTypes.ToList();
            }
        }

        /// <summary>
        /// Получить список поставщиков.
        /// </summary>
        /// <returns></returns>
        public List<Provider> GetProvidersData()
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string reqString = @"SELECT 
                ProviderNumber as id
                FROM Providers";

                var providers = connection.Query<Provider>(reqString);
                return providers.ToList();
            }
        }
    }
}