#region Header
//+ <source name="ContentAttributeId.cs" language="C#" begin="28-Aug-2013">
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
    /// Defines <see cref="ContentAttribute"/> id values.
    /// </summary>
    /// <seealso cref="ContentAttribute"/>
    public enum ContentAttributeId
    {
        /// <summary>
        /// The <see cref="ContentAttribute"/> specified the globally unique identifier of the content.
        /// </summary>
        Guid,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the codec used to encode or decode the content.
        /// </summary>
        Codec,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the codec version used to encode or decode the content.
        /// </summary>
        CodecVersion,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the format of the frames.
        /// </summary>
        FrameFormat,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the width of the frames.
        /// </summary>
        FrameWidth,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the height of the frames.
        /// </summary>
        FrameHeight,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the compression level of the frames.
        /// </summary>
        FrameQuality,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the number of frames per second.
        /// </summary>
        FrameRate,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the total number of frames.
        /// </summary>
        FrameCount,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the format of the audio.
        /// </summary>
        AudioFormat,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the total number of audio blocks.
        /// </summary>
        AudioBlockCount,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the number of audio channels.
        /// </summary>
        AudioChannels,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the number of audio samples per second.
        /// </summary>
        AudioSampleRate,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the content duration, in milliseconds.
        /// </summary>
        Duration,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the content title.
        /// </summary>
        Title,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the content owner.
        /// </summary>
        Copyright,

        /// <summary>
        /// The <see cref="ContentAttribute"/> specifies the compilation time of the content.
        /// </summary>
        Timestamp
    }
}
