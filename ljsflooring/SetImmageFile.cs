using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ljsflooring
{
    public class SetImmageFile
    {
        public string ProcessImageFile(string imagename, HttpRequestBase requestFile, HttpServerUtilityBase server, HttpContextBase httpContext)
        {
            return ResizeSaveImage(0, 600, 400, imagename, requestFile, server, httpContext);
        }

        public void DeleteImageFile(string fileName, HttpContextBase httpContextBase)
        {
            string fileToDelete = httpContextBase.Server.MapPath("~") + fileName.Replace("~", "");
            System.IO.File.Delete(fileToDelete);
        }

        private static string GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            rng.GetBytes(buff);
            string salt = Convert.ToBase64String(buff).Replace("\\", "").Replace("/", "").Replace("+", "").Replace("=", "");
            return salt;
        }

        private string ResizeSaveImage(int fileIndex, int width, int height, string oldImageName, HttpRequestBase requestFile, HttpServerUtilityBase server, HttpContextBase httpContext)
        {
            string imageName = "";
            string salt = GenerateSalt();

            ImageFormat format = GetImageFormat(requestFile.Files[fileIndex].FileName);
            byte[] firstImageBytes = GetResizedImage(requestFile.Files[fileIndex].InputStream, format, width, height);
            string firstImagePath = server.MapPath("~/Images/" + salt + "_" + width + "X" + height + "_" + System.IO.Path.GetFileName(requestFile.Files[fileIndex].FileName));
            System.IO.File.WriteAllBytes(firstImagePath, firstImageBytes);

            //delete old image file
            if (oldImageName != null)
            {
                string fileToDelete = httpContext.Server.MapPath("~") + oldImageName.Replace("~", "");
                System.IO.File.Delete(fileToDelete);
            }

            return imageName = "~\\Images\\" + salt + "_" + width + "X" + height + "_" + System.IO.Path.GetFileName(requestFile.Files[fileIndex].FileName);
        }

        byte[] GetResizedImage(Stream originalStream, ImageFormat imageFormat, int width, int height)
        {
            Bitmap imgIn = new Bitmap(originalStream);
            double y = imgIn.Height;
            double x = imgIn.Width;

            double factor = 1;
            if (width > 0)
            {
                factor = width / x;
            }
            else if (height > 0)
            {
                factor = height / y;
            }
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));

            // Set DPI of image (xDpi, yDpi)
            imgOut.SetResolution(72, 72);

            Graphics g = Graphics.FromImage(imgOut);
            g.Clear(Color.White);
            g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)),
              new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

            imgOut.Save(outStream, imageFormat);
            return outStream.ToArray();
        }

        ImageFormat GetImageFormat(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }
    }
}