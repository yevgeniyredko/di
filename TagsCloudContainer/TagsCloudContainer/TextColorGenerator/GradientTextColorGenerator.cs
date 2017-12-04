using System;
using System.Drawing;

namespace TagsCloudContainer.TextColorGenerator
{
    public class GradientTextColorGenerator : ITextColorGenerator
    {
        private readonly Color textColor;

        private float? maxFontSize;

        public GradientTextColorGenerator(Color textColor)
        {
            this.textColor = textColor;
        }

        public Color GetTextColor(int fontSize)
        {
            if (maxFontSize == null) maxFontSize = fontSize;

            var scale = maxFontSize.Value / fontSize;
            var a = (int) Math.Ceiling(textColor.A / scale);

            return Color.FromArgb(a, textColor);
        }
    }
}