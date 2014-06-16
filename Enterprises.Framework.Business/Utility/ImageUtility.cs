using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;

namespace Enterprises.Framework.Utility
{
	/// <summary>
	/// 图象处理通用类
	/// </summary>
	public class ImageUtility
	{
        /// <summary>
        /// 图像的格式过滤条件
        /// </summary>
        public const string ImageFilter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;*.bmp)|*.jpg;*.jpeg;*.gif;*.png;*.bmp|所有文件(*.*)|*.*";



		/// <summary>
		/// 根据指定的文件的扩展名，获取图像的格式
		/// </summary>
		/// <param name="imageFile"></param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(string imageFile)
		{
            if (imageFile.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                return ImageFormat.Jpeg; ;
            }

            var extension = Path.GetExtension(imageFile);
            if (extension != null)
		    {
                extension = extension.ToUpper();
		        switch(extension)
		        {
		            case "JPG":
		            case "JPEG":
		                return ImageFormat.Jpeg;
		            case "ICO":
		                return ImageFormat.Icon;
		            case "PNG":
		                return ImageFormat.Png;
		            case "GIF":
		                return ImageFormat.Gif;
		            case "BMP":
		                return ImageFormat.Bmp;
		        }
		    }

		    return ImageFormat.Jpeg;
		}
		/// <summary>
		/// 把图像文件转化为字节数组
		/// </summary>
		/// <param name="imageFile"></param>
		/// <returns></returns>
		public static byte[] Image2Bytes(string imageFile)
		{
            using (var ms = new MemoryStream())
            {
                using (Image img = Image.FromFile(imageFile))
                {
                    img.Save(ms, GetImageFormat(imageFile));
                    return StreamUtility.Stream2Bytes(ms);
                }
            }
		}
		/// <summary>
		/// 把图像转化为字节数组
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		public static byte[] Image2Bytes(Image img)
		{
            if (img == null)
            {
                return null;
            }

            using (Stream s = new MemoryStream())
            {
                img.Save(s, ImageFormat.Jpeg);
                return StreamUtility.Stream2Bytes(s);
            }
		}

