using System;
using System.Data;
using System.Drawing.Drawing2D;
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

        #region  水印,缩略图

        //是否已经加载了JPEG编码解码器
        private static bool _isloadjpegcodec = false;
        //当前系统安装的JPEG编码解码器
        private static ImageCodecInfo _jpegcodec = null;

        /// <summary>
        /// 获得当前系统安装的JPEG编码解码器
        /// </summary>
        /// <returns></returns>
        public static ImageCodecInfo GetJPEGCodec()
        {
            if (_isloadjpegcodec == true)
                return _jpegcodec;

            ImageCodecInfo[] codecsList = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecsList)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    _jpegcodec = codec;
                    break;
                }

            }
            _isloadjpegcodec = true;
            return _jpegcodec;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="thumbPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static void GenerateThumb(string imagePath, string thumbPath, int width, int height, string mode)
        {
            Image image = Image.FromFile(imagePath);

            string extension = imagePath.Substring(imagePath.LastIndexOf(".")).ToLower();
            ImageFormat imageFormat = null;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            int toWidth = width > 0 ? width : image.Width;
            int toHeight = height > 0 ? height : image.Height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）           
                    break;
                case "W"://指定宽，高按比例             
                    toHeight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    toWidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）           
                    if ((double)image.Width / (double)image.Height > (double)toWidth / (double)toHeight)
                    {
                        oh = image.Height;
                        ow = image.Height * toWidth / toHeight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / toWidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp
            Image bitmap = new Bitmap(toWidth, toHeight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image,
                        new Rectangle(0, 0, toWidth, toHeight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumbPath, imageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }

        /// <summary>
        /// 生成图片水印
        /// </summary>
        /// <param name="originalPath">源图路径</param>
        /// <param name="watermarkPath">水印图片路径</param>
        /// <param name="targetPath">保存路径</param>
        /// <param name="position">位置</param>
        /// <param name="opacity">透明度</param>
        /// <param name="quality">质量</param>
        public static void GenerateImageWatermark(string originalPath, string watermarkPath, string targetPath, int position, int opacity, int quality)
        {
            Image originalImage = null;
            Image watermarkImage = null;
            //图片属性
            ImageAttributes attributes = null;
            //画板
            Graphics g = null;
            try
            {

                originalImage = Image.FromFile(originalPath);
                watermarkImage = new Bitmap(watermarkPath);

                if (watermarkImage.Height >= originalImage.Height || watermarkImage.Width >= originalImage.Width)
                {
                    originalImage.Save(targetPath);
                    return;
                }

                if (quality < 0 || quality > 100)
                    quality = 80;

                //水印透明度
                float iii;
                if (opacity > 0 && opacity <= 10)
                    iii = (float)(opacity / 10.0F);
                else
                    iii = 0.5F;

                //水印位置
                int x = 0;
                int y = 0;
                switch (position)
                {
                    case 1:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 2:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 3:
                        x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 4:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 5:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 6:
                        x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 7:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                        break;
                    case 8:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                        break;
                    case 9:
                        x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                        y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                        break;
                }

                //颜色映射表
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] newColorMap = { colorMap };

                //颜色变换矩阵,iii是设置透明度的范围0到1中的单精度类型
                float[][] newColorMatrix ={ 
                                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  iii, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                           };
                //定义一个 5 x 5 矩阵
                ColorMatrix matrix = new ColorMatrix(newColorMatrix);

                //图片属性
                attributes = new ImageAttributes();
                attributes.SetRemapTable(newColorMap, ColorAdjustType.Bitmap);
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //画板
                g = Graphics.FromImage(originalImage);
                //绘制水印
                g.DrawImage(watermarkImage, new Rectangle(x, y, watermarkImage.Width, watermarkImage.Height), 0, 0, watermarkImage.Width, watermarkImage.Height, GraphicsUnit.Pixel, attributes);
                //保存图片
                EncoderParameters encoderParams = new EncoderParameters();
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { quality });
                if (GetJPEGCodec() != null)
                    originalImage.Save(targetPath, _jpegcodec, encoderParams);
                else
                    originalImage.Save(targetPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (attributes != null)
                    attributes.Dispose();
                if (watermarkImage != null)
                    watermarkImage.Dispose();
                if (originalImage != null)
                    originalImage.Dispose();
            }
        }

        /// <summary>
        /// 生成文字水印
        /// </summary>
        /// <param name="originalPath">源图路径</param>
        /// <param name="targetPath">保存路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="textSize">文字大小</param>
        /// <param name="textFont">文字字体</param>
        /// <param name="position">位置</param>
        /// <param name="quality">质量</param>
        public static void GenerateTextWatermark(string originalPath, string targetPath, string text, int textSize, string textFont, int position, int quality)
        {
            Image originalImage = null;
            //画板
            Graphics g = null;
            try
            {
                originalImage = Image.FromFile(originalPath);
                //画板
                g = Graphics.FromImage(originalImage);
                if (quality < 0 || quality > 100)
                    quality = 80;

                Font font = new Font(textFont, textSize, FontStyle.Regular, GraphicsUnit.Pixel);
                SizeF sizePair = g.MeasureString(text, font);

                float x = 0;
                float y = 0;

                switch (position)
                {
                    case 1:
                        x = (float)originalImage.Width * (float).01;
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 2:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 3:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 4:
                        x = (float)originalImage.Width * (float).01;
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 5:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 6:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 7:
                        x = (float)originalImage.Width * (float).01;
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                    case 8:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                    case 9:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                }

                g.DrawString(text, font, new SolidBrush(Color.White), x + 1, y + 1);
                g.DrawString(text, font, new SolidBrush(Color.Black), x, y);

                //保存图片
                EncoderParameters encoderParams = new EncoderParameters();
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { quality });
                if (GetJPEGCodec() != null)
                    originalImage.Save(targetPath, _jpegcodec, encoderParams);
                else
                    originalImage.Save(targetPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (originalImage != null)
                    originalImage.Dispose();
            }
        }

        #endregion
	}
}

