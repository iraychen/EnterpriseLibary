using System.Drawing.Imaging;
using System.Drawing;

namespace Enterprises.Framework.VerifyImage
{
    /// <summary>
    /// ��֤��ͼƬ��Ϣ
    /// </summary>
    public class VerifyImageInfo
    {
        /// <summary>
        /// ���ɳ���ͼƬ
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// �����ͼƬ���ͣ��� image/pjpeg
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// ͼƬ�ĸ�ʽ
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// ��֤��
        /// </summary>
        public string VerifyCode { get; set; }
    }

    /// <summary>
    /// ��֤��λ��
    /// </summary>
    public enum VerifyLen
    {
        /// <summary>
        /// 4λ
        /// </summary>
        Four = 4,
        /// <summary>
        /// 5λ
        /// </summary>
        Five = 5,
        /// <summary>
        /// 6λ
        /// </summary>
        Six = 6,
    }
}
