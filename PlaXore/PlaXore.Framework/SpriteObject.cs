#region Header
//+ <source name="SpriteObject.cs" language="C#" begin="25-Mar-2012">
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
using System.Runtime.Serialization;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Provides functionality for dealing with sprite objects.
    /// </summary>
    /// <remarks>
    /// A sprite is a texture mapped to a 2D surface, typically used for animation.
    /// </remarks>
    [DataContract]
    public class SpriteObject : GameObject
    {
        #region Fields
        private Texture2D texture;
        private Rectangle sourceRectangle;
        private string textureName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteObject"/>.
        /// </summary>
        public SpriteObject()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteObject"/> class with
        /// the specified <see cref="GameHost"/>.
        /// </summary>
        /// <param name="gameHost">
        /// The <see cref="GameHost"/> associated with this <see cref="SpriteObject"/>.
        /// </param>
        public SpriteObject(GameHost gameHost)
            : base(gameHost)
        {
            ScaleX = 1;
            ScaleY = 1;
            Color = Color.White;
            AutoHitTestMode = AutoHitTestMode.Rectangle;
            AutoSourceRectangle = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteObject"/> class with
        /// the specified <see cref="GameHost"/> and position.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="SpriteObject"/> position.</param>
        public SpriteObject(GameHost gameHost, Vector2 position)
            : this(gameHost)
        {
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteObject"/> class with
        /// the specified <see cref="GameHost"/>, position, and source rectangle.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="SpriteObject"/> position.</param>
        /// <param name="sourceRectangle">The <see cref="SpriteObject"/> source rectangle.</param>
        public SpriteObject(GameHost gameHost, Vector2 position, Rectangle sourceRectangle)
            : this(gameHost, position)
        {
            SourceRectangle = sourceRectangle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteObject"/> class with
        /// the specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="SpriteObject"/> position.</param>
        /// <param name="texture">The <see cref="SpriteObject"/> texture.</param>
        public SpriteObject(GameHost gameHost, Vector2 position, Texture2D texture)
            : this(gameHost, position)
        {
            Texture = texture;
        }
        #endregion

        #region Properties
        [DataMember]
        public virtual AutoHitTestMode AutoHitTestMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not <see cref="SourceRectangle"/> is
        /// determined automatically.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see cref="SourceRectangle"/> is determined
        /// automatically; otherwise, <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// </remarks>
        public virtual bool AutoSourceRectangle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the texture of this <see cref="SprintObject"/>.
        /// </summary>
        /// <value>The texture of this <see cref="SprintObject"/>.</value>
        public virtual Texture2D Texture
        {
            get {
                if (texture == null && !String.IsNullOrEmpty(textureName) && GameHost != null) {
                    texture = GameHost.Textures[textureName];
                }

                return texture;
            }
            set {
                if (texture != value) {
                    if (AutoSourceRectangle) {
                        sourceRectangle = new Rectangle((int) PositionX, (int) PositionY, value.Width, value.Height);
                    }

                    texture = value;
                    textureName = GameHost.GetContentObjectName<Texture2D>(GameHost.Textures, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the texture associated with this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>
        /// The name of the texture associated with this <see cref="SpriteObject"/>.
        /// </value>
        [DataMember]
        public virtual string TextureName
        {
            get { return textureName; }
            set {
                if (textureName != value) {
                    textureName = value;
                    texture = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="SpriteObject"/> along the x-axis.
        /// </summary>
        /// <value>
        /// The position of this <see cref="SpriteObject"/> along the x-axis.
        /// </value>
        [DataMember]
        public virtual float PositionX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="SpriteObject"/> along the y-axis.
        /// </summary>
        /// <value>
        /// The position of this <see cref="SpriteObject"/> along the y-axis.
        /// </value>
        [DataMember]
        public virtual float PositionY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the origin of this <see cref="SpriteObject"/> along the x-axis.
        /// </summary>
        /// <value>
        /// The origin of this <see cref="SpriteObject"/> along the x-axis.
        /// </value>
        [DataMember]
        public virtual float OriginX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the origin of this <see cref="SpriteObject"/> along the y-axis.
        /// </summary>
        /// <value>
        /// The origin of this <see cref="SpriteObject"/> along the y-axis.
        /// </value>
        [DataMember]
        public virtual float OriginY
        {
            get;
            set;
        }

        [DataMember]
        public virtual float Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="SpriteObject"/> along the x-axis.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="SpriteObject"/> along the x-axis, in percent of the
        /// original width.
        /// </value>
        [DataMember]
        public virtual float ScaleX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="SpriteObject"/> along the y-axis.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="SpriteObject"/> along the y-axis, in percent of the
        /// original height.
        /// </value>
        [DataMember]
        public virtual float ScaleY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source rectangle of this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>
        /// The source rectangle of this <see cref="SpriteObject"/>.
        /// </value>
        [DataMember]
        public virtual Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set {
                AutoSourceRectangle = value.IsEmpty ? true : false;
                sourceRectangle = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>The color of this <see cref="SpriteObject"/>.</value>
        [DataMember]
        public virtual Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the layer depth of this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>The layer depth of this <see cref="SpriteObject"/>.</value>
        [DataMember]
        public virtual float LayerDepth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>
        /// The position of this <see cref="SpriteObject"/>.
        /// </value>
        public virtual Vector2 Position
        {
            get { return new Vector2(PositionX, PositionY); }
            set {
                PositionX = value.X;
                PositionY = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the origin of this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>
        /// The origin of this <see cref="SpriteObject"/>.
        /// </value>
        public virtual Vector2 Origin
        {
            get { return new Vector2(OriginX, OriginY); }
            set {
                OriginX = value.X;
                OriginY = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="SpriteObject"/>.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="SpriteObject"/>, in percent of the
        /// original width and height.
        /// </value>
        public virtual Vector2 Scale
        {
            get { return new Vector2(ScaleX, ScaleY); }
            set {
                ScaleX = value.X;
                ScaleY = value.Y;
            }
        }

        public virtual Rectangle BoundingBox
        {
            get {
                Rectangle result = new Rectangle(
                    (int) PositionX, (int) PositionY,
                    (int) (SourceRectangle.Width * ScaleX), (int) (SourceRectangle.Height * ScaleY));

                result.Offset((int) (-OriginX * ScaleX), (int) (-OriginY * ScaleY));

                return result;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws this <see cref="SpriteObject"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Draw</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            if (Texture != null) {
                Rectangle? sourceRectangle = null;
                if (!AutoSourceRectangle) { sourceRectangle = SourceRectangle; }
                spriteBatch.Draw(Texture, Position, sourceRectangle, Color, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
            }
        }

        public override bool IsPointInObject(Vector2 point)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            switch (AutoHitTestMode) {
                case AutoHitTestMode.Rectangle:
                    return IsPointInRectangle(point);
                case AutoHitTestMode.Ellipse:
                    return IsPointInEllipse(point);
                default:
                    return false;
            }
        }

        protected bool IsPointInRectangle(Vector2 point)
        {
            Rectangle boundingBox = BoundingBox;

            if (Angle == 0) {
                return boundingBox.Contains((int) point.X, (int) point.Y);
            }

            float width = boundingBox.Width;
            float height = boundingBox.Height;
            point -= Position;

            Vector2 rotatedPoint = Vector2.Zero;
            rotatedPoint.X = (float) (Math.Cos(-Angle) * point.X - Math.Sin(-Angle) * point.Y);
            rotatedPoint.Y = (float) (Math.Sin(-Angle) * point.X + Math.Cos(-Angle) * point.Y);

            boundingBox.Offset((int) -PositionX, (int) -PositionY);

            return boundingBox.Contains((int) rotatedPoint.X, (int) rotatedPoint.Y);
        }

        protected bool IsPointInEllipse(Microsoft.Xna.Framework.Vector2 point)
        {
            Rectangle boundingBox = BoundingBox;
            point -= Position;

            Vector2 rotatedPoint = Vector2.Zero;
            rotatedPoint.X = (float) (Math.Cos(-Angle) * point.X - Math.Sin(-Angle) * point.Y);
            rotatedPoint.Y = (float) (Math.Sin(-Angle) * point.X + Math.Cos(-Angle) * point.Y);
            rotatedPoint += Origin * Scale;
            rotatedPoint -= new Vector2(boundingBox.Width / 2, boundingBox.Height / 2);
            rotatedPoint /= new Vector2(boundingBox.Width, boundingBox.Height);

            return (rotatedPoint.Length() <= 0.5f);
        }
        #endregion
    }
}
