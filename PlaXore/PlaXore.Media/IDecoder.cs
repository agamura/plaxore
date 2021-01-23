#region Header
//+ <source name="IDecoder.cs" language="C#" begin="24-Aug-2013">
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
    /// Represents a delegate that is called when a track is available for playback.
    /// </summary>
    /// <param name="frames">The available frames.</param>
    /// <param name="audioBlock">The available audio block.</param>
    /// <returns>
    /// <see langword="true"/> to continue decoding the current media stream; otherwise, 
    /// <see langword="false"/>, in which case <see cref="IDecoder.Decode(TrackAvailable)"/>
    /// has to be invoked again to resume decoding.
    /// </returns>
    /// <remarks>
    /// <paramref name="audioBlock"/> might be <see langword="null"/> if the media
    /// stream does not contain any audio.
    /// </remarks>
    public delegate bool TrackAvailable(byte[][] frames, byte[] audioBlock);

    /// <summary>
    /// Exposes methods for decoding media streams.
    /// </summary>
    public interface IDecoder : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets information that describes this <see cref="IDecoder"/>.
        /// </summary>
        /// <value>Information that describes this <see cref="IDecoder"/>.</value>
        CodecInfo CodecInfo
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="ContentAttributeCollection"/> that describes the content
        /// decoded by this <see cref="IDecoder"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ContentAttributeCollection"/> that describes the content
        /// decoded by this <see cref="IDecoder"/>.
        /// </value>
        ContentAttributeCollection ContentAttributes
        {
            get;
        }

        /// <summary>
        /// Gets the input stream of this <see cref="IDecoder"/>.
        /// </summary>
        /// <value>The input stream of this <see cref="IDecoder"/>.</value>
        Stream Input
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Decodes <see cref="Input"/> and invokes <paramref name="trackAvailable"/>
        /// when a track is available for playback.
        /// </summary>
        /// <param name="trackAvailable">The delegate that handles available tracks.</param>
        /// <returns>
        /// <see langword="true"/> if decoding is complete and there are no tracks left;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The term <b>track</b> refers to the frames and audio block that make up
        /// one second of playback.
        /// </remarks>
        /// <seealso cref="TrackAvailable"/>
        bool Decode(TrackAvailable trackAvailable);
        #endregion
    }
}
