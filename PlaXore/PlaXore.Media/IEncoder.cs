#region Header
//+ <source name="IEncoder.cs" language="C#" begin="24-Aug-2013">
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
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Represents a delegate that is used to get the next input stream to encode.
    /// </summary>
    /// <returns>
    /// The next input stream to encode, or <see langword="null"/> if there is
    /// no more input to encode.
    /// </returns>
    public delegate Stream NextInputStream();

    /// <summary>
    /// Exposes methods for encoding media streams.
    /// </summary>
    public interface IEncoder : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets information that describes this <see cref="IEncoder"/>.
        /// </summary>
        /// <value>Information that describes this <see cref="IEncoder"/>.</value>
        CodecInfo CodecInfo
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="ContentAttributeCollection"/> that describes the content
        /// encoded by this <see cref="IEncoder"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ContentAttributeCollection"/> that describes the content
        /// encoded by this <see cref="IEncoder"/>.
        /// </value>
        ContentAttributeCollection ContentAttributes
        {
            get;
        }

        /// <summary>
        /// Gets the output stream of this <see cref="IEncoder"/>.
        /// </summary>
        /// <value>The output stream of this <see cref="IEncoder"/>.</value>
        Stream Output
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Encodes the streams returned by <paramref name="nextFrame"/> and
        /// <paramref name="nextAudioBlock"/> into the specified output stream.
        /// </summary>
        /// <param name="nextFrame">The delegate that returns the next frame to encode.</param>
        /// <param name="nextAudioBlock">The delegate that returns the next audio block to encode.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nextFrame"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <paramref name="nextAudioBlock"/> might be <see langword="null"/>, in which case
        /// no audio is encoded.
        /// </remarks>
        /// <seealso cref="NextInputStream"/>
        void Encode(NextInputStream nextFrame, NextInputStream nextAudioBlock);
        #endregion
    }
}
