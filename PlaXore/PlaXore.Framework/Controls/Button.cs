#region Header
//+ <source name="Button.cs" language="C#" begin="12-May-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents a button control.
    /// </summary>
    public class Button : Control
    {
        #region Fields
        private const float DefaultLayerDepth = 0.001f;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with the
        /// specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Button"/> position.</param>
        /// <param name="texture">The <see cref="Button"/> texture.</param>
        public Button(GameHost gameHost, Vector2 position, Texture2D texture)
            : base(gameHost, position, texture)
        {
            ScaleX = ScaleY = ScaleWhenReleasedX = ScaleWhenReleasedY = 0.9f;
            LayerDepth = DefaultLayerDepth;
        }
        #endregion

        #region Methods
        #endregion
    }
}
