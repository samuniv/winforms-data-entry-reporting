using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsDataApp.Forms
{
    public partial class ErrorDialog : Form
    {
        private readonly Exception _exception;

        public ErrorDialog(Exception exception)
        {
            _exception = exception;
            InitializeComponent();
            SetupErrorDialog();
        }

        private void SetupErrorDialog()
        {
            // User-friendly message
            labelMessage.Text = "An unexpected error has occurred. The application will continue to run, but some features may not work properly.";

            // Technical details for the expandable text box
            textBoxDetails.Text = $"Error: {_exception.Message}{Environment.NewLine}{Environment.NewLine}" +
                                 $"Type: {_exception.GetType().FullName}{Environment.NewLine}{Environment.NewLine}" +
                                 $"Stack Trace:{Environment.NewLine}{_exception.StackTrace}";

            // Initially hide details
            textBoxDetails.Visible = false;
            AdjustFormSize(false);
        }

        private void ButtonDetails_Click(object sender, EventArgs e)
        {
            // Toggle visibility and resize form
            bool showDetails = !textBoxDetails.Visible;
            textBoxDetails.Visible = showDetails;
            buttonDetails.Text = showDetails ? "Hide Details" : "Show Details";
            AdjustFormSize(showDetails);
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBoxDetails.Text);
                MessageBox.Show("Error details copied to clipboard.", "Copied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard: {ex.Message}", "Copy Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AdjustFormSize(bool showDetails)
        {
            if (showDetails)
            {
                Height = 450;
                MinimumSize = new Size(500, 450);
            }
            else
            {
                Height = 180;
                MinimumSize = new Size(500, 180);
                MaximumSize = new Size(800, 180);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Ensure the form is properly centered and focused
            CenterToScreen();
            buttonClose.Focus();
        }
    }
}
