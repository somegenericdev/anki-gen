using AnkiGen;
using AnkiGen.DTOs;
using AnkiGen.Models;
using AnkiGen.Repository;
using AnkiGen.Utils;
using AnkiNet;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

//.\AnkiGen.exe--word - list - file.\frequency - list.txt--parts - of - speech noun adj adv verb --deck-name foo --redirect --debug

#if (!DEBUG)

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed(async o =>
    {
#endif

var stopWatch = new Stopwatch();
stopWatch.Start();

#if (!DEBUG)

        var includedPartsOfSpeech = o.PartsOfSpeech.Select(x => x.ParseEnum<PartOfSpeechEnum>()).ToArray();
        Console.WriteLine("Parts of speech:");
        foreach (var ps in includedPartsOfSpeech)
        {
            Console.WriteLine(ps);
        }
        var redirect = o.Redirect;
        Console.WriteLine($"Redirect: {redirect}");
        var deckName = o.DeckName;
        var inputFilePath = o.WordListFile;
        var debug = o.Debug;
        var language = o.Language.ParseEnum<LanguageEnum>();
        var maxDefinitions = o.MaxDefinitions;
        var maxCards = o.MaxCards;
        Console.WriteLine($"Language: {language.ToString()}");
#endif

#if (DEBUG)

var includedPartsOfSpeech = new[] { PartOfSpeechEnum.Noun, PartOfSpeechEnum.Adj, PartOfSpeechEnum.Adv, PartOfSpeechEnum.Verb };
var redirect = true;
var deckName = "Serbian-1000";
var inputFilePath = "frequency-list.txt";
var debug = false;
var language = LanguageEnum.Serbian;
int? maxDefinitions = 2;
int? maxCards = 1000;

#endif
Console.WriteLine("Loading data into memory...");

var services = new ServiceCollection();
services.AddDbContext<ProjectDbContext>(options => options.UseInMemoryDatabase(databaseName: "AnkiGenDb"));

var serviceProvider = services.BuildServiceProvider();
var dbContext = serviceProvider.GetService<ProjectDbContext>();


LoadDb(dbContext, language);

Console.WriteLine("Processing word list...");

var wordList = File.ReadAllLines(inputFilePath);

if (debug)
{
    wordList = wordList.Take(100 > wordList.Count() ? wordList.Count() : 100).ToArray();
}

var wordRepository = new WordRepository(dbContext);

var wordObjs = wordList.Select(x => redirect ? (wordRepository.GetWord(x, false, includedPartsOfSpeech) ?? wordRepository.GetRedirect(x, includedPartsOfSpeech)) : wordRepository.GetWord(x, false, includedPartsOfSpeech))
             .Select((x, i) =>
             {
                 Console.WriteLine($"{i + 1}/{wordList.Count()}");
                 return x;
             })
             .ToList();

Console.WriteLine("Generating Anki collection...");

var cardDtos = wordObjs.Where(x => x != null)
                             .Where(x => x.Definitions.Count() > 0)
                             .Select(x =>
                             {
                                 x.Definitions = x.Definitions.GroupBy(x => x).Select(x => x.First()).ToList(); //get rid of duplicates within a card
                                 return x;
                             })
                             .GroupBy(x => x.Name).Select(x => x.First()) //get rid of duplicates within the deck
                             .Select(x =>
                             {
                                 if (maxDefinitions != null)
                                 {
                                     x.Definitions = x.Definitions.Take(maxDefinitions.Value).ToList();
                                 }
                                 return x;
                             })
                             .Select(x => new CardDto(x.Name, x.Definitions.Select((y, i) => $"{i + 1}. {y}").Implode("\n").Replace("\r\n", "<br/>").Replace("\n", "<br/>")))
                             .ToList();

if(maxCards != null)
{
    if (maxCards > cardDtos.Count())
    {
        Console.WriteLine("Error: the --max-cards parameter cannot be greater than card count.");
    }
    cardDtos = cardDtos.Take(maxCards.Value).ToList();
}

File.WriteAllText("debug.json", JsonConvert.SerializeObject(cardDtos));

var ankiCollection = cardDtos.ToAnkiCollection(deckName);


await AnkiFileWriter.WriteToFileAsync($"{deckName}.apkg", ankiCollection);

Console.WriteLine("Done.");
stopWatch.Stop();
Console.WriteLine($"It took {stopWatch.Elapsed.TotalMinutes} minutes for a full run.");
#if (!DEBUG)
    
});
#endif

void LoadDb(ProjectDbContext dbContext, LanguageEnum language)
{
    var dbPath = Gzip.DecompressToTempFile(new FileInfo($"Databases/{language.GetDescription()}.gz"));
    using (var dbStream = dbPath.OpenRead())
    {
        using (StreamReader r = new StreamReader(dbStream))
        {
            using (JsonReader reader = new JsonTextReader(r))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Word> words = serializer.Deserialize<List<Word>>(reader);
                dbContext.AddRange(words);
                dbContext.SaveChanges();
                dbContext.ChangeTracker.Clear();
            }
        }
    }
}



