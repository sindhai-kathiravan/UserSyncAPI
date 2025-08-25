using System;
using System.IO;

namespace UserSyncApi
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory = AppDomain.CurrentDomain.BaseDirectory + "Logs";

        public static void Log(string message)
        {
            try
            {
                lock (_lock) // ensure thread-safety
                {
                    if (!Directory.Exists(_logDirectory))
                    {
                        Directory.CreateDirectory(_logDirectory);
                    }

                    string logFileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
                    string logFilePath = Path.Combine(_logDirectory, logFileName);

                    string logEntry = $"{DateTime.Now:HH:mm:ss} - {message}";

                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Avoid throwing exceptions from the logger itself.
                // You could write to Event Viewer or ignore.
            }
        }
    }
}