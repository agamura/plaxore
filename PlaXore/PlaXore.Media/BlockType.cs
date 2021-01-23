#region Header
//+ <source name="BlockType.cs" language="C#" begin="24-Aug-2013">
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
    /// Defines block type values.
    /// </summary>
    /// <seealso cref="BlockHeader"/>
    internal enum BlockType : byte
    {
        /// <summary>
        /// The block is undefined.
        /// </summary>
        Undefined = 0x00,

        /// <summary>
        /// The block contains global metadata.
        /// </summary>
        GlobalMetadata = 0x01,

        /// <summary>
        /// The block contains frame metadata.
        /// </summary>
        FrameMetadata = 0x02,

        /// <summary>
        /// The block contains audio metadata.
        /// </summary>
        AudioMetadata = 0x03,

        /// <summary>
        /// The block contains a frame.
        /// </summary>
        Frame = 0x04,

        /// <summary>
        /// The block contains audio.
        /// </summary>
        Audio = 0x05
    }
}
