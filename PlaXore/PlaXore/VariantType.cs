#region Header
//+ <source name="VariantType.cs" language="C#" begin="21-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore
{
    /// <summary>
    /// Defines the data type of a <see cref="Variant"/>.
    /// </summary>
    /// <seealso cref="Variant"/>
    public enum VariantType
    {
        /// <summary>
        /// The <see cref="Variant"/> contains undefined data.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The <see cref="Variant"/> contains a Boolean value.
        /// </summary>
        Boolean = 1,

        /// <summary>
        /// The <see cref="Variant"/> contains an 8-bit integer.
        /// </summary>
        Int8 = 2,

        /// <summary>
        /// The <see cref="Variant"/> contains a 16-bit integer.
        /// </summary>
        Int16 = 3,

        /// <summary>
        /// The <see cref="Variant"/> contains a 32-bit integer.
        /// </summary>
        Int32 = 4,

        /// <summary>
        /// The <see cref="Variant"/> contains a 64-bit integer.
        /// </summary>
        Int64 = 5,

        /// <summary>
        /// The <see cref="Variant"/> contains an 8-bit unsigned integer.
        /// </summary>
        UInt8 = 6,

        /// <summary>
        /// The <see cref="Variant"/> contains a 16-bit unsigned integer.
        /// </summary>
        UInt16 = 7,

        /// <summary>
        /// The <see cref="Variant"/> contains a 32-bit unsigned integer.
        /// </summary>
        UInt32 = 8,

        /// <summary>
        /// The <see cref="Variant"/> contains a 64-bit unsigned integer.
        /// </summary>
        UInt64 = 9,

        /// <summary>
        /// The <see cref="Variant"/> contains a 32-bit floating point number.
        /// </summary>
        Single = 10,

        /// <summary>
        /// The <see cref="Variant"/> contains a 64-bit floating point number.
        /// </summary>
        Double = 11,

        /// <summary>
        /// The <see cref="Variant"/> contains an utf-8 character string.
        /// </summary>
        String = 12,

        /// <summary>
        /// The <see cref="Variant"/> contains a raw byte array.
        /// </summary>
        ByteArray = 13
    }
}
