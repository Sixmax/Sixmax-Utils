
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SXUtils.Extensions
{
    public static class ImagingExtensions
    {
        /// <summary>
        /// Round the Corners of a Image.
        /// </summary>
        /// <param name="StartImage"></param>
        /// <param name="CornerRadius"></param>
        /// <param name="OutlineRadius"></param>
        /// <returns></returns>
        public static System.Drawing.Image RoundCorners(this System.Drawing.Image StartImage, int CornerRadius, int? OutlineRadius = null)
        {
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);

            using (Graphics g = Graphics.FromImage(RoundedImage))
            {
                g.Clear(System.Drawing.Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                System.Drawing.Brush brush = new TextureBrush(StartImage);

                Func<int, GraphicsPath> GetPath = (int r) =>
                {
                    GraphicsPath gp = new GraphicsPath();

                    double tx = ((StartImage.Width / 100) * r);
                    double ty = ((StartImage.Height / 100) * r);

                    #region Prevention
                    if (tx == 0 && ty == 0)
                    {
                        tx = r;
                        ty = r;
                    }
                    else
                    if (tx == 0) tx = ty;
                    else
                    if (ty == 0) ty = tx;

                    int xr = Math.Min(Math.Max((int)tx, 1), 100);
                    int yr = Math.Min(Math.Max((int)ty, 1), 100);
                    #endregion

                    gp.AddArc(0, 0, xr, yr, 180, 90); // top left 
                    gp.AddArc(0 + RoundedImage.Width - xr, 0, xr, yr, 270, 90); // top right 
                    gp.AddArc(0 + RoundedImage.Width - xr, 0 + RoundedImage.Height - yr, xr, yr, 0, 90); // bottom right
                    gp.AddArc(0, 0 + RoundedImage.Height - yr, xr, yr, 90, 90); // bottom left 

                    return gp;
                };

                if (OutlineRadius != null
                    && (CornerRadius - (int)OutlineRadius) > 0)
                {
                    g.FillPath(System.Drawing.Brushes.Black, GetPath(CornerRadius));
                    g.FillPath(brush, GetPath(CornerRadius - (int)OutlineRadius));
                }
                else
                    g.FillPath(brush, GetPath(CornerRadius));

                return RoundedImage;
            }
        }

        /// <summary>
        /// Set the Foreground Color of a Bitmap.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <param name="MakeTransparent"></param>
        /// <returns></returns>
        public static Bitmap SetTintColor(this Bitmap input, System.Drawing.Color color, bool MakeTransparent = true)
        {
            Bitmap result = new Bitmap(input.Width, input.Height, input.PixelFormat);

            for (int x = 0; x < input.Width - 1; x++)
            {
                for (int y = 0; y < input.Height - 1; y++)
                {
                    System.Drawing.Color pixel = input.GetPixel(x, y);

                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0 && pixel.A == 0) // skip transparent pixels 
                        continue;

                    result.SetPixel(x, y, color);
                }
            }

            if (MakeTransparent)
                result.MakeTransparent();

            input = result;

            return result;
        }

        /// <summary>
        /// Converts a Bitmap to a Bitmap Source
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Convert a Bitmap to a BitmapImage
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImage(this Bitmap b)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();
            return bImg;
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ToImageSource(this Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(handle);
                return newSource;
            }
            catch 
            {
                DeleteObject(handle);
                return null;
            }
        }

        /// <summary>
        /// Convert a Image to a Bitmap.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this Image img) => (Bitmap)img;

        /// <summary>
        /// Gets a BitmapImage from a Url. Difference from using the Builtin Image Fetch is that in the Built-in one, the StreamSource would be null. With this, there is a StreamSource.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> FromUrl(this BitmapImage input, string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("The Url cannot be Null or an Empty string.");

            using (HttpClient imgClient = new HttpClient())
            {
                HttpResponseMessage imgResp = await imgClient.GetAsync(url);
                return ((Bitmap)Image.FromStream(await imgResp.Content.ReadAsStreamAsync())).ToBitmapImage();
            }       
        }

        /// <summary>
        /// Converts a Bitmap to a Brush which can then be used as Background for (for example) a Canvas. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static System.Windows.Media.Brush ToBrush(this Bitmap input) => new ImageBrush(input.ToBitmapSource());
        
    }
}
