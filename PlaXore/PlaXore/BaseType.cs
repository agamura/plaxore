#region Header
//+ <source name="BaseType.cs" language="C#" begin="2-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Runtime.InteropServices;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Defines an union that lets base types have several representations. 
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BaseType
    {
        #region Fields
        /// <summary>
        /// Gets or sets an 8-bit integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>An 8-bit integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public sbyte Int8;

        /// <summary>
        /// Gets or sets a 16-bit integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 16-bit integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public short Int16;

        /// <summary>
        /// Gets or sets a 32-bit integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 32-bit integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public int Int32;

        /// <summary>
        /// Gets or sets a 64-bit integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 64-bit integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public long Int64;

        /// <summary>
        /// Gets or sets an 8-bit unsigned integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>An 8-bit unsigned integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public byte UInt8;

        /// <summary>
        /// Gets or sets a 16-bit unsigned integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 16-bit unsigned integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public ushort UInt16;

        /// <summary>
        /// Gets or sets a 32-bit unsigned integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 32-bit unsigned integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public uint UInt32;

        /// <summary>
        /// Gets or sets a 64-bit unsigned integer representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 64-bit unsigned integer representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public ulong UInt64;

        /// <summary>
        /// Gets or sets a 32-bit floating point representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 32-bit floating point representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public float Single;

        /// <summary>
        /// Gets or sets a 64-bit floating point representation of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>A 64-bit floating point representation of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public double Double;

        /// <summary>
        /// Gets or sets the byte <b>one</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>one</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(0)]
        public byte Byte1;

        /// <summary>
        /// Gets or sets the byte <b>two</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>two</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(1)]
        public byte Byte2;

        /// <summary>
        /// Gets or sets the byte <b>three</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>three</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(2)]
        public byte Byte3;

        /// <summary>
        /// Gets or sets the byte <b>four</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>four</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(3)]
        public byte Byte4;

        /// <summary>
        /// Gets or sets the byte <b>five</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>five</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(4)]
        public byte Byte5;

        /// <summary>
        /// Gets or sets the byte <b>six</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>six</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(5)]
        public byte Byte6;

        /// <summary>
        /// Gets or sets the byte <b>seven</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>seven</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(6)]
        public byte Byte7;

        /// <summary>
        /// Gets or sets the byte <b>eight</b> of this <see cref="BaseType"/>.
        /// </summary>
        /// <value>The byte <b>eight</b> of this <see cref="BaseType"/>.</value>
        [FieldOffset(7)]
        public byte Byte8;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 8-bit integer.
        /// </summary>
        /// <param name="value">An 8-bit integer.</param>
        public BaseType(sbyte value)
            : this(0L)
        {
            Int8 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 16-bit integer.
        /// </summary>
        /// <param name="value">A 16-bit integer.</param>
        public BaseType(short value)
            : this(0L)
        {
            Int16 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 32-bit integer.
        /// </summary>
        /// <param name="value">A 32-bit integer.</param>
        public BaseType(int value)
            : this(0L)
        {
            Int32 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 64-bit integer.
        /// </summary>
        /// <param name="value">A 64-bit integer.</param>
        public BaseType(long value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = Byte5 = Byte6 = Byte7 = Byte8 = 0;
            Int64 = Int32 = Int16 = Int8 = 0;
            UInt64 = UInt32 = UInt16 = UInt8 = 0;
            Double = Single = 0f;
            Int64 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">An 8-bit unsigned integer.</param>
        public BaseType(byte value)
            : this(0L)
        {
            UInt8 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">A 16-bit unsigned integer.</param>
        public BaseType(ushort value)
            : this(0L)
        {
            UInt16 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">A 32-bit unsigned integer.</param>
        public BaseType(uint value)
            : this(0L)
        {
            UInt32 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">A 64-bit unsigned integer.</param>
        public BaseType(ulong value)
            : this(0L)
        {
            UInt64 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 32-bit floating point.
        /// </summary>
        /// <param name="value">A 32-bit floating point.</param>
        public BaseType(float value)
            : this(0L)
        {
            Single = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType"/> structure
        /// with the specified 64-bit floating point.
        /// </summary>
        /// <param name="value">A 64-bit floating point.</param>
        public BaseType(double value)
            : this(0L)
        {
            Double = value;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to an 8-bit integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 8-bit integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator sbyte(BaseType baseType)
        {
            return baseType.Int8;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 16-bit integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 16-bit integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator short(BaseType baseType)
        {
            return baseType.Int16;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 32-bit integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 32-bit integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator int(BaseType baseType)
        {
            return baseType.Int32;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 64-bit integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 64-bit integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator long(BaseType baseType)
        {
            return baseType.Int64;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to an 8-bit unsigned integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 8-bit unsigned integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator byte(BaseType baseType)
        {
            return baseType.UInt8;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 16-bit unsigned integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 16-bit unsigned integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator ushort(BaseType baseType)
        {
            return baseType.UInt16;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 32-bit unsigned integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator uint(BaseType baseType)
        {
            return baseType.UInt32;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 64-bit unsigned integer converted from <paramref name="baseType"/>.</returns>
        public static implicit operator ulong(BaseType baseType)
        {
            return baseType.UInt64;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 32-bit floating point.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 32-bit floating point converted from <paramref name="baseType"/>.</returns>
        public static implicit operator float(BaseType baseType)
        {
            return baseType.Single;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> implicitly to a 64-bit floating point.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The 64-bit floating point converted from <paramref name="baseType"/>.</returns>
        public static implicit operator double(BaseType baseType)
        {
            return baseType.Double;
        }

        /// <summary>
        /// Converts the specified <see cref="BaseType"/> explicitly to a byte array.
        /// </summary>
        /// <param name="baseType">The <see cref="BaseType"/> to convert.</param>
        /// <returns>The byte array converted from <paramref name="baseType"/>.</returns>
        public static explicit operator byte[](BaseType baseType)
        {
            return new byte[] {
                baseType.Byte1, baseType.Byte2, baseType.Byte3, baseType.Byte4,
                baseType.Byte5, baseType.Byte6, baseType.Byte7, baseType.Byte8
            };
        }

        /// <summary>
        /// Converts the specified 8-bit integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 8-bit integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(sbyte value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 16-bit integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 16-bit integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(short value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 32-bit integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 32-bit integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(int value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 64-bit integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 64-bit integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(long value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 8-bit unsigned integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 8-bit unsigned integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(byte value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 16-bit unsigned integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(ushort value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 32-bit unsigned integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(uint value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 64-bit unsigned integer implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(ulong value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 32-bit floating point implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 32-bit floating point to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(float value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified 64-bit floating point implicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The 64-bit floating point to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        public static implicit operator BaseType(double value)
        {
            return new BaseType(value);
        }

        /// <summary>
        /// Converts the specified byte array explicitly to a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The byte array to convert.</param>
        /// <returns>The <see cref="BaseType"/> converted from <paramref name="value"/>.</returns>
        /// <remarks>
        /// If <paramref name="value"/> is <see langword="null"/> then the value
        /// of the resulting <see cref="BaseType"/> is zero.
        /// </remarks>
        public static explicit operator BaseType(byte[] value)
        {
            BaseType baseType = new BaseType(0);

            if (value != null) {
                if (value.Length > 0) { baseType.Byte1 = value[0]; }
                if (value.Length > 1) { baseType.Byte2 = value[1]; }
                if (value.Length > 2) { baseType.Byte3 = value[2]; }
                if (value.Length > 3) { baseType.Byte4 = value[3]; }
                if (value.Length > 4) { baseType.Byte5 = value[4]; }
                if (value.Length > 5) { baseType.Byte6 = value[5]; }
                if (value.Length > 6) { baseType.Byte7 = value[6]; }
                if (value.Length > 7) { baseType.Byte8 = value[7]; }
            }

            return baseType;
        }
        #endregion
    }
}
