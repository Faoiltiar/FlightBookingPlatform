using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.IO;

namespace LOT
{
    static class ToImage
    {
        // zapisanie obrazu zawartości elementu ScrollViewer zawierającego podsumowanie biletu
        public static void SaveImage(Grid grid, int dpi, string filename, int height, int width)
        {
            var rtb = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Pbgra32);
            rtb.Render(grid);
            SaveRTBAsBMP(rtb, filename);
        }
        // zapis utworzonego obrazu do pliku bmp
        private static void SaveRTBAsBMP(RenderTargetBitmap bmp, string filename)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));
            using (var stm = File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
