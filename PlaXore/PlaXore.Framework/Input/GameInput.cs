#region Header
//+ <source name=GameInput.cs" language="C#" begin="10-Aug-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
#endregion

namespace PlaXore.GameFramework.Input
{
    /// <summary>
    /// Provides functionality for dealing with inputs defined for the current
    /// <see cref="GameHost"/>.
    /// </summary>
    public class GameInput
    {
        #region Fields
        private Dictionary<object, Input> inputs;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInput"/> class.
        /// </summary>
        public GameInput()
        {
            inputs = new Dictionary<object, Input>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified keyboard key to this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="key"/>.</param>
        /// <param name="key">The key to add to this <see cref="GameInput"/>.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="key"/> repeats
        /// the same action until it is released.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddKeyboardInput(object tag, Keys key, bool repeatActionOnHold)
        {
            GetInput(tag, true).AddKeyboardInput(key, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified game pad button to this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="buttonType"/>.</param>
        /// <param name="buttonType">One of the <b>Buttons</b> value.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="buttonType"/>
        /// repeats the same action until it is released.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddGamePadInput(object tag, Buttons buttonType, bool repeatActionOnHold)
        {
            GetInput(tag, true).AddGamePadInput(buttonType, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified touch area to this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="touchArea"/>.</param>
        /// <param name="touchArea">The touch area to add to this <see cref="GameInput"/>.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="touchArea"/> repeats
        /// the same action until it is released.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddTapInput(object tag, Rectangle touchArea, bool repeatActionOnHold)
        {
            GetInput(tag, true).AddTapInput(touchArea, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified sliding <see cref="Direction"/> to this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="direction"/>.</param>
        /// <param name="direction">
        /// The sliding <see cref="Direction"/> to add to this <see cref="GameInput"/>.
        /// </param>
        /// <param name="slideDistance">
        /// Specifies how far to slide the finger to trigger an action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddSlideInput(object tag, Direction direction, float slideDistance)
        {
            GetInput(tag, true).AddSlideInput(direction, slideDistance);
        }

        /// <summary>
        /// Adds the specified <b>GestureType</b> to this <see cref="GameInput"/>, in
        /// the specified touch area.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="gestureType"/>.</param>
        /// <param name="gestureType">One of the <b>GestureType</b> values.</param>
        /// <param name="touchArea">The touch area affected by this <see cref="GameInput"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddGestureInput(object tag, GestureType gestureType, Rectangle touchArea)
        {
            GetInput(tag, true).AddGestureInput(gestureType, touchArea);
        }

        /// <summary>
        /// Adds the specified tilt <see cref="Direction"/> to this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="direction"/>.</param>
        /// <param name="direction">One of the <see cref="Direction"/> values.</param>
        /// <param name="tiltThreshold">The tilt threshold for this <see cref="GameInput"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void AddAccelerometerInput(object tag, Direction direction, float tiltThreshold)
        {
            GetInput(tag, true).AddAccelerometerInput(direction, tiltThreshold);
        }

        /// <summary>
        /// Removes the specified keyboard key from this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="key"/>.</param>
        /// <param name="key">The key to remove from this <see cref="GameInput"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveKeyboardInput(object tag, Keys key)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveKeyboardInput(key); }
        }

        /// <summary>
        /// Removes the specified game pad button from this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="buttonType"/>.</param>
        /// <param name="buttonType">One of the <b>Buttons</b> value.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveGamePadInput(object tag, Buttons buttonType)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveGamePadInput(buttonType); }
        }

        /// <summary>
        /// Removes the specified touch area from this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="touchArea"/>.</param>
        /// <param name="touchArea">The touch area to remove from this <see cref="GameInput"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveTapInput(object tag, Rectangle touchArea)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveTapInput(touchArea); }
        }

        /// <summary>
        /// Removes the specified sliding <see cref="Direction"/> from this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="direction"/>.</param>
        /// <param name="direction">
        /// The sliding <see cref="Direction"/> to remove from this <see cref="GameInput"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveSlideInput(object tag, Direction direction)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveSlideInput(direction); }
        }

        /// <summary>
        /// Removes the specified <b>GestureType</b> from this <see cref="GameInput"/>, in
        /// the specified touch area.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="gestureType"/>.</param>
        /// <param name="gestureType">One of the <b>GestureType</b> values.</param>
        /// <param name="touchArea">The touch area to remove from this <see cref="GameInput"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveGestureInput(object tag, GestureType gestureType, Rectangle touchArea)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveGestureInput(gestureType, touchArea); }
        }

        /// <summary>
        /// Removes the specified tilt <see cref="Direction"/> from this <see cref="GameInput"/>.
        /// </summary>
        /// <param name="tag">The action tag for <paramref name="direction"/>.</param>
        /// <param name="direction">One of the <see cref="Direction"/> values.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public void RemoveAccelerometerInput(object tag, Direction direction)
        {
            Input input = GetInput(tag, false);
            if (input != null) { input.RemoveAccelerometerInput(direction); }
        }

        /// <summary>
        /// Sets the current state of the game pad, touch panel, and keyboard.
        /// </summary>
        public void BeginUpdate()
        {
            Input.BeginUpdate();
        }

        /// <summary>
        /// Sets the previous state of the game pad, touch panel, and keyboard.
        /// </summary>
        public void EndUpdate()
        {
            Input.EndUpdate();
        }

        /// <summary>
        /// Returns a value indicating whether or not the game pad controller based
        /// on the specified <b>PlayerIndex</b> is connected.
        /// </summary>
        /// <param name="playerIndex">
        /// The <b>PlayerIndex</b> the game pad controller is based on.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the game pad controller is connected;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsConnected(PlayerIndex playerIndex)
        {
            if (!Input.GamePadConnectionState[playerIndex]) { return true; }
            return Input.IsConnected(playerIndex);
        }

        /// <summary>
        /// Returns a value indicating whether or not <b>PlayerIndex.One</b>
        /// provided the appropriate input for the action identified by the
        /// specified tag.
        /// </summary>
        /// <param name="tag">The tag that identifies the action on the local inputs.</param>
        /// <returns>
        /// <see langword="true"/> if <b>PlayerIndex.One</b> provided the appropriate
        /// input; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public bool IsPressed(object tag)
        {
            if (tag == null) { throw new ArgumentNullException("tag"); }
            if (!inputs.ContainsKey(tag)) { return false; }
            return inputs[tag].IsPressed(PlayerIndex.One);
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified player
        /// provided the appropriate input for the action identified by the
        /// specified tag.
        /// </summary>
        /// <param name="tag">The tag that identifies the action on the local inputs.</param>
        /// <param name="playerIndex">
        /// The player to be expected to provide the appropriate input
        /// for the action identified by <paramref name="tag"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="playerIndex"/> provided the
        /// appropriate input; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public bool IsPressed(object tag, PlayerIndex playerIndex)
        {
            if (tag == null) { throw new ArgumentNullException("tag"); }
            if (!inputs.ContainsKey(tag)) { return false; }
            return inputs[tag].IsPressed(playerIndex);
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified player
        /// provided the appropriate input for the action identified by the
        /// specified tag.
        /// </summary>
        /// <param name="tag">The tag that identifies the action on the local inputs.</param>
        /// <param name="playerIndex">
        /// The player to be expected to provide the appropriate input
        /// for the action identified by <paramref name="tag"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="playerIndex"/> provided the
        /// appropriate input; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public bool IsPressed(object tag, PlayerIndex? playerIndex)
        {
            if (playerIndex == null) {
                PlayerIndex controllingPlayer;
                return IsPressed(tag, playerIndex, out controllingPlayer);
            }

            return IsPressed(tag, (PlayerIndex) playerIndex);
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified player
        /// provided the appropriate input for the action identified by the
        /// specified tag.
        /// </summary>
        /// <param name="tag">The tag that identifies the action on the local inputs.</param>
        /// <param name="playerIndex">
        /// The player to be expected to provide the appropriate input
        /// for the action identified by <paramref name="tag"/>.
        /// </param>
        /// <param name="controllingPlayer">
        /// Output parameter set to the actual player that raised the action identified
        /// by <paramref name="tag"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="playerIndex"/> provided the
        /// appropriate input; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public bool IsPressed(object tag, PlayerIndex? playerIndex, out PlayerIndex controllingPlayer)
        {
            if (tag == null) { throw new ArgumentNullException("tag"); }

            if (!inputs.ContainsKey(tag)) {
                controllingPlayer = PlayerIndex.One;
                return false;
            }

            if (playerIndex == null) {
                if (IsPressed(tag, PlayerIndex.One)) {
                    controllingPlayer = PlayerIndex.One;
                    return true;
                }

                if (IsPressed(tag, PlayerIndex.Two)) {
                    controllingPlayer = PlayerIndex.Two;
                    return true;
                }

                if (IsPressed(tag, PlayerIndex.Three)) {
                    controllingPlayer = PlayerIndex.Three;
                    return true;
                }

                if (IsPressed(tag, PlayerIndex.Four)) {
                    controllingPlayer = PlayerIndex.Four;
                    return true;
                }

                controllingPlayer = PlayerIndex.One;
                return false;
            }

            controllingPlayer = (PlayerIndex) playerIndex;
            return IsPressed(tag, (PlayerIndex) playerIndex);
        }

        /// <summary>
        /// Returns the gesture delta for the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the gesture delta for.
        /// </param>
        /// <returns>
        /// The gesture delta for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector2 GetGestureDelta(object tag)
        {
            return GetInput(tag, true).GestureDelta;
        }

        /// <summary>
        /// Returns the second gesture delta used in multiple point gestures for
        /// the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the second gesture delta for.
        /// </param>
        /// <returns>
        /// The second gesture delta for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector2 GetGestureDelta2(object tag)
        {
            return GetInput(tag, true).GestureDelta2;
        }

        /// <summary>
        /// Returns the gesture position for the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the gesture position for.
        /// </param>
        /// <returns>
        /// The gesture position for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector2 GetGesturePosition(object tag)
        {
            return GetInput(tag, true).GesturePosition;
        }

         /// <summary>
        /// Returns the second gesture position used in multiple point gestures for
        /// the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the second gesture position for.
        /// </param>
        /// <returns>
        /// The second gesture position for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector2 GetGesturePosition2(object tag)
        {
            return GetInput(tag, true).GesturePosition2;
        }

        /// <summary>
        /// Returns the touch position for the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the touch position for.
        /// </param>
        /// <returns>
        /// The touch position for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector2 GetTouchPosition(object tag)
        {
            Vector2? position = GetInput(tag, true).CurrentTouchPosition;
            if (position == null) { return new Vector2(-1f, -1f); }
            return position.Value;
        }

        /// <summary>
        /// Returns the pinch scale change for the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the pinch scale change for.
        /// </param>
        /// <returns>
        /// The pinch scale change for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public float GetGestureScaleChange(object tag)
        {
            // Scaling is dependent on the Pinch gesture; if no input has been setup
            // for pinch then just return 0 indicating no scale change has occurred
            if (!GetInput(tag, true).IsPinchGestureAvailable) { return 0f; }

            // Get the current and previous locations of the two fingers
            Vector2 currPositionFingerOne = GetGesturePosition(tag);
            Vector2 prevPositionFingerOne = GetGesturePosition(tag) - GetGestureDelta(tag);
            Vector2 currPositionFingerTwo = GetGesturePosition2(tag);
            Vector2 prevPositionFingerTwo = GetGesturePosition2(tag) - GetGestureDelta2(tag);

            // Figure out the distance between the current and previous locations
            float currDistance = Vector2.Distance(currPositionFingerOne, currPositionFingerTwo);
            float prevDistance = Vector2.Distance(prevPositionFingerOne, prevPositionFingerTwo);

            // Calculate the difference between the two and use that to alter the scale
            float scaleChange = (currDistance - prevDistance) * .01f;
            return scaleChange;
        }

        /// <summary>
        /// Returns the accelerometer reading for the action identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the action to get the accelerometer reading for.
        /// </param>
        /// <returns>
        /// The accelerometer reading for the action identified by <paramref name="tag"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        public Vector3 GetAccelerometerReading(object tag)
        {
            return GetInput(tag, true).AccelerometerReading;
        }

        /// <summary>
        /// Returns the <see cref="Input"/> identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the <see cref="Input"/> to get.
        /// </param>
        /// <param name="create">
        /// A value indicating whether or not to create the <see cref="Input"/> if it does not
        /// already exist.
        /// </param>
        /// <returns>The <see cref="Input"/> identified by <paramref name="tag"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is empty or <see langword="null"/>.
        /// </exception>
        private Input GetInput(object tag, bool create)
        {
            if (tag == null) { throw new ArgumentNullException("tag"); }

            Input input = null;

            if (inputs.ContainsKey(tag)) {
                input = inputs[tag];
            } else if (create) {
                input = new Input();
                inputs.Add(tag, input);
            }

            return input;
        }
        #endregion
    }
}
