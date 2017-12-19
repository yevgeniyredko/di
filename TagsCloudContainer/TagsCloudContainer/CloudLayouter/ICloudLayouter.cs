using System.Drawing;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.CloudLayouter
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}