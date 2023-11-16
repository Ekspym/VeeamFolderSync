using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Logging;



namespace VeeamFolderSync
{
    public  class Logger 
    {
        public readonly string filePath;

        public Logger(string filePath)
        {
            this.filePath = filePath;
        }

        public void Log(LogLevel level, string message)
        {
            string levelString = level == LogLevel.Info ? "INFO" : "ERROR";
            string logEntry = $"{DateTime.Now} [{levelString}] {message}";

            Console.WriteLine(logEntry); // Optional: Output to console

            // Append the log entry to the file
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
        }
    }
}
