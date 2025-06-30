using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WinFormsDataApp.Data;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Forms
{
    public partial class MainForm : Form
    {
        private AppDbContext? _dbContext;

        public MainForm()
        {
            InitializeComponent();

            // Apply theme on startup
            ThemeManager.ApplyTheme(this);

            SetStatus("Ready");
            LoggingService.LogInformation("MainForm initialized");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetStatus("Application loaded successfully");

            // Initialize database context
            InitializeDbContext();

            // Load dashboard by default
            OpenDashboard();
        }

        private void InitializeDbContext()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                    ?? "Data Source=data.db";

                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlite(connectionString);

                _dbContext = new AppDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database connection: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatus(string message)
        {
            statusLabel.Text = message;
            statusStrip.Refresh();
        }

        private void ShowProgress(bool show)
        {
            progressBar.Visible = show;
            if (show)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
        }

        // Menu Event Handlers
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCustomerManagement();
        }

        private void OrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenOrderManagement();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "WinForms Data Entry & Reporting Application\n\n" +
                "Version 1.0\n" +
                "Built with .NET 9 and Entity Framework Core\n\n" +
                "Features:\n" +
                "• Customer Management\n" +
                "• Order Processing\n" +
                "• Data Reporting\n" +
                "• Data Import/Export",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void TestExceptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "This will intentionally throw an exception to test the global error handling system.\n\n" +
                "Do you want to continue?",
                "Test Exception Handling",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoggingService.LogInformation("User initiated exception test");
                GlobalExceptionHandler.TestExceptionHandling();
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Opening Settings...");

                using (var settingsForm = new SettingsForm())
                {
                    // Apply current theme to the settings form
                    ThemeManager.ApplyTheme(settingsForm);

                    var result = settingsForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        SetStatus("Settings saved successfully");
                        LoggingService.LogInformation("User updated application settings");
                    }
                    else
                    {
                        SetStatus("Settings cancelled");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error opening settings form");
                MessageBox.Show($"Error opening settings: {ex.Message}", "Settings Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Error opening settings");
            }
        }

        // Toolbar Event Handlers
        private void NewCustomerButton_Click(object sender, EventArgs e)
        {
            OpenCustomerManagement();
        }

        private void NewOrderButton_Click(object sender, EventArgs e)
        {
            OpenOrderManagement();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Save current active form/tab
            SetStatus("Saving data...");
            ShowProgress(true);

            try
            {
                // This will be implemented when individual forms are created
                // For now, just simulate save operation
                System.Threading.Thread.Sleep(1000);
                SetStatus("Data saved successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Save failed");
            }
            finally
            {
                ShowProgress(false);
            }
        }

        private void DashboardButton_Click(object sender, EventArgs e)
        {
            OpenDashboard();
        }

        private void OpenDashboard()
        {
            SetStatus("Opening Dashboard...");

            // Check if dashboard form is already hosted
            var dashboardForm = dashboardTab.Controls.OfType<DashboardForm>().FirstOrDefault();

            if (dashboardForm == null)
            {
                // Create and host the dashboard form
                dashboardForm = new DashboardForm
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };

                dashboardTab.Controls.Add(dashboardForm);
                dashboardForm.Show();
            }

            // Switch to dashboard tab
            tabControl.SelectedTab = dashboardTab;

            SetStatus("Dashboard opened");
        }

        // Helper methods for opening forms
        private void OpenCustomerManagement()
        {
            SetStatus("Opening Customer Management...");

            // Check if Customer tab already exists
            var existingTab = tabControl.TabPages.Cast<TabPage>()
                .FirstOrDefault(tab => tab.Name == "CustomerTab");

            if (existingTab != null)
            {
                tabControl.SelectedTab = existingTab;
                return;
            }

            // Create new customer tab with CustomerForm
            var customerTab = new TabPage("Customers")
            {
                Name = "CustomerTab"
            };

            var customerForm = new CustomerForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            customerTab.Controls.Add(customerForm);
            tabControl.TabPages.Add(customerTab);
            tabControl.SelectedTab = customerTab;

            customerForm.Show();

            SetStatus("Customer Management opened");
        }

        private void OpenOrderManagement()
        {
            SetStatus("Opening Order Management...");

            // Check if Order tab already exists
            var existingTab = tabControl.TabPages.Cast<TabPage>()
                .FirstOrDefault(tab => tab.Name == "OrderTab");

            if (existingTab != null)
            {
                tabControl.SelectedTab = existingTab;
                return;
            }

            // Create new order tab with OrderForm
            var orderTab = new TabPage("Orders")
            {
                Name = "OrderTab"
            };

            var orderForm = new OrderForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            orderTab.Controls.Add(orderForm);
            tabControl.TabPages.Add(orderTab);
            tabControl.SelectedTab = orderTab;

            orderForm.Show();

            SetStatus("Order Management opened");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up database context
            _dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
