#region Header
//+ <source name=GestureDefinition.cs" language="C#" begin="10-Aug-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
#endregion

namespace PlaXore.GameFramework.Input
{
    /// <summary>
    /// Provides information about the type of gesture being performed, the
    /// target area where the gesture is expected to be performed, and
    /// information from the actual <b>GestureSample</b>.
    /// </summary>
    internal class GestureDefinition
    {
        #region Fields
        private const int DummySize = 5;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureDefinition"/> class
        /// with the specified <b>GestureType</b> and gesture area.
        /// </summary>
        /// <param name="gestureType">One of the <b>GestureType</b> values.</param>
        /// <param name="gestureArea">The gesture area.</param>
        public GestureDefinition(GestureType gestureType, Rectangle gestureArea)
        {
            GestureSample = new GestureSample(gestureType, new TimeSpan(0),
                Vector2.Zero, Vector2.Zero,
                Vector2.Zero, Vector2.Zero);

            GestureType = gestureType;
            GestureArea = gestureArea;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureDefinition"/> class
        /// with the specified <b>GestureSample</b>.
        /// </summary>
        /// <param name="gestureSample">Data of multitouch gesture over a span of time.</param>
        public GestureDefinition(GestureSample gestureSample)
        {
            GestureSample = gestureSample;
            GestureType = gestureSample.GestureType;

            GestureArea = new Rectangle(
                (int) gestureSample.Position.X, (int) gestureSample.Position.Y,
                gestureSample.Position.X == 0 ? 0 : DummySize,
                gestureSample.Position.Y == 0 ? 0 : DummySize);

            Delta = gestureSample.Delta;
            Delta2 = gestureSample.Delta2;
            Position = gestureSample.Position;
            Position2 = gestureSample.Position2;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <b>GestureType</b>.
        /// </summary>
        /// <value>One of the <b>GestureType</b> values.</value>
        public GestureType GestureType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the gesture area.
        /// </summary>
        /// <value>The gesture area.</value>
        public Rectangle GestureArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <b>GestureSample</b>.
        /// </summary>
        /// <value>Data of multitouch gesture over a span of time.</value>
        public GestureSample GestureSample
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the gesture delta.
        /// </summary>
        /// <value>The gesture delta.</value>
        public Vector2 Delta
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gesture delta2.
        /// </summary>
        /// <value>The gesture delta2.</value>
        public Vector2 Delta2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gesture position.
        /// </summary>
        /// <value>The gesture position.</value>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gesture position2.
        /// </summary>
        /// <value>The gesture position2.</value>
        public Vector2 Position2
        {
            get;
            set;
        }
        #endregion
    }
}
