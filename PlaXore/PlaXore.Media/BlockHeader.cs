#region Header
//+ <source name="BlockHeader.cs" language="C#" begin="24-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Runtime.InteropServices;
using PlaXore.Media;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Defines a structure for describing content blocks.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockHeader
    {
        #region Fields
        private const uint BlockTypeMask = 0x000000FF;
        private const uint BlockSizeMask = 0xFFFFFF00;

        private uint value;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the block type.
        /// </summary>
        /// <value>One of the <see cref="BlockType"/> values.</value>
        public BlockType BlockType
        {
            get { return (BlockType) (value & BlockTypeMask); }
            set { this.value |= (uint) value & BlockTypeMask; }
        }

        /// <summary>
        /// Gets or sets the block size.
        /// </summary>
        /// <value>The block size, in bytes.</value>
        public uint BlockSize
        {
            get { return (value & BlockSizeMask) >> 8; }
            set { this.value |= value << 8 & BlockSizeMask; }
        }

        /// <summary>
        /// Gets the raw block header.
        /// </summary>
        /// <value>
        /// The lower 8 bits represent the block type while the higher 24 bits
        /// represent the block size.
        /// </value>
        public uint Value
        {
            get { return this.value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockHeader"/> structure
        /// with the specified block type and size.
        /// </summary>
        /// <param name="blockType">One of the <see cref="BlockType"/> values.</param>
        /// <param name="blockSize">The block size.</param>
        public BlockHeader(BlockType blockType, uint blockSize)
        {
            value = (uint) blockType;
            BlockSize = blockSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockHeader"/> structure
        /// with the specified value.
        /// </summary>
        /// <param name="value">
        /// A raw block header value where the lower 8 bits represent the block type
        /// and the higher 24 bits represent the block size.
        /// </param>
        public BlockHeader(uint value)
        {
            this.value = value;
        }
        #endregion
    }
}
