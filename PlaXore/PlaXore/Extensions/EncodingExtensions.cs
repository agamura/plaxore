#region Header
//+ <source name="EncodingExtensions.cs" language="C#" begin="4-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Linq;
using System.Text;
#endregion

namespace PlaXore.Extensions
{
    /// <summary>
    /// Contains extension methods for the <c>Encoding</c> class.
    /// </summary>
    public static class EncodingExtensions
    {
        #region Methods
        /// <summary>
        /// Encodes the specified string into a 16-bit unsigned integer.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The string to encode into a 16-bit unsigned integer.</param>
        /// <returns>A 16-bit unsigned integer containing the result of encoding <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> either contains non-ASCII characters or is too long.
        /// </exception>
        public static ushort GetUInt16(this Encoding encoding, string value)
        {
            return GetBaseType(value, sizeof(ushort));
        }

        /// <summary>
        /// Encodes the specified string into a 32-bit unsigned integer.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The string to encode into a 32-bit unsigned integer.</param>
        /// <returns>A 32-bit unsigned integer containing the result of encoding <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> either contains non-ASCII characters or is too long.
        /// </exception>
        public static uint GetUInt32(this Encoding encoding, string value)
        {
            return GetBaseType(value, sizeof(uint));
        }

        /// <summary>
        /// Encodes the specified string into a 64-bit unsigned integer.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The string to encode into a 64-bit unsigned integer.</param>
        /// <returns>A 64-bit unsigned integer containing the result of encoding <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> either contains non-ASCII characters or is too long.
        /// </exception>
        public static ulong GetUInt64(this Encoding encoding, string value)
        {
            return GetBaseType(value, sizeof(ulong));
        }

        /// <summary>
        /// Decodes the specified 16-bit unsigned integer into a string.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The 16-bit unsigned integer to encode into a string.</param>
        /// <returns>A string containing the result of encoding <paramref name="value"/>.</returns>
        public static string GetString(this Encoding encoding, ushort value)
        {
            return GetString(value, sizeof(ushort));
        }

        /// <summary>
        /// Decodes the specified 32-bit unsigned integer into a string.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The 32-bit unsigned integer to encode into a string.</param>
        /// <returns>A string containing the result of encoding <paramref name="value"/>.</returns>
        public static string GetString(this Encoding encoding, uint value)
        {
            return GetString(value, sizeof(uint));
        }

        /// <summary>
        /// Decodes the specified 64-bit unsigned integer into a string.
        /// </summary>
        /// <param name="encoding">The <c>Encoding</c> instance this extension method applies to.</param>
        /// <param name="value">The 64-bit unsigned integer to encode into a string.</param>
        /// <returns>A string containing the result of encoding <paramref name="value"/>.</returns>
        public static string GetString(this Encoding encoding, ulong value)
        {
            return GetString(value, sizeof(ulong));
        }

        /// <summary>
        /// Decodes the specified <see cref="BaseType"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="BaseType"/> to encode into a string.</param>
        /// <param name="length">The number of bytes in <paramref name="value"/> to decode.</param>
        /// <returns>A string containing the result of encoding <paramref name="value"/>.</returns>
        private static string GetString(BaseType value, int length)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            length *= 8;

            for (int ch = 0, shift = 0; shift < length; shift += 8) {
                if ((ch = (int) ((value.UInt64 >> shift) & 0xFFUL)) == 0x00) { break; }
                stringBuilder.Insert(0, new char[] { (char) ch });
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encodes the specified string into a <see cref="BaseType"/>.
        /// </summary>
        /// <param name="value">The string to encode into a <see cref="BaseType"/>.</param>
        /// <param name="maxLength">The maximum allowed length of <paramref name="value"/>.</param>
        /// <returns>A <see cref="BaseType"/> containing the result of encoding <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> either contains non-ASCII characters or is too long.
        /// </exception>
        private static BaseType GetBaseType(string value, int maxLength)
        {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (value.ToCharArray().Any(ch => ch > Byte.MaxValue)) {
                throw new ArgumentException("String contains non-ASCII characters.", "value");
            }

            if (value.Length > maxLength) {
                throw new ArgumentException(String.Format("String exceeds {0} characters.", maxLength), "value");
            }

            BaseType baseType = new BaseType(0);
            foreach (char c in value) { baseType.UInt64 = (baseType.UInt64 << 8) | c; }
            return baseType;
        }
        #endregion
    }
}
