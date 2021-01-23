#region Header
//+ <source name="AudioMetadata.cs" language="C#" begin="8-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.IO;
using System.Runtime.InteropServices;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Defines a structure for describing audio metadata.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AudioMetadata
    {
        #region Fields
        /// <summary>
        /// Gets or sets the chunk id.
        /// </summary>
        /// <value>The chunk id.</value>
        public string ChunkId;

        /// <summary>
        /// Gets or sets the chunk size.
        /// </summary>
        /// <value>The chunk size, in bytes.</value>
        public uint ChunkSize;

        /// <summary>
        /// Gets or sets the RIFF type.
        /// </summary>
        /// <value>The RIFF type.</value>
        public string RiffType;

        /// <summary>
        /// Gets or sets the id of the format chunk.
        /// </summary>
        /// <value>The id of the format chunk.</value>
        public string FormatId;

        /// <summary>
        /// Gets or sets the size of the format chunk.
        /// </summary>
        /// <value>The size of the format chunk, in bytes.</value>
        public uint FormatSize;

        /// <summary>
        /// Gets or sets the compression type.
        /// </summary>
        /// <value>The compression type.</value>
        public ushort CompressionType;

        /// <summary>
        /// Gets or sets the number of separate audio signals.
        /// </summary>
        /// <value>The number of separate audio signals.</value>
        public ushort Channels;

        /// <summary>
        /// Gets or sets the number of sample slices per second.
        /// </summary>
        /// <value>The number of sample slices per second.</value>
        public uint SampleRate;

        /// <summary>
        /// Gets or sets the average streaming rate required to play the data chunk.
        /// </summary>
        /// <value>The average streaming rate, in bytes per second.</value>
        public uint AverageBytesPerSecond;

        /// <summary>
        /// Gets or sets the number of bytes per sample slice.
        /// </summary>
        /// <value>The number of bytes per sample slice.</value>
        public ushort BlockAlign;

        /// <summary>
        /// Gets or sets the number of bits used to define each sample.
        /// </summary>
        /// <value>The number of bits used to define each sample.</value>
        public ushort BitDepth;

        /// <summary>
        /// Gets or sets any extra format data.
        /// </summary>
        /// <value>Any extra format data.</value>
        public byte[] ExtraFormatData;

        /// <summary>
        /// Gets or sets the id of the data chunk.
        /// </summary>
        /// <value>The id of the data chunk.</value>
        public string DataId;

        /// <summary>
        /// Gets or sets the size of the data chunk.
        /// </summary>
        /// <value>The size of the data chunk, in bytes.</value>
        public uint DataSize;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the size of this <see cref="AudioMetadata"/>.
        /// </summary>
        /// <value>The size of this <see cref="AudioMetadata"/>, in bytes.</value>
        public int Size
        {
            get {
                int size = 44;

                if (FormatSize == 18) { size += sizeof(ushort); }
                if (ExtraFormatData != null) { size += ExtraFormatData.Length; }

                return size;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts this <see cref="AudioMetadata"/> to a byte array.
        /// </summary>
        /// <param name="endianness">One of the <see cref="Endianness"/> values.</param>
        /// <returns>The byte array converted from this <see cref="AudioMetadata"/>.</returns>
        /// <remarks>
        /// Numeric values are always represented in network byte order, except when
        /// restoring the original wav file, in which case values must be represented
        /// in Little-Endian order.
        /// </remarks>
        public byte[] ToByteArray(Endianness endianness)
        {
            MemoryStream memoryStream = new MemoryStream(Size);

            using (ContentWriter contentWriter = new ContentWriter(memoryStream)) {
                contentWriter.Write(ChunkId, true);
                contentWriter.Write(ChunkSize, sizeof(uint), endianness);
                contentWriter.Write(RiffType, true);
                contentWriter.Write(FormatId, true);
                contentWriter.Write(FormatSize, sizeof(uint), endianness);
                contentWriter.Write(CompressionType, sizeof(ushort), endianness);
                contentWriter.Write(Channels, sizeof(ushort), endianness);
                contentWriter.Write(SampleRate, sizeof(uint), endianness);
                contentWriter.Write(AverageBytesPerSecond, sizeof(uint), endianness);
                contentWriter.Write(BlockAlign, sizeof(ushort), endianness);
                contentWriter.Write(BitDepth, sizeof(ushort), endianness);

                if (FormatSize == 18) {
                    if (ExtraFormatData != null && ExtraFormatData.Length > 0) {
                        contentWriter.Write((ushort) ExtraFormatData.Length, sizeof(ushort), endianness);
                        contentWriter.Write(ExtraFormatData);
                    } else {
                        contentWriter.Write((ushort) 0);
                    }                
                } 
                
                contentWriter.Write(DataId, true);
                contentWriter.Write(DataSize, sizeof(uint), endianness);
            }

            return memoryStream.GetBuffer();
        }
        #endregion
    }
}
