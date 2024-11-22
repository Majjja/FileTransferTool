using FileTransferTool.Interfaces;

namespace FileTransferTool.Implementations
{
    public class CreateHelper : ICreateHelper
    {
        public string SourceFolderName { get; private set; } = "Source";
        public string DestinationFolderName { get; private set; } = "Destination";
        public string FileName { get; private set; } = "My_large_file.bin";

        public void CreateFile(string filePath)
        {
            long currentFileSize = 0;
            long targetFileSize = 1L * 1024 * 1024 * 1024;

            byte[] buffer = new byte[1 * 1024 * 1024];
            Random random = new();

            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            while (currentFileSize < targetFileSize)
            {
                random.NextBytes(buffer);

                fs.Write(buffer, 0, buffer.Length);
                currentFileSize += buffer.Length;
            }
        }

        public string CreatePath(string folderName, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);

            return path;
        }
    }
}
