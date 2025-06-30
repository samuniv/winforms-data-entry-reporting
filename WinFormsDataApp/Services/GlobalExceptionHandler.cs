using System;
using System.Threading;
using System.Windows.Forms;
using WinFormsDataApp.Forms;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services
{
    public static class GlobalExceptionHandler
    {
        private static bool _isHandlingException = false;

        public static void Initialize()
        {
            // Set up global exception handlers
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += GlobalThreadExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            LoggingService.LogInformation("Global exception handlers initialized");
        }

        private static void GlobalThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            if (_isHandlingException) return; // Prevent recursive exception handling

            try
            {
                _isHandlingException = true;
                LoggingService.LogError(e.Exception, "An unhandled UI thread exception occurred");
                ShowErrorDialog(e.Exception);
            }
            catch (Exception ex)
            {
                // Last resort - show a simple message box if our error handling fails
                MessageBox.Show(
                    $"A critical error occurred in the error handler:\n\n{ex.Message}",
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isHandlingException = false;
            }
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (_isHandlingException) return; // Prevent recursive exception handling

            try
            {
                _isHandlingException = true;
                var ex = (Exception)e.ExceptionObject;
                LoggingService.LogFatal(ex, $"An unhandled non-UI thread exception occurred. Application is terminating: {e.IsTerminating}");

                // Only show dialog if the application is not terminating
                if (!e.IsTerminating)
                {
                    ShowErrorDialog(ex);
                }
                else
                {
                    // For terminating exceptions, show a simple message box
                    MessageBox.Show(
                        "A fatal error has occurred and the application must close.\n\nPlease check the log files for more details.",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Last resort - write to console if available
                Console.WriteLine($"Critical error in exception handler: {ex.Message}");
            }
            finally
            {
                _isHandlingException = false;
            }
        }

        private static void ShowErrorDialog(Exception ex)
        {
            try
            {            // Ensure dialog is shown on the UI thread if called from a background thread
                if (Application.OpenForms.Count > 0)
                {
                    var mainForm = Application.OpenForms[0];
                    if (mainForm != null && mainForm.InvokeRequired)
                    {
                        mainForm.Invoke(new Action(() => ShowErrorDialogInternal(ex)));
                    }
                    else
                    {
                        ShowErrorDialogInternal(ex);
                    }
                }
                else
                {
                    // No forms available, show a simple message box
                    MessageBox.Show(
                        $"An error occurred: {ex.Message}\n\nPlease check the log files for more details.",
                        "Application Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception dialogEx)
            {
                LoggingService.LogError(dialogEx, "Failed to show error dialog");

                // Fallback to simple message box
                MessageBox.Show(
                    $"Multiple errors occurred. Original error: {ex.Message}\n\nDialog error: {dialogEx.Message}",
                    "Multiple Errors",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void ShowErrorDialogInternal(Exception ex)
        {
            using (var errorDialog = new ErrorDialog(ex))
            {
                // Find the active form to use as parent
                Form? parentForm = null;
                foreach (Form form in Application.OpenForms)
                {
                    if (form.Visible && form.WindowState != FormWindowState.Minimized)
                    {
                        parentForm = form;
                        break;
                    }
                }

                if (parentForm != null)
                {
                    errorDialog.ShowDialog(parentForm);
                }
                else
                {
                    errorDialog.ShowDialog();
                }
            }
        }

        public static void TestExceptionHandling()
        {
            LoggingService.LogInformation("Testing exception handling...");
            throw new InvalidOperationException("This is a test exception to verify the global exception handler is working correctly.");
        }
    }
}
