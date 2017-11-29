using System.Collections.Generic;

namespace TagsCloudContainer.FileReader
{
    public interface IFileReader
    {
        IEnumerable<string> ReadLines(string path);
    }
}