using System;
using System.IO;
using System.Windows.Forms;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Forms
{
    public partial class SettingsForm : Form
    {
        private bool _settingsChanged = false;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                // Load theme setting
                comboBoxTheme.SelectedItem = Properties.Settings.Default.Theme;

                // Load default export path
                textBoxExportPath.Text = Properties.Settings.Default.DefaultExportPath;

                // Load auto-save interval
                numericUpDownAutoSave.Value = Properties.Settings.Default.AutoSaveInterval;

                // Load remember window position setting
                checkBoxRememberPosition.Checked = Properties.Settings.Default.RememberWindowPosition;

                _settingsChanged = false;
                LoggingService.LogInformation("Settings loaded successfully");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Failed to load settings");
                MessageBox.Show($"Error loading settings: {ex.Message}", "Settings Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Save theme setting
                Properties.Settings.Default.Theme = comboBoxTheme.SelectedItem?.ToString() ?? "Light";

                // Save default export path
                Properties.Settings.Default.DefaultExportPath = textBoxExportPath.Text;

                // Save auto-save interval
                Properties.Settings.Default.AutoSaveInterval = (int)numericUpDownAutoSave.Value;

                // Save remember window position setting
                Properties.Settings.Default.RememberWindowPosition = checkBoxRememberPosition.Checked;

                // Persist to user.config file
                Properties.Settings.Default.Save();

                _settingsChanged = false;
                LoggingService.LogInformation($"Settings saved successfully: Theme={Properties.Settings.Default.Theme}, ExportPath={Properties.Settings.Default.DefaultExportPath}");

                // Apply theme immediately to all open forms
                ThemeManager.ApplyThemeToAllForms();

                MessageBox.Show("Settings saved successfully!", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Failed to save settings");
                MessageBox.Show($"Error saving settings: {ex.Message}", "Settings Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select default export folder:";
                    folderDialog.ShowNewFolderButton = true;

                    // Set initial directory to current path if it exists
                    if (!string.IsNullOrEmpty(textBoxExportPath.Text) && Directory.Exists(textBoxExportPath.Text))
                    {
                        folderDialog.SelectedPath = textBoxExportPath.Text;
                    }

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBoxExportPath.Text = folderDialog.SelectedPath;
                        MarkAsChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error browsing for export path");
                MessageBox.Show($"Error browsing for folder: {ex.Message}", "Browse Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (ValidateSettings())
            {
                SaveSettings();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (_settingsChanged)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to cancel?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonDefaults_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "This will reset all settings to their default values. Are you sure?",
                "Reset to Defaults",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                comboBoxTheme.SelectedItem = "Light";
                textBoxExportPath.Text = "C:\\Users\\Public\\Documents";
                numericUpDownAutoSave.Value = 300;
                checkBoxRememberPosition.Checked = true;
                MarkAsChanged();
            }
        }

        private bool ValidateSettings()
        {
            // Validate export path
            if (!string.IsNullOrEmpty(textBoxExportPath.Text))
            {
                try
                {
                    var directory = new DirectoryInfo(textBoxExportPath.Text);
                    if (!directory.Exists)
                    {
                        var result = MessageBox.Show(
                            $"The export path '{textBoxExportPath.Text}' does not exist.\n\nWould you like to create it?",
                            "Path Not Found",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            directory.Create();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Invalid export path: {ex.Message}", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Validate auto-save interval
            if (numericUpDownAutoSave.Value < 30 || numericUpDownAutoSave.Value > 3600)
            {
                MessageBox.Show("Auto-save interval must be between 30 and 3600 seconds.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void MarkAsChanged()
        {
            _settingsChanged = true;
            buttonSave.Enabled = true;
        }

        // Event handlers for change detection
        private void ComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }

        private void TextBoxExportPath_TextChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }

        private void NumericUpDownAutoSave_ValueChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }

        private void CheckBoxRememberPosition_CheckedChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_settingsChanged && DialogResult != DialogResult.OK)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to close?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
        }
    }
}
