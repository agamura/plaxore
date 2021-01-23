#region Header
//+ <source name="DockStyle.cs" language="C#" begin="14-Apr-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Specifies the position in which a <see cref="Control"/> is docked to a
    /// docking area.
    /// </summary>
    public enum DockStyle
    {
        /// <summary>
        /// The <see cref="Control"/> is not docked.
        /// </summary>
        None = 0,

        /// <summary>
        /// All the edges of the <see cref="Control"/> are docked to the all edges
        /// of the docking area and sized appropriately.
        /// </summary>
        Fill,

        /// <summary>
        /// The top edge of the <see cref="Control"/> is docked to the top of
        /// the docking area.
        /// </summary>
        Top,

        /// <summary>
        /// The bottom edge of the <see cref="Control"/> is docked to the bottom
        /// of the docking area.
        /// </summary>
        Bottom,

        /// <summary>
        /// The left edge of the <see cref="Control"/> is docked to the left edge
        /// of the docking area.
        /// </summary>
        Left,

        /// <summary>
        /// The right edge of the <see cref="Control"/> is docked to the right edge
        /// of the docking area.
        /// </summary>
        Right
    }
}
