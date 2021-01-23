#region Header
//+ <source name="StreamExtensions.cs" language="C#" begin="9-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.IO;
#endregion

namespace PlaXore.Extensions
{
    /// <summary>
    /// Contains extension methods for the <c>Stream</c> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts the specified <b>Stream</b> to a byte array.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> to convert to a byte array.</param>
        /// <returns>The byte array converted from <paramref name="stream"/>.</returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream is MemoryStream) {
                return (stream as MemoryStream).ToArray();
            } else {
                using (MemoryStream memoryStream = new MemoryStream()) {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
