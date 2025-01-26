using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiGen
{
    public class CommandLineOptions
    {
        [Option("word-list-file", Required = true, HelpText = "The input file containing the word list, which will be compiled into an .apkg file.")]
        public string WordListFile { get; set; }

        [Option("parts-of-speech", Required = true, HelpText = "The parts of speech to include.")]
        public IEnumerable<string> PartsOfSpeech { get; set; }

        [Option("deck-name", Required = true, HelpText = "The generated deck's name.")]
        public string DeckName { get; set; }
        [Option("redirect", Required = true, HelpText = "Try to resolve derivated forms, like conjunctions and declensions, to their base word.")]
        public bool Redirect { get; set; }
        [Option("debug", Required = false, HelpText = "Process only 100 items.")]
        public bool Debug { get; set; }
        [Option("language", Required = true, HelpText = "The deck's language.")]
        public string Language { get; set; }


    }
}
