#region Header
//+ <source name="ShakeType.cs" language="C#" begin="23-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.ShakeGestures
{
    /// <summary>
    /// Specifies the shake gesture type.
    /// </summary>
    public enum ShakeType
    {
        /// <summary>
        /// Shake gesture on the x-axis.
        /// </summary>
        X = 0,

        /// <summary>
        /// Shake gesture on the y-axis.
        /// </summary>
        Y = 1,

        /// <summary>
        /// Shake gesture on the z-axis.
        /// </summary>
        Z = 2
    }
}
