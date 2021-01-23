#region Header
//+ <source name="GameInputEventArgs.cs" language="C#" begin="14-Aug-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using PlaXore.GameFramework.Input;
using System;
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Provides data for input events raised by <see cref="Control"/>
    /// objects.
    /// </summary>
    public class GameInputEventArgs : EventArgs
    {
        #region Fields
        private GameInput gameInput;
        private object tag;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInputEventArgs"/> with
        /// the specified <see cref="GameInput"/> and action tag.
        /// </summary>
        /// <param name="gameInput">The input data.</param>
        /// <param name="tag">The action tag.</param>
        internal GameInputEventArgs(GameInput gameInput, object tag)
            :this(gameInput, tag, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInputEventArgs"/> with
        /// the specified <see cref="GameInput"/>, action tag, and gesture age.
        /// </summary>
        /// <param name="gameInput">The input data.</param>
        /// <param name="tag">The action tag.</param>
        /// <param name="gestureAge">
        /// The number of times the gesture associated with <paramref name="tag"/>
        /// has been detected since the <see cref="Control"/> was pressed.
        /// </param>
        internal GameInputEventArgs(GameInput gameInput, object tag, int gestureAge)
        {
            this.gameInput = gameInput;
            this.tag = tag;
            GestureAge = gestureAge;
        }
        #endregion

        #region Properties
#if WINDOWS_PHONE
        /// <summary>
        /// Gets the accelerometer reading.
        /// </summary>
        /// <value>The accelerometer reading.</value>
        public Vector3 AccelerometerReading
        {
            get { return gameInput.GetAccelerometerReading(tag); }
        }
#endif

        /// <summary>
        /// Gets the gesture delta.
        /// </summary>
        /// <value>The gesture delta.</value>
        public Vector2 GestureDelta
        {
            get { return gameInput.GetGestureDelta(tag); }
        }

        /// <summary>
        /// Gets the second gesture delta.
        /// </summary>
        /// <value>The second gesture delta.</value>
        public Vector2 GestureDelta2
        {
            get { return gameInput.GetGestureDelta2(tag); }
        }

        /// <summary>
        /// Gets the gesture position.
        /// </summary>
        /// <value>The gesture position.</value>
        public Vector2 GesturePosition
        {
            get { return gameInput.GetGesturePosition(tag); }
        }

        /// <summary>
        /// Gets the second gesture position.
        /// </summary>
        /// <value>The second gesture position.</value>
        public Vector2 GesturePosition2
        {
            get { return gameInput.GetGesturePosition2(tag); }
        }

        /// <summary>
        /// Gets the number of times the gesture has been detected since the
        /// <see cref="Control"/> was pressed.
        /// </summary>
        /// <value>
        /// The number of times the gesture has been detected since the
        /// <see cref="Control"/> was pressed.
        /// </value>
        /// <remarks>
        /// <see cref="GestureAge"/> continues to increase by one until the
        /// <see cref="Control"/> is not released.
        /// </remarks>
        public int GestureAge
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the gesture scale change.
        /// </summary>
        /// <value>The gesture scale change.</value>
        public float GestureScaleChange
        {
            get { return gameInput.GetGestureScaleChange(tag); }
        }

        /// <summary>
        /// Gets the touch position.
        /// </summary>
        /// <value>The touch position.</value>
        public Vector2 TouchPosition
        {
            get { return gameInput.GetTouchPosition(tag); }
        }
        #endregion
    }
}
