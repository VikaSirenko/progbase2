using System.Drawing;
using System.Drawing.Imaging;
using System;


public class FastImageEditor : IImageEditor
{
    public Bitmap ChangeBrightness(Bitmap bmp, int brightness)
    {
        Bitmap result = new Bitmap(bmp.Width, bmp.Height);
        Graphics graphics = Graphics.FromImage(result);
        ImageAttributes attributes = new ImageAttributes();
        ColorMatrix colorMatrix = new ColorMatrix(CreateBrightnessMatrix(brightness));
        attributes.SetColorMatrix(colorMatrix);
        graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
        attributes.Dispose();
        graphics.Dispose();
        return result;
    }


    private static float[][] CreateBrightnessMatrix(int brightness)
    {
        float scale = (float)(brightness / 100.0);
        float darkness = 1;

        if (scale < 0)
        {
            darkness = 1 + scale;
            scale = 0;
        }

        return new float[][]
        {
               new float[] {darkness, 0, 0, 0, 0},
               new float[] {0, darkness, 0, 0, 0},
               new float[] {0, 0, darkness, 0, 0},
               new float[] {0, 0, 0, 1, 0},
               new float[] {scale, scale, scale, 0, 1}
        };
    }


    public Bitmap Crop(Bitmap bmp, Rectangle rect)
    {
        ValidateCropRectangle(bmp, rect);
        Bitmap result = new Bitmap(rect.Width, rect.Height);
        Graphics graphics = Graphics.FromImage(result);
        graphics.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
        return result;
    }


    private void ValidateCropRectangle(Bitmap bmp, Rectangle rect)
    {
        if (rect.Left < 0 || rect.Left >= bmp.Width)
        {
            throw new Exception($"Invalid left: {rect.Left}");
        }
        else if (rect.Right >= bmp.Width)
        {
            throw new Exception($"Invalid right: {rect.Right}");

        }
        else if (rect.Top < 0 || rect.Top >= bmp.Height)
        {
            throw new Exception($"Invalid top : {rect.Top}");

        }
        else if (rect.Bottom >= bmp.Height)
        {
            throw new Exception($"Invalid bottom :{rect.Bottom}");

        }

    }


    public Bitmap ExtractBlue(Bitmap bmp)
    {

        Bitmap result = new Bitmap(bmp.Width, bmp.Height);
        Graphics graphics = Graphics.FromImage(result);
        ImageAttributes attributes = new ImageAttributes();

        float[][] colorMatrixElements = {
                new float[] {0,  0,  0,  0, 0},
                new float[] {0,  0,  0,  0, 0},
                new float[] {0,  0,  1,  0, 0},
                new float[] {0,  0,  0,  1, 0},
                new float[] {.2f, .2f, .2f, 0, 1}};

        ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
        attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
        attributes.Dispose();
        graphics.Dispose();
        return result;

    }

    public Bitmap InvertColors(Bitmap bmp)
    {

        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color color = bmp.GetPixel(x, y);
                int R = color.R;
                int G = color.G;
                int B = color.B;

                R = 255 - R;
                G = 255 - G;
                B = 255 - B;

                Color newColor = Color.FromArgb(255, R, G, B);
                bmp.SetPixel(x, y, newColor);

            }
        }

        return bmp;

    }

    public Bitmap Rotate180(Bitmap bmp)
    {
        bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
        return bmp;
    }
}
