#region Header
//+ <source name="TextObject.cs" language="C#" begin="25-Mar-2012">
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
#endregion

namespace PlaXore.GameFramework
{
    [System.Runtime.Serialization.DataContract]
    public class TextObject : SpriteObject
    {
        #region Fields
        private string text;
        private SpriteFont font;
        private string fontName;
        private TextAlignment horizontalAlignment = TextAlignment.Manual;
        private TextAlignment verticalAlignment = TextAlignment.Manual;
        #endregion

        #region Constructors
        public TextObject()
            : base()
        {
        }

        public TextObject(GameHost gameHost)
            : base(gameHost)
        {
            ScaleX = 1;
            ScaleY = 1;
            Color = Color.White;
        }

        public TextObject(GameHost gameHost, SpriteFont font)
            : this(gameHost)
        {
            Font = font;
        }

        public TextObject(GameHost gameHost, SpriteFont font, Vector2 position)
            : this(gameHost, font)
        {
            PositionX = position.X;
            PositionY = position.Y;
        }

        public TextObject(GameHost gameHost, SpriteFont font, Vector2 position, String text)
            : this(gameHost, font, position)
        {
            Text = text;
        }

        public TextObject(GameHost gameHost, SpriteFont font, Vector2 position, String text, TextAlignment horizontalAlignment, TextAlignment verticalAlignment)
            : this(gameHost, font, position, text)
        {
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
        }
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get {
                if (font == null && !String.IsNullOrEmpty(fontName) && GameHost != null) {
                    font = GameHost.Fonts[fontName];
                }

                return font;
            }
            set {
                if (font != value) {
                    font = value;
                    CalculateAlignmentOrigin();
                    fontName = GameHost.GetContentObjectName<SpriteFont>(GameHost.Fonts, value);
                }
            }
        }

        [System.Runtime.Serialization.DataMember]
        public virtual string FontName
        {
            get { return fontName; }
            set {
                if (fontName != value) {
                    fontName = value;
                    font = null;
                }
            }
        }

        [System.Runtime.Serialization.DataMember]
        public String Text
        {
            get { return text; }
            set {
                if (text != value) {
                    text = value;
                    CalculateAlignmentOrigin();
                }
            }
        }

        [System.Runtime.Serialization.DataMember]
        public TextAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set {
                if (horizontalAlignment != value) {
                    horizontalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }

        [System.Runtime.Serialization.DataMember]
        public TextAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set {
                if (verticalAlignment != value) {
                    verticalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }

        public override Rectangle BoundingBox
        {
            get {
                Vector2 size = Font.MeasureString(Text);
                Rectangle result = new Rectangle((int) PositionX, (int) PositionY, (int) (size.X * ScaleX), (int) (size.Y * ScaleY));
                result.Offset((int) (-OriginX * ScaleX), (int) (-OriginY * ScaleY));

                return result;
            }
        }
        #endregion

        #region Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            if (Font != null && Text != null && Text.Length > 0) {
                spriteBatch.DrawString(Font, Text, Position, Color, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
            }
        }

        private void CalculateAlignmentOrigin()
        {
            if (HorizontalAlignment == TextAlignment.Manual && VerticalAlignment == TextAlignment.Manual) {
                return;
            }

            if (Font == null || Text == null || Text.Length == 0) {
                return;
            }

            Vector2 size = Font.MeasureString(Text);

            switch (HorizontalAlignment) {
                case TextAlignment.Near:
                    OriginX = 0;
                    break;
                case TextAlignment.Center:
                    OriginX = size.X / 2;
                    break;
                case TextAlignment.Far:
                    OriginX = size.X;
                    break;
            }

            switch (VerticalAlignment) {
                case TextAlignment.Near:
                    OriginY = 0;
                    break;
                case TextAlignment.Center:
                    OriginY = size.Y / 2;
                    break;
                case TextAlignment.Far:
                    OriginY = size.Y;
                    break;
            }
        }
        #endregion
    }
}
