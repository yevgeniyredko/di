using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.TextParser
{
    public class SimpleTextParserWithCustomBoringWords : SimpleTextParser
    {
        private readonly string[] boringWords;

        public SimpleTextParserWithCustomBoringWords(string[] boringWords)
        {
            this.boringWords = boringWords;
        }

        public override IEnumerable<(string word, int count)> GetAllWords(IEnumerable<string> text)
        {
            return base.GetAllWords(text)
                .Where(p => !boringWords.Contains(p.word, StringComparer.OrdinalIgnoreCase));
        }
    }
}