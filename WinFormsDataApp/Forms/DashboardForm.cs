using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WinFormsDataApp.Models;
using WinFormsDataApp.Repositories;
using WinFormsDataApp.Services;
using WinFormsDataApp.Data;

namespace WinFormsDataApp.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private DataRefreshService? _dataRefreshService;
        private BindingSource _ordersBindingSource;
        private BindingSource _chartDataBindingSource;

        public DashboardForm()
        {
            InitializeComponent();
            var contextFactory = new AppDbContextFactory();
            var context = contextFactory.CreateDbContext([]);
            _orderRepository = new OrderRepository(context);
            _customerRepository = new CustomerRepository(context);
            _ordersBindingSource = new BindingSource();
            _chartDataBindingSource = new BindingSource();
        }

        public DashboardForm(OrderRepository orderRepository, CustomerRepository customerRepository) : this()
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadDashboardData();
                SetupDataRefreshService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDashboardData()
        {
            // Load orders with customer data
            var orders = await _orderRepository.GetAllWithCustomersAsync();
            _ordersBindingSource.DataSource = orders.Select(o => new
            {
                o.Id,
                o.Quantity,
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                CustomerName = o.Customer?.Name ?? "Unknown",
                CustomerEmail = o.Customer?.Email ?? "",
                TotalValue = o.Quantity * 10.0m // Assuming $10 per unit
            }).ToList();

            // Configure DataGridView
            ConfigureDataGridView();

            // Generate chart data using LINQ
            await GenerateChartData();
        }

        private void ConfigureDataGridView()
        {
            if (dataGridViewOrders == null) return;

            dataGridViewOrders.DataSource = _ordersBindingSource;

            // Configure columns
            if (dataGridViewOrders.Columns.Count > 0)
            {
                dataGridViewOrders.Columns["Id"].HeaderText = "Order ID";
                dataGridViewOrders.Columns["Quantity"].HeaderText = "Quantity";
                dataGridViewOrders.Columns["OrderDate"].HeaderText = "Order Date";
                dataGridViewOrders.Columns["CustomerName"].HeaderText = "Customer";
                dataGridViewOrders.Columns["CustomerEmail"].HeaderText = "Email";
                dataGridViewOrders.Columns["TotalValue"].HeaderText = "Total Value";

                // Format currency column
                dataGridViewOrders.Columns["TotalValue"].DefaultCellStyle.Format = "C";
            }

            // Enable sorting
            dataGridViewOrders.AllowUserToAddRows = false;
            dataGridViewOrders.AllowUserToDeleteRows = false;
            dataGridViewOrders.ReadOnly = true;
            dataGridViewOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Set alternating row colors
            dataGridViewOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Auto-size columns
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private async Task GenerateChartData()
        {
            try
            {
                var orders = await _orderRepository.GetAllWithCustomersAsync();

                // LINQ Query: Group orders by month and calculate totals
                var monthlyData = orders
                    .GroupBy(o => new { Year = o.OrderDate.Year, Month = o.OrderDate.Month })
                    .Select(g => new
                    {
                        Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                        OrderCount = g.Count(),
                        TotalQuantity = g.Sum(o => o.Quantity),
                        TotalValue = g.Sum(o => o.Quantity) * 10.0m // Assuming $10 per unit
                    })
                    .OrderBy(x => x.Period)
                    .ToList();

                _chartDataBindingSource.DataSource = monthlyData;
                ConfigureChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating chart data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureChart()
        {
            if (chart == null) return;

            try
            {
                chart.Series.Clear();
                chart.ChartAreas.Clear();

                // Create chart area
                var chartArea = new ChartArea("MainArea");
                chartArea.AxisX.Title = "Month";
                chartArea.AxisY.Title = "Total Value ($)";
                chartArea.AxisX.MajorGrid.Enabled = false;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chart.ChartAreas.Add(chartArea);

                // Create series
                var series = new Series("Monthly Sales");
                series.ChartType = SeriesChartType.Column;
                series.Color = Color.SteelBlue;
                series.BorderWidth = 2;

                // Add data points
                var chartData = _chartDataBindingSource.DataSource as List<dynamic>;
                if (chartData != null)
                {
                    foreach (var item in chartData)
                    {
                        series.Points.AddXY(item.Period, item.TotalValue);
                    }
                }

                chart.Series.Add(series);

                // Configure chart appearance
                chart.Titles.Clear();
                chart.Titles.Add("Monthly Sales Summary");
                chart.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);

                // Show legend
                chart.Legends.Clear();
                var legend = new Legend("MainLegend");
                legend.Docking = Docking.Right;
                chart.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error configuring chart: {ex.Message}", "Chart Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupDataRefreshService()
        {
            _dataRefreshService = new DataRefreshService(TimeSpan.FromMinutes(5)); // Refresh every 5 minutes
            _dataRefreshService.OnDataChanged += async (sender, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke(new System.Action(async () => await LoadDashboardData()));
                }
                else
                {
                    await LoadDashboardData();
                }
            };
            _dataRefreshService.Start();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                btnRefresh.Enabled = false;
                btnRefresh.Text = "Refreshing...";

                await LoadDashboardData();

                MessageBox.Show("Dashboard data refreshed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
                btnRefresh.Text = "Refresh";
            }
        }

        private void btnExportGrid_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    saveDialog.DefaultExt = "csv";
                    saveDialog.AddExtension = true;

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportGridToCSV(saveDialog.FileName);

                        MessageBox.Show("Grid data exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting grid: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportGridToCSV(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                // Write headers
                var headers = new List<string>();
                foreach (DataGridViewColumn col in dataGridViewOrders.Columns)
                {
                    headers.Add($"\"{col.HeaderText}\"");
                }
                writer.WriteLine(string.Join(",", headers));

                // Write data rows
                foreach (DataGridViewRow row in dataGridViewOrders.Rows)
                {
                    if (row.IsNewRow) continue;

                    var values = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var cellValue = cell.Value?.ToString() ?? "";
                        values.Add($"\"{cellValue}\"");
                    }
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataRefreshService?.Stop();
                _dataRefreshService?.Dispose();
                _ordersBindingSource?.Dispose();
                _chartDataBindingSource?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
