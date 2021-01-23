#region Header
//+ <source name="Endianness.cs" language="C#" begin="17-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Defines the byte order in which data is stored.
    /// </summary>
    internal enum Endianness
    {
        /// <summary>
        /// The most significant byte is on the left end of a word.
        /// </summary>
        BigEndian,

        /// <summary>
        /// The most significant byte is on the right end of a word.
        /// </summary>
        LittleEndian,

        /// <summary>
        /// Byte order depends on the computer architecture.
        /// </summary>
        ByteOrder
    }
}
