#region Header
//+ <source name="MenuItem.cs" language="C#" begin="20-Jul-2012">
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
    /// Represents an individual item that is displayed within a <see cref="Menu"/>. 
    /// </summary>
    public class MenuItem : Menu
    {
        #region Fields
        private const double GestureDelay = 250d;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with
        /// the specified <see cref="GameHost"/> and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="texture">The <see cref="MenuItem"/> texture.</param>
        public MenuItem(GameHost gameHost, Texture2D texture)
            : this(gameHost, texture, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with
        /// the specified <see cref="GameHost"/>, texture, and tag.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="texture">The <see cref="MenuItem"/> texture.</param>
        /// <param name="tag">The tag that identifies this <see cref="MenuItem"/>.</param>
        public MenuItem(GameHost gameHost, Texture2D texture, string tag)
            : this(gameHost, texture, tag, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with
        /// the specified <see cref="GameHost"/>, texture, tag, and submenu items.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="texture">The <see cref="MenuItem"/> texture.</param>
        /// <param name="tag">The tag that identifies this <see cref="MenuItem"/>.</param>
        /// <param name="menuItems">The submenu items of this <see cref="MenuItem"/>.</param>
        public MenuItem(GameHost gameHost, Texture2D texture, string tag, MenuItem[] menuItems)
            : base(gameHost, Vector2.Zero, texture, menuItems)
        {
            ScaleX = ScaleY = ScaleWhenReleasedX = ScaleWhenReleasedY = 0.9f;

            Tag = tag;
            HideMenuItems();

            Click += OnClick;
            Pressed += OnPressed;
            Released += OnReleased;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dimensions of the back buffer rectangle behind
        /// this <see cref="MenuItem"/>.
        /// </summary>
        /// <value>
        /// The dimensions of the back buffer rectangle behind this
        /// <see cref="MenuItem"/>.
        /// </value>
        public override Rectangle BackBufferRectangle
        {
            get { return base.BackBufferRectangle; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="MenuItem"/>
        /// has input control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="MenuItem"/> has input control;
        /// otherwise, <see langword="false"/>.
        /// </value>
        /// <seealso cref="Menu"/>
        public override bool HasInputControl
        {
            get { return GestureElapsedTime != TimeSpan.MinValue; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of this <see cref="MenuItem"/>.
        /// </summary>
        /// <value>The zero-based index of this <see cref="MenuItem"/>.</value>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time interval since the last gesture.
        /// </summary>
        /// <value>
        /// The time interval since the last gesture.
        /// </value>
        internal virtual TimeSpan GestureElapsedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the orientation of this <see cref="ScrollMenu"/>.
        /// </summary>
        /// One of the <see cref="Orientation"/> values.
        /// <exception cref="InvalidOperationException">
        /// <see cref="Orientation"/> cannot be set with the current <see cref="DockStyle"/>.
        /// </exception>
        public override Orientation Orientation
        {
            get { return base.Orientation; }
            set {
                if (DockStyle != DockStyle.None && DockStyle != DockStyle.Fill) {
                    String message = String.Format("Orientation cannot be set when DockStyle is %", DockStyle);
                    throw new InvalidOperationException(message);
                }

                base.Orientation = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hides all the submenu items of this <see cref="MenuItem"/>.
        /// </summary>
        private void HideMenuItems()
        {
            GestureElapsedTime = TimeSpan.MinValue;

            if (MenuItems.Count > 0 && MenuItems[0].IsVisible) {
                foreach (MenuItem menuItem in MenuItems) {
                    menuItem.Hide();
                    menuItem.GestureElapsedTime = TimeSpan.MinValue;
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Click"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnClick(object sender, GameInputEventArgs args)
        {
            MenuItem parent = Parent as MenuItem;

            if (parent != null) {
                parent.GestureElapsedTime = TimeSpan.MaxValue;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Pressed"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnPressed(object sender, GameInputEventArgs args)
        {
            if (GestureElapsedTime < TimeSpan.Zero) {
                GestureElapsedTime = TimeSpan.Zero;
            }

            MenuItem parent = Parent as MenuItem;

            if (parent != null) {
                parent.GestureElapsedTime = TimeSpan.MinValue;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Released"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnReleased(object sender, GameInputEventArgs args)
        {
            GestureElapsedTime = TimeSpan.Zero;

            MenuItem parent = Parent as MenuItem;

            if (parent != null) {
                parent.GestureElapsedTime = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Handles the <see cref="ObservableList&lt;T&gt;.ListChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItemCollection"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        protected override void OnMenuItemCollectionChanged(object sender, ListChangedEventArgs<MenuItem> args)
        {
            switch (args.ListChangedType) {
                case ListChangedType.ItemAdded:
                    OnMenuItemAdded(args.Item, true);
                    break;
                case ListChangedType.ItemChanged:
                    OnMenuItemRemoved(args.OldItem, false);
                    OnMenuItemAdded(args.NewItem, true);
                    break;
                case ListChangedType.ItemRemoved:
                    OnMenuItemRemoved(args.Item, true);
                    break;
            }
        }

        /// <summary>
        /// Called when a submenu item is added to this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="menuItem">
        /// The submenu item added to this <see cref="MenuItem"/>.
        /// </param>
        /// <param name="arrange">
        /// A value indicating whether or not to arrange the submenu items of this <see cref="MenuItem"/>.
        /// </param>
        private void OnMenuItemAdded(MenuItem menuItem, bool arrange)
        {
            if (arrange) {
                if (Orientation == Orientation.Horizontal) {
                    ArrangeVertically(menuItem.Index);
                } else {
                    ArrangeHorizontally(menuItem.Index);
                }
            }
        }

        /// <summary>
        /// Called when a submenu item is removed from this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="menuItem">
        /// The submenu item removed from this <see cref="MenuItem"/>.
        /// </param>
        /// <param name="arrange">
        /// A value indicating whether or not to arrange the submenu items of this <see cref="MenuItem"/>.
        /// </param>
        private void OnMenuItemRemoved(MenuItem menuItem, bool arrange)
        {
            menuItem.TouchArea = menuItem.BoundingBox;
            menuItem.Hide();

            if (arrange) {
                if (Orientation == Orientation.Horizontal) {
                    ArrangeVertically(menuItem.Index);
                } else {
                    ArrangeHorizontally(menuItem.Index);
                }
            }
        }

        /// <summary>
        /// Arranges the submenu items of this <see cref="MenuItem"/> horizontally,
        /// starting at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the <see cref="MenuItem"/> to start rearranging from.
        /// </param>
        private void ArrangeHorizontally(int startIndex)
        {
            int expandDirection = 1;

            if ((BoundingBox.X + (BoundingBox.Width / 2)) > (BackBufferRectangle.X + (BackBufferRectangle.Width / 2))) {
                expandDirection = -1;
            }

            MenuItem menuItem = startIndex > 0 ? MenuItems[startIndex - 1] : this;
            Vector2 position = menuItem.Position;
            Rectangle touchArea = menuItem.TouchArea;
            position.X += (menuItem.SourceRectangle.Width * expandDirection);
            touchArea.X += (menuItem.TouchArea.Width * expandDirection);

            for (int i = startIndex; i < MenuItems.Count; i++) {
                MenuItems[i].Position = position;
                MenuItems[i].TouchArea = touchArea;
                MenuItems[i].Orientation = Orientation.Horizontal;
                position.X += (MenuItems[i].SourceRectangle.Width * expandDirection);
                touchArea.X += (MenuItems[i].TouchArea.Width * expandDirection);
            }
        }

        /// <summary>
        /// Arranges the submenu items of this <see cref="MenuItem"/> vertically,
        /// starting at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the <see cref="MenuItem"/> to start rearranging from.
        /// </param>
        private void ArrangeVertically(int startIndex)
        {
            int expandDirection = 1;

            if ((BoundingBox.Y + (BoundingBox.Height / 2)) > (BackBufferRectangle.Y + (BackBufferRectangle.Height / 2))) {
                expandDirection = -1;
            }

            MenuItem menuItem = startIndex > 0 ? MenuItems[startIndex - 1] : this;
            Vector2 position = menuItem.Position;
            Rectangle touchArea = menuItem.TouchArea;
            position.Y += (menuItem.SourceRectangle.Height * expandDirection);
            touchArea.Y += (menuItem.TouchArea.Height * expandDirection);

            for (int i = startIndex; i < MenuItems.Count; i++) {
                MenuItems[i].Position = position;
                MenuItems[i].TouchArea = touchArea;
                MenuItems[i].Orientation = Orientation.Vertical;
                position.Y += (MenuItems[i].Texture.Height * expandDirection);
                touchArea.Y += (MenuItems[i].TouchArea.Height * expandDirection);
            }
        }

        /// <summary>
        /// Recalculates position, size, and touch area of the submenu items of
        /// this <see cref="MenuItem"/>.
        /// </summary>
        /// <seealso cref="Menu.Invalidate"/>
        protected override void Reset()
        {
            if (Orientation == Orientation.Horizontal) {
                ArrangeVertically(0);
            } else {
                ArrangeHorizontally(0);
            }
        }

        /// <summary>
        /// Updates this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MenuItem"/> has already been disposed of.
        /// </exception>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If the parent has input control or the current gesture timed out,
            // then hide possible submenu items, release this menu item, and return
            if (Parent.HasInputControl || GestureElapsedTime == TimeSpan.MaxValue) {
                HideMenuItems();
                IsPressed = false;
                UpdateVisualState();
                return;
            }

            if (GestureElapsedTime > TimeSpan.MinValue) {
                if (GestureElapsedTime.TotalMilliseconds < GestureDelay) {
                    GestureElapsedTime += gameTime.ElapsedGameTime;
                } else if (IsPressed) {
                    if (Orientation == Orientation.Horizontal) {
                        foreach (MenuItem menuItem in MenuItems) {
                            menuItem.PositionX = PositionX;
                            menuItem.Show();
                        }
                    } else {
                        foreach (MenuItem menuItem in MenuItems) {
                            menuItem.PositionY = PositionY;
                            menuItem.Show();
                        }
                    }
                } else {
                    HideMenuItems();
                }
            }
        }
        #endregion
    }
}
