using System.Text;
using WinFormsDataApp.Services.ImportExport;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Forms
{
    public partial class DataUtilitiesForm : Form
    {
        private DatabaseBackupService _backupService;

        public DataUtilitiesForm()
        {
            InitializeComponent();
            _backupService = new DatabaseBackupService(); // Remove logging service parameter
            SetupForm();
            LoadBackupHistory();
        }

        private void SetupForm()
        {
            this.Text = "Data Import/Export & Backup Utilities";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(800, 600);

            // Set default export paths from settings
            txtExportPath.Text = Properties.Settings.Default.DefaultExportPath;
            txtBackupPath.Text = Properties.Settings.Default.DefaultBackupPath;
        }

        private void LoadBackupHistory()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtBackupPath.Text))
                {
                    var backups = _backupService.GetAvailableBackups(txtBackupPath.Text);
                    listBackups.Items.Clear();

                    foreach (var backup in backups)
                    {
                        var fileInfo = new FileInfo(backup);
                        var item = new ListViewItem(Path.GetFileName(backup));
                        item.SubItems.Add(fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        item.SubItems.Add($"{fileInfo.Length / 1024:N0} KB");
                        item.Tag = backup;
                        listBackups.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error loading backup history");
            }
        }

        // Import events
        private async void btnImportCustomers_Click(object sender, EventArgs e)
        {
            using var wizard = new CsvImportWizardForm(CsvImportWizardForm.ImportType.Customers);
            wizard.DataImported += OnDataImported;

            if (wizard.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Customer import completed successfully!", "Import Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnImportOrders_Click(object sender, EventArgs e)
        {
            using var wizard = new CsvImportWizardForm(CsvImportWizardForm.ImportType.Orders);
            wizard.DataImported += OnDataImported;

            if (wizard.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Order import completed successfully!", "Import Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnDataImported(object? sender, EventArgs e)
        {
            // Notify parent that data has been imported
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        // Export events
        private async void btnExportToCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtExportPath.Text))
                {
                    MessageBox.Show("Please select an export directory first.", "Export Path Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnExportToCsv.Enabled = false;
                btnExportToCsv.Text = "Exporting...";

                var result = await _backupService.ExportToCsvAsync(txtExportPath.Text);

                if (result.Success)
                {
                    var message = new StringBuilder();
                    message.AppendLine("CSV export completed successfully!");
                    message.AppendLine();
                    message.AppendLine("Files created:");
                    foreach (var file in result.ExportedFiles)
                    {
                        message.AppendLine($"  • {Path.GetFileName(file)}");
                    }

                    MessageBox.Show(message.ToString(), "Export Complete",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Export failed: {result.ErrorMessage}", "Export Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error during CSV export");
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportToCsv.Enabled = true;
                btnExportToCsv.Text = "Export to CSV";
            }
        }

        private void btnBrowseExport_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select export directory";
            dialog.SelectedPath = txtExportPath.Text;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtExportPath.Text = dialog.SelectedPath;
                Properties.Settings.Default.DefaultExportPath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        // Backup events
        private async void btnCreateBackup_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBackupPath.Text))
                {
                    MessageBox.Show("Please select a backup directory first.", "Backup Path Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnCreateBackup.Enabled = false;
                btnCreateBackup.Text = "Creating Backup...";

                var result = await _backupService.CreateBackupAsync(txtBackupPath.Text);

                if (result.Success)
                {
                    MessageBox.Show($"Backup created successfully!\n\nFile: {Path.GetFileName(result.BackupFilePath)}\nSize: {result.BackupSizeBytes / 1024:N0} KB",
                                  "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackupHistory(); // Refresh the list
                }
                else
                {
                    MessageBox.Show($"Backup failed: {result.ErrorMessage}", "Backup Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error during backup creation");
                MessageBox.Show($"Backup failed: {ex.Message}", "Backup Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreateBackup.Enabled = true;
                btnCreateBackup.Text = "Create Backup";
            }
        }

        private async void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            if (listBackups.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a backup to restore.", "No Backup Selected",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedBackup = listBackups.SelectedItems[0].Tag as string;
            if (string.IsNullOrEmpty(selectedBackup))
                return;

            var confirmResult = MessageBox.Show(
                "WARNING: This will replace your current database with the selected backup.\n\n" +
                "Your current database will be backed up before the restore operation.\n\n" +
                "Do you want to continue?",
                "Confirm Database Restore",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                btnRestoreBackup.Enabled = false;
                btnRestoreBackup.Text = "Restoring...";

                var result = await _backupService.RestoreDatabaseAsync(selectedBackup);

                if (result.Success)
                {
                    var message = "Database restored successfully!";
                    if (result.RequiresRestart)
                    {
                        message += "\n\nThe application needs to restart to complete the restore operation.";
                        MessageBox.Show(message, "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Restart the application
                        Application.Restart();
                    }
                    else
                    {
                        MessageBox.Show(message, "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    MessageBox.Show($"Restore failed: {result.ErrorMessage}", "Restore Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error during database restore");
                MessageBox.Show($"Restore failed: {ex.Message}", "Restore Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRestoreBackup.Enabled = true;
                btnRestoreBackup.Text = "Restore Selected";
            }
        }

        private void btnBrowseBackup_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select backup directory";
            dialog.SelectedPath = txtBackupPath.Text;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtBackupPath.Text = dialog.SelectedPath;
                Properties.Settings.Default.DefaultBackupPath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                LoadBackupHistory();
            }
        }

        private void btnRefreshBackups_Click(object sender, EventArgs e)
        {
            LoadBackupHistory();
        }

        // Event to notify parent forms of data changes
        public event EventHandler? DataChanged;

        private void DataUtilitiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save current paths to settings
            Properties.Settings.Default.DefaultExportPath = txtExportPath.Text;
            Properties.Settings.Default.DefaultBackupPath = txtBackupPath.Text;
            Properties.Settings.Default.Save();
        }
    }
}
