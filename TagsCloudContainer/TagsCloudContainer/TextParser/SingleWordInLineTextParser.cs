using System.Collections.Generic;
using System.Linq;
using NHunspell;

namespace TagsCloudContainer.TextParser
{
    public class SingleWordInLineTextParser : ITextParser
    {
        public IEnumerable<(string word, int count)> GetAllWords(IEnumerable<string> text)
        {
            using (var hunspell = new Hunspell("russian-aot.aff", "russian-aot.dic"))
            {
                var result = new Dictionary<string, int>();
                foreach (var word in text.Select(w => w.ToLower()))
                {
                    if(string.IsNullOrEmpty(word)) continue;
                    
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
    }
}