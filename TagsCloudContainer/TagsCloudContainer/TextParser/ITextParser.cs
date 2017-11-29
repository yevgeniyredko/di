using System.Collections.Generic;

namespace TagsCloudContainer.TextParser
{
    public interface ITextParser
    {
        IEnumerable<(string word, int count)> GetAllWords(IEnumerable<string> text);
    }
}