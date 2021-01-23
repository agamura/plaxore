#region Header
//+ <source name="MiwaReader.cs" language="C#" begin="5-Sep-2013">
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
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Provides functionality for reading MIWA content from a stream.
    /// </summary>
    internal class MiwaReader : ContentReader
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaReader"/> class with
        /// the specified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public MiwaReader(Stream input)
            : this(input, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaReader"/> class with
        /// the specified input stream and character encoding.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public MiwaReader(Stream input, Encoding encoding)
            : this(input, encoding, false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaReader"/> class with
        /// the specified input stream and character encoding, and with a value
        /// indicating whether or not leave the input stream open.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <param name="leaveOpen">
        /// <see langword="true"/> to leave <paramref name="input"/> open after this
        /// <see cref="MiwaReader"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public MiwaReader(Stream input, Encoding encoding, bool leaveOpen)
            :base(input, encoding, leaveOpen)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reads a <see cref="BlockHeader"/> from <see cref="ContentReader.Input"/>.
        /// </summary>
        /// <returns>The <see cref="BlockHeader"/> read from <see cref="ContentReader.Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public BlockHeader ReadBlockHeader()
        {
            return new BlockHeader(base.ReadUInt32());
        }

        /// <summary>
        /// Reads the content block described by the specifed <see cref="BlockHeader"/>
        /// from <see cref="ContentReader.Input"/>.
        /// </summary>
        /// <param name="blockHeader">The <see cref="BlockHeader"/> that describes the content block to read.</param>
        /// <returns>A byte array containing data read from <see cref="ContentReader.Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaReader"/> has already been disposed of.
        /// </exception>
        public byte[] ReadBlock(BlockHeader blockHeader)
        {
            return base.ReadBytes((int) blockHeader.BlockSize);
        }

        /// <summary>
        /// Reads a <see cref="GlobalMetadata"/> from <see cref="ContentReader.Input"/>.
        /// </summary>
        /// <returns>The <see cref="GlobalMetadata"/> read from <see cref="ContentReader.Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public GlobalMetadata ReadGlobalMetadata()
        {
            GlobalMetadata globalMetadata = new GlobalMetadata();

            globalMetadata.Guid = new Guid(ReadBytes(GlobalMetadata.GuidSize));
            globalMetadata.CodecInfo = new CodecInfo(ReadQuickString(sizeof(uint)), new SimpleVersion(ReadUInt16(), ReadUInt16()));
            globalMetadata.FrameCount = ReadUInt32();
            globalMetadata.AudioBlockCount = ReadUInt32();
            globalMetadata.Duration = ReadUInt64();
            globalMetadata.Title = ReadString();
            globalMetadata.Copyright = ReadString();
            globalMetadata.Timestamp = ReadUInt64();

            return globalMetadata;
        }

        /// <summary>
        /// Reads a <see cref="FrameMetadata"/> from <see cref="ContentReader.Input"/>.
        /// </summary>
        /// <returns>The <see cref="FrameMetadata"/> read from <see cref="ContentReader.Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public FrameMetadata ReadFrameMetadata()
        {
            FrameMetadata frameMetadata = new FrameMetadata();

            frameMetadata.Format = ReadQuickString(sizeof(uint));
            frameMetadata.Height = ReadUInt16();
            frameMetadata.Width = ReadUInt16();
            frameMetadata.Quality = ReadUInt8();
            frameMetadata.Rate = ReadUInt8();

            return frameMetadata;
        }

        /// <summary>
        /// Reads a <see cref="AudioMetadata"/> from <see cref="ContentReader.Input"/>.
        /// </summary>
        /// <returns>The <see cref="AudioMetadata"/> read from <see cref="ContentReader.Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public AudioMetadata ReadAudioMetadata()
        {
            AudioMetadata audioMetadata = new AudioMetadata();

            audioMetadata.ChunkId = ReadQuickString(sizeof(uint));
            audioMetadata.ChunkSize = ReadUInt32();
            audioMetadata.RiffType = ReadQuickString(sizeof(uint));
            audioMetadata.FormatId = ReadQuickString(sizeof(uint));
            audioMetadata.FormatSize = ReadUInt32();
            audioMetadata.CompressionType = ReadUInt16();
            audioMetadata.Channels = ReadUInt16();
            audioMetadata.SampleRate = ReadUInt32();
            audioMetadata.AverageBytesPerSecond = ReadUInt32();
            audioMetadata.BlockAlign = ReadUInt16();
            audioMetadata.BitDepth = ReadUInt16();

            if (audioMetadata.FormatSize > 16) {
                audioMetadata.ExtraFormatData = ReadBytes(ReadUInt16());
            }

            audioMetadata.DataId = ReadQuickString(sizeof(uint));
            audioMetadata.DataSize = ReadUInt32();

            return audioMetadata;
        }
        #endregion
    }
}
