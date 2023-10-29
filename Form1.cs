using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testovoe.DataModels;

namespace Testovoe
{
    public partial class Form1 : Form
    {
        private readonly DataService _dataService;
        private readonly List<ProductType> _productTypes;
        private readonly ReportGenerator _reportGenerator;

        public Form1()
        {
            InitializeComponent();
            _dataService = new DataService(System.Configuration.ConfigurationSettings.AppSettings["DbCon"]);
            _productTypes = _dataService.GetProductsTypeData();
            var providers = _dataService.GetProvidersData();
            foreach (var provider in providers)
            {
                ProvidersComboBox.Items.Add($"Поставщик {provider.Id}");
                providerComboBoxReport.Items.Add($"Поставщик {provider.Id}");
            }
            ProvidersComboBox.Text = ProvidersComboBox.Items[0].ToString();
            foreach (var productType in _productTypes) 
            {
                DataGridViewRow row = (DataGridViewRow)DeliveryGrid.Rows[0].Clone();
                row.Cells[0].Value = productType.Name;
                DeliveryGrid.Rows.Add(row);
            }
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-mm-dd";
            _reportGenerator = new ReportGenerator(_dataService, ProvidersComboBox.Items.Count, _productTypes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var id = _dataService.GetOrdersLastId() + 1;
            var providerId = GetProviderId(ProvidersComboBox.Text);
            var orderList = new List<OrderItem>();
            foreach(DataGridViewRow row in DeliveryGrid.Rows)
            {
                var productName = row.Cells[0].Value?.ToString();
                int value = 0;
                if (int.TryParse(row.Cells[1].Value?.ToString(), out value) && value != 0)
                {
                    orderList.Add(new OrderItem
                    {
                        DeliveryId = id,
                        ProductTypeId = _productTypes.First(x => x.Name == productName).Id,
                        Count = value
                    });
                }
            }
            _dataService.AddNewDelivery(orderList, providerId, dateTimePicker1.Value, id);
        }

        /// <summary>
        /// Извлеч Ид поставщика из текста.
        /// </summary>
        /// <param name="providerName">Имя поставщика.</param>
        /// <returns>Ид поставщика.</returns>
        /// <exception cref="Exception">Если не удалось извлечь Ид поставщика.</exception>
        private int GetProviderId(string providerName)
        {
            string pattern = @"\d+";
            Regex rg = new Regex(pattern);
            if (rg.IsMatch(providerName))
            {
                return int.Parse(rg.Match(providerName).Value);
            }

            MessageBox.Show("Поставщик не найден");
            throw new Exception("Поставщик не найден");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                providerComboBoxReport.Text = "Все поставщики";
                providerComboBoxReport.Enabled = false;
            }
            else
            {
                providerComboBoxReport.Text = providerComboBoxReport.Items[0].ToString();
                providerComboBoxReport.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int result = DateTime.Compare(startDateTimePicker.Value, endDateTimePicker.Value);

            if(result < 0)
            {
                _reportGenerator.GenerateReport(
                    startDateTimePicker.Value,
                    endDateTimePicker.Value,
                    providerComboBoxReport.Enabled ? GetProviderId(providerComboBoxReport.Text) : -1);
            }
            
        }
    }
}
