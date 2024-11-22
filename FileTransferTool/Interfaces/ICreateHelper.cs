namespace FileTransferTool.Interfaces
{
    public interface ICreateHelper
    {
        void CreateFile(string filePath);
        string CreatePath(string folderName, string fileName);
    }
}
