using Microsoft.Data.Sqlite;
using System.IO.Compression;
using WinFormsDataApp.Services;

namespace WinFormsDataApp.Services.ImportExport
{
    public class DatabaseBackupService
    {
        private readonly string _databasePath;

        public DatabaseBackupService(string databasePath = "data.db")
        {
            _databasePath = databasePath;
        }

        public async Task<BackupResult> CreateBackupAsync(string backupDirectory)
        {
            var result = new BackupResult();

            try
            {
                // Ensure backup directory exists
                Directory.CreateDirectory(backupDirectory);

                // Generate timestamped backup filename
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var backupFileName = $"data_backup_{timestamp}.db";
                var backupPath = Path.Combine(backupDirectory, backupFileName);

                LoggingService.LogInformation($"Starting database backup to: {backupPath}");

                // Check if source database exists
                if (!File.Exists(_databasePath))
                {
                    result.Success = false;
                    result.ErrorMessage = "Source database file not found";
                    return result;
                }

                // Use SQLite backup API for safe backup
                using (var sourceConnection = new SqliteConnection($"Data Source={_databasePath}"))
                using (var backupConnection = new SqliteConnection($"Data Source={backupPath}"))
                {
                    await sourceConnection.OpenAsync();
                    await backupConnection.OpenAsync();

                    sourceConnection.BackupDatabase(backupConnection);
                }

                // Verify backup file was created and has content
                var backupInfo = new FileInfo(backupPath);
                if (!backupInfo.Exists || backupInfo.Length == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Backup file was not created or is empty";
                    return result;
                }

                result.Success = true;
                result.BackupFilePath = backupPath;
                result.BackupSizeBytes = backupInfo.Length;

                LoggingService.LogInformation($"Database backup completed successfully. Size: {backupInfo.Length} bytes");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                LoggingService.LogError(ex, "Database backup failed");
            }

            return result;
        }

        public async Task<RestoreResult> RestoreDatabaseAsync(string backupFilePath)
        {
            var result = new RestoreResult();

            try
            {
                LoggingService.LogInformation($"Starting database restore from: {backupFilePath}");

                // Verify backup file exists
                if (!File.Exists(backupFilePath))
                {
                    result.Success = false;
                    result.ErrorMessage = "Backup file not found";
                    return result;
                }

                // Create backup of current database before restore
                var currentBackupPath = $"{_databasePath}.restore_backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                if (File.Exists(_databasePath))
                {
                    File.Copy(_databasePath, currentBackupPath, true);
                    result.CurrentDatabaseBackupPath = currentBackupPath;
                    LoggingService.LogInformation($"Current database backed up to: {currentBackupPath}");
                }

                // Close any existing connections by copying the file
                // This is safer than trying to manage active connections
                File.Copy(backupFilePath, _databasePath, true);

                // Verify the restored database
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
                var tables = new List<string>();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tables.Add(reader.GetString(0));
                }

                if (!tables.Any())
                {
                    result.Success = false;
                    result.ErrorMessage = "Restored database appears to be empty or corrupted";

                    // Restore the original database
                    if (!string.IsNullOrEmpty(result.CurrentDatabaseBackupPath))
                    {
                        File.Copy(result.CurrentDatabaseBackupPath, _databasePath, true);
                    }

                    return result;
                }

                result.Success = true;
                result.RequiresRestart = true; // EF Core context needs to be reinitialized

                LoggingService.LogInformation($"Database restore completed successfully. Found {tables.Count} tables.");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                LoggingService.LogError(ex, "Database restore failed");

                // Attempt to restore original database
                if (!string.IsNullOrEmpty(result.CurrentDatabaseBackupPath) && File.Exists(result.CurrentDatabaseBackupPath))
                {
                    try
                    {
                        File.Copy(result.CurrentDatabaseBackupPath, _databasePath, true);
                        LoggingService.LogInformation("Original database restored after failed restore operation");
                    }
                    catch (Exception restoreEx)
                    {
                        LoggingService.LogError(restoreEx, "Failed to restore original database");
                    }
                }
            }

            return result;
        }

        public async Task<ExportResult> ExportToCsvAsync(string exportDirectory)
        {
            var result = new ExportResult();

            try
            {
                Directory.CreateDirectory(exportDirectory);

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                LoggingService.LogInformation($"Starting CSV export to: {exportDirectory}");

                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                await connection.OpenAsync();

                // Export Customers
                var customersPath = Path.Combine(exportDirectory, $"customers_{timestamp}.csv");
                await ExportTableToCsv(connection, "Customers", customersPath);
                result.ExportedFiles.Add(customersPath);

                // Export Orders with Customer names
                var ordersPath = Path.Combine(exportDirectory, $"orders_{timestamp}.csv");
                await ExportOrdersWithCustomerNames(connection, ordersPath);
                result.ExportedFiles.Add(ordersPath);

                result.Success = true;
                LoggingService.LogInformation($"CSV export completed. Files: {string.Join(", ", result.ExportedFiles)}");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                LoggingService.LogError(ex, "CSV export failed");
            }

            return result;
        }

        private async Task ExportTableToCsv(SqliteConnection connection, string tableName, string filePath)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {tableName}";

            using var reader = await command.ExecuteReaderAsync();
            using var writer = new StreamWriter(filePath);

            // Write headers
            var headers = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                headers.Add(reader.GetName(i));
            }
            await writer.WriteLineAsync(string.Join(",", headers.Select(h => $"\"{h}\"")));

            // Write data
            while (await reader.ReadAsync())
            {
                var values = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString();
                    values.Add($"\"{value?.Replace("\"", "\"\"")}\""); // Escape quotes
                }
                await writer.WriteLineAsync(string.Join(",", values));
            }
        }

        private async Task ExportOrdersWithCustomerNames(SqliteConnection connection, string filePath)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT o.Id, c.Name as CustomerName, o.CustomerId, o.Quantity, o.OrderDate, o.IsDeleted
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerId = c.Id
                ORDER BY o.Id";

            using var reader = await command.ExecuteReaderAsync();
            using var writer = new StreamWriter(filePath);

            // Write headers
            await writer.WriteLineAsync("\"Id\",\"CustomerName\",\"CustomerId\",\"Quantity\",\"OrderDate\",\"IsDeleted\"");

            // Write data
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                var customerName = reader.GetString(reader.GetOrdinal("CustomerName")).Replace("\"", "\"\"");
                var customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                var quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                var orderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")).ToString("yyyy-MM-dd");
                var isDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));

                await writer.WriteLineAsync($"\"{id}\",\"{customerName}\",\"{customerId}\",\"{quantity}\",\"{orderDate}\",\"{isDeleted}\"");
            }
        }

        public List<string> GetAvailableBackups(string backupDirectory)
        {
            try
            {
                if (!Directory.Exists(backupDirectory))
                    return new List<string>();

                return Directory.GetFiles(backupDirectory, "data_backup_*.db")
                               .OrderByDescending(f => new FileInfo(f).CreationTime)
                               .ToList();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, "Error getting available backups");
                return new List<string>();
            }
        }
    }

    public class BackupResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string BackupFilePath { get; set; } = string.Empty;
        public long BackupSizeBytes { get; set; }
    }

    public class RestoreResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string CurrentDatabaseBackupPath { get; set; } = string.Empty;
        public bool RequiresRestart { get; set; }
    }

    public class ExportResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> ExportedFiles { get; set; } = new List<string>();
    }
}
