#region Header
//+ <source name="Control.cs" language="C#" begin="9-May-2012">
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
using Microsoft.Xna.Framework.Input.Touch;
using System;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents a graphical control that reacts to touch gestures.
    /// </summary>
    public class Control : SpriteObject
    {
        #region Fields
        private DockStyle dockStyle;
        private Rectangle dockRectangle;
        private Rectangle touchArea;
        private Rectangle actualSourceRectangle;
        private Vector2 actualPosition;
        private int holdAge;
        private int freeDragAge;
        private int horizontalDragAge;
        private int verticalDragAge;
        private int dragCompleteAge;
        #endregion

        #region Events
        private event EventHandler<GameInputEventArgs> _Click;
        private event EventHandler<GameInputEventArgs> _Pressed;
        private event EventHandler<GameInputEventArgs> _Released;
        private event EventHandler<GameInputEventArgs> _Flick;
        private event EventHandler<GameInputEventArgs> _Hold;
        private event EventHandler<GameInputEventArgs> _FreeDrag;
        private event EventHandler<GameInputEventArgs> _HorizontalDrag;
        private event EventHandler<GameInputEventArgs> _VerticalDrag;
        private event EventHandler<GameInputEventArgs> _DragComplete;

        /// <summary>
        /// Occurs when this <see cref="Control"/> is clicked.
        /// </summary>
        public event EventHandler<GameInputEventArgs> Click
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_Click == null && _Pressed == null && _Released == null) {
                    ClickEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddTapInput(ClickEventTag, TouchArea, false);
                }

                _Click += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _Click -= value;

                if (_Click == null && _Pressed == null && _Released == null) {
                    GameHost.GameInput.RemoveTapInput(ClickEventTag, touchArea);
                    ClickEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is pressed.
        /// </summary>
        public event EventHandler<GameInputEventArgs> Pressed
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_Click == null && _Pressed == null && _Released == null) {
                    ClickEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddTapInput(ClickEventTag, TouchArea, false);
                }

                _Pressed += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _Pressed -= value;

                if (_Click == null && _Pressed == null && _Released == null) {
                    GameHost.GameInput.RemoveTapInput(ClickEventTag, touchArea);
                    ClickEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is released.
        /// </summary>
        public event EventHandler<GameInputEventArgs> Released
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_Click == null && _Pressed == null && _Released == null) {
                    ClickEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddTapInput(ClickEventTag, TouchArea, false);
                }

                _Released += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _Released -= value;

                if (_Click == null && _Pressed == null && _Released == null) {
                    GameHost.GameInput.RemoveTapInput(ClickEventTag, touchArea);
                    ClickEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is flicked.
        /// </summary>
        public event EventHandler<GameInputEventArgs> Flick
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_Flick == null) {
                    FlickEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(FlickEventTag, GestureType.Flick, TouchArea);
                }

                _Flick += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _Flick -= value;

                if (_Flick == null) {
                    GameHost.GameInput.RemoveGestureInput(FlickEventTag, GestureType.Flick, TouchArea);
                    FlickEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is held.
        /// </summary>
        public event EventHandler<GameInputEventArgs> Hold
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_Hold == null) {
                    HoldEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(HoldEventTag, GestureType.Hold, TouchArea);
                }

                _Hold += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _Hold -= value;

                if (_Hold == null) {
                    GameHost.GameInput.RemoveGestureInput(HoldEventTag, GestureType.Hold, TouchArea);
                    HoldEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is dragged in free-form.
        /// </summary>
        public event EventHandler<GameInputEventArgs> FreeDrag
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_FreeDrag == null) {
                    FreeDragEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(FreeDragEventTag, GestureType.FreeDrag, TouchArea);
                }

                _FreeDrag += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _FreeDrag -= value;

                if (_FreeDrag == null) {
                    GameHost.GameInput.RemoveGestureInput(FreeDragEventTag, GestureType.FreeDrag, TouchArea);
                    FreeDragEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is dragged horizontally.
        /// </summary>
        public event EventHandler<GameInputEventArgs> HorizontalDrag
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_HorizontalDrag == null) {
                    HorizontalDragEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(HorizontalDragEventTag, GestureType.HorizontalDrag, TouchArea);
                }

                _HorizontalDrag += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _HorizontalDrag -= value;

                if (_HorizontalDrag == null) {
                    GameHost.GameInput.RemoveGestureInput(HorizontalDragEventTag, GestureType.HorizontalDrag, TouchArea);
                    HorizontalDragEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is dragged vertically.
        /// </summary>
        public event EventHandler<GameInputEventArgs> VerticalDrag
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_VerticalDrag == null) {
                    VerticalDragEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(VerticalDragEventTag, GestureType.VerticalDrag, TouchArea);
                }

                _VerticalDrag += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _VerticalDrag -= value;

                if (_VerticalDrag == null) {
                    GameHost.GameInput.RemoveGestureInput(VerticalDragEventTag, GestureType.VerticalDrag, TouchArea);
                    VerticalDragEventTag = null;
                }
            }
        }

        /// <summary>
        /// Occurs when this <see cref="Control"/> is no longer being dragged.
        /// </summary>
        public event EventHandler<GameInputEventArgs> DragComplete
        {
            add {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (_DragComplete == null) {
                    DragCompleteEventTag = Guid.NewGuid().ToString();
                    GameHost.GameInput.AddGestureInput(DragCompleteEventTag, GestureType.DragComplete, TouchArea);
                }

                _DragComplete += value;
            }
            remove {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                _DragComplete -= value;

                if (_DragComplete == null) {
                    GameHost.GameInput.RemoveGestureInput(DragCompleteEventTag, GestureType.DragComplete, TouchArea);
                    DragCompleteEventTag = null;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class with
        /// the specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Control"/> position.</param>
        /// <param name="texture">The <see cref="Control"/> texture.</param>
        public Control(GameHost gameHost, Vector2 position, Texture2D texture)
            : this(gameHost, position, texture, Rectangle.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class with
        /// the specified <see cref="GameHost"/>, position, texture, and touch area.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Control"/> position.</param>
        /// <param name="texture">The <see cref="Control"/> texture.</param>
        /// <param name="touchArea">The touch area associated with the <see cref="Control"/>.</param>
        public Control(GameHost gameHost, Vector2 position, Texture2D texture, Rectangle touchArea)
            : base(gameHost, position, texture)
        {
            Initialize(touchArea);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class with
        /// the specified <see cref="GameHost"/>, position, and source rectangle.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Control"/> position.</param>
        /// <param name="sourceRectangle">The <see cref="Control"/> source rectangle.</param>
        public Control(GameHost gameHost, Vector2 position, Rectangle sourceRectangle)
            : this(gameHost, position, sourceRectangle, Rectangle.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class with the
        /// specified <see cref="GameHost"/>, position, source rectangle, and touch area.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Control"/> position.</param>
        /// <param name="sourceRectangle">The <see cref="Control"/> source rectangle.</param>
        /// <param name="touchArea">The touch area associated with the <see cref="Control"/>.</param>
        public Control(GameHost gameHost, Vector2 position, Rectangle sourceRectangle, Rectangle touchArea)
            : base(gameHost, position, sourceRectangle)
        {
            Initialize(touchArea);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dimensions of the back buffer rectangle behind
        /// this <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The dimensions of the back buffer rectangle behind this
        /// <see cref="Control"/>.
        /// </value>
        public virtual Rectangle BackBufferRectangle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rectangle where this <see cref="Control"/> can be arranged
        /// either horizontally or vertically.
        /// </summary>
        /// <value>
        /// The rectangle where this <see cref="Control"/> can be arranged either
        /// horizontally or vertically.
        /// </value>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        /// <remarks>
        /// If the specified <b>Rectangle</b> contains this <see cref="Control"/>,
        /// then <see cref="DockRectangle"/> also sets <see cref="TouchArea"/> to
        /// the updated <see cref="BoundingBox"/>.
        /// </remarks>
        public virtual Rectangle DockRectangle
        {
            get { return dockRectangle; }
            set {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (value != dockRectangle && value.Contains(BoundingBox)) {
                    dockRectangle = value;
                    Arrange();
                }
            }
        }

        /// <summary>
        /// Gets or sets which borders are docked to <see cref="DockRectangle"/> and
        /// determines how this <see cref="Control"/> is resized.
        /// </summary>
        /// <value>
        /// One of the <see cref="DockStyle"/> values;
        /// </value>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        /// <remarks>
        /// <see cref="DockStyle"/> applies only if <see cref="DockRectangle"/> contains
        /// <see cref="BoundingBox"/>, and if it does, it also resets <see cref="TouchArea"/>
        /// to <see cref="BoundingBox"/>.
        /// </remarks>
        public virtual DockStyle DockStyle
        {
            get { return dockStyle; }
            set {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (value != dockStyle && DockRectangle.Contains(BoundingBox)) {
                    dockStyle = value;
                    Arrange();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this <see cref="Control"/>
        /// should handle touch gestures even when in background.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Control"/> should handle
        /// touch gestures even when in background; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool HandleGesturesWhenInBackground
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Control"/> is enabled. 
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Control"/> is enabled; otherwise,
        /// <see langword="false"/>.
        /// </value>
        /// <seealso cref="Enable"/>
        /// <seealso cref="Disable"/>
        public bool IsEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="Control"/>
        /// is pressed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Control"/> is pressed;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool IsPressed
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the touch area associated with this <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The touch area associated with this <see cref="Control"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has been already disposed of.
        /// </exception>
        /// <remarks>
        /// If not otherwise specified, <see cref="Control.TouchArea"/> is default
        /// to <see cref="SpriteObject.BoundingBox"/>.
        /// </remarks>
        public virtual Rectangle TouchArea
        {
            get { return touchArea; }
            set {
                if (IsDisposed) {
                    throw new ObjectDisposedException(GetType().FullName);
                }

                if (value != touchArea) {
                    if (_Click != null || _Pressed != null || _Released != null) {
                        GameHost.GameInput.RemoveTapInput(ClickEventTag, touchArea);
                        GameHost.GameInput.AddTapInput(ClickEventTag, value, false);
                    }

                    if (_Flick != null) {
                        GameHost.GameInput.RemoveGestureInput(FlickEventTag, GestureType.Flick, touchArea);
                        GameHost.GameInput.AddGestureInput(FlickEventTag, GestureType.Flick, value);
                    }

                    if (_Hold != null) {
                        GameHost.GameInput.RemoveGestureInput(HoldEventTag, GestureType.Hold, touchArea);
                        GameHost.GameInput.AddGestureInput(HoldEventTag, GestureType.Hold, value);
                    }

                    if (_FreeDrag != null) {
                        GameHost.GameInput.RemoveGestureInput(FreeDragEventTag, GestureType.FreeDrag, touchArea);
                        GameHost.GameInput.AddGestureInput(FreeDragEventTag, GestureType.FreeDrag, value);
                    }

                    if (_HorizontalDrag != null) {
                        GameHost.GameInput.RemoveGestureInput(HorizontalDragEventTag, GestureType.HorizontalDrag, touchArea);
                        GameHost.GameInput.AddGestureInput(HorizontalDragEventTag, GestureType.HorizontalDrag, value);
                    }

                    if (_VerticalDrag != null) {
                        GameHost.GameInput.RemoveGestureInput(VerticalDragEventTag, GestureType.VerticalDrag, touchArea);
                        GameHost.GameInput.AddGestureInput(VerticalDragEventTag, GestureType.VerticalDrag, value);
                    }

                    if (_DragComplete != null) {
                        GameHost.GameInput.RemoveGestureInput(DragCompleteEventTag, GestureType.DragComplete, touchArea);
                        GameHost.GameInput.AddGestureInput(DragCompleteEventTag, GestureType.DragComplete, value);
                    }

                    touchArea = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the source rectangle of this <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The source rectangle of this <see cref="Control"/>.
        /// </value>
        public override Rectangle SourceRectangle
        {
            get { return base.SourceRectangle; }
            set { base.SourceRectangle =  actualSourceRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Control"/> along the x-axis.
        /// </summary>
        /// <value>
        /// The position of this <see cref="Control"/> along the x-axis.
        /// </value>
        public override float PositionX
        {
            get { return base.PositionX; }
            set { actualPosition.X = base.PositionX = value; }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Control"/> along the y-axis.
        /// </summary>
        /// <value>
        /// The position of this <see cref="Control"/> along the y-axis.
        /// </value>
        public override float PositionY
        {
            get { return base.PositionY; }
            set { actualPosition.Y = base.PositionY = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> along the x-axis,
        /// when pressed.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> along the x-axis, when pressed,
        /// in percent of the original width.
        /// </value>
        public virtual float ScaleWhenPressedX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> along the y-axis,
        /// when pressed.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> along the y-axis, when pressed,
        /// in percent of the original height.
        /// </value>
        public virtual float ScaleWhenPressedY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> along the x-axis,
        /// when released.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> along the x-axis, when released,
        /// in percent of the original width.
        /// </value>
        public virtual float ScaleWhenReleasedX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> along the x-axis,
        /// when released.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> along the x-axis, when released,
        /// in percent of the original height.
        /// </value>
        public virtual float ScaleWhenReleasedY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> when pressed.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> when pressed, in percent of the
        /// original width and height.
        /// </value>
        public virtual Vector2 ScaleWhenPressed
        {
            get { return new Vector2(ScaleWhenPressedX, ScaleWhenPressedY); }
            set {
                ScaleWhenPressedX = value.X;
                ScaleWhenPressedY = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the this <see cref="Control"/> when released.
        /// </summary>
        /// <value>
        /// The scale of this <see cref="Control"/> when released, in percent of the
        /// original width and height.
        /// </value>
        public virtual Vector2 ScaleWhenReleased
        {
            get { return new Vector2(ScaleWhenReleasedX, ScaleWhenReleasedY); }
            set {
                ScaleWhenReleasedX = value.X;
                ScaleWhenReleasedY = value.Y;
            }
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.Click"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.Click"/> event.
        /// </value>
        protected string ClickEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.Flick"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.Flick"/> event.
        /// </value>
        protected string FlickEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.Hold"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.Hold"/> event.
        /// </value>
        protected string HoldEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.FreeDrag"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.FreeDrag"/> event.
        /// </value>
        protected string FreeDragEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.HorizontalDrag"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.HorizontalDrag"/> event.
        /// </value>
        protected string HorizontalDragEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.VerticalDrag"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.VerticalDrag"/> event.
        /// </value>
        protected string VerticalDragEventTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action tag for the <see cref="Control.DragComplete"/> event.
        /// </summary>
        /// <value>
        /// The action tag for the <see cref="Control.DragComplete"/> event.
        /// </value>
        protected string DragCompleteEventTag
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Arranges this <see cref="Control"/> according to current <see cref="DockStype"/>
        /// and <see cref="DockRectangle"/>.
        /// </summary>
        private void Arrange()
        {
            switch (DockStyle) {
                case DockStyle.Fill:
                    base.PositionX = DockRectangle.Left;
                    base.PositionY = DockRectangle.Top;
                    base.SourceRectangle = new Rectangle(0, 0, DockRectangle.Width, DockRectangle.Height);
                    break;
                case DockStyle.Top:
                    base.PositionX = DockRectangle.Left;
                    base.PositionY = DockRectangle.Top;
                    if (!AutoSourceRectangle) {
                        base.SourceRectangle = new Rectangle(0, 0, DockRectangle.Width, actualSourceRectangle.Height);
                    }
                    break;
                case DockStyle.Bottom:
                    base.PositionX = DockRectangle.Left;
                    base.PositionY = DockRectangle.Bottom - actualSourceRectangle.Height;
                    if (!AutoSourceRectangle) {
                        base.SourceRectangle = new Rectangle(0, 0, DockRectangle.Width, actualSourceRectangle.Height);
                    }
                    break;
                case DockStyle.Left:
                    base.PositionX = DockRectangle.Left;
                    base.PositionY = DockRectangle.Top;
                    if (!AutoSourceRectangle) {
                        base.SourceRectangle = new Rectangle(0, 0, actualSourceRectangle.Width, DockRectangle.Height);
                    }
                    break;
                case DockStyle.Right:
                    base.PositionX = DockRectangle.Right - BoundingBox.Width;
                    base.PositionY = DockRectangle.Top;
                    if (!AutoSourceRectangle) {
                        base.SourceRectangle = new Rectangle(0, 0, actualSourceRectangle.Width, DockRectangle.Height);
                    }
                    break;
                default: // DockStyle.None
                    base.PositionX = actualPosition.X;
                    base.PositionY = actualPosition.Y;
                    if (!AutoSourceRectangle) {
                        base.SourceRectangle = actualSourceRectangle;
                    }
                    break;
            }

            TouchArea = BoundingBox;
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="Control"/>
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
                    ReleaseInputs();
                }

                // Release unmanaged resources
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Disables this <see cref="Control"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        public virtual void Disable()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            IsPressed = false;
            IsEnabled = false;
        }

        /// <summary>
        /// Enables this <see cref="Control"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        public virtual void Enable()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            IsEnabled = true;
        }

        /// <summary>
        /// Updates this <see cref="Control"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsEnabled) {
                RaiseEventOnClick();
                RaiseEventOnFlick();
                RaiseEventOnHold();
                RaiseEventOnFreeDrag();
                RaiseEventOnHorizontalDrag();
                RaiseEventOnVerticalDrag();
                RaiseEventOnDragComplete();

                UpdateVisualState();
            }
        }

        /// <summary>
        /// Updates the visual state of this <see cref="Control"/> according
        /// to the current state.
        /// </summary>
        /// <remarks>
        /// The default implementation of <see cref="UpdateVisualState"/> sets
        /// <see cref="Scale"/> to <see cref="ScaleWhenPressed"/> or
        /// <see cref="ScaleWhenReleased"/> depending on whether or not the
        /// <see cref="Control"/> is pressed.
        /// </remarks>
        protected virtual void UpdateVisualState()
        {
            // Adjust scale and position according to current state
            if (ScaleWhenPressed != ScaleWhenReleased) {
                if (IsPressed) {
                    ScaleX = ScaleWhenPressedX;
                    ScaleY = ScaleWhenPressedY;
                } else {
                    ScaleX = ScaleWhenReleasedX;
                    ScaleY = ScaleWhenReleasedY;
                }

                Rectangle rectangle = !SourceRectangle.IsEmpty ? SourceRectangle : Texture.Bounds;

                OriginX = -(rectangle.Width - BoundingBox.Width) / 2;
                OriginY = -(rectangle.Height - BoundingBox.Height) / 2;
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Click"/>, <see cref="Control.Pressed"/>,
        /// and <see cref="Control.Released"/> events if this <see cref="Control"/>
        /// is being clicked and at least one delegate is attached. 
        /// </summary>
        private void RaiseEventOnClick()
        {
            if (_Click != null || _Pressed != null || _Released != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(ClickEventTag);
                bool isPressed = GameHost.GameInput.IsPressed(ClickEventTag) && IsTouchInObject(touchPosition);

                if (isPressed != IsPressed) {
                    IsPressed = isPressed;
                    GameInputEventArgs args = new GameInputEventArgs(GameHost.GameInput, ClickEventTag);

                    if (IsPressed) {
                        if (_Pressed != null) { _Pressed(this, args); }
                    } else {
                        if (_Released != null) { _Released(this, args); }

                        if (holdAge == 0 && (touchPosition.X < 0 || touchPosition.Y < 0)) {
                            if (_Click != null) { _Click(this, args); }
                        }
                        holdAge = horizontalDragAge = verticalDragAge = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Flick"/> event if this <see cref="Control"/>
        /// is being flicked and at least one delegate is attached. 
        /// </summary>
        private void RaiseEventOnFlick()
        {
            if (_Flick != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(FlickEventTag);
                if (GameHost.GameInput.IsPressed(FlickEventTag) && IsTouchInObject(touchPosition)) {
                    _Flick(this, new GameInputEventArgs(GameHost.GameInput, FlickEventTag));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Hold"/> event if this <see cref="Control"/>
        /// is being held and at least one delegate is attached. 
        /// </summary>
        private void RaiseEventOnHold()
        {
            if (_Hold != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(HoldEventTag);
                if (GameHost.GameInput.IsPressed(HoldEventTag) && IsTouchInObject(touchPosition)) {
                    _Hold(this, new GameInputEventArgs(GameHost.GameInput, HoldEventTag, ++holdAge));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.FreeDrag"/> event if this
        /// <see cref="Control"/> is being dragged in free-form and at least one
        /// delegate is attached. 
        /// </summary>
        private void RaiseEventOnFreeDrag()
        {
            if (_FreeDrag != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(FreeDragEventTag);
                if (GameHost.GameInput.IsPressed(FreeDragEventTag) && IsTouchInObject(touchPosition)) {
                    _FreeDrag(this, new GameInputEventArgs(GameHost.GameInput, FreeDragEventTag, ++freeDragAge));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.HorizontalDrag"/> event if this
        /// <see cref="Control"/> is being dragged horizontally and at least one
        /// delegate is attached. 
        /// </summary>
        private void RaiseEventOnHorizontalDrag()
        {
            if (_HorizontalDrag != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(HorizontalDragEventTag);
                if (GameHost.GameInput.IsPressed(HorizontalDragEventTag) && IsTouchInObject(touchPosition)) {
                    _HorizontalDrag(this, new GameInputEventArgs(GameHost.GameInput, HorizontalDragEventTag, ++horizontalDragAge));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.VerticalDrag"/> event if this
        /// <see cref="Control"/> is being dragged vertically and at least one
        /// delegate is attached. 
        /// </summary>
        private void RaiseEventOnVerticalDrag()
        {
            if (_VerticalDrag != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(VerticalDragEventTag);
                if (GameHost.GameInput.IsPressed(VerticalDragEventTag) && IsTouchInObject(touchPosition)) {
                    _VerticalDrag(this, new GameInputEventArgs(GameHost.GameInput, VerticalDragEventTag, ++verticalDragAge));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.DragComplete"/> event if this
        /// <see cref="Control"/> is no longer being dragged and at least one
        /// delegate is attached. 
        /// </summary>
        private void RaiseEventOnDragComplete()
        {
            if (_DragComplete != null) {
                Vector2 touchPosition = GameHost.GameInput.GetTouchPosition(DragCompleteEventTag);
                if (GameHost.GameInput.IsPressed(DragCompleteEventTag) && IsTouchInObject(touchPosition)) {
                    _DragComplete(this, new GameInputEventArgs(GameHost.GameInput, DragCompleteEventTag, ++dragCompleteAge));
                }
            }
        }

        /// <summary>
        /// Initializes this <see cref="Control"/> with the specified touch area.
        /// </summary>
        /// <param name="touchArea">The touch area associated with the <see cref="Control"/>.</param>
        private void Initialize(Rectangle touchArea)
        {
            ScaleWhenReleasedX = ScaleWhenReleasedY = 1f;
            ScaleWhenPressedX = ScaleWhenPressedY = 1f;

            // If no position is specified, center this control automatically
            if (base.PositionX < 0 || base.PositionY < 0) {
                base.PositionX = base.PositionY = 0f;

                if (GameHost.Window.ClientBounds.Width > BoundingBox.Width) {
                    base.PositionX = (GameHost.Window.ClientBounds.Right - BoundingBox.Width) / 2f;
                }
                if (GameHost.Window.ClientBounds.Height > BoundingBox.Height) {
                    base.PositionY = (GameHost.Window.ClientBounds.Bottom - BoundingBox.Height) / 2f;
                }
            }

            // Store actual source rectangle and position in case DockStyle is
            // set back to DockStyle.None
            actualSourceRectangle = base.SourceRectangle;
            actualPosition.X = base.PositionX;
            actualPosition.Y = base.PositionY;

            // If no touch area is specified, default it to BoundingBox
            TouchArea = touchArea != Rectangle.Empty ? touchArea : BoundingBox;

            // Enable this control by default
            IsEnabled = true;

            // Add to GameHost for rendering
            GameHost.GameObjects.Add(this);
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified touch happened in this
        /// <see cref="Control"/>.
        /// </summary>
        /// <param name="touchPosition">The touch position.</param>
        /// <returns>
        /// <see langword="true" /> if the specified touch happened in this <see cref="Control"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool IsTouchInObject(Vector2 touchPosition)
        {
            return HandleGesturesWhenInBackground || GameHost.GetSpriteAtPoint(touchPosition,
                delegate(GameObject gameObject) {
                    return !(gameObject is Control);
                }) == this;
        }

        /// <summary>
        /// Releases all the active inputs.
        /// </summary>
        private void ReleaseInputs()
        {
            if (_Click != null || _Pressed != null || _Released != null) {
                GameHost.GameInput.RemoveTapInput(ClickEventTag, BoundingBox);
            }

            if (_Flick != null) {
                GameHost.GameInput.RemoveGestureInput(FlickEventTag, GestureType.Flick, BoundingBox);
            }

            if (_Hold != null) {
                GameHost.GameInput.RemoveGestureInput(HoldEventTag, GestureType.Hold, BoundingBox);
            }

            if (_FreeDrag != null) {
                GameHost.GameInput.RemoveGestureInput(FreeDragEventTag, GestureType.FreeDrag, BoundingBox);
            }

            if (_HorizontalDrag != null) {
                GameHost.GameInput.RemoveGestureInput(HorizontalDragEventTag, GestureType.HorizontalDrag, BoundingBox);
            }

            if (_VerticalDrag != null) {
                GameHost.GameInput.RemoveGestureInput(VerticalDragEventTag, GestureType.VerticalDrag, BoundingBox);
            }

            if (_DragComplete != null) {
                GameHost.GameInput.RemoveGestureInput(DragCompleteEventTag, GestureType.DragComplete, BoundingBox);
            }
        }
        #endregion
    }
}
