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
using WinFormsDataApp.Services.Reports;
using WinFormsDataApp.Data;

namespace WinFormsDataApp.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ReportService _reportService;
        private readonly ExportService _exportService;
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
            _reportService = new ReportService(_customerRepository, _orderRepository);
            _exportService = new ExportService();
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
                var idColumn = dataGridViewOrders.Columns["Id"];
                if (idColumn != null)
                    idColumn.HeaderText = "Order ID";

                var quantityColumn = dataGridViewOrders.Columns["Quantity"];
                if (quantityColumn != null)
                    quantityColumn.HeaderText = "Quantity";

                var orderDateColumn = dataGridViewOrders.Columns["OrderDate"];
                if (orderDateColumn != null)
                    orderDateColumn.HeaderText = "Order Date";

                var customerNameColumn = dataGridViewOrders.Columns["CustomerName"];
                if (customerNameColumn != null)
                    customerNameColumn.HeaderText = "Customer";

                var customerEmailColumn = dataGridViewOrders.Columns["CustomerEmail"];
                if (customerEmailColumn != null)
                    customerEmailColumn.HeaderText = "Email";

                var totalValueColumn = dataGridViewOrders.Columns["TotalValue"];
                if (totalValueColumn != null)
                {
                    totalValueColumn.HeaderText = "Total Value";
                    // Format currency column
                    totalValueColumn.DefaultCellStyle.Format = "C";
                }
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

                // Try to configure chart, with fallback handling
                try
                {
                    ConfigureChart();
                }
                catch (Exception chartEx)
                {
                    LoggingService.LogError(chartEx, "Chart configuration failed, hiding chart");

                    // Hide chart if it's not working
                    if (chart != null)
                    {
                        chart.Visible = false;
                    }

                    // Show error message to user
                    MessageBox.Show(
                        "Chart functionality is not available due to compatibility issues.\n" +
                        "Data grid and export features are still fully functional.",
                        "Chart Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error generating chart data");
                MessageBox.Show($"Error generating chart data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureChart()
        {
            if (chart == null) return;

            try
            {
                // Clear existing chart data
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.DataSource = null;

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

                // Add data points manually (avoiding data binding issues)
                var chartData = _chartDataBindingSource.DataSource as List<dynamic>;
                if (chartData != null && chartData.Count > 0)
                {
                    foreach (var item in chartData)
                    {
                        try
                        {
                            var period = item.Period?.ToString() ?? "Unknown";
                            var value = Convert.ToDouble(item.TotalValue);
                            series.Points.AddXY(period, value);
                        }
                        catch (Exception ex)
                        {
                            LoggingService.LogWarning($"Failed to add chart point: {ex.Message}");
                        }
                    }
                }
                else
                {
                    // Add placeholder data if no data available
                    series.Points.AddXY("No Data", 0);
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

                // Force chart refresh
                chart.Invalidate();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error configuring chart");

                // Display error message on chart
                try
                {
                    chart.Series.Clear();
                    chart.ChartAreas.Clear();
                    chart.Titles.Clear();
                    chart.Titles.Add("Chart Error: " + ex.Message);
                    chart.Titles[0].Font = new Font("Arial", 10, FontStyle.Bold);
                    chart.Titles[0].ForeColor = Color.Red;
                }
                catch
                {
                    // If even basic chart operations fail, we'll handle it in the UI
                }
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

        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                btnExportExcel.Enabled = false;
                btnExportExcel.Text = "Exporting...";

                using var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    DefaultExt = "xlsx",
                    AddExtension = true,
                    FileName = $"Orders_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var orders = await _orderRepository.GetAllWithCustomersAsync();
                    var customers = await _customerRepository.GetAllAsync();

                    await _exportService.ExportOrdersToExcelAsync(orders, customers, saveDialog.FileName);

                    var result = MessageBox.Show(
                        "Excel export completed successfully!\n\nWould you like to open the file?",
                        "Export Complete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        _exportService.OpenExcelFile(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Failed to export orders to Excel");
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportExcel.Enabled = true;
                btnExportExcel.Text = "Export Excel";
            }
        }

        private async void btnSummaryReport_Click(object sender, EventArgs e)
        {
            try
            {
                btnSummaryReport.Enabled = false;
                btnSummaryReport.Text = "Generating...";

                // Show date range picker dialog
                using var dateRangeForm = new DateRangeForm();
                if (dateRangeForm.ShowDialog() == DialogResult.OK)
                {
                    var startDate = dateRangeForm.StartDate;
                    var endDate = dateRangeForm.EndDate;

                    using var saveDialog = new SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                        DefaultExt = "pdf",
                        AddExtension = true,
                        FileName = $"Summary_Report_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.pdf"
                    };

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);
                        var customers = await _customerRepository.GetAllAsync();

                        if (Path.GetExtension(saveDialog.FileName).ToLower() == ".pdf")
                        {
                            // Generate PDF report
                            var filePath = await _reportService.GenerateSummaryReportAsync(orders, customers, startDate, endDate, saveDialog.FileName, true);
                        }
                        else
                        {
                            // Generate Excel report
                            await _exportService.ExportSummaryToExcelAsync(orders, customers, startDate, endDate, saveDialog.FileName);

                            var result = MessageBox.Show(
                                "Summary report generated successfully!\n\nWould you like to open the file?",
                                "Report Complete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                _exportService.OpenExcelFile(saveDialog.FileName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Failed to generate summary report");
                MessageBox.Show($"Error generating summary report: {ex.Message}", "Report Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSummaryReport.Enabled = true;
                btnSummaryReport.Text = "Summary Report";
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
