using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using tmp;

string[] filePaths = Directory.GetFiles("/home/debian/Documents/progetti/serbian-dictionary-utils/output-langs", "fi.json",
     SearchOption.TopDirectoryOnly).Where(x=>!x.Contains("fi_pretty.json")).ToArray();

// string[] filePaths7z = Directory.GetFiles("/home/debian/Documents/progetti/serbian-dictionary-utils/output-langs", "*.7z",
//     SearchOption.TopDirectoryOnly).Where(x=>!x.Contains("fi.7z")).ToArray();
//
//
// var filePathsRilevanti = filePaths.Select(x => x.Replace(".json", ""))
//     .Where(x => !filePaths7z.Select(x => x.Replace(".7z", "")).Contains(x)).Select(x=>x + ".json").ToList();
//
//
// foreach (var file in filePathsRilevanti)
// {
//     var compressed = Lzma.Compress(File.ReadAllBytes(file));
//     File.WriteAllBytes(file.Replace(".json", ".7z"), compressed);
// }


//
// filePaths.ToList().ForEach(path =>
// {
//      
//      
//      using ( var file = MemoryMappedFile.CreateFromFile(path) )
//      {
//           using (FileStream fs = new FileStream(path.Replace(".json", ".gz"), FileMode.CreateNew))
//           using (GZipStream zipStream = new GZipStream(fs, CompressionLevel.SmallestSize, false))
//           {
//                MemoryStream ms = new MemoryStream();
//                file.CreateViewStream().CopyToAsync(ms);
//
//                byte[] bytes = ms.ToArray();
//                
//                zipStream.Write(bytes, 0, bytes.Length);
//           }
//      }
// });
//

















var bytearr =
     Gzip.Decompress(new FileInfo("/home/debian/Documents/progetti/serbian-dictionary-utils/output-langs/gz/fi.gz"));
Console.WriteLine("!");
