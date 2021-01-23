#region Header
//+ <source name="GameObject.cs" language="C#" begin="25-Mar-2012">
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
using System.Runtime.Serialization;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Represents a game object.
    /// </summary>
    [DataContract]
    public abstract class GameObject : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class with
        /// the specified <see cref="GameHost"/>.
        /// </summary>
        /// <param name="gameHost">
        /// The <see cref="GameHost"/> associated with this <see cref="GameObject"/>.
        /// </param>
        public GameObject(GameHost gameHost)
            : this()
        {
            GameHost = gameHost;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="GameHost"/> associated with this <see cref="GameObject"/>.
        /// </summary>
        /// <value>
        /// The <see cref="GameHost"/> associated with this <see cref="GameObject"/>.
        /// </value>
        public GameHost GameHost
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="GameObject"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="GameObject"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="GameObject"/>
        /// is visible.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="GameObject"/> is visible;
        /// otherwise, <see langword="false"/>.
        /// </value>
        [DataMember]
        public virtual bool IsVisible
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of times this <see cref="GameObject"/> has
        /// been updated so far.
        /// </summary>
        /// <value>
        /// The number of times this <see cref="GameObject"/> has been updated
        /// so far.
        /// </value>
        [DataMember]
        public int UpdateCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="GameObject"/>
        /// should be updated even when not visible.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="GameObject"/> should be
        /// updated even when not visible; otherwise, <see langword="false"/>.
        /// </value>
        [DataMember]
        public virtual bool UpdateWhenHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag that identifies this <see cref="GameObject"/>.
        /// </summary>
        /// <value>The tag that identifies this <see cref="GameObject"/>.</value>
        [DataMember]
        public string Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not to store this <see cref="GameObject"/>
        /// in the phone state.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="GameObject"/> should be stored in
        /// the phone state; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool WriteToPhoneState
        {
            get { return true; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disposes off this <see cref="GameObject"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="GameObject"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Shows this <see cref="GameObject"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="GameObject"/> has already been disposed of.
        /// </exception>
        public virtual void Show()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            IsVisible = true;
        }

        /// <summary>
        /// Hides this <see cref="GameObject"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="GameObject"/> has already been disposed of.
        /// </exception>
        public virtual void Hide()
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            IsVisible = false;
        }

        /// <summary>
        /// Determines whether or not the specified point is within this
        /// <see cref="GameObject"/>.
        /// </summary>
        /// <param name="point">
        /// The point to check whether or not it is within this <see cref="GameObject"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="point"/> is within this
        /// <see cref="GameObject"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public abstract bool IsPointInObject(Vector2 point);

        /// <summary>
        /// Updates this <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="GameObject"/> has already been disposed of.
        /// </exception>
        /// <remarks>
        /// <see cref="Update"/> increases <see cref="UpdateCount"/> by one.
        /// </remarks>
        public virtual void Update(GameTime gameTime)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            UpdateCount++;
        }
        #endregion
    }
}
