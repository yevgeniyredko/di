using System.Collections.Generic;
using System.Linq;
using Xceed.Words.NET;

namespace TagsCloudContainer.FileReader
{
    public class DocxFileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            using (var document = DocX.Load(path))
            {
                return document.Paragraphs.Select(p => p.Text);
            }
        }
    }
}