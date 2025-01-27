using System.IO.Compression;

namespace AnkiGen.Utils;

public static class Gzip
{

    public static byte[] Decompress(FileInfo fileToDecompress)
    {
        using (FileStream originalFileStream = fileToDecompress.OpenRead())
        {
            string currentFileName = fileToDecompress.FullName;
            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            using (FileStream decompressedFileStream = File.Create(newFileName))
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
                MemoryStream ms = new MemoryStream();
                decompressedFileStream.Position = 0;
                decompressedFileStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
    
}