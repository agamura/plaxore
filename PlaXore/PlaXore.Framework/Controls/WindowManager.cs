#region Header
//+ <source name="WindowManager.cs" language="C#" begin="10-May-2012">
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
using System.Collections.Generic;
using System.Reflection;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Provides functionality for managing <see cref="Window"/> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="Window"/> instances managed by <see cref="WindowManager"/>
    /// are modal.
    /// </remarks>
    public class WindowManager
    {
        #region Fields
        private Dictionary<string, Window> windows;
        #endregion

        #region Events
        private event EventHandler<EventArgs> _WindowShown;
        private event EventHandler<EventArgs> _WindowHidden;
        private event EventHandler<EventArgs> _WindowClosed;

        /// <summary>
        /// Occurs when a <see cref="Window"/> is displayed.
        /// </summary>
        public event EventHandler<EventArgs> WindowShown
        {
            add {
                _WindowShown += value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Shown += value;
                }
            }
            remove {
                _WindowShown -= value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Shown -= value;
                }
            }
        }

        /// <summary>
        /// Occurs when a <see cref="Window"/> is hidden.
        /// </summary>
        public event EventHandler<EventArgs> WindowHidden
        {
            add {
                _WindowHidden += value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Hidden += value;
                }
            }
            remove {
                _WindowHidden -= value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Hidden -= value;
                }
            }
        }
           
        /// <summary>
        /// Occurs when a <see cref="Window"/> is closed.
        /// </summary>
        public event EventHandler<EventArgs> WindowClosed
        {
            add {
                _WindowClosed += value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Closed += value;
                }
            }
            remove {
                _WindowClosed -= value;

                foreach (KeyValuePair<string, Window> window in windows) {
                    window.Value.Closed -= value;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManager"/> class
        /// with the specified <see cref="GameHost"/>.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        public WindowManager(GameHost gameHost)
            : this(gameHost, Rectangle.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManager"/> class
        /// with the specified <see cref="GameHost"/> and back buffer area.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        /// <param name="backBufferRectangle">
        /// The dimensions of the back buffer rectangle behind the <see cref="Window"/>
        /// instances created by this <see cref="WindowManager"/>.
        /// </param>
        public WindowManager(GameHost gameHost, Rectangle backBufferRectangle)
        {
            windows = new Dictionary<string, Window>();
            GameHost = gameHost;
            BackBufferRectangle = backBufferRectangle;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the active <see cref="Window"/>.
        /// </summary>
        /// <value>The active <see cref="Window"/>.</value>
        public Window ActiveWindow
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the <see cref="GameHost"/> associated with this <see cref="WindowManager"/>.
        /// </summary>
        /// <value>
        /// The <see cref="GameHost"/> associated with this <see cref="WindowManager"/>.
        /// </value>
        public GameHost GameHost
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a <see cref="Window"/> instance of the specified type.
        /// </summary>
        /// <param name="type">The full qualified <see cref="Window"/> type.</param>
        /// <returns>The <see cref="Window"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <see langword="null"/> or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> does not exist.
        /// </exception>
        public Window this[string type]
        {
            get {
                if (String.IsNullOrEmpty(type)) {
                    throw new ArgumentNullException("type");
                }

                if (!windows.ContainsKey(type)) {
                    // First try to get the specified window type from the current assembly
                    Assembly assembly = PortableAssembly.GetExecutingAssembly();
                    Type windowType = assembly.GetType(type);

                    // If the current assembly does not contain the specified window type,
                    // then try to get it from the calling assembly
                    if (windowType == null) {
                        assembly = PortableAssembly.GetCallingAssembly();
                        windowType = assembly.GetType(type);

                        if (windowType == null) {
                            throw new ArgumentException("Type does not exist.", "type");
                        }
                    }

                    Window window = (Window) Activator.CreateInstance(windowType, new object[] { GameHost });
                    window.BackBufferRectangle = BackBufferRectangle;
                    window.Shown += OnWindowShown;
                    window.Hidden += OnWindowHidden;
                    window.Closed += OnWindowClosed;
                    HookEventHandlers(window);
                    windows.Add(type, window);
                }

                return windows[type];
            }
        }

        /// <summary>
        /// Gets or sets the dimensions of the back buffer rectangle behind the
        /// <see cref="Window"/> instances created by this <see cref="WindowManager"/>.
        /// </summary>
        /// <value>
        /// The dimensions of the back buffer rectangle behind the <see cref="Window"/>
        /// instances created by this <see cref="WindowManager"/>.
        /// </value>
        public Rectangle BackBufferRectangle
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Removes the <see cref="Window"/> of the specified type, if any.
        /// </summary>
        /// <param name="type">The <see cref="Window"/> type.</param>
        /// <returns>
        /// The removed <see cref="Window"/>, if found; otherwise, <see langword="null"/>.
        /// </returns>
        public Window Remove(string type)
        {
            Window window = null;

            if (windows.ContainsKey(type)) {
                window = windows[type];
                UnhookEventHandlers(window);
                window.Shown -= OnWindowShown;
                window.Hidden -= OnWindowHidden;
                window.Closed -= OnWindowClosed;
                windows.Remove(type);
            }

            return window;
        }

        /// <summary>
        /// Removes all the inactive <see cref="Window"/> instances from this
        /// <see cref="WindowManager"/>.
        /// </summary>
        public void Clear()
        {
            List<string> keys = new List<string>(this.windows.Count);

            foreach (KeyValuePair<string, Window> window in this.windows) {
                if (window.Value != ActiveWindow) {
                    keys.Add(window.Key);
                }
            }

            foreach (string key in keys) {
                this.windows.Remove(key);
            }
        }

        /// <summary>
        /// Handles the <see cref="Window.Shown"/> event.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="Window"/> that generated the event.
        /// </param>
        /// <param name="args">The event data.</param>
        private void OnWindowShown(object sender, EventArgs args)
        {
            Window window = sender as Window;

            if (ActiveWindow != null && ActiveWindow != window) {
                ActiveWindow.Close();
            }

            ActiveWindow = window;
        }

        /// <summary>
        /// Handles the <see cref="Window.Hidden"/> event.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="Window"/> that generated the event.
        /// </param>
        /// <param name="args">The event data.</param>
        private void OnWindowHidden(object sender, EventArgs args)
        {
            ActiveWindow = null;
        }

        /// <summary>
        /// Handles the <see cref="Window.Closed"/> event.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="Window"/> that generated the event.
        /// </param>
        /// <param name="args">The event data.</param>
        private void OnWindowClosed(object sender, EventArgs args)
        {
            Window window = sender as Window;
            Remove(window.GetType().FullName);
            ActiveWindow = null;
        }

        /// <summary>
        /// Hooks event handlers registered with this <see cref="WindowManager"/>
        /// to the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        /// The <see cref="Window"/> to hook the event handlers to.
        /// </param>
        private void HookEventHandlers(Window window)
        {
            if (_WindowShown != null) {
                foreach (Delegate callback in _WindowShown.GetInvocationList()) {
                    window.Shown += callback as EventHandler<EventArgs>;
                }
            }

            if (_WindowHidden != null) {
                foreach (Delegate callback in _WindowHidden.GetInvocationList()) {
                    window.Hidden += callback as EventHandler<EventArgs>;
                }
            }

            if (_WindowClosed != null) {
                foreach (Delegate callback in _WindowClosed.GetInvocationList()) {
                    window.Closed += callback as EventHandler<EventArgs>;
                }
            }
        }

        /// <summary>
        /// Unhooks event handlers registered with this <see cref="WindowManager"/>
        /// from the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        /// The <see cref="Window"/> to unhook the event handlers from.
        /// </param>
        private void UnhookEventHandlers(Window window)
        {
            if (_WindowShown != null) {
                foreach (Delegate callback in _WindowShown.GetInvocationList()) {
                    window.Shown -= callback as EventHandler<EventArgs>;
                }
            }

            if (_WindowHidden != null) {
                foreach (Delegate callback in _WindowHidden.GetInvocationList()) {
                    window.Hidden -= callback as EventHandler<EventArgs>;
                }
            }

            if (_WindowClosed != null) {
                foreach (Delegate callback in _WindowClosed.GetInvocationList()) {
                    window.Closed -= callback as EventHandler<EventArgs>;
                }
            }
        }
        #endregion
    }
}
