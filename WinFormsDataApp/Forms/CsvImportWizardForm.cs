using System.Text;
using WinFormsDataApp.DTOs;
using WinFormsDataApp.Services.ImportExport;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Forms
{
    public partial class CsvImportWizardForm : Form
    {
        public enum ImportType { Customers, Orders }
        public enum WizardStep { SelectFile, Preview, Import, Complete }

        private ImportType _importType;
        private WizardStep _currentStep;
        private string _selectedFilePath = string.Empty;
        private object? _importResult;

        public CsvImportWizardForm(ImportType importType)
        {
            InitializeComponent();
            _importType = importType;
            _currentStep = WizardStep.SelectFile;
            SetupForm();
            ShowStep(_currentStep);
        }

        private void SetupForm()
        {
            this.Text = $"Import {_importType} from CSV - Step 1 of 4";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(800, 600);
        }

        private void ShowStep(WizardStep step)
        {
            _currentStep = step;

            // Hide all panels first
            panelSelectFile.Visible = false;
            panelPreview.Visible = false;
            panelImport.Visible = false;
            panelComplete.Visible = false;

            // Update title and navigation
            this.Text = $"Import {_importType} from CSV - Step {(int)step + 1} of 4";
            btnBack.Enabled = step != WizardStep.SelectFile;
            btnNext.Enabled = false; // Will be enabled based on step validation
            btnCancel.Enabled = step != WizardStep.Import; // Disable cancel during import

            switch (step)
            {
                case WizardStep.SelectFile:
                    ShowSelectFileStep();
                    break;
                case WizardStep.Preview:
                    ShowPreviewStep();
                    break;
                case WizardStep.Import:
                    ShowImportStep();
                    break;
                case WizardStep.Complete:
                    ShowCompleteStep();
                    break;
            }
        }

        private void ShowSelectFileStep()
        {
            panelSelectFile.Visible = true;
            lblFileInstruction.Text = $"Please select a CSV file containing {_importType.ToString().ToLower()} data.";

            if (_importType == ImportType.Customers)
            {
                lblFileFormat.Text = "Expected columns: Name, Email, Phone, Address\nAlternative column names are supported (e.g., 'customer_name', 'email_address').";
            }
            else
            {
                lblFileFormat.Text = "Expected columns: Customer ID or Customer Name, Quantity, Order Date\nDate format: YYYY-MM-DD or MM/DD/YYYY";
            }

            txtFilePath.Text = _selectedFilePath;
            btnNext.Enabled = !string.IsNullOrEmpty(_selectedFilePath) && File.Exists(_selectedFilePath);
            btnNext.Text = "Next >";
        }

        private async void ShowPreviewStep()
        {
            panelPreview.Visible = true;
            btnNext.Text = "Import";
            btnNext.Enabled = false;

            try
            {
                if (_importType == ImportType.Customers)
                {
                    await PreviewCustomers();
                }
                else
                {
                    await PreviewOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing file: {ex.Message}", "Preview Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowStep(WizardStep.SelectFile);
            }
        }

        private async void ShowImportStep()
        {
            panelImport.Visible = true;
            btnNext.Enabled = false;
            progressBar.Value = 0;
            lblImportStatus.Text = "Starting import...";

            try
            {
                await PerformImport();
                ShowStep(WizardStep.Complete);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}", "Import Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowStep(WizardStep.Preview);
            }
        }

        private void ShowCompleteStep()
        {
            panelComplete.Visible = true;
            btnNext.Text = "Finish";
            btnNext.Enabled = true;
            btnBack.Enabled = false;

            // Show import summary
            DisplayImportSummary();
        }

        // Event handlers will be implemented in the designer partial class
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select CSV File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _selectedFilePath = openFileDialog.FileName;
                txtFilePath.Text = _selectedFilePath;
                btnNext.Enabled = true;
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            switch (_currentStep)
            {
                case WizardStep.SelectFile:
                    ShowStep(WizardStep.Preview);
                    break;
                case WizardStep.Preview:
                    ShowStep(WizardStep.Import);
                    break;
                case WizardStep.Import:
                    // Should not happen - button is disabled during import
                    break;
                case WizardStep.Complete:
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (_currentStep)
            {
                case WizardStep.Preview:
                    ShowStep(WizardStep.SelectFile);
                    break;
                case WizardStep.Import:
                    ShowStep(WizardStep.Preview);
                    break;
                case WizardStep.Complete:
                    ShowStep(WizardStep.Preview);
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_currentStep == WizardStep.Import)
            {
                // Import in progress - show confirmation
                var result = MessageBox.Show("Import is in progress. Are you sure you want to cancel?",
                                           "Cancel Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async Task PreviewCustomers()
        {
            // Create required services
            using var context = new Data.AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            var customerRepo = new Repositories.CustomerRepository(context);
            var orderRepo = new Repositories.OrderRepository(context);
            var importService = new CsvImportService(customerRepo, orderRepo);

            var result = await importService.ImportCustomersFromCsvAsync(_selectedFilePath, chkHasHeaders.Checked);
            _importResult = result;

            // Display preview data
            var previewData = result.ValidRecords.Take(100).ToList(); // Show first 100 valid records
            var bindingSource = new BindingSource();
            bindingSource.DataSource = previewData;
            dataGridPreview.DataSource = bindingSource;

            // Update validation summary
            lblValidationSummary.Text = $"Found {result.TotalRecords} records: {result.ValidRecords.Count} valid, {result.InvalidRecords.Count} invalid";

            // Show validation errors
            listValidationErrors.Items.Clear();
            foreach (var invalidRecord in result.InvalidRecords.Take(50)) // Show first 50 errors
            {
                listValidationErrors.Items.Add($"Record '{invalidRecord.Name}': {string.Join(", ", invalidRecord.ValidationErrors)}");
            }

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    listValidationErrors.Items.Add($"File Error: {error}");
                }
            }

            btnNext.Enabled = result.ValidRecords.Any() && !result.Errors.Any();
        }

        private async Task PreviewOrders()
        {
            // Create required services
            using var context = new Data.AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            var customerRepo = new Repositories.CustomerRepository(context);
            var orderRepo = new Repositories.OrderRepository(context);
            var importService = new CsvImportService(customerRepo, orderRepo);

            var result = await importService.ImportOrdersFromCsvAsync(_selectedFilePath, chkHasHeaders.Checked);
            _importResult = result;

            // Display preview data
            var previewData = result.ValidRecords.Take(100).ToList(); // Show first 100 valid records
            var bindingSource = new BindingSource();
            bindingSource.DataSource = previewData;
            dataGridPreview.DataSource = bindingSource;

            // Update validation summary
            lblValidationSummary.Text = $"Found {result.TotalRecords} records: {result.ValidRecords.Count} valid, {result.InvalidRecords.Count} invalid";

            // Show validation errors
            listValidationErrors.Items.Clear();
            foreach (var invalidRecord in result.InvalidRecords.Take(50)) // Show first 50 errors
            {
                var identifier = !string.IsNullOrEmpty(invalidRecord.CustomerName) ? invalidRecord.CustomerName :
                               invalidRecord.CustomerId?.ToString() ?? "Unknown";
                listValidationErrors.Items.Add($"Record '{identifier}': {string.Join(", ", invalidRecord.ValidationErrors)}");
            }

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    listValidationErrors.Items.Add($"File Error: {error}");
                }
            }

            btnNext.Enabled = result.ValidRecords.Any() && !result.Errors.Any();
        }

        private async Task PerformImport()
        {
            // Create required services
            using var context = new Data.AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            var customerRepo = new Repositories.CustomerRepository(context);
            var orderRepo = new Repositories.OrderRepository(context);
            var importService = new CsvImportService(customerRepo, orderRepo);

            progressBar.Style = ProgressBarStyle.Marquee;

            if (_importType == ImportType.Customers)
            {
                var result = _importResult as CsvImportResult<CustomerImportDto>;
                if (result == null) throw new InvalidOperationException("No preview data available");

                lblImportStatus.Text = $"Importing {result.ValidRecords.Count} customers...";
                Application.DoEvents();

                var saveResult = await importService.SaveCustomersAsync(result.ValidRecords);
                _importResult = saveResult;
            }
            else
            {
                var result = _importResult as CsvImportResult<OrderImportDto>;
                if (result == null) throw new InvalidOperationException("No preview data available");

                lblImportStatus.Text = $"Importing {result.ValidRecords.Count} orders...";
                Application.DoEvents();

                var saveResult = await importService.SaveOrdersAsync(result.ValidRecords);
                _importResult = saveResult;
            }

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = 100;
            lblImportStatus.Text = "Import completed!";

            // Trigger data refresh event for other forms to update
            DataImported?.Invoke(this, EventArgs.Empty);
        }

        private void DisplayImportSummary()
        {
            var summary = new StringBuilder();
            summary.AppendLine($"Import completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            summary.AppendLine($"File: {Path.GetFileName(_selectedFilePath)}");
            summary.AppendLine();

            if (_importResult is ImportSaveResult saveResult)
            {
                summary.AppendLine($"Records successfully imported: {saveResult.SuccessCount}");
                summary.AppendLine($"Records failed to import: {saveResult.FailureCount}");

                if (saveResult.Errors.Any())
                {
                    summary.AppendLine();
                    summary.AppendLine("Errors encountered:");
                    foreach (var error in saveResult.Errors)
                    {
                        summary.AppendLine($"  • {error}");
                    }
                }
            }
            else
            {
                summary.AppendLine("Import summary not available.");
            }

            txtImportSummary.Text = summary.ToString();
        }

        // Event to notify parent forms of successful import
        public event EventHandler? DataImported;
    }
}
