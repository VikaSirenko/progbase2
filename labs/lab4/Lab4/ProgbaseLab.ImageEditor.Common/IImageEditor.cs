using System.Drawing;
public interface IImageEditor
{
    Bitmap Crop(Bitmap bmp, Rectangle rect);
    Bitmap Rotate180(Bitmap bmp);
    Bitmap ExtractBlue(Bitmap bmp);
    Bitmap InvertColors(Bitmap bmp);
    Bitmap ChangeBrightness(Bitmap bmp, int brightness);
}
