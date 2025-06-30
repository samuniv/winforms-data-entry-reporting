namespace WinFormsDataApp.Forms
{
    partial class ErrorDialog
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
            pictureBoxIcon = new PictureBox();
            labelMessage = new Label();
            textBoxDetails = new TextBox();
            buttonDetails = new Button();
            buttonCopy = new Button();
            buttonClose = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxIcon
            // 
            pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
            pictureBoxIcon.Location = new Point(12, 12);
            pictureBoxIcon.Name = "pictureBoxIcon";
            pictureBoxIcon.Size = new Size(32, 32);
            pictureBoxIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxIcon.TabIndex = 0;
            pictureBoxIcon.TabStop = false;
            // 
            // labelMessage
            // 
            labelMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelMessage.Location = new Point(54, 12);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(414, 60);
            labelMessage.TabIndex = 1;
            labelMessage.Text = "An unexpected error has occurred.";
            // 
            // textBoxDetails
            // 
            textBoxDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxDetails.BackColor = SystemColors.Window;
            textBoxDetails.Font = new Font("Consolas", 9F);
            textBoxDetails.Location = new Point(12, 85);
            textBoxDetails.Multiline = true;
            textBoxDetails.Name = "textBoxDetails";
            textBoxDetails.ReadOnly = true;
            textBoxDetails.ScrollBars = ScrollBars.Both;
            textBoxDetails.Size = new Size(456, 280);
            textBoxDetails.TabIndex = 2;
            textBoxDetails.WordWrap = false;
            // 
            // buttonDetails
            // 
            buttonDetails.Location = new Point(12, 8);
            buttonDetails.Name = "buttonDetails";
            buttonDetails.Size = new Size(100, 30);
            buttonDetails.TabIndex = 3;
            buttonDetails.Text = "Show Details";
            buttonDetails.UseVisualStyleBackColor = true;
            buttonDetails.Click += ButtonDetails_Click;
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonCopy.Location = new Point(268, 8);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(100, 30);
            buttonCopy.TabIndex = 4;
            buttonCopy.Text = "Copy Details";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += ButtonCopy_Click;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonClose.DialogResult = DialogResult.OK;
            buttonClose.Location = new Point(374, 8);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(75, 30);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += ButtonClose_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonDetails);
            panel1.Controls.Add(buttonClose);
            panel1.Controls.Add(buttonCopy);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 134);
            panel1.Name = "panel1";
            panel1.Size = new Size(484, 46);
            panel1.TabIndex = 6;
            // 
            // ErrorDialog
            // 
            AcceptButton = buttonClose;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 180);
            Controls.Add(panel1);
            Controls.Add(textBoxDetails);
            Controls.Add(labelMessage);
            Controls.Add(pictureBoxIcon);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(500, 180);
            Name = "ErrorDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Application Error";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxIcon;
        private Label labelMessage;
        private TextBox textBoxDetails;
        private Button buttonDetails;
        private Button buttonCopy;
        private Button buttonClose;
        private Panel panel1;
    }
}
