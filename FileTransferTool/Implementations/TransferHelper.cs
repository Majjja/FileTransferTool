using FileTransferTool.Interfaces;
using System.Security.Cryptography;

namespace FileTransferTool.Implementations
{
    public class TransferHelper : ITransferHelper
    {
        public void TransferFile(string sourcePath, string destinationPath)
        {
            const int chunkSize = 1 * 1024 * 1024;
            byte[] buffer = new byte[chunkSize];
            int bytesRead;
            int blockNumber = 0;
            long position = 0;

            using (FileStream sourceStream = new(sourcePath, FileMode.Open, FileAccess.Read))
            using (FileStream destStream = new(destinationPath, FileMode.Create, FileAccess.ReadWrite))
            {
                while ((bytesRead = sourceStream.Read(buffer, 0, chunkSize)) > 0)
                {
                    byte[] chunk = new byte[bytesRead];
                    Array.Copy(buffer, chunk, bytesRead);

                    string sourceHash = ComputeMD5Hash(chunk);
                    destStream.Write(chunk, 0, bytesRead);

                    destStream.Flush();
                    destStream.Position -= bytesRead;
                    byte[] destChunk = new byte[bytesRead];
                    destStream.Read(destChunk, 0, bytesRead);

                    string destHash = ComputeMD5Hash(destChunk);
                    if (sourceHash != destHash)
                    {
                        Console.WriteLine($"Hash mismatch at position {position}. Retrying...");
                        destStream.Position -= bytesRead;
                        destStream.Write(chunk, 0, bytesRead);
                    }
                    blockNumber++;
                    Console.WriteLine($"{blockNumber}. Position = {position}, Hash = {sourceHash}");
                    position += bytesRead;
                }
            }

            string sourceFileHash = ComputeSHA256Hash(sourcePath);
            string destFileHash = ComputeSHA256Hash(destinationPath);

            Console.WriteLine();
            Console.WriteLine($"Source File SHA256: {sourceFileHash}");
            Console.WriteLine($"Destination File SHA256: {destFileHash}");
        }

        private string ComputeMD5Hash(byte[] data)
        {
            byte[] hash = MD5.HashData(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private string ComputeSHA256Hash(string filePath)
        {
            byte[] hash = SHA256.HashData(File.OpenRead(filePath));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
