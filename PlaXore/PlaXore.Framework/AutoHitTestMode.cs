#region Header
//+ <source name="AutoHitTestMode.cs" language="C#" begin="25-Mar-2012">
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
    /// Specifies the auto-hide test mode.
    /// </summary>
    public enum AutoHitTestMode
    {
        /// <summary>
        /// The auto-hide test is performed on a rectangle.
        /// </summary>
        Rectangle,

        /// <summary>
        /// The auto-hide test is performed on an ellipse.
        /// </summary>
        Ellipse
    };
}
