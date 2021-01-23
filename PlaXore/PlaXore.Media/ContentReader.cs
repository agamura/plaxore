#region Header
//+ <source name="ContentReader.cs" language="C#" begin="5-Sep-2013">
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
using PlaXore.Extensions;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Provides functionality for reading decoded content from a stream.
    /// </summary>
    public class ContentReader : IDisposable
    {
        #region Fields
        private const int BufferSize = 128;
        private Stream input;
        private byte[] buffer;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentReader"/> class with
        /// the specified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public ContentReader(Stream input)
            : this(input, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentReader"/> class with
        /// the specified input stream and character encoding.
        /// </summary>
        /// <param name="input">The output stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public ContentReader(Stream input, Encoding encoding)
            : this(input, encoding, false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentReader"/> class with
        /// the specified input stream and character encoding, and with a value
        /// indicating whether or not leave the input stream open.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <param name="leaveOpen">
        /// <see langword="true"/> to leave <paramref name="input"/> open after this
        /// <see cref="ContentReader"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> does not support reading, seeking, or is already closed.
        /// </exception>
        public ContentReader(Stream input, Encoding encoding, bool leaveOpen)
        {
            if (input == null) { throw new ArgumentNullException("input"); }
            if (encoding == null) { throw new ArgumentNullException("encoding"); }
            if (!input.CanRead) { throw new ArgumentException("Input stream does not support reading or is already closed."); }
            if (!input.CanSeek) { throw new ArgumentException("Input stream does not support seeking or is already closed."); }

            this.input = input;
            Encoding = encoding;
            LeaveOpen = leaveOpen;
            buffer = new byte[BufferSize];
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the encoding in which text input is read.
        /// </summary>
        /// <value>The encoding in which text input is read.</value>
        public Encoding Encoding
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the underlying input stream of this <see cref="ContentReader"/>. 
        /// </summary>
        /// <value>The underlying input stream of this <see cref="ContentReader"/>.</value>
        /// <exception cref="ArgumentNullException">
        /// The specified value is <see langword="null"/>.
        /// </exception>
        public virtual Stream Input
        {
            get { return input; }
            protected set {
                if (value == null) { throw new ArgumentNullException(); }
                input = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="ContentReader"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="ContentReader"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not to leave <see cref="Input"/>
        /// open after this <see cref="ContentReader"/> has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to leave <see cref="Input"/> open after this
        /// <see cref="ContentReader"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </value>
        public bool LeaveOpen
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Closes this <see cref="ContentReader"/> and the underlying stream.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes of this <see cref="ContentReader"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="ContentReader"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                if (disposing && input != null && !LeaveOpen) {
                    // Release managed resources
                    input.Close();
                }

                // Release unmanaged resources

                input = null;
                Encoding = null;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Fills the specified buffer with the specified number of bytes read
        /// from <see cref="Input"/>, starting at the specified offset.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="offset">The offset in <paramref name="buffer"/> at which to start filling.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger
        /// than the length of <paramref name="buffer"/>.
        /// </exception>
        public virtual void FillBuffer(byte[] buffer, int offset, int count)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            if (offset < 0) { throw new ArgumentOutOfRangeException("offset"); }
            if (count < 0) { throw new ArgumentOutOfRangeException("count"); }

            if (offset + count > buffer.Length) {
                throw new ArgumentException(String.Format(
                    "Cannot read {0} bytes from a buffer of length {1} starting at position {2}.",
                    count, buffer.Length, offset
                ));
            }

            int position = offset;
            int end = offset + count;
            int bytesRead = 0;

            while (position < end) {
                bytesRead = input.Read(buffer, position, end - position);
                if (bytesRead == 0) { throw new EndOfStreamException(); }
                position += bytesRead;
            }
        }

        /// <summary>
        /// Reads the specified number of bytes from <see cref="Input"/>.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array containing data read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than zero.
        /// </exception>
        public virtual byte[] ReadBytes(int count)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            if (count < 0) { throw new ArgumentOutOfRangeException("count"); }

            byte[] buffer = new byte[count];
            FillBuffer(buffer, 0, count);

            return buffer;
        }

        /// <summary>
        /// Reads an 8-bit integer from <see cref="Input"/>.
        /// </summary>
        /// <returns>The 8-bit integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual sbyte ReadInt8()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            int value = input.ReadByte();
            if (value == -1) { throw new EndOfStreamException(); }

            return (sbyte) value;
        }

        /// <summary>
        /// Reads a 16-bit integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 16-bit integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual short ReadInt16()
        {
            return ReadBaseType(sizeof(short), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 32-bit integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 32-bit integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual int ReadInt32()
        {
            return ReadBaseType(sizeof(int), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 64-bit integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 64-bit integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual long ReadInt64()
        {
            return ReadBaseType(sizeof(long), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads an 8-bit unsigned integer from <see cref="Input"/>.
        /// </summary>
        /// <returns>The 8-bit unsigned integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual byte ReadUInt8()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            int value = input.ReadByte();
            if (value == -1) { throw new EndOfStreamException(); }

            return (byte) value;
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 16-bit unsigned integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual ushort ReadUInt16()
        {
            return ReadBaseType(sizeof(ushort), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 32-bit unsigned integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual uint ReadUInt32()
        {
            return ReadBaseType(sizeof(uint), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 64-bit unsigned integer read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual ulong ReadUInt64()
        {
            return ReadBaseType(sizeof(ulong), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 32-bit floating point from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 32-bit floating point read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual float ReadSingle()
        {
            return ReadBaseType(sizeof(float), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a 64-bit floating point from <see cref="Input"/> in host byte order.
        /// </summary>
        /// <returns>The 64-bit floating point read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual double ReadDouble()
        {
            return ReadBaseType(sizeof(double), Endianness.ByteOrder);
        }

        /// <summary>
        /// Reads a string from <see cref="Input"/>. The string is prefixed
        /// with the length, encoded as a 16-bit unsigned integer.
        /// </summary>
        /// <returns>The string read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">byte[] value =
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        public virtual string ReadString()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            ushort length = ReadBaseType(sizeof(ushort), Endianness.ByteOrder);
            if (length == 0) { return String.Empty; }
            byte[] buffer = length > BufferSize ? new byte[length] : this.buffer;
            FillBuffer(buffer, 0, length);

            return Encoding.GetString(buffer, 0, length);
        }

        /// <summary>
        /// Reads a quick string from <see cref="Input"/>.
        /// </summary>
        /// <param name="length">The length of the quick string to read.</param>
        /// <returns>The quick string read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">byte[] value =
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is greater than <b>8</b>.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        /// <remarks>
        /// Quick strings are ASCII strings up to 8 characters used for labelling or
        /// markup in general.
        /// </remarks>
        public virtual string ReadQuickString(int length)
        {
            if (length > sizeof(ulong)) {
                throw new ArgumentOutOfRangeException(
                    String.Format("The length of a quick string cannot exceed {0}.", sizeof(ulong)), "length");
            }

            if (length == 0) { return String.Empty; }
            if (length == sizeof(byte)) { return new String(new char[] { (char) ReadInt8() }); }
            if (length == sizeof(ushort)) { return Encoding.GetString(ReadBaseType(length, Endianness.ByteOrder).UInt16); }
            if (length <= sizeof(uint)) { return Encoding.GetString(ReadBaseType(length, Endianness.ByteOrder).UInt32); }
            return Encoding.GetString(ReadBaseType(length, Endianness.ByteOrder).UInt64);
        }

        /// <summary>
        /// Reads a <see cref="BaseType"/> from <see cref="Input"/> and
        /// advances the current position of the stream by <paramref name="count"/>.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="endianness">One of the <see cref="Endianness"/> values.</param>
        /// <returns>A <see cref="BaseType"/> read from <see cref="Input"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentReader"/> has already been disposed of.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached.
        /// </exception>
        internal BaseType ReadBaseType(int count, Endianness endianness)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            Array.Clear(buffer, 0, count << 1);
            FillBuffer(buffer, count, count);

            if (endianness == Endianness.ByteOrder && BitConverter.IsLittleEndian) {
                for (int i = 0, j = count - 1; i < count; i++, j--) { buffer[j] = buffer[i + count]; }
            } else {
                for (int i = 0; i < count; i++) { buffer[i] = buffer[i + count]; }
            }

            return (BaseType) buffer;
        }
        #endregion
    }
}
