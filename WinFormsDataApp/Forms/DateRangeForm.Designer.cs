namespace WinFormsDataApp.Forms
{
    partial class DateRangeForm
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
            groupBoxDateRange = new GroupBox();
            buttonThisYear = new Button();
            buttonLast3Months = new Button();
            buttonLastMonth = new Button();
            buttonLastWeek = new Button();
            dateTimePickerEnd = new DateTimePicker();
            dateTimePickerStart = new DateTimePicker();
            labelEndDate = new Label();
            labelStartDate = new Label();
            buttonOK = new Button();
            buttonCancel = new Button();
            groupBoxDateRange.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxDateRange
            // 
            groupBoxDateRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDateRange.Controls.Add(buttonThisYear);
            groupBoxDateRange.Controls.Add(buttonLast3Months);
            groupBoxDateRange.Controls.Add(buttonLastMonth);
            groupBoxDateRange.Controls.Add(buttonLastWeek);
            groupBoxDateRange.Controls.Add(dateTimePickerEnd);
            groupBoxDateRange.Controls.Add(dateTimePickerStart);
            groupBoxDateRange.Controls.Add(labelEndDate);
            groupBoxDateRange.Controls.Add(labelStartDate);
            groupBoxDateRange.Location = new Point(12, 12);
            groupBoxDateRange.Name = "groupBoxDateRange";
            groupBoxDateRange.Size = new Size(360, 180);
            groupBoxDateRange.TabIndex = 0;
            groupBoxDateRange.TabStop = false;
            groupBoxDateRange.Text = "Select Date Range";
            // 
            // buttonThisYear
            // 
            buttonThisYear.Location = new Point(265, 140);
            buttonThisYear.Name = "buttonThisYear";
            buttonThisYear.Size = new Size(80, 30);
            buttonThisYear.TabIndex = 7;
            buttonThisYear.Text = "This Year";
            buttonThisYear.UseVisualStyleBackColor = true;
            buttonThisYear.Click += ButtonThisYear_Click;
            // 
            // buttonLast3Months
            // 
            buttonLast3Months.Location = new Point(180, 140);
            buttonLast3Months.Name = "buttonLast3Months";
            buttonLast3Months.Size = new Size(80, 30);
            buttonLast3Months.TabIndex = 6;
            buttonLast3Months.Text = "3 Months";
            buttonLast3Months.UseVisualStyleBackColor = true;
            buttonLast3Months.Click += ButtonLast3Months_Click;
            // 
            // buttonLastMonth
            // 
            buttonLastMonth.Location = new Point(95, 140);
            buttonLastMonth.Name = "buttonLastMonth";
            buttonLastMonth.Size = new Size(80, 30);
            buttonLastMonth.TabIndex = 5;
            buttonLastMonth.Text = "Last Month";
            buttonLastMonth.UseVisualStyleBackColor = true;
            buttonLastMonth.Click += ButtonLastMonth_Click;
            // 
            // buttonLastWeek
            // 
            buttonLastWeek.Location = new Point(10, 140);
            buttonLastWeek.Name = "buttonLastWeek";
            buttonLastWeek.Size = new Size(80, 30);
            buttonLastWeek.TabIndex = 4;
            buttonLastWeek.Text = "Last Week";
            buttonLastWeek.UseVisualStyleBackColor = true;
            buttonLastWeek.Click += ButtonLastWeek_Click;
            // 
            // dateTimePickerEnd
            // 
            dateTimePickerEnd.Format = DateTimePickerFormat.Short;
            dateTimePickerEnd.Location = new Point(100, 90);
            dateTimePickerEnd.Name = "dateTimePickerEnd";
            dateTimePickerEnd.Size = new Size(200, 27);
            dateTimePickerEnd.TabIndex = 3;
            dateTimePickerEnd.ValueChanged += DateTimePickerEnd_ValueChanged;
            // 
            // dateTimePickerStart
            // 
            dateTimePickerStart.Format = DateTimePickerFormat.Short;
            dateTimePickerStart.Location = new Point(100, 50);
            dateTimePickerStart.Name = "dateTimePickerStart";
            dateTimePickerStart.Size = new Size(200, 27);
            dateTimePickerStart.TabIndex = 2;
            dateTimePickerStart.ValueChanged += DateTimePickerStart_ValueChanged;
            // 
            // labelEndDate
            // 
            labelEndDate.AutoSize = true;
            labelEndDate.Location = new Point(20, 95);
            labelEndDate.Name = "labelEndDate";
            labelEndDate.Size = new Size(70, 20);
            labelEndDate.TabIndex = 1;
            labelEndDate.Text = "End Date:";
            // 
            // labelStartDate
            // 
            labelStartDate.AutoSize = true;
            labelStartDate.Location = new Point(20, 55);
            labelStartDate.Name = "labelStartDate";
            labelStartDate.Size = new Size(76, 20);
            labelStartDate.TabIndex = 0;
            labelStartDate.Text = "Start Date:";
            // 
            // buttonOK
            // 
            buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOK.Location = new Point(216, 210);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 35);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "&OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(297, 210);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 35);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // DateRangeForm
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(384, 261);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(groupBoxDateRange);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DateRangeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Date Range";
            Load += DateRangeForm_Load;
            groupBoxDateRange.ResumeLayout(false);
            groupBoxDateRange.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxDateRange;
        private Label labelStartDate;
        private Label labelEndDate;
        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private Button buttonLastWeek;
        private Button buttonLastMonth;
        private Button buttonLast3Months;
        private Button buttonThisYear;
        private Button buttonOK;
        private Button buttonCancel;
    }
}
