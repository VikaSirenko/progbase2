using System;
using System.Drawing;

public class PixelImageEditor : IImageEditor
{
    public Bitmap Crop(Bitmap bmp, Rectangle rect)
    {
        ValidateCropRectangle(bmp, rect);
        Bitmap croppedImage = new Bitmap(rect.Width, rect.Height);

        for (int y = 0; y < croppedImage.Height; y++)
        {
            for (int x = 0; x < croppedImage.Width; x++)
            {
                Color color = bmp.GetPixel(x + rect.Left, y + rect.Top);
                croppedImage.SetPixel(x, y, color);

            }
        }
        return croppedImage;


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

    public Bitmap Rotate180(Bitmap bmp)
    {
        Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color color = bmp.GetPixel(x, y);
                int newX = (bmp.Width - 1) - x;
                int newY = (bmp.Height - 1) - y;
                rotatedImage.SetPixel(newX, newY, color);

            }
        }
        return rotatedImage;
    }

    public Bitmap ExtractBlue(Bitmap bmp)
    {
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color color = bmp.GetPixel(x, y);
                Color newColor = Color.FromArgb(255, 0, 0, color.B);
                bmp.SetPixel(x, y, newColor);

            }
        }
        return bmp;

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
                double h = 0;
                double l = 0;
                double s = 0;
                RgbToHls(R, G, B, out h, out l, out s);
                h = (h + 180) % 360;
                HlsToRgb(h, l, s, out R, out G, out B);
                Color newColor = Color.FromArgb(255, R, G, B);
                bmp.SetPixel(x, y, newColor);


            }
        }
        return bmp;
    }

  
        public Bitmap ChangeBrightness(Bitmap bmp, int brightness)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);

            double scale = 1.0 + (double)brightness / 100.0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    Color newColor;

                    if (scale > 1)
                    {
                        int R = (byte)(Math.Min(255, scale * color.R));
                        int G = (byte)(Math.Min(255, scale * color.G));
                        int B = (byte)(Math.Min(255, scale * color.B));

                        newColor = Color.FromArgb(color.A, R, G, B);
                    }
                    else
                    {
                        int R = (int)(scale * color.R);
                        int G = (int)(scale * color.G);
                        int B = (int)(scale * color.B);

                        newColor = Color.FromArgb(color.A, R, G, B);
                    }

                    result.SetPixel(x, y, newColor);
                }
            }

            return result;
        }
       

    




    private static void RgbToHls(int r, int g, int b, out double h, out double l, out double s)
    {
        double double_r = r / 255.0;
        double double_g = g / 255.0;
        double double_b = b / 255.0;

        double max = double_r;
        if (max < double_g) max = double_g;
        if (max < double_b) max = double_b;

        double min = double_r;
        if (min > double_g) min = double_g;
        if (min > double_b) min = double_b;

        double diff = max - min;
        l = (max + min) / 2;
        if (Math.Abs(diff) < 0.00001)
        {
            s = 0;
            h = 0;
        }
        else
        {
            if (l <= 0.5) s = diff / (max + min);
            else s = diff / (2 - max - min);

            double r_dist = (max - double_r) / diff;
            double g_dist = (max - double_g) / diff;
            double b_dist = (max - double_b) / diff;

            if (double_r == max) h = b_dist - g_dist;
            else if (double_g == max) h = 2 + r_dist - b_dist;
            else h = 4 + g_dist - r_dist;

            h = h * 60;
            if (h < 0) h += 360;
        }
    }

    private static void HlsToRgb(double h, double l, double s, out int r, out int g, out int b)
    {
        double p2;
        if (l <= 0.5) p2 = l * (1 + s);
        else p2 = l + s - l * s;

        double p1 = 2 * l - p2;
        double double_r, double_g, double_b;
        if (s == 0)
        {
            double_r = l;
            double_g = l;
            double_b = l;
        }
        else
        {
            double_r = QqhToRgb(p1, p2, h + 120);
            double_g = QqhToRgb(p1, p2, h);
            double_b = QqhToRgb(p1, p2, h - 120);
        }

        r = (int)(double_r * 255.0);
        g = (int)(double_g * 255.0);
        b = (int)(double_b * 255.0);
    }

    private static double QqhToRgb(double q1, double q2, double hue)
    {
        if (hue > 360) hue -= 360;
        else if (hue < 0) hue += 360;

        if (hue < 60) return q1 + (q2 - q1) * hue / 60;
        if (hue < 180) return q2;
        if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
        return q1;
    }




}