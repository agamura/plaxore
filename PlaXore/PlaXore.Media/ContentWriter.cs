#region Header
//+ <source name="ContentWriter.cs" language="C#" begin="5-Sep-2013">
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
    /// Provides functionality for writing encoded content to a stream.
    /// </summary>
    public class ContentWriter : IDisposable
    {
        #region Fields
        private Stream output;
        private byte[] buffer;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentWriter"/> class with
        /// the specified output stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="output"/> does not support writing or is already closed.
        /// </exception>
        public ContentWriter(Stream output)
            : this(output, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentWriter"/> class with
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
        public ContentWriter(Stream output, Encoding encoding)
            : this(output, encoding, false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentWriter"/> class
        /// with the specified output stream and character encoding, and with a
        /// value indicating whether or not leave the output stream open.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <param name="leaveOpen">
        /// <see langword="true"/> to leave <paramref name="output"/> open after this
        /// <see cref="ContentWriter"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="output"/> does not support writing or seeking, or is already closed.
        /// </exception>
        public ContentWriter(Stream output, Encoding encoding, bool leaveOpen)
        {
            if (output == null) { throw new ArgumentNullException("output"); }
            if (encoding == null) { throw new ArgumentNullException("encoding"); }
            if (!output.CanWrite) { throw new ArgumentException("Output stream does not support writing or is already closed."); }
            if (!output.CanSeek) { throw new ArgumentException("Output stream does not support seeking or is already closed."); }

            this.output = output;
            Encoding = encoding;
            LeaveOpen = leaveOpen;
            buffer = new byte[sizeof(long)];
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the encoding in which text output is written.
        /// </summary>
        /// <value>The encoding in which text output is written.</value>
        public Encoding Encoding
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="ContentWriter"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="ContentWriter"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not to leave <see cref="Output"/>
        /// open after this <see cref="ContentWriter"/> has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to leave <see cref="Output"/> open after this
        /// <see cref="ContentWriter"/> has been disposed of; otherwisde, <see langword="false"/>.
        /// </value>
        public bool LeaveOpen
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the underlying output stream of this <see cref="ContentWriter"/>. 
        /// </summary>
        /// <value>The underlying output stream of this <see cref="ContentWriter"/>.</value>
        /// <exception cref="ArgumentNullException">
        /// The specified value is <see langword="null"/>.
        /// </exception>
        public virtual Stream Output
        {
            get {
                Flush();
                return output;
            }
            protected set {
                if (value == null) { throw new ArgumentNullException(); }
                output = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Closes this <see cref="ContentWriter"/> and the underlying stream.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes of this <see cref="ContentWriter"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="ContentWriter"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                if (disposing && output != null && !LeaveOpen) {
                    // Release managed resources
                    output.Close();
                }

                // Release unmanaged resources

                output = null;
                Encoding = null;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Clears all buffers for this <see cref="ContentWriter"/> and causes
        /// any buffered data to be written to the underlying device. 
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Flush()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            output.Flush();
        }

        /// <summary>
        /// Sets the position within <see cref="Output"/>.
        /// </summary>
        /// <param name="offset">The offset relative to <see paramref="origin"/>.</param>
        /// <param name="origin">One of the <b>SeekOrigin</b> values.</param>
        /// <returns>The position within <see cref="Output"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual long Seek(int offset, SeekOrigin origin)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            return output.Seek(offset, origin);
        }

        /// <summary>
        /// Writes the specified byte array to <see cref="Output"/>.
        /// </summary>
        /// <param name="buffer">The byte array to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(byte[] buffer)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            output.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the specified byte array to <see cref="Output"/> starting
        /// at the specified offset,
        /// </summary>
        /// <param name="buffer">The byte array to write to <see cref="Output"/>.</param>
        /// <param name="offset">The offset in <paramref name="buffer"/> at which to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger
        /// than the length of <paramref name="buffer"/>.
        /// </exception>
        public virtual void Write(byte[] buffer, int offset, int count)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            output.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes the specified 8-bit integer to <see cref="Output"/>.
        /// </summary>
        /// <param name="value">The 8-bit integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(sbyte value)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            output.WriteByte((byte) value);
        }

        /// <summary>
        /// Writes the specified 16-bit integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 16-bit integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(short value)
        {
            Write(value, sizeof(short), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 32-bit integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 32-bit integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(int value)
        {
            Write(value, sizeof(int), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 64-bit integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 64-bit integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(long value)
        {
            Write(value, sizeof(long), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 8-bit unsigned integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 8-bit unsigned integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(byte value)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            output.WriteByte(value);
        }

        /// <summary>
        /// Writes the specified 16-bit unsigned integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(ushort value)
        {
            Write(value, sizeof(ushort), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 32-bit unsigned integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(uint value)
        {
            Write(value, sizeof(uint), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 64-bit unsigned integer to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(ulong value)
        {
            Write(value, sizeof(ulong), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 32-bit floating point to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 32-bit floating point to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(float value)
        {
            Write(value, sizeof(float), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified 64-bit floating point to <see cref="Output"/> in
        /// network byte order.
        /// </summary>
        /// <param name="value">The 64-bit floating point to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        public virtual void Write(double value)
        {
            Write(value, sizeof(double), Endianness.BigEndian);
        }

        /// <summary>
        /// Writes the specified string to <see cref="Output"/>.
        /// </summary>
        /// <param name="value">The string to write to <see cref="Output"/>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is too long.
        /// </exception>
        /// <remarks>
        /// <see cref="Write(string)"/> also writes the length, in bytes, of the byte array
        /// converted from <paramref name="value"/> using <see cref="Encoding"/>.
        /// </remarks>
        public virtual void Write(string value)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            if (String.IsNullOrEmpty(value)) {
                Write(0, sizeof(ushort), Endianness.BigEndian);
            } else {
                byte[] byteArray = Encoding.GetBytes(value);

                if (byteArray.Length > UInt16.MaxValue) {
                    throw new ArgumentException(String.Format("String exceeds {0} characters.", UInt16.MaxValue), "value");
                }

                Write(byteArray.Length, sizeof(ushort), Endianness.BigEndian);
                output.Write(byteArray, 0, byteArray.Length);
            }
        }

        /// <summary>
        /// Writes the specified string to <see cref="Output"/>.
        /// </summary>
        /// <param name="value">The string to write to <see cref="Output"/>.</param>
        /// <param name="quickString">
        /// <see langword="true"/> to encode <paramref name="value"/> as a quick string;
        /// otherwise, <see langword="false"/>.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="quickString"/> is <see langword="true"/> and <paramref name="value"/>
        /// is <see langword="null"/> or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="quickString"/> is <see langword="false"/> and <paramref name="value"/>
        /// is too long.
        /// </exception>
        /// <remarks>
        /// Quick strings are ASCII strings up to 8 characters used for labelling or
        /// markup in general.
        /// </remarks>
        public virtual void Write(string value, bool quickString)
        {
            if (quickString) {
                if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
                if (String.IsNullOrEmpty(value)) { throw new ArgumentNullException("String is null or empty", "value"); }

                BaseType baseType = Encoding.GetUInt64(value);

                if (baseType.UInt64 <= Byte.MaxValue) { Write(baseType.UInt8); }
                else if (baseType.UInt64 <= UInt16.MaxValue) { Write(baseType, sizeof(ushort), Endianness.BigEndian); }
                else if (baseType.UInt64 <= UInt32.MaxValue) { Write(baseType, sizeof(uint), Endianness.BigEndian); }
                else { Write(baseType, sizeof(ulong), Endianness.BigEndian); }
            } else {
                Write(value);
            }
        }

        /// <summary>
        /// Writes the specified <see cref="BaseType"/> to <see cref="Output"/>.
        /// </summary>
        /// <param name="value">The <see cref="BaseType"/> to write to <see cref="Output"/>.</param>
        /// <param name="count">The number of bytes to write to <see cref="Output"/>.</param>
        /// <param name="endianness">One of the <see cref="Endianness"/> values.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentWriter"/> has already been disposed of.
        /// </exception>
        internal void Write(BaseType value, int count, Endianness endianness)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            byte[] bytes = (byte[]) value;

            if (endianness == Endianness.BigEndian && BitConverter.IsLittleEndian) {
                for (int i = 0, j = count - 1; i < count; i++, j--) { buffer[i] = bytes[j]; }
            } else {
                for (int i = 0; i < count; i++) { buffer[i] = bytes[i]; }
            }

            output.Write(buffer, 0, count);
        }
        #endregion
    }
}
