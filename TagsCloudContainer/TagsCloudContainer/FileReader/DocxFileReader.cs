using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Infrastructure;
using Xceed.Words.NET;

namespace TagsCloudContainer.FileReader
{
    public class DocxFileReader : IFileReader
    {
        public Result<IEnumerable<string>> ReadLines(string path)
        {
            return Result.Of(() => InternalReadLines(path));
        }

        private static IEnumerable<string> InternalReadLines(string path)
        {
            using (var document = DocX.Load(path))
            {
                return document.Paragraphs.Select(p => p.Text);
            }
        }
    }
}