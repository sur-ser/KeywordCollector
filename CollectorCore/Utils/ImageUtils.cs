using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Utils
{
    public class ImageUtils
    {
        public static Image GetImage(string fileName, int width, int height)
        {
            if (fileName == null)
                return null;

            Image img = null;
            try
            {
                img = Image.FromFile(fileName);
                img = ZoomPicture(img, width, height);
            }
            catch { }
            return img;
        }

        // 按比例缩放图片
        public static Image ZoomPicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {

            //新的图片宽
            int IntWidth;
            //新的图片高
            int IntHeight;
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(TargetWidth, TargetHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);


                //宽度比目的图片宽度大，长度比目的图片长度小
                if (SourceImage.Width > TargetWidth && SourceImage.Height <= TargetHeight)
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                //宽度比目的图片宽度小，长度比目的图片长度大
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height > TargetHeight)
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                //长宽比目的图片长宽都小
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <= TargetHeight)
                {
                    IntHeight = SourceImage.Width;
                    IntWidth = SourceImage.Height;
                }
                //长宽比目的图片的长宽都大
                else
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }

                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight - IntHeight) / 2, IntWidth, IntHeight);
                SourceImage.Dispose();

                return SaveImage;
            }
            catch { }

            return null;
        }
    }
}
