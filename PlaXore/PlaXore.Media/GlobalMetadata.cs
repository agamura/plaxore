#region Header
//+ <source name="GlobalMetadata.cs" language="C#" begin="4-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Defines a structure for describing global metadata. Global metadata applies
    /// to whole content and is neither audio nor video specific.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct GlobalMetadata
    {
        #region Fields
        public const int GuidSize = 16;

        /// <summary>
        /// Gets or sets the globally unique identifier of the content.
        /// </summary>
        /// <value>The globally unique identifier of the content.</value>
        public Guid Guid;

        /// <summary>
        /// Gets or sets the codec used to encode/decode this <see cref="GlobalMetadata"/>.
        /// </summary>
        /// <value>The codec used to encode/decode this <see cref="GlobalMetadata"/>.</value>
        public CodecInfo CodecInfo;

        /// <summary>
        /// Gets or sets the total number of frames.
        /// </summary>
        /// <value>The total number of frames.</value>
        public uint FrameCount;

        /// <summary>
        /// Gets or sets the total number of audio blocks.
        /// </summary>
        /// <value>The total number of audio blocks.</value>
        public uint AudioBlockCount;

        /// <summary>
        /// Gets or sets the duration of the content.
        /// </summary>
        /// <value>The duration of the content, in milliseconds</value>
        public ulong Duration;

        /// <summary>
        /// Gets or sets the content title.
        /// </summary>
        /// <value>The content title.</value>
        public string Title;

        /// <summary>
        /// Gets or sets the copyright notice.
        /// </summary>
        /// <value>The copyright notice.</value>
        public string Copyright;

        /// <summary>
        /// Gets or sets the creation time of the content.
        /// </summary>
        /// <value>The creation time of the content.</value>
        public ulong Timestamp;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the size of this <see cref="GlobalMetadata"/>.
        /// </summary>
        /// <value>The size of this <see cref="GlobalMetadata"/>, in bytes.</value>
        public int Size
        {
            get {
                int titleLength = String.IsNullOrEmpty(Title) ? 0 : Encoding.UTF8.GetByteCount(Title);
                int copyrightLength = String.IsNullOrEmpty(Copyright) ? 0 : Encoding.UTF8.GetByteCount(Copyright);
                return 52 + titleLength + copyrightLength;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts this <see cref="GlobalMetadata"/> to a byte array.
        /// </summary>
        /// <returns>The byte array converted from this <see cref="GlobalMetadata"/>.</returns>
        public byte[] ToByteArray()
        {
            MemoryStream memoryStream = new MemoryStream(Size);

            using (ContentWriter contentWriter = new ContentWriter(memoryStream)) {
                contentWriter.Write(Guid.ToByteArray());
                contentWriter.Write(CodecInfo.Name, true);
                contentWriter.Write(CodecInfo.Version.Major);
                contentWriter.Write(CodecInfo.Version.Minor);
                contentWriter.Write(FrameCount);
                contentWriter.Write(AudioBlockCount);
                contentWriter.Write(Duration);
                contentWriter.Write(Title);
                contentWriter.Write(Copyright);
                contentWriter.Write(Timestamp);
            }

            return memoryStream.GetBuffer();
        }
        #endregion
    }
}
