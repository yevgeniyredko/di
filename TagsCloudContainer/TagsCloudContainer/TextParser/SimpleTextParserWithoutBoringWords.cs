using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.TextParser
{
    public class SimpleTextParserWithoutBoringWords : SimpleTextParser
    {
        private readonly string[] boringWords;

        public SimpleTextParserWithoutBoringWords(string[] boringWords)
        {
            this.boringWords = boringWords;
        }

        public override Result<IEnumerable<(string word, int count)>> GetAllWords(IEnumerable<string> text)
        {
            return base.GetAllWords(text)
                .Then(tags => tags.Where(tag => !boringWords.Contains(tag.word, StringComparer.OrdinalIgnoreCase)));
        }
    }
}