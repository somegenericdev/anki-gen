using tmp;

// string[] filePaths = Directory.GetFiles("/home/debian/Documents/progetti/serbian-dictionary-utils/output-langs", "*.json",
//     SearchOption.TopDirectoryOnly).Where(x=>!x.Contains("fi.json")).Where(x=>!x.Contains("fi_pretty.json")).ToArray();
//
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


var fi = File.ReadAllBytes("/home/debian/Documents/progetti/serbian-dictionary-utils/output-langs/fi.7z");

var res = Lzma.Decompress(fi);