using System.IO.Compression;

namespace AnkiGen.Utils;

public static class Gzip
{
    public static FileInfo Compress(FileInfo source, FileInfo target)
    {
        using (var sourceStream = source.OpenRead())
        using (var outputStream = File.Create(target.ToString()))
        using (var gzipStream = new GZipStream(outputStream, CompressionLevel.SmallestSize))
        {
            sourceStream.CopyTo(gzipStream);
        }

        return target;
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