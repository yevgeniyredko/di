using System.Collections.Generic;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.FileReader
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> ReadLines(string path);
    }
}