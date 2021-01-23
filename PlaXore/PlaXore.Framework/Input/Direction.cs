#region Header
//+ <source name="Direction.cs" language="C#" begin="1-Apr-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ //+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.GameFramework.Input
{
    /// <summary>
    /// Specifies the direction of a gesture.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// The gesture was from right to left.
        /// </summary>
        Left,

        /// <summary>
        /// The gesture was from left to right.
        /// </summary>
        Right,

        /// <summary>
        /// The gesture was from bottom to up.
        /// </summary>
        Up,

        /// <summary>
        /// The gesture was from up to bottom.
        /// </summary>
        Down,

        /// <summary>
        /// There was no gesture.
        /// </summary>
        None
    }
}
