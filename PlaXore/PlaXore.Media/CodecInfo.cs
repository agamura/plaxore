#region Header
//+ <source name="CodecInfo.cs" language="C#" begin="24-Aug-2013">
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
    /// Provides information about a codec.
    /// </summary>
    /// <remarks>
    /// The term <i>codec</i> refers to either an <see cref="IEncoder"/> or an
    /// <see cref="IDecoder"/> implementation.
    /// </remarks>
    public class CodecInfo
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CodecInfo"/> class with
        /// the specified name and version.
        /// </summary>
        /// <param name="name">The codec name.</param>
        /// <param name="version">The codec version.</param>
        internal CodecInfo(string name, SimpleVersion version)
        {
            Name = name;
            Version = version;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the codec name.
        /// </summary>
        /// <value>The codec name.</value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the codec version.
        /// </summary>
        /// <value>The codec version.</value>
        public SimpleVersion Version
        {
            get;
            private set;
        }
        #endregion
    }
}
