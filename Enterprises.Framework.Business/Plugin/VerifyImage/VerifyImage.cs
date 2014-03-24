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
    /// 验证码默认图片类（生成Jpeg）
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
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
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
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }


        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="onlyNum">只是数字</param>
        /// <returns>验证码对象</returns>
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
                //定义前景色为黑色
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
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="onlyNum">只是数字</param>
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

    }
}