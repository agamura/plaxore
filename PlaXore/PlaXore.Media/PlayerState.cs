#region Header
//+ <source name="PlayerState.cs" language="C#" begin="17-Sep-2013">
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
    /// Defines player state values.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The <see cref="MediaPlayer"/> is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The <see cref="MediaPlayer"/> is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The <see cref="MediaPlayer"/> is stopped.
        /// </summary>
        Stopped
    }
}
