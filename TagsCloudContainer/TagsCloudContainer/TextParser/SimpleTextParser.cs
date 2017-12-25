using System.Collections.Generic;
using System.Linq;
using NHunspell;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.TextParser
{
    public class SimpleTextParser : ITextParser
    {
        public virtual Result<IEnumerable<(string word, int count)>> GetAllWords(IEnumerable<string> text)
        {
            return Result.Of(() => new Hunspell("ru_RU.aff", "ru_RU.dic"))
                .Then(hunspell => text
                    .SelectMany(SplitToWords)
                    .Select(word => GetStem(word, hunspell)))
                .Then(GetStatistics);
        }

        private static string[] SplitToWords(string text)
        {
            const int boringWordLength = 3;

            var punctuation = text
                .Where(char.IsPunctuation)
                .Distinct()
                .ToArray();
            return text
                .Split()
                .Select(w => w.Trim(punctuation).ToLower())
                .Where(w => w.Length > boringWordLength)
                .ToArray();
        }

        private static string GetStem(string word, Hunspell hunspell)
        {
            var stems = hunspell.Stem(word);
            return stems.Any() ? stems[0] : word;
        }

        private static IEnumerable<(string word, int count)> GetStatistics(IEnumerable<string> words)
        {
            return words
                .GroupBy(w => w)
                .Select(g => (g.Key, g.Count()));
        }
    }
}