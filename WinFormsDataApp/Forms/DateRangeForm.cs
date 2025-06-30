using System;
using System.Windows.Forms;

namespace WinFormsDataApp.Forms
{
    public partial class DateRangeForm : Form
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateRangeForm()
        {
            InitializeComponent();

            // Set default date range to last 30 days
            EndDate = DateTime.Today;
            StartDate = DateTime.Today.AddDays(-30);

            dateTimePickerStart.Value = StartDate;
            dateTimePickerEnd.Value = EndDate;
        }

        private void DateRangeForm_Load(object sender, EventArgs e)
        {
            // Focus on start date picker
            dateTimePickerStart.Focus();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (ValidateDateRange())
            {
                StartDate = dateTimePickerStart.Value.Date;
                EndDate = dateTimePickerEnd.Value.Date;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateDateRange()
        {
            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Invalid Date Range",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dateTimePickerEnd.Value > DateTime.Today)
            {
                MessageBox.Show("End date cannot be in the future.", "Invalid Date Range",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var daysDifference = (dateTimePickerEnd.Value - dateTimePickerStart.Value).Days;
            if (daysDifference > 365)
            {
                MessageBox.Show("Date range cannot exceed 365 days.", "Invalid Date Range",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void DateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            // Auto-adjust end date if start date is later
            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                dateTimePickerEnd.Value = dateTimePickerStart.Value;
            }
        }

        private void DateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            // Auto-adjust start date if end date is earlier
            if (dateTimePickerEnd.Value < dateTimePickerStart.Value)
            {
                dateTimePickerStart.Value = dateTimePickerEnd.Value;
            }
        }

        private void ButtonLastWeek_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dateTimePickerEnd.Value = today;
            dateTimePickerStart.Value = today.AddDays(-7);
        }

        private void ButtonLastMonth_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dateTimePickerEnd.Value = today;
            dateTimePickerStart.Value = today.AddDays(-30);
        }

        private void ButtonLast3Months_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dateTimePickerEnd.Value = today;
            dateTimePickerStart.Value = today.AddDays(-90);
        }

        private void ButtonThisYear_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dateTimePickerEnd.Value = today;
            dateTimePickerStart.Value = new DateTime(today.Year, 1, 1);
        }
    }
}
