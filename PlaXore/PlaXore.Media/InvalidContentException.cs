#region Header
//+ <source name="InvalidContentException.cs" language="C#" begin="13-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// The exception that is thrown when a content stream is in an invalid format.
    /// </summary>
    public class InvalidContentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidContentException"/> class.
        /// </summary>
        public InvalidContentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidContentException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidContentException(string message)
            :base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidContentException"/> class
        /// with the specified error message and inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidContentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
