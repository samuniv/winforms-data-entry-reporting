using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using WinFormsDataApp.Data;
using WinFormsDataApp.Models;
using WinFormsDataApp.Repositories;

namespace WinFormsDataApp.Forms
{
    public partial class CustomerForm : Form
    {
        private readonly AppDbContext _dbContext;
        private readonly CustomerRepository _customerRepository;
        private Customer _currentCustomer;
        private bool _isNewCustomer;
        private bool _isDirty;

        public CustomerForm()
        {
            InitializeComponent();

            // Initialize database context
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=data.db";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            _dbContext = new AppDbContext(optionsBuilder.Options);

            // Initialize repository
            _customerRepository = new CustomerRepository(_dbContext);

            // Initialize current customer
            _currentCustomer = new Customer();
            _isNewCustomer = false;
            _isDirty = false;

            // Set up data grid columns
            SetupDataGridColumns();
        }

        private async void CustomerForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Loading customers...");
                await LoadCustomersAsync();
                SetStatus("Ready");

                // Initialize form state
                EnableDetailControls(false);
                ClearValidationErrors();
            }
            catch (Exception ex)
            {
                SetStatus("Error loading customers");
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
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
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                Width = 150,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 200,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Phone",
                HeaderText = "Phone",
                Width = 120,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Address",
                HeaderText = "Address",
                Width = 250,
                ReadOnly = true
            });
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                bindingSource.DataSource = customers;
                dataGridView.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load customers: {ex.Message}", ex);
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is Customer selectedCustomer)
            {
                LoadCustomerDetails(selectedCustomer);
                EnableDetailControls(true);
                _isNewCustomer = false;
            }
            else
            {
                EnableDetailControls(false);
            }
        }

        private void LoadCustomerDetails(Customer customer)
        {
            if (customer == null) return;

            _currentCustomer = new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            };

            // Bind to controls
            textBoxId.Text = _currentCustomer.Id.ToString();
            textBoxName.Text = _currentCustomer.Name;
            textBoxEmail.Text = _currentCustomer.Email;
            maskedTextBoxPhone.Text = _currentCustomer.Phone ?? string.Empty;
            textBoxAddress.Text = _currentCustomer.Address ?? string.Empty;

            _isDirty = false;
            ClearValidationErrors();
        }

        private void EnableDetailControls(bool enabled)
        {
            textBoxName.Enabled = enabled;
            textBoxEmail.Enabled = enabled;
            maskedTextBoxPhone.Enabled = enabled;
            textBoxAddress.Enabled = enabled;
            buttonSave.Enabled = enabled;
            buttonCancel.Enabled = enabled;
            buttonDelete.Enabled = enabled && !_isNewCustomer;
        }

        private void ClearValidationErrors()
        {
            errorProvider.SetError(textBoxName, string.Empty);
            errorProvider.SetError(textBoxEmail, string.Empty);
            errorProvider.SetError(maskedTextBoxPhone, string.Empty);
            errorProvider.SetError(textBoxAddress, string.Empty);
        }

        private void SetStatus(string message)
        {
            statusLabel.Text = message;
            statusStrip.Refresh();
        }

        // Validation Events
        private void TextBoxName_Validating(object sender, CancelEventArgs e)
        {
            var control = sender as TextBox;
            if (control == null) return;

            if (string.IsNullOrWhiteSpace(control.Text))
            {
                errorProvider.SetError(control, "Name is required.");
                e.Cancel = true;
            }
            else if (control.Text.Trim().Length > 100)
            {
                errorProvider.SetError(control, "Name cannot exceed 100 characters.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(control, string.Empty);
                _isDirty = true;
            }
        }

        private void TextBoxEmail_Validating(object sender, CancelEventArgs e)
        {
            var control = sender as TextBox;
            if (control == null) return;

            if (string.IsNullOrWhiteSpace(control.Text))
            {
                errorProvider.SetError(control, "Email is required.");
                e.Cancel = true;
            }
            else if (control.Text.Trim().Length > 100)
            {
                errorProvider.SetError(control, "Email cannot exceed 100 characters.");
                e.Cancel = true;
            }
            else if (!IsValidEmail(control.Text.Trim()))
            {
                errorProvider.SetError(control, "Please enter a valid email address.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(control, string.Empty);
                _isDirty = true;
            }
        }

        private void MaskedTextBoxPhone_Validating(object sender, CancelEventArgs e)
        {
            var control = sender as MaskedTextBox;
            if (control == null) return;

            var phoneText = control.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

            if (!string.IsNullOrWhiteSpace(control.Text) && phoneText.Length != 10)
            {
                errorProvider.SetError(control, "Please enter a complete phone number.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(control, string.Empty);
                _isDirty = true;
            }
        }

        private void TextBoxAddress_Validating(object sender, CancelEventArgs e)
        {
            var control = sender as TextBox;
            if (control == null) return;

            if (!string.IsNullOrWhiteSpace(control.Text) && control.Text.Trim().Length > 200)
            {
                errorProvider.SetError(control, "Address cannot exceed 200 characters.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(control, string.Empty);
                _isDirty = true;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use regex pattern for email validation
                var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateForm()
        {
            // Validate all controls
            return ValidateChildren();
        }

        // Button Event Handlers
        private void ButtonNew_Click(object sender, EventArgs e)
        {
            _currentCustomer = new Customer();
            _isNewCustomer = true;
            _isDirty = false;

            // Clear form
            textBoxId.Text = "[New]";
            textBoxName.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
            maskedTextBoxPhone.Text = string.Empty;
            textBoxAddress.Text = string.Empty;

            EnableDetailControls(true);
            buttonDelete.Enabled = false;
            ClearValidationErrors();

            // Focus on name field
            textBoxName.Focus();
            SetStatus("Ready to add new customer");
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

                SetStatus("Saving customer...");

                // Update current customer object
                _currentCustomer.Name = textBoxName.Text.Trim();
                _currentCustomer.Email = textBoxEmail.Text.Trim();
                _currentCustomer.Phone = string.IsNullOrWhiteSpace(maskedTextBoxPhone.Text) ? null : maskedTextBoxPhone.Text;
                _currentCustomer.Address = string.IsNullOrWhiteSpace(textBoxAddress.Text) ? null : textBoxAddress.Text.Trim();

                Customer savedCustomer;
                if (_isNewCustomer)
                {
                    savedCustomer = await _customerRepository.AddAsync(_currentCustomer);
                    SetStatus("Customer added successfully");
                }
                else
                {
                    savedCustomer = await _customerRepository.UpdateAsync(_currentCustomer);
                    SetStatus("Customer updated successfully");
                }

                // Reload the grid
                await LoadCustomersAsync();

                // Select the saved customer in the grid
                SelectCustomerInGrid(savedCustomer.Id);

                _isDirty = false;
                _isNewCustomer = false;
            }
            catch (Exception ex)
            {
                SetStatus("Error saving customer");
                MessageBox.Show($"Error saving customer: {ex.Message}", "Save Error",
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

            if (_isNewCustomer)
            {
                // Clear form and disable controls
                EnableDetailControls(false);
                textBoxId.Text = string.Empty;
                textBoxName.Text = string.Empty;
                textBoxEmail.Text = string.Empty;
                maskedTextBoxPhone.Text = string.Empty;
                textBoxAddress.Text = string.Empty;
                _isNewCustomer = false;
            }
            else if (dataGridView.CurrentRow?.DataBoundItem is Customer selectedCustomer)
            {
                // Reload current customer details
                LoadCustomerDetails(selectedCustomer);
            }

            _isDirty = false;
            ClearValidationErrors();
            SetStatus("Changes cancelled");
        }

        private async void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (_isNewCustomer || _currentCustomer.Id == 0)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete customer '{_currentCustomer.Name}'?\n\nThis action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                SetStatus("Deleting customer...");

                var deleted = await _customerRepository.DeleteAsync(_currentCustomer.Id);
                if (deleted)
                {
                    SetStatus("Customer deleted successfully");
                    await LoadCustomersAsync();

                    // Clear form and disable controls
                    EnableDetailControls(false);
                    textBoxId.Text = string.Empty;
                    textBoxName.Text = string.Empty;
                    textBoxEmail.Text = string.Empty;
                    maskedTextBoxPhone.Text = string.Empty;
                    textBoxAddress.Text = string.Empty;
                }
                else
                {
                    SetStatus("Customer not found");
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error deleting customer");
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Refreshing...");
                await LoadCustomersAsync();
                SetStatus("Ready");
            }
            catch (Exception ex)
            {
                SetStatus("Error refreshing");
                MessageBox.Show($"Error refreshing customers: {ex.Message}", "Refresh Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectCustomerInGrid(int customerId)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].DataBoundItem is Customer customer && customer.Id == customerId)
                {
                    dataGridView.ClearSelection();
                    dataGridView.Rows[i].Selected = true;
                    dataGridView.CurrentCell = dataGridView.Rows[i].Cells[0];
                    break;
                }
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
