
using Microsoft.Extensions.Configuration;
using VeeamFolderSync;


    if (args.Length != 4)
    {
        Console.WriteLine("Usage: VeeamFolderSync <sourceFolderPath> <replicaFolderPath> <syncIntervalInSeconds> <logFilePath>");
        return;
    }

    string sourceFolderPath = args[0];
    string replicaFolderPath = args[1];

    if (!int.TryParse(args[2], out int syncIntervalInSeconds))
    {
        Console.WriteLine("Invalid sync interval. Please provide a valid integer.");
        return;
    }

    string logFilePath = args[3];

    Console.WriteLine($"Source Folder: {sourceFolderPath}");
    Console.WriteLine($"Replica Folder: {replicaFolderPath}");
    Console.WriteLine($"Sync Interval: {syncIntervalInSeconds} seconds");
    Console.WriteLine($"Log File: {logFilePath}");

    // Initialize variables
    Logger logger = new Logger(logFilePath);
    FolderSynchronizer folderSynchronizer = new FolderSynchronizer(sourceFolderPath, replicaFolderPath, syncIntervalInSeconds, logger);

    try
    {
        // Perform initial synchronization
        folderSynchronizer.SynchronizeFolders();

        // Keep the program running
        Console.WriteLine("Press 'Enter' to exit.");
        Console.ReadLine();
    }
    catch (Exception ex) 
    {
        Console.WriteLine(ex.Message);
    }
  




