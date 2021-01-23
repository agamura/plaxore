#region Header
//+ <source name="BenchmarkObject.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#if DEBUG
#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Provides functionality for benchmarking active <see cref="GameObject"/>
    /// instances. Benchmarking includes frames per second and updates per second.
    public class BenchmarkObject : TextObject
    {
        #region Fields
        private double lastUpdateMilliseconds;
        private int drawCount;
        private int lastDrawCount;
        private int lastUpdateCount;
        private StringBuilder stringBuilder;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkObject"/> class
        /// with the specified game host, font, position, and color.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="font">The font used by the <see cref="BenchmarkObject"/>.</param>
        /// <param name="position">The <see cref="BenchmarkObject"/> position.</param>
        /// <param name="color">The <see cref="BenchmarkObject"/> color.</param>
        public BenchmarkObject(GameHost gameHost, SpriteFont font, Vector2 position, Color color)
            : base(gameHost, font, position)
        {
            stringBuilder = new StringBuilder();
            Color = color;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws this <see cref="BenchmarkObject"/>
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameTime.TotalGameTime.TotalMilliseconds > lastUpdateMilliseconds + 1000) {
                int drawCount = this.drawCount - lastDrawCount;
                int updateCount = UpdateCount - lastUpdateCount;
                double elapsedTime = gameTime.TotalGameTime.TotalMilliseconds - lastUpdateMilliseconds;

                stringBuilder.Length = 0;
                stringBuilder.AppendLine("Object count: " + GameHost.GameObjects.Count.ToString());
                stringBuilder.AppendLine("Frames per second: " + ((float) drawCount / elapsedTime * 1000).ToString("0.0"));
                stringBuilder.AppendLine("Updates per second: " + ((float) updateCount / elapsedTime * 1000).ToString("0.0"));
                Text = stringBuilder.ToString();

                lastUpdateMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
                lastDrawCount = this.drawCount;
                lastUpdateCount = UpdateCount;
            }
        }

        /// <summary>
        /// Draws this <see cref="BenchmarkObject"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Draw</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            drawCount += 1;
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
#endif
