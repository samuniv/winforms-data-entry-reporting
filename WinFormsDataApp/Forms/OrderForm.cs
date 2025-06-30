using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Configuration;
using WinFormsDataApp.Data;
using WinFormsDataApp.Models;
using WinFormsDataApp.Repositories;
using WinFormsDataApp.Services.Reports;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Forms
{
    public partial class OrderForm : Form
    {
        private readonly AppDbContext _dbContext;
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ReportService _reportService;
        private Order _currentOrder;
        private bool _isNewOrder;
        private bool _isDirty;

        public OrderForm()
        {
            InitializeComponent();

            // Initialize database context
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=data.db";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            _dbContext = new AppDbContext(optionsBuilder.Options);

            // Initialize repositories
            _orderRepository = new OrderRepository(_dbContext);
            _customerRepository = new CustomerRepository(_dbContext);

            // Initialize services
            _reportService = new ReportService(_customerRepository, _orderRepository);

            // Initialize current order
            _currentOrder = new Order();
            _isNewOrder = false;
            _isDirty = false;

            // Set up data grid columns
            SetupDataGridColumns();
        }

        private async void OrderForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Loading data...");

                // Load customers first
                await LoadCustomersAsync();

                // Check if there are any customers
                if (customerBindingSource.Count == 0)
                {
                    // No customers available - show message and disable controls
                    MessageBox.Show(
                        "No customers found in the database.\n\n" +
                        "Please add customers first before creating orders.",
                        "No Customers",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    EnableOrderControls(false);
                    SetStatus("No customers available - add customers first");
                    return;
                }

                // Load orders
                await LoadOrdersAsync();
                SetStatus("Ready");

                // Initialize form state
                EnableDetailControls(false);
                ClearValidationErrors();
            }
            catch (Exception ex)
            {
                SetStatus("Error loading data");
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridColumns()
        {
            // Configure DataGridView columns
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Order ID",
                Width = 80,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Customer.Name",
                HeaderText = "Customer",
                Width = 200,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Quantity",
                Width = 100,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDate",
                HeaderText = "Order Date",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = { Format = "MM/dd/yyyy" }
            });
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                customerBindingSource.DataSource = customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load customers: {ex.Message}", ex);
            }
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                bindingSource.DataSource = orders;
                dataGridView.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load orders: {ex.Message}", ex);
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is Order selectedOrder)
            {
                LoadOrderDetails(selectedOrder);
                EnableDetailControls(true);
                _isNewOrder = false;
            }
            else
            {
                EnableDetailControls(false);
            }
        }

        private void LoadOrderDetails(Order order)
        {
            if (order == null) return;

            _currentOrder = new Order
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate
            };

            // Bind to controls
            textBoxId.Text = _currentOrder.Id.ToString();
            comboBoxCustomer.SelectedValue = _currentOrder.CustomerId;
            numericUpDownQuantity.Value = _currentOrder.Quantity;
            dateTimePickerOrderDate.Value = _currentOrder.OrderDate;

            _isDirty = false;
            ClearValidationErrors();
        }

        private void EnableOrderControls(bool enabled)
        {
            buttonNew.Enabled = enabled;
            dataGridView.Enabled = enabled;
        }

        private void EnableDetailControls(bool enabled)
        {
            comboBoxCustomer.Enabled = enabled;
            numericUpDownQuantity.Enabled = enabled;
            dateTimePickerOrderDate.Enabled = enabled;
            buttonSave.Enabled = enabled && customerBindingSource.Count > 0;
            buttonCancel.Enabled = enabled;
            buttonDelete.Enabled = enabled && !_isNewOrder;
        }

        private void ClearValidationErrors()
        {
            errorProvider.SetError(comboBoxCustomer, string.Empty);
            errorProvider.SetError(numericUpDownQuantity, string.Empty);
            errorProvider.SetError(dateTimePickerOrderDate, string.Empty);
        }

        private void SetStatus(string message)
        {
            statusLabel.Text = message;
            statusStrip.Refresh();
        }

        // Validation and Change Events
        private void ComboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomer.SelectedValue != null)
            {
                errorProvider.SetError(comboBoxCustomer, string.Empty);
                _isDirty = true;
            }
        }

        private void NumericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownQuantity.Value >= 1 && numericUpDownQuantity.Value <= 1000)
            {
                errorProvider.SetError(numericUpDownQuantity, string.Empty);
                _isDirty = true;
            }
        }

        private void DateTimePickerOrderDate_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerOrderDate.Value.Date <= DateTime.Now.Date)
            {
                errorProvider.SetError(dateTimePickerOrderDate, string.Empty);
                _isDirty = true;
            }
            else
            {
                errorProvider.SetError(dateTimePickerOrderDate, "Order date cannot be in the future.");
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Validate customer selection
            if (comboBoxCustomer.SelectedValue == null)
            {
                errorProvider.SetError(comboBoxCustomer, "Please select a customer.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(comboBoxCustomer, string.Empty);
            }

            // Validate quantity
            if (numericUpDownQuantity.Value < 1 || numericUpDownQuantity.Value > 1000)
            {
                errorProvider.SetError(numericUpDownQuantity, "Quantity must be between 1 and 1000.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(numericUpDownQuantity, string.Empty);
            }

            // Validate order date
            if (dateTimePickerOrderDate.Value.Date > DateTime.Now.Date)
            {
                errorProvider.SetError(dateTimePickerOrderDate, "Order date cannot be in the future.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(dateTimePickerOrderDate, string.Empty);
            }

            return isValid;
        }

        // Button Event Handlers
        private void ButtonNew_Click(object sender, EventArgs e)
        {
            // Check if customers are available
            if (customerBindingSource.Count == 0)
            {
                MessageBox.Show(
                    "No customers available to create an order.\n\n" +
                    "Please add customers first.",
                    "No Customers",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _currentOrder = new Order();
            _isNewOrder = true;
            _isDirty = false;

            // Clear form
            textBoxId.Text = "[New]";
            comboBoxCustomer.SelectedIndex = -1;
            numericUpDownQuantity.Value = 1;
            dateTimePickerOrderDate.Value = DateTime.Now;

            EnableDetailControls(true);
            buttonDelete.Enabled = false;
            ClearValidationErrors();

            // Focus on customer selection
            comboBoxCustomer.Focus();
            SetStatus("Ready to add new order");
        }

        private async void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    SetStatus("Please correct validation errors");
                    return;
                }

                SetStatus("Saving order...");

                // Update current order object
                _currentOrder.CustomerId = (int)(comboBoxCustomer.SelectedValue ?? 0);
                _currentOrder.Quantity = (int)numericUpDownQuantity.Value;
                _currentOrder.OrderDate = dateTimePickerOrderDate.Value;

                Order savedOrder;
                if (_isNewOrder)
                {
                    savedOrder = await _orderRepository.AddAsync(_currentOrder);
                    SetStatus("Order added successfully");
                }
                else
                {
                    savedOrder = await _orderRepository.UpdateAsync(_currentOrder);
                    SetStatus("Order updated successfully");
                }

                // Reload the grid
                await LoadOrdersAsync();

                // Select the saved order in the grid
                SelectOrderInGrid(savedOrder.Id);

                _isDirty = false;
                _isNewOrder = false;
            }
            catch (Exception ex)
            {
                SetStatus("Error saving order");
                MessageBox.Show($"Error saving order: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to cancel?",
                    "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            if (_isNewOrder)
            {
                // Clear form and disable controls
                EnableDetailControls(false);
                textBoxId.Text = string.Empty;
                comboBoxCustomer.SelectedIndex = -1;
                numericUpDownQuantity.Value = 1;
                dateTimePickerOrderDate.Value = DateTime.Now;
                _isNewOrder = false;
            }
            else if (dataGridView.CurrentRow?.DataBoundItem is Order selectedOrder)
            {
                // Reload current order details
                LoadOrderDetails(selectedOrder);
            }

            _isDirty = false;
            ClearValidationErrors();
            SetStatus("Changes cancelled");
        }

        private async void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (_isNewOrder || _currentOrder.Id == 0)
                return;

            var customerName = comboBoxCustomer.Text;
            var result = MessageBox.Show(
                $"Are you sure you want to delete this order?\n\n" +
                $"Order ID: {_currentOrder.Id}\n" +
                $"Customer: {customerName}\n" +
                $"Quantity: {_currentOrder.Quantity}\n" +
                $"Order Date: {_currentOrder.OrderDate:MM/dd/yyyy}\n\n" +
                "This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                SetStatus("Deleting order...");

                var deleted = await _orderRepository.SoftDeleteAsync(_currentOrder.Id);
                if (deleted)
                {
                    SetStatus("Order deleted successfully");
                    await LoadOrdersAsync();

                    // Clear form and disable controls
                    EnableDetailControls(false);
                    textBoxId.Text = string.Empty;
                    comboBoxCustomer.SelectedIndex = -1;
                    numericUpDownQuantity.Value = 1;
                    dateTimePickerOrderDate.Value = DateTime.Now;
                }
                else
                {
                    SetStatus("Order not found");
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error deleting order");
                MessageBox.Show($"Error deleting order: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Refreshing...");
                await LoadCustomersAsync();
                await LoadOrdersAsync();

                // Re-check if customers are available and enable/disable controls accordingly
                EnableOrderControls(customerBindingSource.Count > 0);
                if (customerBindingSource.Count == 0)
                {
                    EnableDetailControls(false);
                    SetStatus("No customers available - add customers first");
                }
                else
                {
                    SetStatus("Ready");
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error refreshing");
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Refresh Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectOrderInGrid(int orderId)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].DataBoundItem is Order order && order.Id == orderId)
                {
                    dataGridView.ClearSelection();
                    dataGridView.Rows[i].Selected = true;
                    dataGridView.CurrentCell = dataGridView.Rows[i].Cells[0];
                    break;
                }
            }
        }

        private async void ButtonPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is not Order selectedOrder)
            {
                MessageBox.Show("Please select an order to print an invoice.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SetStatus("Generating invoice...");

                // Get customer information
                var customer = await _customerRepository.GetByIdAsync(selectedOrder.CustomerId);
                if (customer == null)
                {
                    MessageBox.Show("Customer information not found for this order.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Show save dialog
                using var saveDialog = new SaveFileDialog
                {
                    Title = "Save Invoice PDF",
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Invoice_{selectedOrder.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    DefaultExt = "pdf"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Generate and save the invoice
                    var filePath = await _reportService.GenerateInvoiceAsync(selectedOrder, customer,
                                                                          saveDialog.FileName, true);

                    SetStatus($"Invoice generated: {Path.GetFileName(filePath)}");

                    // Ask if user wants to open the file
                    var result = MessageBox.Show(
                        "Invoice generated successfully!\n\nWould you like to open the PDF file?",
                        "Invoice Generated",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        // Open manually if they said no to auto-open
                        _reportService.OpenPdfFile(filePath);
                    }
                }
                else
                {
                    SetStatus("Invoice generation cancelled");
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error generating invoice");
                LoggingService.LogError(ex, $"Failed to generate invoice for Order {selectedOrder.Id}");
                MessageBox.Show($"Error generating invoice: {ex.Message}", "Invoice Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to close?",
                    "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
