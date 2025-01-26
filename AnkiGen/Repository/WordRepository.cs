using AnkiGen.DTOs;
using AnkiGen.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiGen.Repository
{
    public class WordRepository
    {
        private ProjectDbContext _dbContext;
        public WordRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public WordDto? GetWord(string word, bool includeForms, PartOfSpeechEnum[] includedPartsOfSpeech)
        {
            var matchingWords = _dbContext.Words.Include(x => x.Definitions)
                .Where(x => x.Value.ToLower() == word.ToLower().Trim())
                .Where(x => includedPartsOfSpeech.Contains(x.Pos))
                .ToList();

            var wordsDefinitions = matchingWords
                .SelectMany(x => x.Definitions.Select(x => $"[{x.Word.Pos.ToString().ToLower()}] {x.Value}")).ToList();

            List<string> definitions = null;

            if (includeForms)
            {
                var matchingForms = _dbContext.Forms.Include(x => x.Word)
                    .Where(x => x.Value.ToLower() == word.ToLower().Trim()).ToList();
                var formsDefinitions = matchingForms
                    .Select(x => $"{x.Description} of {x.Word.Pos.ToString().ToLower()} '{x.Word.Value}'").ToList();

                definitions = wordsDefinitions.Concat(formsDefinitions).ToList();
            }
            else
            {
                definitions = wordsDefinitions.ToList();
            }

            if (!definitions.Any())
            {
                return null;
            }

            return new WordDto(word, definitions);
        }


        public WordDto? GetRedirect(string word, PartOfSpeechEnum[] includedPartsOfSpeech)
        {
            var matchingForms = _dbContext.Forms.Include(x => x.Word).ThenInclude(x => x.Definitions)
                                                         .Where(x => x.Value.ToLower() == word.ToLower().Trim())
                                                         .ToList();

            var matchingWords = matchingForms.Select(x => x.Word)
                                             .Where(x => includedPartsOfSpeech.Contains(x.Pos)).ToList(); //filter by part of speech

            var groupedWords = matchingWords.GroupBy(x => x.Value).ToList();


            var filteredMatchingWords = (groupedWords.Count() == 0) ? [] : groupedWords.MaxBy(x => x.Count()).ToList(); //prevent ambiguity




            var wordsDefinitions = filteredMatchingWords.SelectMany(x => x.Definitions.Select(x => $"[{x.Word.Pos.ToString().ToLower()}] {x.Value}")).ToList();

            if (!wordsDefinitions.Any())
            {
                return null;
            }

            return new WordDto(filteredMatchingWords.First().Value, wordsDefinitions);
        }
    }
}
