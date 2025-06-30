namespace WinFormsDataApp.Forms
{
    partial class CustomerForm
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
            textBoxAddress = new TextBox();
            maskedTextBoxPhone = new MaskedTextBox();
            textBoxEmail = new TextBox();
            textBoxName = new TextBox();
            textBoxId = new TextBox();
            labelAddress = new Label();
            labelPhone = new Label();
            labelEmail = new Label();
            labelName = new Label();
            labelId = new Label();
            buttonNew = new Button();
            buttonRefresh = new Button();
            errorProvider = new ErrorProvider(components);
            bindingSource = new BindingSource(components);
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBoxDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).BeginInit();
            statusStrip.SuspendLayout();
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
            dataGridView.Size = new Size(660, 300);
            dataGridView.TabIndex = 0;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            // 
            // groupBoxDetails
            // 
            groupBoxDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDetails.Controls.Add(buttonDelete);
            groupBoxDetails.Controls.Add(buttonCancel);
            groupBoxDetails.Controls.Add(buttonSave);
            groupBoxDetails.Controls.Add(textBoxAddress);
            groupBoxDetails.Controls.Add(maskedTextBoxPhone);
            groupBoxDetails.Controls.Add(textBoxEmail);
            groupBoxDetails.Controls.Add(textBoxName);
            groupBoxDetails.Controls.Add(textBoxId);
            groupBoxDetails.Controls.Add(labelAddress);
            groupBoxDetails.Controls.Add(labelPhone);
            groupBoxDetails.Controls.Add(labelEmail);
            groupBoxDetails.Controls.Add(labelName);
            groupBoxDetails.Controls.Add(labelId);
            groupBoxDetails.Location = new Point(12, 350);
            groupBoxDetails.Name = "groupBoxDetails";
            groupBoxDetails.Size = new Size(660, 200);
            groupBoxDetails.TabIndex = 1;
            groupBoxDetails.TabStop = false;
            groupBoxDetails.Text = "Customer Details";
            // 
            // buttonDelete
            // 
            buttonDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDelete.Location = new Point(473, 160);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 30);
            buttonDelete.TabIndex = 12;
            buttonDelete.Text = "&Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += ButtonDelete_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Location = new Point(392, 160);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 30);
            buttonCancel.TabIndex = 11;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(311, 160);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 30);
            buttonSave.TabIndex = 10;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxAddress.Location = new Point(110, 120);
            textBoxAddress.Multiline = true;
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(438, 34);
            textBoxAddress.TabIndex = 9;
            textBoxAddress.Validating += TextBoxAddress_Validating;
            // 
            // maskedTextBoxPhone
            // 
            maskedTextBoxPhone.Location = new Point(110, 90);
            maskedTextBoxPhone.Mask = "(999) 000-0000";
            maskedTextBoxPhone.Name = "maskedTextBoxPhone";
            maskedTextBoxPhone.Size = new Size(150, 27);
            maskedTextBoxPhone.TabIndex = 8;
            maskedTextBoxPhone.Validating += MaskedTextBoxPhone_Validating;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxEmail.Location = new Point(110, 60);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(438, 27);
            textBoxEmail.TabIndex = 7;
            textBoxEmail.Validating += TextBoxEmail_Validating;
            // 
            // textBoxName
            // 
            textBoxName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxName.Location = new Point(110, 30);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(438, 27);
            textBoxName.TabIndex = 6;
            textBoxName.Validating += TextBoxName_Validating;
            // 
            // textBoxId
            // 
            textBoxId.Location = new Point(110, 0);
            textBoxId.Name = "textBoxId";
            textBoxId.ReadOnly = true;
            textBoxId.Size = new Size(100, 27);
            textBoxId.TabIndex = 5;
            textBoxId.TabStop = false;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(20, 120);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(65, 20);
            labelAddress.TabIndex = 4;
            labelAddress.Text = "Address:";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(20, 90);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(53, 20);
            labelPhone.TabIndex = 3;
            labelPhone.Text = "Phone:";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(20, 60);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(49, 20);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "Email:";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(20, 30);
            labelName.Name = "labelName";
            labelName.Size = new Size(52, 20);
            labelName.TabIndex = 1;
            labelName.Text = "Name:";
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
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 558);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(684, 26);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(49, 20);
            statusLabel.Text = "Ready";
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 584);
            Controls.Add(statusStrip);
            Controls.Add(buttonRefresh);
            Controls.Add(buttonNew);
            Controls.Add(groupBoxDetails);
            Controls.Add(dataGridView);
            MinimumSize = new Size(700, 600);
            Name = "CustomerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Customer Management";
            Load += CustomerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBoxDetails.ResumeLayout(false);
            groupBoxDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private GroupBox groupBoxDetails;
        private Label labelId;
        private Label labelName;
        private Label labelEmail;
        private Label labelPhone;
        private Label labelAddress;
        private TextBox textBoxId;
        private TextBox textBoxName;
        private TextBox textBoxEmail;
        private MaskedTextBox maskedTextBoxPhone;
        private TextBox textBoxAddress;
        private Button buttonSave;
        private Button buttonCancel;
        private Button buttonDelete;
        private Button buttonNew;
        private Button buttonRefresh;
        private ErrorProvider errorProvider;
        private BindingSource bindingSource;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
    }
}
