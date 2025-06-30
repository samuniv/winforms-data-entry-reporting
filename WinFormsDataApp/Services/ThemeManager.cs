using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services
{
    public static class ThemeColors
    {
        // Light Theme
        public static Color LightBackColor = SystemColors.Control;
        public static Color LightForeColor = SystemColors.ControlText;
        public static Color LightTextBoxBack = SystemColors.Window;
        public static Color LightMenuBack = SystemColors.Menu;
        public static Color LightButtonBack = SystemColors.ButtonFace;

        // Dark Theme
        public static Color DarkBackColor = Color.FromArgb(45, 45, 48);
        public static Color DarkForeColor = Color.White;
        public static Color DarkTextBoxBack = Color.FromArgb(60, 60, 60);
        public static Color DarkMenuBack = Color.FromArgb(37, 37, 38);
        public static Color DarkButtonBack = Color.FromArgb(62, 62, 64);
    }

    public static class ThemeManager
    {
        public static void ApplyTheme(Control parentControl)
        {
            try
            {
                string theme = Properties.Settings.Default.Theme;
                bool isDark = theme == "Dark";

                LoggingService.LogInformation($"Applying {theme} theme to form: {parentControl.Name}");

                var backColor = isDark ? ThemeColors.DarkBackColor : ThemeColors.LightBackColor;
                var foreColor = isDark ? ThemeColors.DarkForeColor : ThemeColors.LightForeColor;

                ApplyThemeRecursive(parentControl, backColor, foreColor, isDark);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to apply theme to control: {parentControl?.Name ?? "Unknown"}");
            }
        }

        private static void ApplyThemeRecursive(Control control, Color backColor, Color foreColor, bool isDark)
        {
            try
            {
                // Skip certain controls that shouldn't be themed
                if (control is PictureBox || control is ProgressBar)
                    return;

                // Apply base colors
                control.BackColor = backColor;
                control.ForeColor = foreColor;

                // Special handling for specific control types
                switch (control)
                {
                    case TextBox textBox:
                        textBox.BackColor = isDark ? ThemeColors.DarkTextBoxBack : ThemeColors.LightTextBoxBack;
                        textBox.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case ComboBox comboBox:
                        comboBox.BackColor = isDark ? ThemeColors.DarkTextBoxBack : ThemeColors.LightTextBoxBack;
                        comboBox.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case NumericUpDown numericUpDown:
                        numericUpDown.BackColor = isDark ? ThemeColors.DarkTextBoxBack : ThemeColors.LightTextBoxBack;
                        numericUpDown.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case MaskedTextBox maskedTextBox:
                        maskedTextBox.BackColor = isDark ? ThemeColors.DarkTextBoxBack : ThemeColors.LightTextBoxBack;
                        maskedTextBox.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case Button button:
                        button.BackColor = isDark ? ThemeColors.DarkButtonBack : ThemeColors.LightButtonBack;
                        button.ForeColor = isDark ? Color.White : Color.Black;
                        button.FlatStyle = isDark ? FlatStyle.Flat : FlatStyle.Standard;
                        if (isDark)
                        {
                            button.FlatAppearance.BorderColor = Color.Gray;
                            button.FlatAppearance.BorderSize = 1;
                        }
                        break;

                    case MenuStrip menuStrip:
                        menuStrip.BackColor = isDark ? ThemeColors.DarkMenuBack : ThemeColors.LightMenuBack;
                        menuStrip.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case StatusStrip statusStrip:
                        statusStrip.BackColor = isDark ? ThemeColors.DarkMenuBack : ThemeColors.LightMenuBack;
                        statusStrip.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case ToolStrip toolStrip:
                        toolStrip.BackColor = isDark ? ThemeColors.DarkMenuBack : ThemeColors.LightMenuBack;
                        toolStrip.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case DataGridView dataGridView:
                        ApplyDataGridViewTheme(dataGridView, isDark);
                        break;

                    case TabControl tabControl:
                        tabControl.BackColor = isDark ? ThemeColors.DarkBackColor : ThemeColors.LightBackColor;
                        tabControl.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case GroupBox groupBox:
                        groupBox.ForeColor = isDark ? Color.White : Color.Black;
                        break;

                    case Label label:
                        label.ForeColor = isDark ? Color.White : Color.Black;
                        break;
                }

                // Apply to all child controls recursively
                foreach (Control child in control.Controls)
                {
                    ApplyThemeRecursive(child, backColor, foreColor, isDark);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to apply theme to control: {control?.GetType().Name ?? "Unknown"} - {control?.Name ?? "Unknown"}");
            }
        }

        private static void ApplyDataGridViewTheme(DataGridView dataGridView, bool isDark)
        {
            if (isDark)
            {
                dataGridView.BackgroundColor = ThemeColors.DarkBackColor;
                dataGridView.ForeColor = Color.White;
                dataGridView.GridColor = Color.Gray;
                dataGridView.DefaultCellStyle.BackColor = ThemeColors.DarkTextBoxBack;
                dataGridView.DefaultCellStyle.ForeColor = Color.White;
                dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(75, 110, 175);
                dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(55, 55, 58);
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = ThemeColors.DarkMenuBack;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView.RowHeadersDefaultCellStyle.BackColor = ThemeColors.DarkMenuBack;
                dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                dataGridView.BackgroundColor = SystemColors.Window;
                dataGridView.ForeColor = SystemColors.ControlText;
                dataGridView.GridColor = SystemColors.ControlDark;
                dataGridView.DefaultCellStyle.BackColor = SystemColors.Window;
                dataGridView.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                dataGridView.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                dataGridView.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dataGridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            }
        }

        public static void ApplyThemeToAllForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    ApplyTheme(form);
                    form.Refresh();
                }
                LoggingService.LogInformation($"Applied theme to {Application.OpenForms.Count} open forms");
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Failed to apply theme to all forms");
            }
        }
    }
}
