#region Header
//+ <source name="Window.cs" language="C#" begin="3-May-2012">
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

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Provides base functionality for custom windows.
    /// </summary>
    public class Window : ContentControl
    {
        #region Fields
        private const float DefaultLayerDepth = 0.01f;
        private const int DefaultThickness = 25;
        private const int DefaultMaxDarkenAlpha = 30;

        private Texture2D blank;
        private Rectangle[] darkenPanels;
        private int darkenAlpha;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when this <see cref="Window"/> is displayed.
        /// </summary>
        public event EventHandler<EventArgs> Shown;

        /// <summary>
        /// Occurs when this <see cref="Window"/> is hidden.
        /// </summary>
        public event EventHandler<EventArgs> Hidden;

        /// <summary>
        /// Occurs when this <see cref="Window"/> is closed.
        /// </summary>
        public event EventHandler<EventArgs> Closed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class with the
        /// specified <see cref="GameHost"/> and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="texture">The <see cref="Window"/> texture.</param>
        /// <remarks>
        /// This constructor automatically centers the <see cref="Window"/> in
        /// the main game window.
        /// </remarks>
        public Window(GameHost gameHost, Texture2D texture)
            : this(gameHost, new Vector2(-1, -1), texture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class with the
        /// specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Window"/> position.</param>
        /// <param name="texture">The <see cref="Window"/> texture.</param>
        public Window(GameHost gameHost, Vector2 position, Texture2D texture)
            : base(gameHost, position, texture)
        {
            LayerDepth = DefaultLayerDepth;
            Margin = new Thickness(DefaultThickness);
            MaxDarkenAlpha = DefaultMaxDarkenAlpha;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the maximum darken alpha applied to the back buffer
        /// when this <see cref="Window"/> is shown.
        /// </summary>
        /// <value>The maximum darken alpha.</value>
        public virtual int MaxDarkenAlpha
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a blank bitmap to be used to darken the back buffer.
        /// </summary>
        /// <value>A blank bitmap.</value>
        private Texture2D Blank
        {
            get {
                if (blank == null) {
                    uint[] bitmap = new uint[] {
                        0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00,
                        0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00,
                        0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00,
                        0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00, 0xFFFFFF00
                    };

                    blank = new Texture2D(
                        GameHost.GraphicsDevice,
                        (int) Math.Sqrt(bitmap.Length),
                        (int) Math.Sqrt(bitmap.Length),
                        false, SurfaceFormat.Color);

                    blank.SetData<uint>(bitmap, 0, bitmap.Length);
                }

                return blank;
            }
        }

        /// <summary>
        /// Gets the panels to be used to darken the back buffer behind this
        /// <see cref="Window"/>.
        /// </summary>
        /// <value>
        /// The panels to be used to darken the back buffer behind this <see cref="Window"/>.
        /// </value>
        private Rectangle[] DarkenPanels
        {
            get {
                if (BackBufferRectangle != Rectangle.Empty) {
                    if (darkenPanels == null) {
                        darkenPanels = new Rectangle[] {
                        new Rectangle(BackBufferRectangle.Left, BackBufferRectangle.Top,
                            BackBufferRectangle.Width, BoundingBox.Top - BackBufferRectangle.Top),
                        new Rectangle(BackBufferRectangle.Left, BoundingBox.Bottom,
                            BackBufferRectangle.Width, BackBufferRectangle.Bottom - BoundingBox.Bottom),
                        new Rectangle(BackBufferRectangle.Left, BoundingBox.Top,
                            BoundingBox.Left - BackBufferRectangle.Left, BoundingBox.Height),
                        new Rectangle(BoundingBox.Right, BoundingBox.Top,
                            BackBufferRectangle.Right - BoundingBox.Right, BoundingBox.Height)
                        };
                    }
                }

                return darkenPanels;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="Window"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                if (disposing) {
                    // Release managed resources
                    if (blank != null) { blank.Dispose(); }
                }

                // Release unmanaged resources
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Draws this <see cref="Window"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Draw</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Window"/> has already been disposed of.
        /// </exception>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DarkenBackBuffer(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Opens this <see cref="Window"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Window"/> has already been disposed of.
        /// </exception>
        public override void Show()
        {
            if (!IsVisible) {
                darkenAlpha = 0;
                OnShown(new EventArgs());
                base.Show();
            }
        }

        /// <summary>
        /// Hides this <see cref="Window"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Window"/> has already been disposed of.
        /// </exception>
        public override void Hide()
        {
            if (IsVisible) {
                OnHidden(new EventArgs());
                base.Hide();
            }
        }

        /// <summary>
        /// Closes this <see cref="Window"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Window"/> has already been disposed of.
        /// </exception>
        public virtual void Close()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            OnClosed(new EventArgs());
            Dispose();
        }

        /// <summary>
        /// Raises the <see cref="Window.Shown"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Window.OnShown"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnShown(EventArgs args)
        {
            if (Shown != null) {
                Shown(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="Window.Hidden"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Window.OnHidden"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnHidden(EventArgs args)
        {
            if (Hidden != null) {
                Hidden(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="Window.Closed"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Window.OnClosed"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnClosed(EventArgs args)
        {
            if (Closed != null) {
                Closed(this, args);
            }
        }

        /// <summary>
        /// Gradually darkens the back buffer behind this <see cref="Window"/>.
        /// </summary>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        private void DarkenBackBuffer(SpriteBatch spriteBatch)
        {
            if (DarkenPanels != null) {
                if (darkenAlpha <= DefaultMaxDarkenAlpha) {
                    darkenAlpha++;
                }

                foreach (Rectangle panel in DarkenPanels) {
                    spriteBatch.Draw(Blank, panel, Color.Black * ((float) darkenAlpha / 100f));
                }
            }
        }
        #endregion
    }
}
