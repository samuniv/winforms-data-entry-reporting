namespace WinFormsDataApp.Forms
{
    partial class CsvImportWizardForm
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
            this.panelButtons = new Panel();
            this.btnCancel = new Button();
            this.btnNext = new Button();
            this.btnBack = new Button();
            this.panelMain = new Panel();
            this.panelSelectFile = new Panel();
            this.lblFileInstruction = new Label();
            this.lblFileFormat = new Label();
            this.txtFilePath = new TextBox();
            this.btnBrowse = new Button();
            this.chkHasHeaders = new CheckBox();
            this.panelPreview = new Panel();
            this.lblPreviewTitle = new Label();
            this.dataGridPreview = new DataGridView();
            this.lblValidationSummary = new Label();
            this.listValidationErrors = new ListBox();
            this.panelImport = new Panel();
            this.lblImportTitle = new Label();
            this.progressBar = new ProgressBar();
            this.lblImportStatus = new Label();
            this.panelComplete = new Panel();
            this.lblCompleteTitle = new Label();
            this.txtImportSummary = new TextBox();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelSelectFile.SuspendLayout();
            this.panelPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPreview)).BeginInit();
            this.panelImport.SuspendLayout();
            this.panelComplete.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnNext);
            this.panelButtons.Controls.Add(this.btnBack);
            this.panelButtons.Dock = DockStyle.Bottom;
            this.panelButtons.Location = new Point(0, 520);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new Size(784, 50);
            this.panelButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCancel.Location = new Point(697, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnNext.Location = new Point(616, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(75, 26);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnBack.Location = new Point(535, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(75, 26);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelSelectFile);
            this.panelMain.Controls.Add(this.panelPreview);
            this.panelMain.Controls.Add(this.panelImport);
            this.panelMain.Controls.Add(this.panelComplete);
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Location = new Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(784, 520);
            this.panelMain.TabIndex = 1;
            // 
            // panelSelectFile
            // 
            this.panelSelectFile.Controls.Add(this.chkHasHeaders);
            this.panelSelectFile.Controls.Add(this.btnBrowse);
            this.panelSelectFile.Controls.Add(this.txtFilePath);
            this.panelSelectFile.Controls.Add(this.lblFileFormat);
            this.panelSelectFile.Controls.Add(this.lblFileInstruction);
            this.panelSelectFile.Dock = DockStyle.Fill;
            this.panelSelectFile.Location = new Point(0, 0);
            this.panelSelectFile.Name = "panelSelectFile";
            this.panelSelectFile.Size = new Size(784, 520);
            this.panelSelectFile.TabIndex = 0;
            // 
            // lblFileInstruction
            // 
            this.lblFileInstruction.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblFileInstruction.Location = new Point(20, 20);
            this.lblFileInstruction.Name = "lblFileInstruction";
            this.lblFileInstruction.Size = new Size(744, 30);
            this.lblFileInstruction.TabIndex = 0;
            this.lblFileInstruction.Text = "Please select a CSV file";
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.Location = new Point(20, 60);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new Size(744, 80);
            this.lblFileFormat.TabIndex = 1;
            this.lblFileFormat.Text = "Expected file format information";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new Point(20, 160);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new Size(580, 23);
            this.txtFilePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new Point(610, 159);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(75, 25);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Checked = true;
            this.chkHasHeaders.CheckState = CheckState.Checked;
            this.chkHasHeaders.Location = new Point(20, 200);
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.Size = new Size(183, 19);
            this.chkHasHeaders.TabIndex = 4;
            this.chkHasHeaders.Text = "First row contains column headers";
            this.chkHasHeaders.UseVisualStyleBackColor = true;
            // 
            // panelPreview
            // 
            this.panelPreview.Controls.Add(this.listValidationErrors);
            this.panelPreview.Controls.Add(this.lblValidationSummary);
            this.panelPreview.Controls.Add(this.dataGridPreview);
            this.panelPreview.Controls.Add(this.lblPreviewTitle);
            this.panelPreview.Dock = DockStyle.Fill;
            this.panelPreview.Location = new Point(0, 0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new Size(784, 520);
            this.panelPreview.TabIndex = 1;
            this.panelPreview.Visible = false;
            // 
            // lblPreviewTitle
            // 
            this.lblPreviewTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblPreviewTitle.Location = new Point(20, 20);
            this.lblPreviewTitle.Name = "lblPreviewTitle";
            this.lblPreviewTitle.Size = new Size(744, 30);
            this.lblPreviewTitle.TabIndex = 0;
            this.lblPreviewTitle.Text = "Preview and Validation";
            // 
            // dataGridPreview
            // 
            this.dataGridPreview.AllowUserToAddRows = false;
            this.dataGridPreview.AllowUserToDeleteRows = false;
            this.dataGridPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPreview.Location = new Point(20, 60);
            this.dataGridPreview.Name = "dataGridPreview";
            this.dataGridPreview.ReadOnly = true;
            this.dataGridPreview.Size = new Size(744, 250);
            this.dataGridPreview.TabIndex = 1;
            // 
            // lblValidationSummary
            // 
            this.lblValidationSummary.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblValidationSummary.Location = new Point(20, 320);
            this.lblValidationSummary.Name = "lblValidationSummary";
            this.lblValidationSummary.Size = new Size(744, 25);
            this.lblValidationSummary.TabIndex = 2;
            this.lblValidationSummary.Text = "Validation Results";
            // 
            // listValidationErrors
            // 
            this.listValidationErrors.FormattingEnabled = true;
            this.listValidationErrors.ItemHeight = 15;
            this.listValidationErrors.Location = new Point(20, 350);
            this.listValidationErrors.Name = "listValidationErrors";
            this.listValidationErrors.Size = new Size(744, 150);
            this.listValidationErrors.TabIndex = 3;
            // 
            // panelImport
            // 
            this.panelImport.Controls.Add(this.lblImportStatus);
            this.panelImport.Controls.Add(this.progressBar);
            this.panelImport.Controls.Add(this.lblImportTitle);
            this.panelImport.Dock = DockStyle.Fill;
            this.panelImport.Location = new Point(0, 0);
            this.panelImport.Name = "panelImport";
            this.panelImport.Size = new Size(784, 520);
            this.panelImport.TabIndex = 2;
            this.panelImport.Visible = false;
            // 
            // lblImportTitle
            // 
            this.lblImportTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblImportTitle.Location = new Point(20, 20);
            this.lblImportTitle.Name = "lblImportTitle";
            this.lblImportTitle.Size = new Size(744, 30);
            this.lblImportTitle.TabIndex = 0;
            this.lblImportTitle.Text = "Importing Data...";
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(20, 200);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(744, 30);
            this.progressBar.TabIndex = 1;
            // 
            // lblImportStatus
            // 
            this.lblImportStatus.Location = new Point(20, 240);
            this.lblImportStatus.Name = "lblImportStatus";
            this.lblImportStatus.Size = new Size(744, 30);
            this.lblImportStatus.TabIndex = 2;
            this.lblImportStatus.Text = "Starting import...";
            this.lblImportStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelComplete
            // 
            this.panelComplete.Controls.Add(this.txtImportSummary);
            this.panelComplete.Controls.Add(this.lblCompleteTitle);
            this.panelComplete.Dock = DockStyle.Fill;
            this.panelComplete.Location = new Point(0, 0);
            this.panelComplete.Name = "panelComplete";
            this.panelComplete.Size = new Size(784, 520);
            this.panelComplete.TabIndex = 3;
            this.panelComplete.Visible = false;
            // 
            // lblCompleteTitle
            // 
            this.lblCompleteTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblCompleteTitle.Location = new Point(20, 20);
            this.lblCompleteTitle.Name = "lblCompleteTitle";
            this.lblCompleteTitle.Size = new Size(744, 30);
            this.lblCompleteTitle.TabIndex = 0;
            this.lblCompleteTitle.Text = "Import Complete";
            // 
            // txtImportSummary
            // 
            this.txtImportSummary.Font = new Font("Consolas", 9F);
            this.txtImportSummary.Location = new Point(20, 60);
            this.txtImportSummary.Multiline = true;
            this.txtImportSummary.Name = "txtImportSummary";
            this.txtImportSummary.ReadOnly = true;
            this.txtImportSummary.ScrollBars = ScrollBars.Vertical;
            this.txtImportSummary.Size = new Size(744, 440);
            this.txtImportSummary.TabIndex = 1;
            // 
            // CsvImportWizardForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 570);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CsvImportWizardForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "CSV Import Wizard";
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelSelectFile.ResumeLayout(false);
            this.panelSelectFile.PerformLayout();
            this.panelPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPreview)).EndInit();
            this.panelImport.ResumeLayout(false);
            this.panelComplete.ResumeLayout(false);
            this.panelComplete.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelButtons;
        private Button btnCancel;
        private Button btnNext;
        private Button btnBack;
        private Panel panelMain;
        private Panel panelSelectFile;
        private Label lblFileInstruction;
        private Label lblFileFormat;
        private TextBox txtFilePath;
        private Button btnBrowse;
        private CheckBox chkHasHeaders;
        private Panel panelPreview;
        private Label lblPreviewTitle;
        private DataGridView dataGridPreview;
        private Label lblValidationSummary;
        private ListBox listValidationErrors;
        private Panel panelImport;
        private Label lblImportTitle;
        private ProgressBar progressBar;
        private Label lblImportStatus;
        private Panel panelComplete;
        private Label lblCompleteTitle;
        private TextBox txtImportSummary;
    }
}
