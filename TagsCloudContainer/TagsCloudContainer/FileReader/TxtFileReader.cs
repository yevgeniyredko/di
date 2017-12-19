using System.Collections.Generic;
using System.IO;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<IEnumerable<string>> ReadLines(string path)
        {
            return Result.Of(() => File.ReadLines(path));
        }
    }
}