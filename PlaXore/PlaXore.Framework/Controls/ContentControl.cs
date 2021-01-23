#region Header
//+ <source name="ContentControl.cs" language="C#" begin="10-May-2012">
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
using System;
using System.Text;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents a control with content of any type.
    /// </summary>
    public class ContentControl : Control
    {
        #region Fields
        private const string TextSample = "ip";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class
        /// with the specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="ContentControl"/> position.</param>
        /// <param name="texture">The <see cref="ContentControl"/> texture.</param>
        public ContentControl(GameHost gameHost, Vector2 position, Texture2D texture)
            : base(gameHost, position, texture)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControl"/> class
        /// with the specified <see cref="GameHost"/>, position, and source rectangle.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="ContentControl"/> position.</param>
        /// <param name="sourceRectangle">The <see cref="ContentControl"/> source rectangle.</param>
        public ContentControl(GameHost gameHost, Vector2 position, Rectangle sourceRectangle)
            : base(gameHost, position, sourceRectangle)
        {
            Initialize();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the outer margin of this <see cref="ContentControl"/>.
        /// </summary>
        /// <value>The outer margin of this <see cref="ContentControl"/>.</value>
        public virtual Thickness Margin
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Arranges the specified text in the specified area, according to specified
        /// font and alignments.
        /// </summary>
        /// <param name="text">The text to arrange.</param>
        /// <param name="area">The area where to arrange <paramref name="text"/>.</param>
        /// <param name="font">The text font.</param>
        /// <param name="color">The text color.</param>
        /// <param name="horizontalAlignment">One of the <see cref="TextAlignment"/> values.</param>
        /// <param name="verticalAlignment">One of the <see cref="TextAlignment"/> values.</param>
        /// <returns>
        /// An array of <see cref="TextObject"/> instances that contains the arranged text.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ContentControl"/> has already been disposed of.
        /// </exception>
        public TextObject[] ArrangeText(
            string text, Rectangle area, SpriteFont font, Color color,
            TextAlignment horizontalAlignment, TextAlignment verticalAlignment)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            if (area == Rectangle.Empty) {
                area.Width = BoundingBox.Width - Margin.Left - Margin.Right;
                area.Height = BoundingBox.Height - Margin.Top - Margin.Bottom;
                area.X = BoundingBox.X + Margin.Left;
                area.Y = BoundingBox.Y + Margin.Top;
            } else {
                area.Width = area.Width == 0
                    ? BoundingBox.Width - Margin.Left - Margin.Right
                    : Math.Min(area.Width, BoundingBox.Width - Margin.Left - Margin.Right);
                area.Height = area.Height == 0
                    ? BoundingBox.Height - Margin.Top - Margin.Bottom
                    : Math.Min(area.Height, BoundingBox.Height - Margin.Top - Margin.Bottom);
                area.X = Math.Max(area.X, BoundingBox.X + Margin.Left);
                area.Y = Math.Max(area.Y, BoundingBox.Y + Margin.Top);
            }

            Vector2 position = Vector2.Zero;
            StringBuilder stringBuilder = new StringBuilder();
            string[] words = text.Split(' ');
            float textWidth = 0f;

            for (int i = 0; i < words.Length; i++) {
                if (i < words.Length - 1) { words[i] = words[i] + " "; }
                position = font.MeasureString(words[i]);

                if (textWidth + position.X > area.Width) {
                    textWidth = position.X;
                    stringBuilder.Append(Environment.NewLine.ToCharArray()[0]);
                } else {
                    textWidth += position.X;
                }

                stringBuilder.Append(words[i]);
            }

            string[] lines = stringBuilder.ToString().Split(Environment.NewLine.ToCharArray()[0]);
            float textHeight = GetMaxTextHeight(font) * .9f, areaHeight = 0f;
            int lineCount = lines.Length;

            while ((areaHeight = textHeight * lineCount) > area.Height) { lineCount--; }

            float currentY = textHeight / 2;
            switch (verticalAlignment) {
                case TextAlignment.Far:
                    currentY += area.Y + (area.Height - areaHeight);
                    break;
                case TextAlignment.Near:
                    currentY += area.Y;
                    break;
                default:
                    currentY += area.Y + ((area.Height - areaHeight) / 2f);
                    break;
            }

            TextObject[] textObjects = new TextObject[lineCount];

            for (int i = 0; i < textObjects.Length; i++) {
                switch (horizontalAlignment) {
                    case TextAlignment.Near:
                        position.X = area.Left;
                        break;
                    case TextAlignment.Far:
                        position.X = area.Right;
                        break;
                    default:
                        position.X = area.Left + (area.Width / 2);
                        break;
                }

                position.Y = currentY;
                textObjects[i] = new TextObject(GameHost, font, position, lines[i], horizontalAlignment, TextAlignment.Center);
                textObjects[i].Color = color;
                currentY += textHeight;
            }

            return textObjects;
        }

        /// <summary>
        /// Gets the maximum text height for the specified font.
        /// </summary>
        /// <param name="font">The font to get the maximum height for.</param>
        /// <returns>The maximum text height for <paramref name="font"/>.</returns>
        public static float GetMaxTextHeight(SpriteFont font)
        {
            return font.MeasureString(TextSample).Y;
        }

        /// <summary>
        /// Initializes this <see cref="ContentControl"/>.
        /// </summary>
        private void Initialize()
        {
            Margin = new Thickness(0);
        }
        #endregion
    }
}
