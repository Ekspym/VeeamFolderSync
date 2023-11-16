using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VeeamFolderSync
{
    internal class FolderSynchronizer : IFolderSynchronizer
    {
        private string sourceFolderPath;
        private string replicaFolderPath;
        private int syncIntervalInSeconds;
        private Timer syncTimer;
        private Logger logger;

        public FolderSynchronizer(string sourceFolderPath, string replicaFolderPath, int syncIntervalInSeconds, Logger logger)
        {
            this.sourceFolderPath = sourceFolderPath;
            this.replicaFolderPath = replicaFolderPath;
            this.syncIntervalInSeconds = syncIntervalInSeconds;
            this.logger = logger;
        }

        public void SynchronizeFolders()
        {
            Console.WriteLine($"Source Folder: {sourceFolderPath}");
            Console.WriteLine($"Replica Folder: {replicaFolderPath}");
            Console.WriteLine($"Sync Interval: {syncIntervalInSeconds} seconds");
            Console.WriteLine($"logger.Log File: {logger.filePath}");

            // Set up initial synchronization
            SyncFolders();

            // Set up periodic synchronization using Timer
            syncTimer = new Timer(SyncFolders, null, TimeSpan.Zero, TimeSpan.FromSeconds(syncIntervalInSeconds));

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private void SyncFolders(object state = null)
        {
            try
            {
                logger.Log(LogLevel.Info, "Sync started.");

                // Ensure the replica folder exists
                if (!Directory.Exists(replicaFolderPath))
                {
                    Directory.CreateDirectory(replicaFolderPath);
                }

                // Synchronize files
                SyncFiles(sourceFolderPath, replicaFolderPath);

                // Delete excess files in replica
                DeleteExcessFiles(sourceFolderPath, replicaFolderPath);

                logger.Log(LogLevel.Info,"Sync completed successfully.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,$"Error during synchronization: {ex.Message}");
            }
        }

        private void SyncFiles(string sourcePath, string replicaPath)
        {
            foreach (var sourceFile in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFile);
                string replicaFile = Path.Combine(replicaPath, fileName);

                if (File.Exists(replicaFile) && AreFilesIdentical(sourceFile, replicaFile))
                {
                    logger.Log(LogLevel.Info,$"Skipped: {sourceFile} (already exists with identical content in {replicaFile})");
                    continue;
                }

                File.Copy(sourceFile, replicaFile, true);
                logger.Log(LogLevel.Info, $"Copied: {sourceFile} to {replicaFile}");
            }
        }

        private void DeleteExcessFiles(string sourcePath, string replicaPath)
        {
            foreach (var replicaFile in Directory.GetFiles(replicaPath))
            {
                string fileName = Path.GetFileName(replicaFile);
                string sourceFile = Path.Combine(sourcePath, fileName);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(replicaFile);
                    logger.Log(LogLevel.Info, $"Deleted: {replicaFile}");
                }
            }
        }

        private bool AreFilesIdentical(string filePath1, string filePath2)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream1 = File.OpenRead(filePath1))
                using (var stream2 = File.OpenRead(filePath2))
                {
                    byte[] hash1 = md5.ComputeHash(stream1);
                    byte[] hash2 = md5.ComputeHash(stream2);

                    return BitConverter.ToString(hash1) == BitConverter.ToString(hash2);
                }
            }
        }
    }
}
