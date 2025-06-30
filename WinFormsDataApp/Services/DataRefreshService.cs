using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsDataApp.Services
{
    public class DataRefreshService : IDisposable
    {
        private readonly TimeSpan _refreshInterval;
        private System.Threading.Timer? _timer;
        private bool _disposed = false;

        public event EventHandler? OnDataChanged;

        public DataRefreshService(TimeSpan refreshInterval)
        {
            _refreshInterval = refreshInterval;
        }

        public void Start()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DataRefreshService));

            _timer = new System.Threading.Timer(TimerCallback, null, _refreshInterval, _refreshInterval);
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private void TimerCallback(object? state)
        {
            try
            {
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Log the exception if logging is available
                // For now, we'll silently handle it to prevent timer crashes
                Console.WriteLine($"Error in DataRefreshService timer callback: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}
