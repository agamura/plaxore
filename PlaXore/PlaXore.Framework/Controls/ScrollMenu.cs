#region Header
//+ <source name="ScrollMenu.cs" language="C#" begin="20-Jul-2012">
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
using PlaXore.GameFramework.Input;
using System;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Implements a <see cref="Menu"/> with graphical items that scroll either
    /// horizontally or vertically.
    /// </summary>
    public class ScrollMenu : Menu
    {
        #region Fields
        /// <summary>
        /// Specifies the default additional inertia to be applied to the kinetic scroll.
        /// </summary>
        public const float DefaultVirtualMass = 10f;

        private const float NoFriction = 0f;
        private const float FrictionWhenDragging = NoFriction;
        private const float FrictionWhenFlicking = 0.1f;

        private Vector2 kineticScroll;
        private Vector2 velocity;
        private Direction lastScrollDirection;
        private MenuExtent menuExtent;
        private float friction;
        private bool snapMenuItems;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollMenu"/> class with
        /// the specified <see cref="GameHost"/> and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="texture">The <see cref="ScrollMenu"/> texture.</param>
        public ScrollMenu(GameHost gameHost, Texture2D texture)
            : this (gameHost, Rectangle.Empty, texture, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollMenu"/> class with
        /// the specified <see cref="GameHost"/>, dock rectangle, and texture.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="dockRectangle">The rectangle where to arrange the <see cref="ScrollMenu"/>.</param>
        /// <param name="texture">The <see cref="ScrollMenu"/> texture.</param>
        public ScrollMenu(GameHost gameHost, Rectangle dockRectangle, Texture2D texture)
            : this(gameHost, dockRectangle, texture, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollMenu"/> class with
        /// the specified <see cref="GameHost"/>, dock rectangle, texture, and submenu items.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="dockRectangle">The rectangle where to arrange the <see cref="ScrollMenu"/>.</param>
        /// <param name="texture">The <see cref="ScrollMenu"/> texture.</param>
        /// <param name="menuItems">The submenu items of the <see cref="ScrollMenu"/>.</param>
        public ScrollMenu(GameHost gameHost, Rectangle dockRectangle, Texture2D texture, MenuItem[] menuItems)
            : base(gameHost, Vector2.Zero, texture, menuItems)
        {
            DockRectangle = dockRectangle;
            Orientation = Orientation.Horizontal;
            ScrollDirection = Direction.None;
            VirtualMass = DefaultVirtualMass;

            Pressed += OnPressed;
            Released += OnReleased;
            Flick += OnFlick;
            HorizontalDrag += OnHorizontalDrag;

            HandleGesturesWhenInBackground = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the scroll direction of this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <value>One of the <see cref="Direction"/> values.</value>
        public Direction ScrollDirection
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets which borders are docked to this <see cref="ScrollMenu"/>
        /// and determines how it is resized.
        /// </summary>
        /// <value>
        /// One of the <see cref="DockStyle"/> values.
        /// </value>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Control"/> has already been disposed of.
        /// </exception>
        public override DockStyle DockStyle
        {
            get { return base.DockStyle; }
            set {
                if (value != base.DockStyle) {
                    Position = Vector2.Zero;
                    DockStyle oldDockStyle = base.DockStyle;
                    base.DockStyle = value;

                    if (value == DockStyle.Left || value == DockStyle.Right) {
                        if (oldDockStyle != DockStyle.Left && oldDockStyle != DockStyle.Right) {
                            HorizontalDrag -= OnHorizontalDrag;
                            VerticalDrag += OnVerticalDrag;
                            Orientation = Orientation.Vertical;
                            if (MenuItems.Count > 0) { ArrangeVertically(0, true); }
                        }
                    } else {
                        if (oldDockStyle != DockStyle.None && oldDockStyle != DockStyle.Bottom && oldDockStyle != DockStyle.Bottom) {
                            VerticalDrag -= OnVerticalDrag;
                            HorizontalDrag += OnHorizontalDrag;
                            Orientation = Orientation.Horizontal;
                            if (MenuItems.Count > 0) { ArrangeHorizontally(0, true); }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="ScrollMenu"/>
        /// has input control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ScrollMenu"/> has input control;
        /// otherwise, <see langword="false"/>.
        /// </value>
        /// <seealso cref="Menu"/>
        public override bool HasInputControl
        {
            get { return ScrollDirection != Direction.None; }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="ScrollMenu"/>
        /// is circular.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ScrollMenu"/> is circular;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool IsCircular
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the menu extent of this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <value>
        /// The menu extent of this <see cref="ScrollMenu"/>.
        /// </value>
        /// <remarks>
        /// <see cref="MenuExtent"/> is set at least to <see cref="DockRectangle.Width"/> or
        /// <see cref="DockRectangle.Height"/>, according to the current <see cref="Orientation"/>.
        /// </remarks>
        public virtual MenuExtent MenuExtent
        {
            get {
                int minMenuExtent = Orientation == Orientation.Horizontal
                    ? DockRectangle.Width
                    : DockRectangle.Height;

                return menuExtent < minMenuExtent
                    ? new MenuExtent(minMenuExtent)
                    : menuExtent;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not submenu items should snap
        /// to their width or height, according to the <see cref="Orientation"/> of
        /// this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if submenu items should snap; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool SnapMenuItems
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the additional inertia to be applied to the kinetic scroll.
        /// </summary>
        /// <value>The additional inertia to be applied to the kinetic scroll.</value>
        public float VirtualMass
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the delta distance to snap the submenu items of this <see cref="ScrollMenu"/>,
        /// according to the specified offset and the current scroll <see cref="Direction"/>.
        /// </summary>
        /// <param name="offset">
        /// The number of pixels to shift the submenu items of this <see cref="ScrollMenu"/>.
        /// </param>
        /// <returns>
        /// The delta distance to snap the submenu items of this <see cref="ScrollMenu"/>.
        /// </returns>
        private Vector2 GetSnapDelta(int offset)
        {
            Vector2 delta = Vector2.Zero;

            if (lastScrollDirection != Direction.None) {
                Vector2 point;
                float menuExtent = (float) this.menuExtent;

                if (Orientation == Orientation.Horizontal) {
                    if (lastScrollDirection == Direction.Left) {
                        foreach (MenuItem menuItem in MenuItems) {
                            point = new Vector2(BoundingBox.Left, menuItem.BoundingBox.Top + (menuItem.BoundingBox.Height / 2));
                            if (menuItem.IsPointInObject(point)) {
                                delta.X = -((menuItem.PositionX + menuItem.SourceRectangle.Width) - BoundingBox.Left);
                                menuExtent *= -1; // Invert extent direction
                                break;
                            }
                        }
                    } else {
                        foreach (MenuItem menuItem in MenuItems) {
                            point = new Vector2(BoundingBox.Right, menuItem.BoundingBox.Top + (menuItem.BoundingBox.Height / 2));
                            if (menuItem.IsPointInObject(point)) {
                                delta.X = BoundingBox.Right - menuItem.PositionX;
                                break;
                            }
                        }
                    }
                    if (offset != 0f) { delta.X += menuExtent - offset; }
                } else {
                    if (lastScrollDirection == Direction.Up) {
                        foreach (MenuItem menuItem in MenuItems) {
                            point = new Vector2(menuItem.BoundingBox.Left + (menuItem.BoundingBox.Width / 2), BoundingBox.Top);
                            if (menuItem.IsPointInObject(point)) {
                                delta.Y = -((menuItem.PositionY + menuItem.SourceRectangle.Height) - BoundingBox.Top);
                                menuExtent *= -1; // Invert extent direction
                                break;
                            }
                        }
                    } else {
                        foreach (MenuItem menuItem in MenuItems) {
                            point = new Vector2(menuItem.BoundingBox.Left + (menuItem.BoundingBox.Width / 2), BoundingBox.Bottom);
                            if (menuItem.IsPointInObject(point)) {
                                delta.Y = BoundingBox.Bottom - menuItem.PositionY;
                                break;
                            }
                        }
                    }
                    if (offset != 0f) { delta.Y += menuExtent - offset; }
                }
            }

            return delta;
        }

        /// <summary>
        /// Handles the <see cref="Control.Pressed"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ScrollMenu"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnPressed(object sender, GameInputEventArgs args)
        {
            kineticScroll = velocity = Vector2.Zero;
            ScrollDirection = Direction.None;
        }

        /// <summary>
        /// Handles the <see cref="Control.Released"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ScrollMenu"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnReleased(object sender, GameInputEventArgs args)
        {
            if (velocity != Vector2.Zero) {
                Vector2 kineticScroll = velocity * VirtualMass * (float) GameHost.TargetElapsedTime.TotalSeconds;

                if (SnapMenuItems) {
                    int offset = Orientation == Orientation.Horizontal
                        ? (int) Math.Round(kineticScroll.X) % (int) menuExtent
                        : (int) Math.Round(kineticScroll.Y) % (int) menuExtent;

                    kineticScroll += GetSnapDelta(offset);
                }

                SetKineticScroll(kineticScroll, FrictionWhenFlicking);
            } else if (SnapMenuItems) {
                SetKineticScroll(GetSnapDelta(0), FrictionWhenFlicking);
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Flick"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ScrollMenu"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnFlick(object sender, GameInputEventArgs args)
        {
            velocity = args.GestureDelta;
        }

        /// <summary>
        /// Handles the <see cref="Control.HorizontalDrag"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ScrollMenu"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnHorizontalDrag(object sender, GameInputEventArgs args)
        {
            SetKineticScroll(args.GestureDelta, FrictionWhenDragging);
        }

        /// <summary>
        /// Handles the <see cref="Control.VerticalDrag"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ScrollMenu"/> that raised the event.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnVerticalDrag(object sender, GameInputEventArgs args)
        {
            SetKineticScroll(args.GestureDelta, FrictionWhenDragging);
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
        /// Called when a submenu item is added to this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <param name="menuItem">
        /// The submenu item added to this <see cref="ScrollMenu"/>.
        /// </param>
        /// <param name="arrange">
        /// A value indicating whether or not to arrange the submenu items of this <see cref="ScrollMenu"/>.
        /// </param>
        private void OnMenuItemAdded(MenuItem menuItem, bool arrange)
        {
            menuExtent += menuItem;

            if (IsVisible) {
                menuItem.Show();
            }

            if (arrange) {
                if (Orientation == Orientation.Horizontal) {
                    ArrangeHorizontally(menuItem.Index, false);
                } else {
                    ArrangeVertically(menuItem.Index, false);
                }
            }

            menuItem.Orientation = Orientation;
            menuItem.TouchArea = BoundingBox;
            menuItem.Invalidate();
        }

        /// <summary>
        /// Called when a submenu item is removed from this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <param name="menuItem">
        /// The submenu item removed from this <see cref="ScrollMenu"/>.
        /// </param>
        /// <param name="arrange">
        /// A value indicating whether or not to arrange the submenu items of this <see cref="ScrollMenu"/>.
        /// </param>
        private void OnMenuItemRemoved(MenuItem menuItem, bool arrange)
        {
            menuExtent -= menuItem;

            menuItem.TouchArea = menuItem.BoundingBox;
            menuItem.Hide();

            if (arrange) {
                if (Orientation == Orientation.Horizontal) {
                    ArrangeHorizontally(menuItem.Index, false);
                } else {
                    ArrangeVertically(menuItem.Index, false);
                }
            }
        }

        /// <summary>
        /// Recalculates position, size, and touch area of the submenu items of
        /// this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <seealso cref="Menu.Invalidate"/>
        protected override void Reset()
        {
            if (Orientation == Orientation.Horizontal) {
                ArrangeHorizontally(0, true);
            } else {
                ArrangeVertically(0, true);
            }
        }

        /// <summary>
        /// Arranges the submenu items of this <see cref="ScrollMenu"/> horizontally,
        /// starting at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the <see cref="MenuItem"/> to start rearranging from.
        /// </param>
        /// <param name="reset">
        /// A value indicating whether or not to reset the submenu items starting at
        /// <paramref name="startIndex"/>.
        /// </param>
        private void ArrangeHorizontally(int startIndex, bool reset)
        {
            MenuExtent currentExtent = new MenuExtent(BoundingBox.Left);

            float positionY = DockStyle == DockStyle.Top
                ? BoundingBox.Top
                : BoundingBox.Bottom - MenuItems[MenuItems.Count - 1].SourceRectangle.Height;

            for (int i = 0; i < MenuItems.Count; i++) {
                if (i >= startIndex) {
                    MenuItems[i].PositionX = (float) currentExtent;
                    MenuItems[i].PositionY = positionY;

                    if (reset) {
                        MenuItems[i].Orientation = Orientation;
                        MenuItems[i].TouchArea = BoundingBox;
                        MenuItems[i].Invalidate();
                    }
                }

                currentExtent += MenuItems[i].SourceRectangle.Width;
            }
        }

        /// <summary>
        /// Arranges the submenu items of this <see cref="ScrollMenu"/> vertically,
        /// starting at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the <see cref="MenuItem"/> to start rearranging from.
        /// </param>
        /// <param name="reset">
        /// A value indicating whether or not to reset the submenu items starting at
        /// <paramref name="startIndex"/>.
        /// </param>
        private void ArrangeVertically(int startIndex, bool reset)
        {
            MenuExtent currentExtent = new MenuExtent(BoundingBox.Top);

            float positionX = DockStyle == DockStyle.Left
                ? BoundingBox.Left
                : BoundingBox.Right - MenuItems[MenuItems.Count - 1].SourceRectangle.Width;

            for (int i = 0; i < MenuItems.Count; i++) {
                if (i >= startIndex) {
                    MenuItems[i].PositionX = positionX;
                    MenuItems[i].PositionY = (float) currentExtent;

                    if (reset) {
                        MenuItems[i].Orientation = Orientation;
                        MenuItems[i].TouchArea = BoundingBox;
                        MenuItems[i].Invalidate();
                    }
                }

                currentExtent += MenuItems[i].SourceRectangle.Height;
            }
        }

        /// <summary>
        /// Scrolls the submenu items of this <see cref="ScrollMenu"/> horizontally.
        /// </summary>
        private void ScrollHorizontally()
        {
            float deltaX = friction == NoFriction ? kineticScroll.X : (float) Math.Round(kineticScroll.X * friction, 1);
            kineticScroll.X = kineticScroll.X < 0f ? Math.Min(kineticScroll.X - deltaX, 0f) : Math.Max(kineticScroll.X - deltaX, 0f);

            if (deltaX < 0f) {
                ScrollDirection = Direction.Left;
            } else if (deltaX > 0f) {
                ScrollDirection = Direction.Right;
            } else {
                ScrollDirection = Direction.None;
            }

            lastScrollDirection = ScrollDirection;

            if (IsCircular) {
                foreach (MenuItem menuItem in MenuItems) {
                    menuItem.PositionX = (float) Math.Round(menuItem.PositionX + deltaX, 1);

                    if (BoundingBox.Width < MenuExtent) {
                        float bound = menuItem.PositionX + menuItem.SourceRectangle.Width;

                        if (bound > BoundingBox.Left + MenuExtent) {
                            menuItem.PositionX = (BoundingBox.Left - menuItem.SourceRectangle.Width) + (bound - MenuExtent);
                        } else if (bound <= BoundingBox.Left) {
                            menuItem.PositionX += MenuExtent;
                        }
                    }
                }
            } else {
                if (deltaX > 0 && MenuItems[0].PositionX < BoundingBox.Left) {
                    if (MenuItems[0].PositionX + deltaX > BoundingBox.Left) {
                        deltaX = BoundingBox.Left - MenuItems[0].PositionX;
                    }
                } else if (deltaX < 0 && MenuItems[0].PositionX + MenuExtent > BoundingBox.Right) {
                    if (MenuItems[0].PositionX + MenuExtent + deltaX < BoundingBox.Right) {
                        deltaX = BoundingBox.Right - (MenuItems[0].PositionX + MenuExtent);
                    }
                } else { return; }

                foreach (MenuItem menuItem in MenuItems) {
                    menuItem.PositionX = (float) Math.Round(menuItem.PositionX + deltaX, 1);
                }
            }
        }

        /// <summary>
        /// Scrolls the submenu items of this <see cref="ScrollMenu"/> vertically.
        /// </summary>
        private void ScrollVertically()
        {
            float deltaY = friction == NoFriction ? kineticScroll.Y : (float) Math.Round(kineticScroll.Y * friction, 1);
            kineticScroll.Y = kineticScroll.Y < 0f ? Math.Min(kineticScroll.Y - deltaY, 0f) : Math.Max(kineticScroll.Y - deltaY, 0f);

            if (deltaY < 0f) {
                ScrollDirection = Direction.Up;
            } else if (deltaY > 0f) {
                ScrollDirection = Direction.Down;
            } else {
                ScrollDirection = Direction.None;
            }

            lastScrollDirection = ScrollDirection;

            if (IsCircular) {
                foreach (MenuItem menuItem in MenuItems) {
                    menuItem.PositionY = (float) Math.Round(menuItem.PositionY + deltaY, 1);

                    if (BoundingBox.Height < MenuExtent) {
                        float bound = menuItem.PositionY + menuItem.SourceRectangle.Height;

                        if (bound > BoundingBox.Bottom + MenuExtent) {
                            menuItem.PositionY = (BoundingBox.Bottom - menuItem.SourceRectangle.Height) + (bound - MenuExtent);
                        } else if (bound <= BoundingBox.Bottom) {
                            menuItem.PositionY += MenuExtent;
                        }
                    }
                }
            } else {
                if (deltaY > 0 && MenuItems[0].PositionY < BoundingBox.Bottom) {
                    if (MenuItems[0].PositionY + deltaY > BoundingBox.Bottom) {
                        deltaY = BoundingBox.Bottom - MenuItems[0].PositionY;
                    }
                } else if (deltaY < 0 && MenuItems[0].PositionY + MenuExtent > BoundingBox.Top) {
                    if (MenuItems[0].PositionY + MenuExtent + deltaY < BoundingBox.Top) {
                        deltaY = BoundingBox.Top - (MenuItems[0].PositionY + MenuExtent);
                    }
                } else { return; }

                foreach (MenuItem menuItem in MenuItems) {
                    menuItem.PositionY = (float) Math.Round(menuItem.PositionY + deltaY, 1);
                }
            }
        }

        /// <summary>
        /// Causes this <see cref="ScrollMenu"/> to perform a kinetic scroll with a
        /// velocity estimated on the basis of the current <see cref="VirtualMass"/>
        /// that lets its menu items stop exactly where they were before starting.
        /// </summary>
        /// <remarks>
        /// Usually <see cref="ScrollTest"/> is called once when the application
        /// first starts to show how the <see cref="ScrollMenu"/> behaves when
        /// dragged or flicked.
        /// </remarks>
        public void ScrollTest()
        {
            ScrollTest(VirtualMass);
        }

        /// <summary>
        /// Causes this <see cref="ScrollMenu"/> to perform a kinetic scroll with
        /// a velocity estimated on the basis of the specified virtual mass.
        /// </summary>
        /// <param name="virtualMass">
        /// The additional inertia to be applied to the kinetic scroll.
        /// </param>
        /// <seealso cref="ScrollTest"/>
        public void ScrollTest(float virtualMass)
        {
            Vector2 kineticScroll = Vector2.Zero;

            if (Orientation == Orientation.Horizontal) {
                kineticScroll.X = (float) MenuExtent;
            } else {
                kineticScroll.Y = (float) MenuExtent;
            }

            SetKineticScroll(kineticScroll * virtualMass, FrictionWhenFlicking);
        }

        /// <summary>
        /// Sets the kinetic scroll for the submenu items of this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <param name="kineticScroll">The kinetic scroll.</param>
        /// <param name="friction">The friction to apply to <paramref name="kineticScroll"/>.</param>
        private void SetKineticScroll(Vector2 kineticScroll, float friction)
        {
            this.kineticScroll = kineticScroll;
            this.friction = friction;

            // Possible change to SnapMenuItems gets effective only after
            // the current kinetic scroll has ended
            snapMenuItems = SnapMenuItems;
        }

        /// <summary>
        /// Shows this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ScrollMenu"/> has already been disposed of.
        /// </exception>
        public override void Show()
        {
            base.Show();

            foreach (MenuItem menuItem in MenuItems) {
                menuItem.Show();
            }
        }

        /// <summary>
        /// Updates this <see cref="ScrollMenu"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="ScrollMenu"/> has already been disposed of.
        /// </exception>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MenuItems.Count > 0 && kineticScroll != Vector2.Zero) {
                if (Orientation == Orientation.Horizontal) {
                    if (menuExtent > BoundingBox.Width) { ScrollHorizontally(); }
                } else {
                    if (menuExtent > BoundingBox.Height) { ScrollVertically(); }
                }
            }
        }
        #endregion
    }
}
