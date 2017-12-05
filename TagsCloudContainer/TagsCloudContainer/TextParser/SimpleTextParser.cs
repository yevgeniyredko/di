using System.Collections.Generic;
using System.Linq;
using NHunspell;

namespace TagsCloudContainer.TextParser
{
    public class SimpleTextParser : ITextParser
    {
        public virtual IEnumerable<(string word, int count)> GetAllWords(IEnumerable<string> text)
        {
            using (var hunspell = new Hunspell("ru_RU.aff", "ru_RU.dic"))
            {
                var result = new Dictionary<string, int>();
                foreach (var word in text.SelectMany(SplitToWords))
                {
                    if (string.IsNullOrEmpty(word)) continue;
                    
                    var stems = hunspell.Stem(word);
                    var stem = stems.Any() ? stems[0] : word;

                    if (!result.TryGetValue(stem, out var count))
                        count = 0;

                    result[stem] = count + 1;
                }
                return result
                    .Select(kvp => (kvp.Key, kvp.Value));
            }
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
    }
}