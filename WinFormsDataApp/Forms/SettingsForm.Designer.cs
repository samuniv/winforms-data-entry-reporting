namespace WinFormsDataApp.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabPageAppearance = new TabPage();
            groupBoxTheme = new GroupBox();
            labelThemeDescription = new Label();
            labelTheme = new Label();
            comboBoxTheme = new ComboBox();
            tabPagePaths = new TabPage();
            groupBoxExportPath = new GroupBox();
            buttonBrowse = new Button();
            textBoxExportPath = new TextBox();
            labelExportPath = new Label();
            labelExportDescription = new Label();
            tabPageBehavior = new TabPage();
            groupBoxAutoSave = new GroupBox();
            labelAutoSaveSeconds = new Label();
            numericUpDownAutoSave = new NumericUpDown();
            labelAutoSave = new Label();
            checkBoxRememberPosition = new CheckBox();
            panelButtons = new Panel();
            buttonDefaults = new Button();
            buttonCancel = new Button();
            buttonSave = new Button();
            tabControl.SuspendLayout();
            tabPageAppearance.SuspendLayout();
            groupBoxTheme.SuspendLayout();
            tabPagePaths.SuspendLayout();
            groupBoxExportPath.SuspendLayout();
            tabPageBehavior.SuspendLayout();
            groupBoxAutoSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAutoSave).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageAppearance);
            tabControl.Controls.Add(tabPagePaths);
            tabControl.Controls.Add(tabPageBehavior);
            tabControl.Location = new Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(460, 300);
            tabControl.TabIndex = 0;
            // 
            // tabPageAppearance
            // 
            tabPageAppearance.Controls.Add(groupBoxTheme);
            tabPageAppearance.Location = new Point(4, 29);
            tabPageAppearance.Name = "tabPageAppearance";
            tabPageAppearance.Padding = new Padding(3);
            tabPageAppearance.Size = new Size(452, 267);
            tabPageAppearance.TabIndex = 0;
            tabPageAppearance.Text = "Appearance";
            tabPageAppearance.UseVisualStyleBackColor = true;
            // 
            // groupBoxTheme
            // 
            groupBoxTheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxTheme.Controls.Add(labelThemeDescription);
            groupBoxTheme.Controls.Add(labelTheme);
            groupBoxTheme.Controls.Add(comboBoxTheme);
            groupBoxTheme.Location = new Point(6, 6);
            groupBoxTheme.Name = "groupBoxTheme";
            groupBoxTheme.Size = new Size(440, 100);
            groupBoxTheme.TabIndex = 0;
            groupBoxTheme.TabStop = false;
            groupBoxTheme.Text = "Theme Settings";
            // 
            // labelThemeDescription
            // 
            labelThemeDescription.ForeColor = SystemColors.GrayText;
            labelThemeDescription.Location = new Point(6, 60);
            labelThemeDescription.Name = "labelThemeDescription";
            labelThemeDescription.Size = new Size(428, 32);
            labelThemeDescription.TabIndex = 2;
            labelThemeDescription.Text = "Changes to the theme will be applied immediately to all open forms.";
            // 
            // labelTheme
            // 
            labelTheme.AutoSize = true;
            labelTheme.Location = new Point(6, 26);
            labelTheme.Name = "labelTheme";
            labelTheme.Size = new Size(54, 20);
            labelTheme.TabIndex = 0;
            labelTheme.Text = "Theme:";
            // 
            // comboBoxTheme
            // 
            comboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTheme.FormattingEnabled = true;
            comboBoxTheme.Items.AddRange(new object[] { "Light", "Dark" });
            comboBoxTheme.Location = new Point(80, 23);
            comboBoxTheme.Name = "comboBoxTheme";
            comboBoxTheme.Size = new Size(150, 28);
            comboBoxTheme.TabIndex = 1;
            comboBoxTheme.SelectedIndexChanged += ComboBoxTheme_SelectedIndexChanged;
            // 
            // tabPagePaths
            // 
            tabPagePaths.Controls.Add(groupBoxExportPath);
            tabPagePaths.Location = new Point(4, 29);
            tabPagePaths.Name = "tabPagePaths";
            tabPagePaths.Padding = new Padding(3);
            tabPagePaths.Size = new Size(452, 267);
            tabPagePaths.TabIndex = 1;
            tabPagePaths.Text = "Paths";
            tabPagePaths.UseVisualStyleBackColor = true;
            // 
            // groupBoxExportPath
            // 
            groupBoxExportPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxExportPath.Controls.Add(buttonBrowse);
            groupBoxExportPath.Controls.Add(textBoxExportPath);
            groupBoxExportPath.Controls.Add(labelExportPath);
            groupBoxExportPath.Controls.Add(labelExportDescription);
            groupBoxExportPath.Location = new Point(6, 6);
            groupBoxExportPath.Name = "groupBoxExportPath";
            groupBoxExportPath.Size = new Size(440, 120);
            groupBoxExportPath.TabIndex = 0;
            groupBoxExportPath.TabStop = false;
            groupBoxExportPath.Text = "Default Export Path";
            // 
            // buttonBrowse
            // 
            buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowse.Location = new Point(359, 48);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(75, 30);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // textBoxExportPath
            // 
            textBoxExportPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxExportPath.Location = new Point(6, 48);
            textBoxExportPath.Name = "textBoxExportPath";
            textBoxExportPath.Size = new Size(347, 27);
            textBoxExportPath.TabIndex = 1;
            textBoxExportPath.TextChanged += TextBoxExportPath_TextChanged;
            // 
            // labelExportPath
            // 
            labelExportPath.AutoSize = true;
            labelExportPath.Location = new Point(6, 25);
            labelExportPath.Name = "labelExportPath";
            labelExportPath.Size = new Size(129, 20);
            labelExportPath.TabIndex = 0;
            labelExportPath.Text = "Default folder for:";
            // 
            // labelExportDescription
            // 
            labelExportDescription.ForeColor = SystemColors.GrayText;
            labelExportDescription.Location = new Point(6, 82);
            labelExportDescription.Name = "labelExportDescription";
            labelExportDescription.Size = new Size(428, 32);
            labelExportDescription.TabIndex = 3;
            labelExportDescription.Text = "This folder will be used as the default location for exported reports, backups, and other files.";
            // 
            // tabPageBehavior
            // 
            tabPageBehavior.Controls.Add(groupBoxAutoSave);
            tabPageBehavior.Controls.Add(checkBoxRememberPosition);
            tabPageBehavior.Location = new Point(4, 29);
            tabPageBehavior.Name = "tabPageBehavior";
            tabPageBehavior.Padding = new Padding(3);
            tabPageBehavior.Size = new Size(452, 267);
            tabPageBehavior.TabIndex = 2;
            tabPageBehavior.Text = "Behavior";
            tabPageBehavior.UseVisualStyleBackColor = true;
            // 
            // groupBoxAutoSave
            // 
            groupBoxAutoSave.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxAutoSave.Controls.Add(labelAutoSaveSeconds);
            groupBoxAutoSave.Controls.Add(numericUpDownAutoSave);
            groupBoxAutoSave.Controls.Add(labelAutoSave);
            groupBoxAutoSave.Location = new Point(6, 50);
            groupBoxAutoSave.Name = "groupBoxAutoSave";
            groupBoxAutoSave.Size = new Size(440, 80);
            groupBoxAutoSave.TabIndex = 1;
            groupBoxAutoSave.TabStop = false;
            groupBoxAutoSave.Text = "Auto-Save Settings";
            // 
            // labelAutoSaveSeconds
            // 
            labelAutoSaveSeconds.AutoSize = true;
            labelAutoSaveSeconds.Location = new Point(180, 28);
            labelAutoSaveSeconds.Name = "labelAutoSaveSeconds";
            labelAutoSaveSeconds.Size = new Size(63, 20);
            labelAutoSaveSeconds.TabIndex = 2;
            labelAutoSaveSeconds.Text = "seconds";
            // 
            // numericUpDownAutoSave
            // 
            numericUpDownAutoSave.Increment = new decimal(new int[] { 30, 0, 0, 0 });
            numericUpDownAutoSave.Location = new Point(120, 26);
            numericUpDownAutoSave.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numericUpDownAutoSave.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            numericUpDownAutoSave.Name = "numericUpDownAutoSave";
            numericUpDownAutoSave.Size = new Size(54, 27);
            numericUpDownAutoSave.TabIndex = 1;
            numericUpDownAutoSave.Value = new decimal(new int[] { 300, 0, 0, 0 });
            numericUpDownAutoSave.ValueChanged += NumericUpDownAutoSave_ValueChanged;
            // 
            // labelAutoSave
            // 
            labelAutoSave.AutoSize = true;
            labelAutoSave.Location = new Point(6, 28);
            labelAutoSave.Name = "labelAutoSave";
            labelAutoSave.Size = new Size(108, 20);
            labelAutoSave.TabIndex = 0;
            labelAutoSave.Text = "Auto-save every";
            // 
            // checkBoxRememberPosition
            // 
            checkBoxRememberPosition.AutoSize = true;
            checkBoxRememberPosition.Location = new Point(6, 16);
            checkBoxRememberPosition.Name = "checkBoxRememberPosition";
            checkBoxRememberPosition.Size = new Size(201, 24);
            checkBoxRememberPosition.TabIndex = 0;
            checkBoxRememberPosition.Text = "Remember window position";
            checkBoxRememberPosition.UseVisualStyleBackColor = true;
            checkBoxRememberPosition.CheckedChanged += CheckBoxRememberPosition_CheckedChanged;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonDefaults);
            panelButtons.Controls.Add(buttonCancel);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 324);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(484, 50);
            panelButtons.TabIndex = 1;
            // 
            // buttonDefaults
            // 
            buttonDefaults.Location = new Point(12, 12);
            buttonDefaults.Name = "buttonDefaults";
            buttonDefaults.Size = new Size(100, 30);
            buttonDefaults.TabIndex = 0;
            buttonDefaults.Text = "Reset to Defaults";
            buttonDefaults.UseVisualStyleBackColor = true;
            buttonDefaults.Click += ButtonDefaults_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(397, 12);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 30);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSave.Location = new Point(316, 12);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 30);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // SettingsForm
            // 
            AcceptButton = buttonSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(484, 374);
            Controls.Add(panelButtons);
            Controls.Add(tabControl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Application Settings";
            tabControl.ResumeLayout(false);
            tabPageAppearance.ResumeLayout(false);
            groupBoxTheme.ResumeLayout(false);
            groupBoxTheme.PerformLayout();
            tabPagePaths.ResumeLayout(false);
            groupBoxExportPath.ResumeLayout(false);
            groupBoxExportPath.PerformLayout();
            tabPageBehavior.ResumeLayout(false);
            tabPageBehavior.PerformLayout();
            groupBoxAutoSave.ResumeLayout(false);
            groupBoxAutoSave.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAutoSave).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPageAppearance;
        private TabPage tabPagePaths;
        private TabPage tabPageBehavior;
        private GroupBox groupBoxTheme;
        private Label labelTheme;
        private ComboBox comboBoxTheme;
        private Label labelThemeDescription;
        private GroupBox groupBoxExportPath;
        private Label labelExportPath;
        private TextBox textBoxExportPath;
        private Button buttonBrowse;
        private Label labelExportDescription;
        private CheckBox checkBoxRememberPosition;
        private GroupBox groupBoxAutoSave;
        private Label labelAutoSave;
        private NumericUpDown numericUpDownAutoSave;
        private Label labelAutoSaveSeconds;
        private Panel panelButtons;
        private Button buttonSave;
        private Button buttonCancel;
        private Button buttonDefaults;
    }
}
