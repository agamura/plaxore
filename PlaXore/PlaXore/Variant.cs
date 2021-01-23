#region Header
//+ <source name="Variant.cs" language="C#" begin="21-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using PlaXore.Utilities;
using System;
using System.Text;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Implements a tagged union for representing base types, character
    /// strings, or raw byte arrays.
    /// </summary>
    public class Variant
    {
        #region Fields
        private const string InvalidCastMessage = "Cannot convert type {0} to {1}.";

        private byte[] value;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class.
        /// </summary>
        public Variant()
        {
            Type = VariantType.Undefined;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified Boolean value.
        /// </summary>
        /// <param name="value">The Boolean value to be represented by this <see cref="Variant"/>.</param>
        public Variant(bool value)
        {
            Type = VariantType.Boolean;
            IsBaseType = true;

            this.value = new byte[sizeof(bool)];
            this.value[0] = (byte) (value ? 1 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 8-bit integer.
        /// </summary>
        /// <param name="value">The 8-bit integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(sbyte value)
        {
            Type = VariantType.Int8;
            IsBaseType = true;

            this.value = new byte[sizeof(sbyte)];
            this.value[0] = (byte) value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 16-bit integer.
        /// </summary>
        /// <param name="value">The 16-bit integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(short value)
        {
            Type = VariantType.Int16;
            IsBaseType = true;

            this.value = new byte[sizeof(short)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 32-bit integer.
        /// </summary>
        /// <param name="value">The 32-bit integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(int value)
        {
            Type = VariantType.Int32;
            IsBaseType = true;

            this.value = new byte[sizeof(int)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 64-bit integer.
        /// </summary>
        /// <param name="value">The 64-bit integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(long value)
        {
            Type = VariantType.Int64;
            IsBaseType = true;

            this.value = new byte[sizeof(long)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with the specified 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">The 8-bit unsigned integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(byte value)
        {
            Type = VariantType.UInt8;
            IsBaseType = true;

            this.value = new byte[sizeof(byte)];
            this.value[0] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with the specified 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(ushort value)
        {
            Type = VariantType.UInt16;
            IsBaseType = true;

            this.value = new byte[sizeof(ushort)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(uint value)
        {
            Type = VariantType.UInt32;
            IsBaseType = true;

            this.value = new byte[sizeof(uint)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to be represented by this <see cref="Variant"/>.</param>
        public Variant(ulong value)
        {
            Type = VariantType.UInt64;
            IsBaseType = true;

            this.value = new byte[sizeof(ulong)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 32-bit floating point number.
        /// </summary>
        /// <param name="value">The 32-bit floating point number to be represented by this <see cref="Variant"/>.</param>
        public Variant(float value)
        {
            Type = VariantType.Single;
            IsBaseType = true;

            this.value = new byte[sizeof(float)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified 64-bit floating point number.
        /// </summary>
        /// <param name="value">The 64-bit floating point number to be represented by this <see cref="Variant"/>.</param>
        public Variant(double value)
        {
            Type = VariantType.Double;
            IsBaseType = true;

            this.value = new byte[sizeof(double)];
            this.value = BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified character string.
        /// </summary>
        /// <param name="value">The character string to be represented by this <see cref="Variant"/>.</param>
        public Variant(string value)
        {
            Type = VariantType.String;
            this.value = value != null && value != "" ? Encoding.UTF8.GetBytes(value) : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variant"/> class with
        /// the specified byte array.
        /// </summary>
        /// <param name="value">The byte array to be represented by this <see cref="Variant"/>.</param>
        public Variant(byte[] value)
        {
            Type = VariantType.ByteArray;
            this.value = value != null && value.Length > 0 ? value : null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether or not this <see cref="Variant"/> is
        /// a base type.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Variant"/> is a base type;
        /// otherwise, <see langowrd="false"/>.
        /// </value>
        public virtual bool IsBaseType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the size of this <see cref="Variant"/>.
        /// </summary>
        /// <value>The size of this <see cref="Variant"/>, in bytes.</value>
        public virtual int Size
        {
            get { return value != null ? value.Length : 0; }
        }

        /// <summary>
        /// Gets the type of this <see cref="Variant"/>.
        /// </summary>
        /// <value>One of the <see cref="VariantType"/> values.</value>
        public virtual VariantType Type
        {
            get;
            private set;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Variant"/> classes for equality.
        /// </summary>
        /// <param name="variant1">The first <see cref="Variant"/> to compare.</param>
        /// <param name="variant2">The second <see cref="Variant"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both value and type of <paramref name="variant1"/>
        /// and <paramref name="variant2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Variant variant1, Variant variant2)
        {
            return Equals(variant1, variant2);
        }

        /// <summary>
        /// Compares the specified <see cref="Variant"/> classes for inequality.
        /// </summary>
        /// <param name="variant1">The first <see cref="Variant"/> to compare.</param>
        /// <param name="variant2">The second <see cref="Variant"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="variant1"/> and <paramref name="variant2"/>
        /// have different values or types; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Variant variant1, Variant variant2)
        {
            return !Equals(variant1, variant2);
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to an 8-bit
        /// integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 8-bit integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to an 8-bit integer.
        /// </exception>
        public static implicit operator sbyte(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Int8));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (sbyte) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (sbyte) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).Int8;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 16-bit
        /// integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 16-bit integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 16-bit integer.
        /// </exception>
        public static implicit operator short(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Int16));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (short) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (short) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).Int16;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 32-bit
        /// integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 32-bit integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 32-bit integer.
        /// </exception>
        public static implicit operator int(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Int32));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (int) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (int) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).Int32;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 64-bit
        /// integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 64-bit integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 64-bit integer.
        /// </exception>
        public static implicit operator long(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Int64));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (long) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (long) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).Int64;
                }
            }

            return 0L;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to an 8-bit unsigned integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 8-bit unsigned integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to an 8-bit unsigned integer.
        /// </exception>
        public static implicit operator byte(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.UInt8));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (byte) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (byte) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).UInt8;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 16-bit unsigned integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 16-bit unsigned integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 16-bit unsigned integer.
        /// </exception>
        public static implicit operator ushort(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.UInt16));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (ushort) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (ushort) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).UInt16;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 32-bit unsigned integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 32-bit unsigned integer.
        /// </exception>
        public static implicit operator uint(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.UInt32));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (uint) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (uint) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).UInt32;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 64-bit unsigned integer.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 64-bit unsigned integer.
        /// </exception>
        public static implicit operator ulong(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.UInt64));
                }

                switch (variant.Type) {
                    case VariantType.Single: return (ulong) ((BaseType) variant.value).Single;
                    case VariantType.Double: return (ulong) ((BaseType) variant.value).Double;
                    default: return ((BaseType) variant.value).UInt64;
                }
            }

            return 0UL;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 32-bit
        /// floating point number.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 32-bit floating point number.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 32-bit floating
        /// point number.
        /// </exception>
        public static implicit operator float(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Single));
                }

                switch (variant.Type) {
                    case VariantType.ByteArray: 
                    case VariantType.Single: return ((BaseType) variant.value).Single;
                    case VariantType.Double: return (float) ((BaseType) variant.value).Double;
                    default: return (float) ((BaseType) variant.value).Int64;
                }
            }
            
            return 0f;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a 64-bit
        /// floating point number.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting 64-bit floating point number.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a 64-bit floating
        /// point number.
        /// </exception>
        public static implicit operator double(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (!variant.IsBaseType && variant.Type != VariantType.ByteArray) {
                    throw new InvalidCastException(String.Format(InvalidCastMessage, variant.Type, VariantType.Double));
                }

                switch (variant.Type) {
                    case VariantType.ByteArray:
                    case VariantType.Double: return ((BaseType) variant.value).Double;
                    case VariantType.Single: return (double) ((BaseType) variant.value).Single;
                    default: return (double) ((BaseType) variant.value).Int64;
                }
            }
            
            return 0d;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a Boolean
        /// value.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting Boolean value.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a Boolean value.
        /// </exception>
        public static implicit operator bool(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (variant.Type != VariantType.ByteArray && !variant.IsBaseType) {
                    throw new InvalidCastException(
                        String.Format(InvalidCastMessage, variant.Type, VariantType.Boolean));
                }

                return variant.value[0] != 0 ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a character
        /// string.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting character string.</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="variant"/> cannot be converted to a character string.
        /// </exception>
        public static implicit operator string(Variant variant)
        {
            if (variant != null && variant.Size > 0) {
                if (variant.IsBaseType) {
                    throw new InvalidCastException(
                        String.Format(InvalidCastMessage, variant.Type, VariantType.String));
                }

                return Encoding.UTF8.GetString(variant.value, 0, variant.value.Length);
            }

            return null;
        }

        /// <summary>
        /// Converts the specified <see cref="Variant"/> implicitly to a byte
        /// array.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to convert.</param>
        /// <returns>The resulting byte array.</returns>
        public static implicit operator byte[](Variant variant)
        {
            return variant != null ? variant.value : null;
        }

        /// <summary>
        /// Converts the specified Boolean value implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The Boolean value to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(bool value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 8-bit integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 8-bit integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(sbyte value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 16-bit integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 16-bit integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(short value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 32-bit integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 32-bit integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(int value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 64-bit integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 64-bit integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(long value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 8-bit unsigned integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 8-bit unsigned integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(byte value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 16-bit unsigned integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(ushort value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 32-bit unsigned integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(uint value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 64-bit unsigned integer implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(ulong value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 32-bit floating point number implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 32-bit floating point number to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(float value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified 64-bit floating point number implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The 64-bit floating point number to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(double value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified character string implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The character string to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(string value)
        {
            return new Variant(value);
        }

        /// <summary>
        /// Converts the specified byte array implicitly to a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">The byte array to convert.</param>
        /// <returns>The resulting <see cref="Variant"/>.</returns>
        public static implicit operator Variant(byte[] value)
        {
            return new Variant(value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Variant"/> classes for equality.
        /// </summary>
        /// <param name="variant1">The first <see cref="Variant"/> to compare.</param>
        /// <param name="variant2">The second <see cref="Variant"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both value and type of <paramref name="variant1"/>
        /// and <paramref name="variant2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Variant variant1, Variant variant2)
        {
            if ((variant1 as object) == (variant2 as object)) { return true; }
            if ((variant1 as object) == null || (variant2 as object) == null) { return false; }
            if (variant1.Type != variant2.Type) { return false; }

            return BufferUtility.Equals(variant1.value, variant2.value);
        }

        /// <summary>
        /// Compares the specified <see cref="Variant"/> with this <see cref="Variant"/>
        /// for equality.
        /// </summary>
        /// <param name="variant">The <see cref="Variant"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Variant"/> contains
        /// the same value and type as this <see cref="Variant"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public virtual bool Equals(Variant variant)
        {
            return Equals(this, variant);
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Variant"/> and
        /// whether it contains the same value and type as this <see cref="Variant"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Variant"/>
        /// and contains the same value and type as this <see cref="Variant"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Variant);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Variant"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Variant"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
