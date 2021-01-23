#region Header
//+ <source name="MiwaWriter.cs" language="C#" begin="5-Sep-2013">
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
    /// Provides functionality for writing MIWA content to a stream.
    /// </summary>
    internal class MiwaWriter : ContentWriter
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaWriter"/> class with
        /// the specified output stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="output"/> does not support writing or is already closed.
        /// </exception>
        public MiwaWriter(Stream output)
            : this(output, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaWriter"/> class with
        /// the specified output stream and character encoding.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="output"/> does not support writing or is already closed.
        /// </exception>
        public MiwaWriter(Stream output, Encoding encoding)
            : this(output, encoding, false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaWriter"/> class
        /// with the specified output stream and character encoding, and with a
        /// value indicating whether or not leave the output stream open.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <param name="leaveOpen">
        /// <see langword="true"/> to leave <paramref name="output"/> open after this
        /// <see cref="MiwaWriter"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="output"/> does not support writing or is already closed.
        /// </exception>
        public MiwaWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Writes the specified <see cref="GlobalMetadata"/> to <see cref="ContentWriter.Output"/>.
        /// </summary>
        /// <param name="globalMetadata">
        /// The <see cref="GlobalMetadata"/> to write to <see cref="ContentWriter.Output"/>.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaWriter"/> has already been disposed of.
        /// </exception>
        public void Write(GlobalMetadata globalMetadata)
        {
            Write(BlockType.GlobalMetadata, globalMetadata.ToByteArray());
        }

        /// <summary>
        /// Writes the specified <see cref="FrameMetadata"/> to <see cref="ContentWriter.Output"/>.
        /// </summary>
        /// <param name="frameMetadata">
        /// The <see cref="FrameMetadata"/> to write to <see cref="ContentWriter.Output"/>.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaWriter"/> has already been disposed of.
        /// </exception>
        public void Write(FrameMetadata frameMetadata)
        {
            Write(BlockType.FrameMetadata, frameMetadata.ToByteArray());
        }

        /// <summary>
        /// Writes the specified <see cref="AudioMetadata"/> to <see cref="ContentWriter.Output"/>.
        /// </summary>
        /// <param name="audioMetadata">
        /// The <see cref="AudioMetadata"/> to write to <see cref="ContentWriter.Output"/>.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaWriter"/> has already been disposed of.
        /// </exception>
        public void Write(AudioMetadata audioMetadata)
        {
            Write(BlockType.AudioMetadata, audioMetadata.ToByteArray(Endianness.BigEndian));
        }

        /// <summary>
        /// Writes the specified byte array to <see cref="ContentWriter.Output"/>.
        /// </summary>
        /// <param name="blockType">One of the <see cref="BlockType"/> values.</param>
        /// <param name="buffer">The byte array to write to <see cref="ContentWriter.Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaWriter"/> has already been disposed of.
        /// </exception>
        public void Write(BlockType blockType, byte[] buffer)
        {
            Write(blockType, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the specified byte array to <see cref="ContentWriter.Output"/> starting
        /// at the specified offset,
        /// </summary>
        /// <param name="blockType">One of the <see cref="BlockType"/> values.</param>
        /// <param name="buffer">The byte array to write to <see cref="ContentWriter.Output"/>.</param>
        /// <param name="offset">The offset in <paramref name="buffer"/> at which to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaWriter"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger
        /// than the length of <paramref name="buffer"/>.
        /// </exception>
        public void Write(BlockType blockType, byte[] buffer, int offset, int count)
        {
            BlockHeader blockHeader = new BlockHeader(blockType, (uint) count);
            Write(blockHeader.Value); 
            Write(buffer, offset, count);
        }
        #endregion
    }
}
