using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labCG4
{
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class Contraster
    {

        public static unsafe Image Contrast(Image pic, int numberContrast)
        {
            // принимает аргумент - величина смены контраста
            if (numberContrast < -100) return pic;
            if (numberContrast > 100) return pic;
            double pixel = 0, contrast = (100.0 + numberContrast) / 100.0; // контраст можеть быть от -100 до 100
            contrast *= contrast;
            int red, green, blue;
            var b = new Bitmap(pic);
            var newBmp = new Bitmap(pic.Width, pic.Height);
            var bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var stride = bmData.Stride;
            var scan0 = bmData.Scan0;

            var p = (byte*)(void*)scan0;

            var nOffset = stride - b.Width * 3;

            for (var y = 0; y < b.Height; ++y)
            {
                for (var x = 0; x < b.Width; ++x)
                {
                    blue = p[0];
                    green = p[1];
                    red = p[2];

                    pixel = red / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    if (pixel > 255) pixel = 255;
                    p[2] = (byte)pixel;

                    pixel = green / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    if (pixel > 255) pixel = 255;
                    p[1] = (byte)pixel;

                    pixel = blue / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    if (pixel > 255) pixel = 255;
                    p[0] = (byte)pixel;
                    p += 3;
                }
                p += nOffset;
            }
            b.UnlockBits(bmData);
            return newBmp = new Bitmap(b);
        }

        public static Image ConstrastImage(Image pic, int fMin, int fMax)
        {
            var picture = new Bitmap(pic);
            int gMax = 0, gMin = 0;
            for (var a = 0; a < picture.Width - 1; a++)
                for (var b = 0; b < picture.Height - 1; b++)
                {
                    var br = (int)(255 * picture.GetPixel(a, b).GetBrightness());
                    if (gMax < br) gMax = br;
                    if (gMin > br) gMin = br;
                }
            for (var a = 0; a < picture.Width - 1; a++)
                for (var b = 0; b < picture.Height - 1; b++)
                {
                    var f = (int)(255 * picture.GetPixel(a, b).GetBrightness());
                    if (fMax != fMin && f != 0)
                    {
                        //var g = ((f - fMin) / (fMax - fMin)) * (gMax - gMin) + gMin;
                       // var g = ((fMax - gMin) + gMin * fMax - gMax * fMin) * f / (fMax - fMin);
                        var g = (gMax - gMin) * f / (fMax - fMin) + (fMax * gMin - fMin * gMax) / (fMax - fMin);
                        double newR = picture.GetPixel(a, b).R;
                        double newG = picture.GetPixel(a, b).G;
                        double newB = picture.GetPixel(a, b).B;
                        double value = f - g;

                        if ((newR + value) > 255) newR = 255;
                        else if ((newR + value) < 0) newR = 0;
                        else newR += value;

                        if ((newG + value) > 255) newG = 255;
                        else if ((newG + value) < 0) newG = 0;
                        else newG += value;

                        if ((newB + value) > 255) newB = 255;
                        else if ((newB + value) < 0) newB = 0;
                        else newB += value;
                        
                        picture.SetPixel(a, b, Color.FromArgb((int)newR, (int)newG, (int)newB));
                    }
                }

            return picture;
        }
    }
}
