#region Header
//+ <source name="GameState.cs" language="C#" begin="8-Apr-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ //+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Represents a delegate that is invoked to handle and update the game state.
    /// <param name="gameTime">Time elapsed since the last call to <b>Update</b>.</param>
    /// <returns>The updated game state.</returns>
    /// <remarks>
    /// <see cref="GameState"/> uses the value returned by <see cref="Update(GameTime)"/>
    /// to update <see cref="GameState.Current"/>.
    /// </remarks>
    public delegate int Update(GameTime gameTime);

    /// <summary>
    /// Provides functionality for managing game state transitions.
    /// </summary>
    public class GameState
    {
        #region Fields
        private Update update;
        private Update internalUpdate;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        public GameState()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class
        /// with the specified update delegate.
        /// </summary>
        /// <param name="update"/>
        /// The update delegate to be invoked when the game first starts.
        /// </param>
        /// <seealso cref="GameState.Update"/>
        public GameState(Update update)
        {
            Update = update;
            internalUpdate = Internal_Update;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current value of this <see cref="GameState"/>.
        /// </summary>
        /// <value>
        /// The current value of this <see cref="GameState"/>.
        /// </value>
        public int Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked to handle and update the
        /// current game state.
        /// </summary>
        /// <value>
        /// The delegate to be invoked to handle and update the current game state.
        /// </value>
        /// <seealso cref="GameState.Current"/>
        public Update Update
        {
            get { return internalUpdate; }
            set {
                update = value != null ? value : delegate(GameTime gameTime) {
                    return Current;
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates this <see cref="GameState"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>Internal_Update</b>.</param>
        private int Internal_Update(GameTime gameTime)
        {
            Current = update(gameTime);
            return Current;
        }
        #endregion
    }
}
