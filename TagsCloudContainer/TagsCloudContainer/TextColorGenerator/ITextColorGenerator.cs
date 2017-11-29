using System.Drawing;

namespace TagsCloudContainer.TextColorGenerator
{
    public interface ITextColorGenerator
    {
        Color GetTextColor(int fontSize);
    }
}