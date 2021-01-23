#region Header
//+ <source name="FrameMetadata.cs" language="C#" begin="8-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using PlaXore.Extensions;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Defines a structure for describing frame metadata.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FrameMetadata
    {
        #region Fields
        /// <summary>
        /// Gets or sets the frame format.
        /// </summary>
        /// <value>
        /// A quick string representing the frame format.
        /// </value>
        public string Format;

        /// <summary>
        /// Gets or sets the frame height.
        /// </summary>
        /// <value>The frame height.</value>
        public ushort Height;

        /// <summary>
        /// Gets or sets the frame width.
        /// </summary>
        /// <value>The frame width.</value>
        public ushort Width;

        /// <summary>
        /// Gets or sets the frame quality.
        /// </summary>
        /// <value>
        /// A value between 0 and 100 that represents the frame quality.
        /// </value>
        /// <remarks>
        /// The lower the value, the higher the compression and therefore the
        /// lower the quality. 0 gives the lowest quality and 100 the highest.
        /// </remarks>
        public byte Quality;

        /// <summary>
        /// Gets or sets the number of frames per second.
        /// </summary>
        /// <value>The number of frames per second, in the range of 8-60.</value>
        public byte Rate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the size of this <see cref="FrameMetadata"/>.
        /// </summary>
        /// <value>The size of this <see cref="FrameMetadata"/>, in bytes.</value>
        public int Size
        {
            get { return 10; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts this <see cref="FrameMetadata"/> to a byte array.
        /// </summary>
        /// <returns>The byte array converted from this <see cref="FrameMetadata"/>.</returns>
        public byte[] ToByteArray()
        {
            MemoryStream memoryStream = new MemoryStream(Size);

            using (ContentWriter contentWriter = new ContentWriter(memoryStream)) {
                contentWriter.Write(Format, true);
                contentWriter.Write(Height);
                contentWriter.Write(Width);
                contentWriter.Write(Quality);
                contentWriter.Write(Rate);
            }

            return memoryStream.GetBuffer();
        }
        #endregion
    }
}
