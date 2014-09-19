using System;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;

namespace Enterprises.Framework.VerifyImage
{
    /// <summary>
    /// ��֤��Ĭ��ͼƬ�ࣨ����Jpeg��
    /// </summary>
    public class VerifyImage:IVerifyImage
    {
        private static readonly byte[] Randb = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        private static readonly Matrix M = new Matrix();
        private static readonly Bitmap Charbmp = new Bitmap(35, 35);

        private static readonly Font[] Fonts = {
                                        new Font(new FontFamily("Times New Roman"), 16 + Next(3), FontStyle.Regular),
                                        new Font(new FontFamily("Georgia"), 16 + Next(3), FontStyle.Regular),
                                        new Font(new FontFamily("Arial"), 16 + Next(3), FontStyle.Regular),
                                        new Font(new FontFamily("Comic Sans MS"), 16 + Next(3), FontStyle.Regular)
                                     };
        private static readonly string[] VerifycodeRange = { "1","2","3","4","5","6","7","8","9",
                                                    "a","b","c","d","e","f","g",
                                                    "h",    "j","k",    "m","n",
                                                        "p","q",    "r","s","t",
                                                    "u","v","w",    "x","y"
                                                    
                                                  };
        private static readonly Random VerifycodeRandom = new Random();

        /// <summary>
        /// �����һ�������
        /// </summary>
        /// <param name="max">���ֵ</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            Rand.GetBytes(Randb);
            int value = BitConverter.ToInt32(Randb, 0);
            value = value % (max + 1);
            if (value < 0)
                value = -value;
            return value;
        }

        /// <summary>
        /// �����һ�������
        /// </summary>
        /// <param name="min">��Сֵ</param>
        /// <param name="max">���ֵ</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }


        /// <summary>
        /// ������֤��
        /// </summary>
        /// <param name="len">����</param>
        /// <param name="onlyNum">ֻ������</param>
        /// <returns>��֤�����</returns>
        public  VerifyImageInfo GenerateImage(int len, bool onlyNum)
        {
            var verifyimage = new VerifyImageInfo {ImageFormat = ImageFormat.Jpeg, ContentType = "image/pjpeg"};
            var code = CreateVerifyCode(len, onlyNum);
         
            int width = 23*code.Length;
            const int height = 30;

            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Color bgcolor = Color.White;
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.Clear(bgcolor);

            const int fixedNumber = 60;

            var drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100)));
            for (int i = 0; i < 3; i++)
            {
                var linePen =
                    new Pen(Color.FromArgb(Next(50) + fixedNumber, Next(50) + fixedNumber, Next(50) + fixedNumber), 1);
                g.DrawArc(linePen, Next(20) - 10, Next(20) - 10, Next(width) + 10, Next(height) + 10, Next(-100, 100),
                          Next(-200, 200));
            }

            Graphics charg = Graphics.FromImage(Charbmp);
            Color[] colors =
                {
                    Color.Black, Color.Blue, Color.Green, Color.Navy, Color.MediumBlue, Color.Orange,
                    Color.Red, Color.DarkGoldenrod, Color.Maroon, Color.Sienna
                };
            float charx = -18;
            for (int i = 0; i < code.Length; i++)
            {
                M.Reset();
                M.RotateAt(Next(10) - 5, new PointF(Next(3) + 2, Next(3) + 2));

                charg.Clear(Color.Transparent);
                charg.Transform = M;
                //����ǰ��ɫΪ��ɫ
                drawBrush.Color = colors[Next(9)]; // Color.Black;

                charx = charx + 18 + Next(3);
                var drawPoint = new PointF(charx, 1.0F);
                charg.DrawString(code[i].ToString(CultureInfo.InvariantCulture), Fonts[Next(Fonts.Length - 1)],
                                 drawBrush, new PointF(0, 0));

                charg.ResetTransform();

                g.DrawImage(Charbmp, drawPoint);
            }


            drawBrush.Dispose();
            g.Dispose();
            charg.Dispose();

            verifyimage.Image = bitmap;
            verifyimage.VerifyCode = code;

            return verifyimage;
        }

        /// <summary>
        /// ������֤��
        /// </summary>
        /// <param name="len">����</param>
        /// <param name="onlyNum">ֻ������</param>
        /// <returns>string</returns>
        private  string CreateVerifyCode(int len, bool onlyNum)
        {
            var checkCode = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int number = VerifycodeRandom.Next(0, !onlyNum ? VerifycodeRange.Length : 10);
                checkCode.Append(VerifycodeRange[number]);
            }

            return checkCode.ToString();
        }

        /// <summary>
        /// �������ͼƬ
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="imageWidth">ͼƬ���</param>
        /// <param name="imageHeight">ͼƬ�߶�</param>
        /// <param name="imageBgColor">ͼƬ������ɫ</param>
        /// <param name="imageTextColor1">ͼƬ������ɫ</param>
        /// <param name="imageTextColor2">ͼƬ������ɫ</param>
        /// <returns>���ͼƬ</returns>
        public VerifyImageInfo CreateRandomImage(string value, int imageWidth, int imageHeight, Color imageBgColor, Color imageTextColor1, Color imageTextColor2)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            //����ͼƬ����
            //var stream = new MemoryStream();
            try
            {
                //������������� 
                Random random = new Random();

                //���ͼƬ����ɫ 
                g.Clear(imageBgColor);

                //��ͼƬ�ı��������� 
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255))), x1, y1, x2, y2);
                }

                var font = new Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                                                                    imageTextColor1,
                                                                    imageTextColor2,
                                                                    1.2f,
                                                                    true);
                g.DrawString(value, font, brush, 2, 2);

                //��ͼƬ��ǰ�������� 
                for (int i = 0; i < 80; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //��ͼƬ�ı߿��� 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                var verifyImage = new VerifyImageInfo();
                //image.Save(stream, ImageFormat.Jpeg);
                verifyImage.Image = image;// stream.ToArray();
                verifyImage.ContentType = "image/jpeg";
                return verifyImage;
            }
            finally
            {
                //if (stream != null)
                //    stream.Dispose();
                if (g != null)
                    g.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }
    }
}