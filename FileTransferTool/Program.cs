// See https://aka.ms/new-console-template for more information
using FileTransferTool.Implementations;

var create = new CreateHelper();
var sourceFilePath = create.CreatePath(create.SourceFolderName, create.FileName);
var destinationFilePath = create.CreatePath(create.DestinationFolderName, create.FileName);
create.CreateFile(sourceFilePath);

var transfer = new TransferHelper();
transfer.TransferFile(sourceFilePath, destinationFilePath);
