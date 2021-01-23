#region Header
//+ <source name="MenuItemCollection.cs" language="C#" begin="20-Jul-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using System;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents a collection of menu items in a <see cref="Menu"/> control.
    /// <see cref="MenuItemCollection"/> cannot be inherited.
    /// </summary>
    public sealed class MenuItemCollection : ObservableList<MenuItem>
    {
        #region Fields
        private Menu parent;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemCollection"/>
        /// class with the specified parent <see cref="Menu"/>.
        /// </summary>
        /// <param name="parent">The parent <see cref="Menu"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent"/> is <see langword="null"/>.
        /// </exception>
        internal MenuItemCollection(Menu parent)
        {
            if (parent == null) { throw new ArgumentNullException("parent"); }
            this.parent = parent;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="MenuItem"/> at the specified point.
        /// </summary>
        /// <param name="point">The point of the <see cref="MenuItem"/> to get or set.</param>
        /// <returns>The <see cref="MenuItem"/> at <paramref name="point"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The specified <see cref="MenuItem"/> is <see langword="null"/>.
        /// </exception>
        public MenuItem this[Vector2 point]
        {
            get {
                foreach (MenuItem item in this) {
                    if (item.IsPointInObject(point)) { return item; }
                }

                return null;
            }
            set {
                if (value == null) { throw new ArgumentNullException(); }

                for (int i = 0; i < Count; i++) {
                    if (this[i].IsPointInObject(point)) {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="MenuItem"/> identified by the specified tag.
        /// </summary>
        /// <param name="tag">The tag of the <see cref="MenuItem"/> to get or set.</param>
        /// <returns>The <see cref="MenuItem"/> identified by <paramref name="tag"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The specified <see cref="MenuItem"/> is <see langword="null"/>.
        /// </exception>
        public MenuItem this[string tag]
        {
            get {
                foreach (MenuItem item in this) {
                    if (item.Tag == tag) {
                        return item;
                    }
                }

                return null;
            }
            set {
                if (value == null) { throw new ArgumentNullException(); }

                for (int i = 0; i < Count; i++) {
                    if (this[i].Tag == tag) {
                        this[i] = value;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Handles the <see cref="ObservableList&lt;T&gt;.ListChanged"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        protected override void OnListChanged(ListChangedEventArgs<MenuItem> args)
        {
            switch (args.ListChangedType) {
                case ListChangedType.Cleared:
                    foreach (MenuItem item in this) { item.Parent = null; }
                    break;
                case ListChangedType.ItemAdded:
                    if (args.Item == null) { throw new ArgumentNullException("item"); }
                    args.Item.Parent = parent;
                    args.Item.InternalBackBufferRectangle = parent.BackBufferRectangle;
                    for (int i = args.Index; i < Count; i++) { this[i].Index = i; }
                    break;
                case ListChangedType.ItemChanged:
                    if (args.NewItem == null) { throw new ArgumentNullException("item"); }
                    args.NewItem.Index = args.Index;
                    args.NewItem.Parent = parent;
                    args.NewItem.InternalBackBufferRectangle = parent.BackBufferRectangle;
                    args.OldItem.Parent = null;
                    break;
                case ListChangedType.ItemRemoved:
                    args.Item.Parent = null;
                    for (int i = 0; i < Count; i++) { this[i].Index = i; }
                    break;
            }

            base.OnListChanged(args);
        }
        #endregion
    }
}