        /// <summary>
        /// 把图片转换成流
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public static Stream Image2Stream(Image img,ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, imageFormat);
                return ms;
            }
        }

        /// <summary>
        /// 把字节数组转化为图像对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image Bytes2Image(byte[] bytes)
        {
            try
            {
                return Image.FromStream(StreamUtility.Bytes2Stream(bytes));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 把Image存入指定的文件
        /// </summary>
        /// <param name="img"></param>
        /// <param name="saveFileName"></param>
        public static void SaveImage(Image img, string saveFileName)
        {
            string path = FileUtility.GetFilePath(saveFileName);
            FileUtility.CreatePath(path);
            img.Save(saveFileName);
        }

        /// <summary>
        /// 把//二进制表示的图像存入指定的文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="saveFileName"></param>
        public static void SaveImage(byte[] bytes, string saveFileName)
        {            
            Image img = Bytes2Image(bytes);
            if(img != null)
            {
                if (!FileUtility.ExistsFile(saveFileName))
                {
                    FileUtility.CreateFile(saveFileName);
                }
                img.Save(saveFileName);
            }           
        }

        /// <summary>
        /// 从文件获取Image对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Image GetImage(string fileName)
        {
            return Image.FromFile(fileName);
        }

		/// <summary>
		/// 按照给定的长宽格式化图片,
		/// 对于图像的格式化,最好以原始数据为依据来进行,否则容易失真
		/// </summary>
		/// <param name="srcImage"></param>
		/// <param name="dstWidth"></param>
		/// <param name="dstHeight"></param>
		/// <returns></returns>
        public static Image ResizePicture(Image srcImage, int dstWidth, int dstHeight)
		{
			var callBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
			Image dstImage = srcImage.GetThumbnailImage(dstWidth,dstHeight,callBack,new System.IntPtr());
			return dstImage;
		}

	    /// <summary>
	    /// 格式化图像流（按照默认的最佳尺寸比例进行缩放）
	    /// </summary>
	    /// <param name="srcImage"></param>
	    /// <param name="width"></param>
	    /// <param name="height"></param>
	    public static Image ScaledPicture(Image srcImage, int width=640, int height=480)
		{
			int srcWidth = srcImage.Width;
			int srcHeight = srcImage.Height;

            int dstWidth;
            int dstHeight;

            if (srcWidth == width && srcHeight == height)
            {
                //正好匹配，不用格式化了
                return srcImage;
            }
            else if (srcWidth * height > width * srcHeight)
            {
                //以宽度进行按比例压缩，因为太宽，所以让宽度适应整个图片框
                dstWidth = width;
                dstHeight = srcHeight * dstWidth / srcWidth;
            }
            else
            {
                //以高度进行按比例压缩，因为太高，所以让高度适应整个图片框
                dstHeight = height;
                dstWidth = srcWidth * dstHeight / srcHeight;
            }

            return ResizePicture(srcImage, dstWidth, dstHeight);		
		}


		/// <summary>
		/// 格式化图片为缩略图（按照默认的最佳尺寸比例进行缩放）
		/// </summary>
		/// <param name="fullFileName"></param>
		/// <param name="newFullFileName">新的图片的文件名</param>
        public static void ScaledPicture(string fullFileName, string newFullFileName)
		{
			Image srcImage = null;
			Image dstImage = null;
			srcImage = Image.FromFile(fullFileName);
            dstImage = ScaledPicture(srcImage);			
			dstImage.Save(newFullFileName);
			dstImage.Dispose();
			srcImage.Dispose();
		}

		/// <summary>
		/// 按指定的长宽格式化图片
		/// </summary>
		/// <param name="fullFileName"></param>
		/// <param name="newFullFileName"></param>
		/// <param name="dstWidth"></param>
		/// <param name="dstHeight"></param>
        public static void ResizePicture(string fullFileName, string newFullFileName, int dstWidth, int dstHeight)
		{
			Image srcImage = Image.FromFile(fullFileName);
            Image dstImage = ResizePicture(srcImage, dstWidth, dstHeight);			
			dstImage.Save(newFullFileName);
			dstImage.Dispose();
			srcImage.Dispose();
		}

		/// <summary>
		/// 格式化图片为缩略图,新的图片前面加上Small_前缀,表示为缩略图.
		/// </summary>
		/// <param name="fullFileName"></param>
        public static void ResizePicture(string fullFileName)
		{
			string path = FileUtility.GetFilePath(fullFileName);
			string fileName = FileUtility.GetFileName(fullFileName);
            ScaledPicture(fullFileName, path + "Small_" + fileName);
		}

        /// <summary>
        /// 按照指定的百分比压缩图像
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static byte[] ResizePicture(byte[] srcBytes, int percent)
        {
            Image img = Bytes2Image(srcBytes);
            int width = img.Width * percent / 100;
            int height = img.Height * percent / 100;

            return Image2Bytes(ResizePicture(img, width, height));
        }


		/// <summary>
		/// 内部代理程序
		/// </summary>
		/// <returns></returns>
		private static bool ThumbnailCallback() { return false; }

        /// <summary>
        /// 调整图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public Bitmap ResizeImage(Bitmap bmp, int newW, int newH, int mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 剪裁图片 -- 用GDI+
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="startX">开始坐标X</param>
        /// <param name="startY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public Bitmap CutImage(Bitmap b, int startX, int startY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (startX >= w || startY >= h)
            {
                return null;
            }

            if (startX + iWidth > w)
            {
                iWidth = w - startX;
            }
            if (startY + iHeight > h)
            {
                iHeight = h - startY;
            }

            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(startX, startY, iWidth, iHeight), GraphicsUnit.Pixel);

                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
	}
}

