namespace WinFormsDataApp.Forms
{
    partial class OrderForm
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
            components = new System.ComponentModel.Container();
            dataGridView = new DataGridView();
            groupBoxDetails = new GroupBox();
            buttonDelete = new Button();
            buttonCancel = new Button();
            buttonSave = new Button();
            dateTimePickerOrderDate = new DateTimePicker();
            numericUpDownQuantity = new NumericUpDown();
            comboBoxCustomer = new ComboBox();
            textBoxId = new TextBox();
            labelOrderDate = new Label();
            labelQuantity = new Label();
            labelCustomer = new Label();
            labelId = new Label();
            buttonNew = new Button();
            buttonRefresh = new Button();
            buttonPrintInvoice = new Button();
            errorProvider = new ErrorProvider(components);
            bindingSource = new BindingSource(components);
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            customerBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBoxDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).BeginInit();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)customerBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.DataSource = bindingSource;
            dataGridView.Location = new Point(12, 12);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(760, 300);
            dataGridView.TabIndex = 0;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            // 
            // groupBoxDetails
            // 
            groupBoxDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDetails.Controls.Add(buttonDelete);
            groupBoxDetails.Controls.Add(buttonCancel);
            groupBoxDetails.Controls.Add(buttonSave);
            groupBoxDetails.Controls.Add(dateTimePickerOrderDate);
            groupBoxDetails.Controls.Add(numericUpDownQuantity);
            groupBoxDetails.Controls.Add(comboBoxCustomer);
            groupBoxDetails.Controls.Add(textBoxId);
            groupBoxDetails.Controls.Add(labelOrderDate);
            groupBoxDetails.Controls.Add(labelQuantity);
            groupBoxDetails.Controls.Add(labelCustomer);
            groupBoxDetails.Controls.Add(labelId);
            groupBoxDetails.Location = new Point(12, 350);
            groupBoxDetails.Name = "groupBoxDetails";
            groupBoxDetails.Size = new Size(760, 160);
            groupBoxDetails.TabIndex = 1;
            groupBoxDetails.TabStop = false;
            groupBoxDetails.Text = "Order Details";
            // 
            // buttonDelete
            // 
            buttonDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDelete.Location = new Point(594, 120);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 30);
            buttonDelete.TabIndex = 10;
            buttonDelete.Text = "&Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += ButtonDelete_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Location = new Point(513, 120);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 30);
            buttonCancel.TabIndex = 9;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(432, 120);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 30);
            buttonSave.TabIndex = 8;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // dateTimePickerOrderDate
            // 
            dateTimePickerOrderDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dateTimePickerOrderDate.Format = DateTimePickerFormat.Short;
            dateTimePickerOrderDate.Location = new Point(110, 90);
            dateTimePickerOrderDate.Name = "dateTimePickerOrderDate";
            dateTimePickerOrderDate.Size = new Size(200, 27);
            dateTimePickerOrderDate.TabIndex = 7;
            dateTimePickerOrderDate.ValueChanged += DateTimePickerOrderDate_ValueChanged;
            // 
            // numericUpDownQuantity
            // 
            numericUpDownQuantity.Location = new Point(110, 60);
            numericUpDownQuantity.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDownQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownQuantity.Name = "numericUpDownQuantity";
            numericUpDownQuantity.Size = new Size(120, 27);
            numericUpDownQuantity.TabIndex = 6;
            numericUpDownQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownQuantity.ValueChanged += NumericUpDownQuantity_ValueChanged;
            // 
            // comboBoxCustomer
            // 
            comboBoxCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxCustomer.DataSource = customerBindingSource;
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCustomer.FormattingEnabled = true;
            comboBoxCustomer.Location = new Point(110, 30);
            comboBoxCustomer.Name = "comboBoxCustomer";
            comboBoxCustomer.Size = new Size(559, 28);
            comboBoxCustomer.TabIndex = 5;
            comboBoxCustomer.ValueMember = "Id";
            comboBoxCustomer.SelectedIndexChanged += ComboBoxCustomer_SelectedIndexChanged;
            // 
            // textBoxId
            // 
            textBoxId.Location = new Point(110, 0);
            textBoxId.Name = "textBoxId";
            textBoxId.ReadOnly = true;
            textBoxId.Size = new Size(100, 27);
            textBoxId.TabIndex = 4;
            textBoxId.TabStop = false;
            // 
            // labelOrderDate
            // 
            labelOrderDate.AutoSize = true;
            labelOrderDate.Location = new Point(20, 90);
            labelOrderDate.Name = "labelOrderDate";
            labelOrderDate.Size = new Size(85, 20);
            labelOrderDate.TabIndex = 3;
            labelOrderDate.Text = "Order Date:";
            // 
            // labelQuantity
            // 
            labelQuantity.AutoSize = true;
            labelQuantity.Location = new Point(20, 60);
            labelQuantity.Name = "labelQuantity";
            labelQuantity.Size = new Size(68, 20);
            labelQuantity.TabIndex = 2;
            labelQuantity.Text = "Quantity:";
            // 
            // labelCustomer
            // 
            labelCustomer.AutoSize = true;
            labelCustomer.Location = new Point(20, 30);
            labelCustomer.Name = "labelCustomer";
            labelCustomer.Size = new Size(75, 20);
            labelCustomer.TabIndex = 1;
            labelCustomer.Text = "Customer:";
            // 
            // labelId
            // 
            labelId.AutoSize = true;
            labelId.Location = new Point(20, 0);
            labelId.Name = "labelId";
            labelId.Size = new Size(25, 20);
            labelId.TabIndex = 0;
            labelId.Text = "ID:";
            // 
            // buttonNew
            // 
            buttonNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNew.Location = new Point(12, 318);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(75, 30);
            buttonNew.TabIndex = 2;
            buttonNew.Text = "&New";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += ButtonNew_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonRefresh.Location = new Point(93, 318);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(75, 30);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "&Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += ButtonRefresh_Click;
            // 
            // buttonPrintInvoice
            // 
            buttonPrintInvoice.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonPrintInvoice.Location = new Point(174, 318);
            buttonPrintInvoice.Name = "buttonPrintInvoice";
            buttonPrintInvoice.Size = new Size(100, 30);
            buttonPrintInvoice.TabIndex = 4;
            buttonPrintInvoice.Text = "Print &Invoice";
            buttonPrintInvoice.UseVisualStyleBackColor = true;
            buttonPrintInvoice.Click += ButtonPrintInvoice_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 518);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(784, 26);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(49, 20);
            statusLabel.Text = "Ready";
            // 
            // OrderForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 544);
            Controls.Add(statusStrip);
            Controls.Add(buttonPrintInvoice);
            Controls.Add(buttonRefresh);
            Controls.Add(buttonNew);
            Controls.Add(groupBoxDetails);
            Controls.Add(dataGridView);
            MinimumSize = new Size(800, 600);
            Name = "OrderForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Order Management";
            Load += OrderForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBoxDetails.ResumeLayout(false);
            groupBoxDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)customerBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private GroupBox groupBoxDetails;
        private Label labelId;
        private Label labelCustomer;
        private Label labelQuantity;
        private Label labelOrderDate;
        private TextBox textBoxId;
        private ComboBox comboBoxCustomer;
        private NumericUpDown numericUpDownQuantity;
        private DateTimePicker dateTimePickerOrderDate;
        private Button buttonSave;
        private Button buttonCancel;
        private Button buttonDelete;
        private Button buttonNew;
        private Button buttonRefresh;
        private Button buttonPrintInvoice;
        private ErrorProvider errorProvider;
        private BindingSource bindingSource;
        private BindingSource customerBindingSource;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
    }
}
