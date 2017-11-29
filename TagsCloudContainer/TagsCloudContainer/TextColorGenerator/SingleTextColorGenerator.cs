using System.Drawing;

namespace TagsCloudContainer.TextColorGenerator
{
    public class SingleTextColorGenerator : ITextColorGenerator
    {
        private readonly Color textColor;

        public SingleTextColorGenerator(Color textColor)
        {
            this.textColor = textColor;
        }

        public Color GetTextColor(int fontSize)
        {
            return textColor;
        }
    }
}