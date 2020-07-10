﻿using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace GPB
{
    static public class BitmapTools
    {
        static public async Task<BitmapImage> LoadBitmapAsync(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    var bitmap = new BitmapImage();
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await bitmap.SetSourceAsync(stream);
                    return bitmap;
                }
            }
            return null;
        }

        static public async Task<BitmapImage> LoadBitmapAsync(IRandomAccessStreamReference randomStreamReference)
        {
            var bitmap = new BitmapImage();
            try
            {
                using (var stream = await randomStreamReference.OpenReadAsync())
                {
                    await bitmap.SetSourceAsync(stream);
                }
            }
            catch { }
            return bitmap;
        }
    }
}
