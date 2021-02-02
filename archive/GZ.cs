using CzomPack.Logging;
using System.IO;
using System.IO.Compression;

namespace CzomPack.Archive
{
    public class GZ
    {
        public static void Compress(DirectoryInfo directorySelected)
        {
            var directoryPath = directorySelected.Name;
            foreach (var fileToCompress in directorySelected.GetFiles())
            {
                var originalFileStream = fileToCompress.OpenRead();
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    var compressedFileStream = File.Create(fileToCompress.FullName + ".gz");
                    var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
                    originalFileStream.CopyTo(compressionStream);
                    var info = new FileInfo(Path.Combine(directoryPath, fileToCompress.Name + ".gz"));
                    Logger.Info($"Compressed {fileToCompress.Name} from {fileToCompress.Length} to {info.Length} bytes.");
                }
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            var originalFileStream = fileToDecompress.OpenRead();
            var currentFileName = fileToDecompress.FullName;
            var newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            var decompressedFileStream = File.Create(newFileName);
            new GZipStream(originalFileStream, CompressionMode.Decompress).CopyTo(decompressedFileStream);

            Logger.Info($"Decompressed: {fileToDecompress.Name}");
        }
    }
}