#region Header
//+ <source name="ImageExtensions.cs" language="C#" begin="15-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
#endregion

namespace PlaXore.Media.Extensions
{
    /// <summary>
    /// Contains extension methods for the <c>Image</c> class.
    /// </summary>
    public static class ImageExtensions
    {
        #region Methods
        /// <summary>
        /// Returns a <c>ImageCodecInfo</c> that contains information about the image decoder
        /// for the specified file format.
        /// </summary>
        /// <param name="imageFormat">One of the <c>ImageFormat</c> values.</param>
        /// <returns>
        /// A <c>ImageCodecInfo</c> that contains information about the image decoder
        /// for <paramref name="imageFormat"/>.
        /// </returns>
        static private ImageCodecInfo GetEncoder(ImageFormat imageFormat)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == imageFormat.Guid);
        }

        /// <summary>
        /// Saves the specified image to the specified stream, with the specified quality.
        /// </summary>
        /// <param name="image">The <c>Image</c> instance this extension method applies to.</param>
        /// <param name="stream">The stream where to save <paramref name="image"/>.</param>
        /// <param name="imageFormat">One of the <c>ImageFormat</c> values.</param>
        /// <param name="quality">A value between 0 and 100 that represents the image quality.</param>
        /// <remarks>
        /// The lower the value, the higher the compression and therefore the lower the quality.
        /// 0 gives the lowest quality and 100 the highest.
        /// </remarks>
        public static void Save(this Image image, Stream stream, ImageFormat imageFormat, long quality)
        {
            ImageCodecInfo encoder = GetEncoder(imageFormat);

            using (EncoderParameters encoderParametetr = new EncoderParameters(1)) {
                using (EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, quality)) {
                    encoderParametetr.Param[0] = encoderParameter;
                    image.Save(stream, encoder, encoderParametetr);
                }
            }
        }

        /// <summary>
        /// Resizes the specified image with the specified width and height.
        /// </summary>
        /// <param name="image">The <c>Image</c> instance this extension method applies to.</param>
        /// <param name="width">The image width.</param>
        /// <param name="height">The image height.</param>
        /// <returns>The resized image.</returns>
        public static Image Resize(this Image image, int width, int height)
        {
            return image.Resize(width, height, InterpolationMode.HighQualityBicubic);
        }

        /// <summary>
        /// Resizes the specified image with the specified width, height, and scaling algorithm.
        /// </summary>
        /// <param name="image">The <c>Image</c> instance this extension method applies to.</param>
        /// <param name="width">The image width.</param>
        /// <param name="height">The image height.</param>
        /// <param name="interpolationMode">One of the <c>InterpolationMode</c> values.</param>
        /// <returns>The resized image.</returns>
        public static Image Resize(this Image image, int width, int height, InterpolationMode interpolationMode)
        {
            Image resized = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(resized)) {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = interpolationMode;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return resized;
        }
        #endregion
    }
}
