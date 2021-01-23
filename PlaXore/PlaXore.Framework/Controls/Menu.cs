#region Header
//+ <source name="Menu.cs" language="C#" begin="20-Jul-2012">
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
    /// Defines the minimal functionality a menu or menu item must implement.
    /// </summary>
    public abstract class Menu : Control
    {
        #region Fields
        private bool isInvalidated;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class with the
        /// specified <see cref="GameHost"/>, position, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Menu"/> position.</param>
        /// <param name="texture">The <see cref="Menu"/> texture.</param>
        public Menu(GameHost gameHost, Vector2 position, Texture2D texture)
            : this(gameHost, position, texture, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class with the
        /// specified <see cref="GameHost"/>, position, texture, and menu items.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="position">The <see cref="Menu"/> position.</param>
        /// <param name="texture">The <see cref="Menu"/> texture.</param>
        /// <param name="menuItems">The submenu items of this <see cref="Menu"/>.</param>
        public Menu(GameHost gameHost, Vector2 position, Texture2D texture, MenuItem[] menuItems)
            : base(gameHost, position, texture)
        {
            InternalBackBufferRectangle = gameHost.GraphicsDevice.PresentationParameters.Bounds;

            MenuItems = new MenuItemCollection(this);
            MenuItems.ListChanged += OnMenuItemCollectionChanged;

            if (menuItems != null && menuItems.Length > 0) {
                foreach (MenuItem menuItem in menuItems) {
                    MenuItems.Add(menuItem);
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dimensions of the back buffer rectangle behind
        /// this <see cref="Menu"/>.
        /// </summary>
        /// <value>
        /// The dimensions of the back buffer rectangle behind this
        /// <see cref="Menu"/>.
        /// </value>
        public override Rectangle BackBufferRectangle
        {
            get { return base.BackBufferRectangle; }
            set { InternalBackBufferRectangle = value; }
        }

        internal Rectangle InternalBackBufferRectangle
        {
            set {
                if (value != base.BackBufferRectangle) {
                    base.BackBufferRectangle = value;

                    if (MenuItems != null && MenuItems.Count > 0) {
                        foreach (MenuItem menuItem in MenuItems) {
                            menuItem.InternalBackBufferRectangle = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets any custom data associated with this <see cref="Menu"/>.
        /// </summary>
        /// <value>
        /// Any custom data associated with this <see cref="Menu"/>.
        /// </value>
        public virtual object CustomData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="Menu"/>
        /// has input control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Menu"/> has input control;
        /// otherwise, <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When a <see cref="Menu"/> has input control, child menus should wait
        /// to respond to events until <see cref="HasInputControl"/> does not
        /// return <see langword="false"/>.
        /// </remarks>
        /// <seealso cref="Plaxore.Input.GameInput"/>
        public virtual bool HasInputControl
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the submenu items of this <see cref="Menu"/>.
        /// </summary>
        /// <value>
        /// The submenu items of this <see cref="Menu"/>, or <see langword="null"/>
        /// if this <see cref="Menu"/> has no submenu items.
        /// </value>
        public virtual MenuItemCollection MenuItems
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the orientation of this <see cref="Menu"/>.
        /// </summary>
        /// One of the <see cref="Orientation"/> values.
        public virtual Orientation Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the parent of this <see cref="Menu"/>.
        /// </summary>
        /// <value>The parent of this <see cref="Menu"/>.</value>
        public virtual Menu Parent
        {
            get;
            internal set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hides this <see cref="Menu"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Menu.Hide"/> also hides all the child menu items of this
        /// <see cref="Menu"/>.
        /// </remarks>
        public override void Hide()
        {
            foreach (MenuItem menuItem in MenuItems) {
                menuItem.Hide();
            }

            base.Hide();
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="Menu"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            foreach (MenuItem menuItem in MenuItems) {
                menuItem.Dispose(disposing);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Causes this <see cref="Menu"/> to recalculate position, size, and touch
        /// area of its submenu items according to the current state.
        /// </summary>
        public virtual void Invalidate()
        {
            isInvalidated = true;
        }

        /// <summary>
        /// Handles the <see cref="ObservableList&lt;T&gt;.ListChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItemCollection"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Menu.OnMenuItemCollectionChanged"/> method also allows derived classes
        /// to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnMenuItemCollectionChanged(object sender, ListChangedEventArgs<MenuItem> args)
        {
        }

        /// <summary>
        /// Updates this <see cref="Menu"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Menu"/> has already been disposed of.
        /// </exception>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isInvalidated) {
                Reset();
                isInvalidated = false;
            }

            // If the parent menu still has input control, then ignore any possible gesture
            IsPressed = IsPressed && (Parent == null || !Parent.HasInputControl);
        }

        /// <summary>
        /// Called by <see cref="Update"/> when this <see cref="Menu"/> has been
        /// invalidated.
        /// </summary>
        /// <remarks>
        /// Derived classes must implement <see cref="Reset"/> to provide specific
        /// implementation for recalculating position, size, and touch area of their
        /// submenu items.
        /// </remarks>
        /// <seealso cref="Invalidate"/>
        protected abstract void Reset();
        #endregion
    }
}
