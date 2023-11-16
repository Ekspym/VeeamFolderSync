# VeeamFolderSync
VeeamFolderSync is a command-line utility for one-way synchronization of folders, maintaining an identical copy of the source folder in the replica folder.

VeeamFolderSync.exe <sourceFolderPath> <replicaFolderPath> <syncIntervalInSeconds> <logFilePath>

<sourceFolderPath>: Path to the source folder.
<replicaFolderPath>: Path to the replica folder.
<syncIntervalInSeconds>: Synchronization interval in seconds.
<logFilePath>: Path to the log file.

Logging

The utility logs synchronization activities to the specified log file, including a timestamp, log level (INFO or ERROR), and a message.
