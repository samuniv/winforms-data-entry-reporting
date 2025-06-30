namespace WinFormsDataApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            dataToolStripMenuItem = new ToolStripMenuItem();
            customersToolStripMenuItem = new ToolStripMenuItem();
            ordersToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            importToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            customerReportToolStripMenuItem = new ToolStripMenuItem();
            orderReportToolStripMenuItem = new ToolStripMenuItem();
            summaryReportToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            backupToolStripMenuItem = new ToolStripMenuItem();
            restoreToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStrip = new ToolStrip();
            newCustomerButton = new ToolStripButton();
            newOrderButton = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            saveButton = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            dashboardButton = new ToolStripButton();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            tabControl = new TabControl();
            dashboardTab = new TabPage();
            menuStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, dataToolStripMenuItem, reportsToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1200, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, toolStripSeparator1, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(195, 26);
            newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(195, 26);
            openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(192, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(195, 26);
            saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(195, 26);
            saveAsToolStripMenuItem.Text = "Save &As...";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(195, 26);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // dataToolStripMenuItem
            // 
            dataToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { customersToolStripMenuItem, ordersToolStripMenuItem, toolStripSeparator3, importToolStripMenuItem, exportToolStripMenuItem });
            dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            dataToolStripMenuItem.Size = new Size(55, 24);
            dataToolStripMenuItem.Text = "&Data";
            // 
            // customersToolStripMenuItem
            // 
            customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            customersToolStripMenuItem.Size = new Size(163, 26);
            customersToolStripMenuItem.Text = "&Customers";
            customersToolStripMenuItem.Click += CustomersToolStripMenuItem_Click;
            // 
            // ordersToolStripMenuItem
            // 
            ordersToolStripMenuItem.Name = "ordersToolStripMenuItem";
            ordersToolStripMenuItem.Size = new Size(163, 26);
            ordersToolStripMenuItem.Text = "&Orders";
            ordersToolStripMenuItem.Click += OrdersToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(160, 6);
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(163, 26);
            importToolStripMenuItem.Text = "&Import...";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(163, 26);
            exportToolStripMenuItem.Text = "&Export...";
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { customerReportToolStripMenuItem, orderReportToolStripMenuItem, summaryReportToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(74, 24);
            reportsToolStripMenuItem.Text = "&Reports";
            // 
            // customerReportToolStripMenuItem
            // 
            customerReportToolStripMenuItem.Name = "customerReportToolStripMenuItem";
            customerReportToolStripMenuItem.Size = new Size(208, 26);
            customerReportToolStripMenuItem.Text = "&Customer Report";
            // 
            // orderReportToolStripMenuItem
            // 
            orderReportToolStripMenuItem.Name = "orderReportToolStripMenuItem";
            orderReportToolStripMenuItem.Size = new Size(208, 26);
            orderReportToolStripMenuItem.Text = "&Order Report";
            // 
            // summaryReportToolStripMenuItem
            // 
            summaryReportToolStripMenuItem.Name = "summaryReportToolStripMenuItem";
            summaryReportToolStripMenuItem.Size = new Size(208, 26);
            summaryReportToolStripMenuItem.Text = "&Summary Report";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, backupToolStripMenuItem, restoreToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(58, 24);
            toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(144, 26);
            settingsToolStripMenuItem.Text = "&Settings";
            // 
            // backupToolStripMenuItem
            // 
            backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            backupToolStripMenuItem.Size = new Size(144, 26);
            backupToolStripMenuItem.Text = "&Backup...";
            // 
            // restoreToolStripMenuItem
            // 
            restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            restoreToolStripMenuItem.Size = new Size(144, 26);
            restoreToolStripMenuItem.Text = "&Restore...";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(142, 26);
            aboutToolStripMenuItem.Text = "&About...";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] { newCustomerButton, newOrderButton, toolStripSeparator4, saveButton, toolStripSeparator5, dashboardButton });
            toolStrip.Location = new Point(0, 28);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1200, 27);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
            // 
            // newCustomerButton
            // 
            newCustomerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            newCustomerButton.Name = "newCustomerButton";
            newCustomerButton.Size = new Size(103, 24);
            newCustomerButton.Text = "New Customer";
            newCustomerButton.Click += NewCustomerButton_Click;
            // 
            // newOrderButton
            // 
            newOrderButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            newOrderButton.Name = "newOrderButton";
            newOrderButton.Size = new Size(82, 24);
            newOrderButton.Text = "New Order";
            newOrderButton.Click += NewOrderButton_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 27);
            // 
            // saveButton
            // 
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(44, 24);
            saveButton.Text = "Save";
            saveButton.Click += SaveButton_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 27);
            // 
            // dashboardButton
            // 
            dashboardButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            dashboardButton.Name = "dashboardButton";
            dashboardButton.Size = new Size(85, 24);
            dashboardButton.Text = "Dashboard";
            dashboardButton.Click += DashboardButton_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, progressBar });
            statusStrip.Location = new Point(0, 728);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1200, 26);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(49, 20);
            statusLabel.Text = "Ready";
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(100, 18);
            progressBar.Visible = false;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(dashboardTab);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 55);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1200, 673);
            tabControl.TabIndex = 3;
            // 
            // dashboardTab
            // 
            dashboardTab.Location = new Point(4, 29);
            dashboardTab.Name = "dashboardTab";
            dashboardTab.Padding = new Padding(3);
            dashboardTab.Size = new Size(1192, 640);
            dashboardTab.TabIndex = 0;
            dashboardTab.Text = "Dashboard";
            dashboardTab.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 754);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WinForms Data Entry & Reporting Application";
            WindowState = FormWindowState.Maximized;
            Load += MainForm_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem dataToolStripMenuItem;
        private ToolStripMenuItem customersToolStripMenuItem;
        private ToolStripMenuItem ordersToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem customerReportToolStripMenuItem;
        private ToolStripMenuItem orderReportToolStripMenuItem;
        private ToolStripMenuItem summaryReportToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem backupToolStripMenuItem;
        private ToolStripMenuItem restoreToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton newCustomerButton;
        private ToolStripButton newOrderButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton saveButton;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton dashboardButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripProgressBar progressBar;
        private TabControl tabControl;
        private TabPage dashboardTab;
    }
}
