using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Util
{
    public static class ImageHelper
    {
        public static BitmapImage LoadPngImage(string path)
        {
            MemoryStream memoryStream = new MemoryStream();
            new Bitmap(path).Save((Stream)memoryStream, ImageFormat.Png);
            memoryStream.Position = 0L;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            memoryStream.Seek(0L, SeekOrigin.Begin);
            bitmapImage.StreamSource = (Stream)memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            memoryStream.Close();
            return bitmapImage;
        }

        public static ImageSource ImageToSource(Image image)
        {
            if (image == null)
                return (ImageSource)null;
            MemoryStream memoryStream = new MemoryStream();
            image.Save((Stream)memoryStream, ImageFormat.Bmp);
            memoryStream.Position = 0L;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = (Stream)memoryStream;
            bitmapImage.EndInit();
            return (ImageSource)bitmapImage;
        }
    }
}
