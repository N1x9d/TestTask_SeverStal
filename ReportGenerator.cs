using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testovoe.DataModels;

namespace Testovoe
{
    public class ReportGenerator
    {
        private readonly DataService _dataService;
        private readonly int _providersCount;
        private readonly List<ProductType> _productTypes;

        public ReportGenerator(DataService dataService, int providersCount, List<ProductType> productTypes)
        {
            _dataService = dataService;
            _providersCount = providersCount;
            _productTypes = productTypes;
        }

        /// <summary>
        /// Сформировать общий отчет по поставкам за период.
        /// </summary>
        /// <param name="startDate">Начало периода.</param>
        /// <param name="endDate">Конец периода.</param>
        /// <param name="providerId">Поставщик (если -1 то все поставщики).</param>
        public void GenerateReport(DateTime startDate, DateTime endDate, int providerId = -1)
        {
            var reportName = $"{DateTime.Now.ToString("yyyyMMdd_hhmmss")}-report.txt";
            StreamWriter sw = new StreamWriter(reportName);
            sw.WriteLine($"Отчет по поставкам за период с {startDate.ToString("yyyy-MM-dd")} по {endDate.ToString("yyyy-MM-dd")}");
            if (providerId == -1)
            {                
                for (int i = 1; i <= _providersCount; i++)
                {
                    WriteReportForProvider(startDate, endDate, i, sw);
                }
            }
            else
            {
                WriteReportForProvider(startDate, endDate, providerId, sw);
            }
            sw.Dispose();
            Process.Start(reportName);
        }

        /// <summary>
        /// Отчет по поставкам поставщика.
        /// </summary>
        //// <param name="startDate">Начало периода.</param>
        /// <param name="endDate">Конец периода.</param>
        /// <param name="providerId">Поставщик.</param>
        /// <param name="sw">Stream Writer.</param>
        private void WriteReportForProvider(DateTime startDate, DateTime endDate, int providerId, StreamWriter sw)
        {
            var cultureInfo = new CultureInfo("en-Us");
            sw.WriteLine($"Отчет по поставкам поставщика {providerId}");
            var deliveries = _dataService.GetDeliveriesFromProviderForPeriod(startDate, endDate, providerId);
            var orderedItemsForPeriod = new List<OrderItem>();
            foreach (var delivery in deliveries)
            {
                _dataService.GetPriceForOrderedItem(delivery.OrderedItems, providerId, DateTime.Parse(delivery.DeliveryDate, cultureInfo));
                orderedItemsForPeriod.AddRange(delivery.OrderedItems);
            }
            foreach (var productType in _productTypes)
            { 
                var sumCount = orderedItemsForPeriod.Where(c => c.ProductTypeId == productType.Id).Sum(c => c.Count); 
                var sumPrise = orderedItemsForPeriod.Where(c => c.ProductTypeId == productType.Id).Sum(c => c.Count * c.Price);
                sw.WriteLine($"{productType.Name} объем = {sumCount} сумма = {sumPrise}");
            }
        }
    }
}
