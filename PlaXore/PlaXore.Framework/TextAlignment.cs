#region Header
//+ <source name="TextAlignment.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Specifies the alignment of a <see cref="TextObject"/> depending on orientation.
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// The <see cref="TextObject"/> is aligned manually.
        /// </summary>
        Manual,

        /// <summary>
        /// The <see cref="TextObject"/> is aligned closer to the orientation edge.
        /// </summary>
        Near,

        /// <summary>
        /// The <see cref="TextObject"/> is centered in the orientation.
        /// </summary>
        Center,

        /// <summary>
        /// The <see cref="TextObject"/> is aligned farther from the orientation edge.
        /// </summary>
        Far
    };
}
