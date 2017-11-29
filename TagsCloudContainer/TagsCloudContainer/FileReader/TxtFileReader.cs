using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudContainer.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }
    }
}