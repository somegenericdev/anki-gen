using System.IO.Compression;

namespace AnkiGen.Utils;

public static class Gzip
{

    public static byte[] Decompress(FileInfo fileToDecompress)
    {
        using (FileStream originalFileStream = fileToDecompress.OpenRead())
        {
            string currentFileName = fileToDecompress.FullName;

            MemoryStream decompressedStream = new MemoryStream();
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {

                decompressionStream.CopyTo(decompressedStream);
            }
            decompressedStream.Position = 0;
            return decompressedStream.ToArray();
        }
    }

    public static FileInfo DecompressToTempFile(FileInfo fileToDecompress)
    {
        var destinationPath = Path.GetTempFileName();

        using (FileStream sourceFile = fileToDecompress.OpenRead())
        using (FileStream destinationFile = File.Create(destinationPath))
        using (GZipStream gzipStream = new GZipStream(sourceFile, CompressionMode.Decompress))
        {
            gzipStream.CopyTo(destinationFile);
        }

        return new FileInfo(destinationPath);
    }

}