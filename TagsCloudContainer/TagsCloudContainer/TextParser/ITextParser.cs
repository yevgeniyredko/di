using System.Collections.Generic;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.TextParser
{
    public interface ITextParser
    {
        Result<IEnumerable<(string word, int count)>> GetAllWords(IEnumerable<string> text);
    }
}