#region Header
//+ <source name=Input.cs" language="C#" begin="10-Aug-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
#if WINDOWS_PHONE
using Microsoft.Devices.Sensors;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace PlaXore.GameFramework.Input
{
    /// <summary>
    /// Stores information about each input type defined for the current
    /// <see cref="GameHost"/>.
    /// </summary>
    internal class Input
    {
        #region Fields
#if WINDOWS_PHONE
        private static bool isAccelerometerStarted;
        private static Accelerometer accelerometerSensor;
        private static Vector3 currentAccelerometerReading;
#endif
        private static Dictionary<PlayerIndex, bool> gamePadConnectionState;
        private static Dictionary<PlayerIndex, GamePadState> currGamePadState;
        private static Dictionary<PlayerIndex, GamePadState> prevGamePadState;
        private static TouchCollection currTouchCollection;
        private static TouchCollection prevTouchCollection;
        private static KeyboardState currKeyboardState;
        private static KeyboardState prevKeyboardState;
        private static List<GestureDefinition> detectedGestures;

        private GestureDefinition currGestureDefinition;
        private Dictionary<Keys, bool> keyboardInputs;
        private Dictionary<Buttons, bool> gamePadInputs;
        private Dictionary<Rectangle, bool> tapInputs;
        private Dictionary<Direction, float> slideInputs;
        private Dictionary<int, GestureDefinition> gestureInputs;
#if WINDOWS_PHONE
        private Dictionary<Direction, float> accelerometerInputs;
#endif
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes static members.
        /// </summary>
        static Input()
        {
            Input.gamePadConnectionState = new Dictionary<PlayerIndex, bool>();
            Input.currGamePadState = new Dictionary<PlayerIndex, GamePadState>();
            Input.prevGamePadState = new Dictionary<PlayerIndex, GamePadState>();
            Input.detectedGestures = new List<GestureDefinition>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        public Input()
        {
            keyboardInputs = new Dictionary<Keys, bool>();
            gamePadInputs = new Dictionary<Buttons, bool>();
            tapInputs = new Dictionary<Rectangle, bool>();
            slideInputs = new Dictionary<Direction, float>();
            gestureInputs = new Dictionary<int, GestureDefinition>();
#if WINDOWS_PHONE
            accelerometerInputs = new Dictionary<Direction, float>();
#endif

            if (Input.currGamePadState.Count == 0) {
                Input.currGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                Input.currGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                Input.currGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                Input.currGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                Input.prevGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                Input.prevGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                Input.prevGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                Input.prevGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                Input.gamePadConnectionState.Add(PlayerIndex.One, Input.currGamePadState[PlayerIndex.One].IsConnected);
                Input.gamePadConnectionState.Add(PlayerIndex.Two, Input.currGamePadState[PlayerIndex.Two].IsConnected);
                Input.gamePadConnectionState.Add(PlayerIndex.Three, Input.currGamePadState[PlayerIndex.Three].IsConnected);
                Input.gamePadConnectionState.Add(PlayerIndex.Four, Input.currGamePadState[PlayerIndex.Four].IsConnected);
            }

#if WINDOWS_PHONE
            if (Input.accelerometerSensor == null) {
                Input.accelerometerSensor = new Accelerometer();
                Input.accelerometerSensor.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(OnCurrentValueChanged);
            }
#endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current touch position.
        /// </summary>
        /// <value>
        /// The current touch position, if any; otherwise, <see langword="null"/>.
        /// </value>
        public Vector2? CurrentTouchPosition
        {
            get { return GetTouchPosition(Input.currTouchCollection); }
        }

        /// <summary>
        /// Gets the previous touch position.
        /// </summary>
        /// <value>
        /// The previous touch position, if any; otherwise, <see langword="null"/>.
        /// </value>
        public Vector2? PreviousTouchPosition
        {
            get { return GetTouchPosition(Input.prevTouchCollection); }
        }

        /// <summary>
        /// Gets the gesture position.
        /// </summary>
        /// <value>The gesture position.</value>
        public Vector2 GesturePosition
        {
            get {
                if (currGestureDefinition == null) { return Vector2.Zero; }
                return currGestureDefinition.Position;
            }
        }

        /// <summary>
        /// Gets the second gesture position used in multiple point gestures.
        /// </summary>
        /// <value>The second gesture position.</value>
        public Vector2 GesturePosition2
        {
            get {
                if (currGestureDefinition == null) { return Vector2.Zero; }
                return currGestureDefinition.Position2;
            }
        }

        /// <summary>
        /// Gets the gesture delta.
        /// </summary>
        /// <value>The gesture delta.</value>
        public Vector2 GestureDelta
        {
            get {
                if (currGestureDefinition == null) { return Vector2.Zero; }
                return currGestureDefinition.Delta;
            }
        }

        /// <summary>
        /// Gets the second gesture delta used in multiple point gestures.
        /// </summary>
        /// <value>The second gesture delta.</value>
        public Vector2 GestureDelta2
        {
            get {
                if (currGestureDefinition == null) { return Vector2.Zero; }
                return currGestureDefinition.Delta2;
            }
        }

        /// <summary>
        /// Gets the current accelerometer reading.
        /// </summary>
        /// <value>The current accelerometer reading.</value>
        public Vector3 AccelerometerReading
        {
            get {
#if WINDOWS_PHONE
                return Input.currentAccelerometerReading;
#else
                throw new NotSupportedException();
#endif
            }
        }


        /// <summary>
        /// Gets the state of the game pad connection.
        /// </summary>
        /// <value>The state of the game pad connection.</value>
        public static Dictionary<PlayerIndex, bool> GamePadConnectionState
        {
            get { return Input.gamePadConnectionState; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the pinch gesture is available.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the pinch gesture is available; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool IsPinchGestureAvailable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the touch rectangle.
        /// </summary>
        /// <value>The touch rectangle.</value>
        private Rectangle TouchRectangle
        {
            get {
                Vector2? touchPosition = CurrentTouchPosition;
                if (touchPosition == null) { return Rectangle.Empty; }

                return new Rectangle(
                    (int) touchPosition.Value.X - 5,
                    (int) touchPosition.Value.Y - 5,
                    10,
                    10);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the current state of the game pad, touch panel, and keyboard.
        /// </summary>
        static public void BeginUpdate()
        {
            Input.currGamePadState[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            Input.currGamePadState[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            Input.currGamePadState[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            Input.currGamePadState[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            Input.currTouchCollection = TouchPanel.GetState();
            Input.currKeyboardState = Keyboard.GetState(PlayerIndex.One);

            Input.detectedGestures.Clear();

            if (TouchPanel.EnabledGestures != GestureType.None) {
                while (TouchPanel.IsGestureAvailable) {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    Input.detectedGestures.Add(new GestureDefinition(gesture));
                }
            }
        }

        /// <summary>
        /// Sets the previous state of the game pad, touch panel, and keyboard.
        /// </summary>
        static public void EndUpdate()
        {
            Input.prevGamePadState[PlayerIndex.One] = Input.currGamePadState[PlayerIndex.One];
            Input.prevGamePadState[PlayerIndex.Two] = Input.currGamePadState[PlayerIndex.Two];
            Input.prevGamePadState[PlayerIndex.Three] = Input.currGamePadState[PlayerIndex.Three];
            Input.prevGamePadState[PlayerIndex.Four] = Input.currGamePadState[PlayerIndex.Four];

            Input.prevTouchCollection = Input.currTouchCollection;
            Input.prevKeyboardState = Input.currKeyboardState;
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Handles the <b>AccelerometerSensor.CurrentValueChanged</b> event.
        /// </summary>
        /// <param name="sender">The accelerometer that raised the event.</param>
        /// <param name="args">The event data.</param>
        private void OnCurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> args)
        {
            Input.currentAccelerometerReading.X = args.SensorReading.Acceleration.X;
            Input.currentAccelerometerReading.Y = args.SensorReading.Acceleration.Y;
            Input.currentAccelerometerReading.Z = args.SensorReading.Acceleration.Z;
        }
#endif

        /// <summary>
        /// Adds the specified keyboard key to this <see cref="Input"/>.
        /// </summary>
        /// <param name="key">The key to add to this <see cref="Input"/>.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="key"/> repeats
        /// the same action until it is released.
        /// </param>
        public void AddKeyboardInput(Keys key, bool repeatActionOnHold)
        {
            if (keyboardInputs.ContainsKey(key)) {
                keyboardInputs[key] = repeatActionOnHold;
                return;
            }

            keyboardInputs.Add(key, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified game pad button to this <see cref="Input"/>.
        /// </summary>
        /// <param name="buttonType">One of the <b>Buttons</b> value.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="buttonType"/>
        /// repeats the same action until it is released.
        /// </param>
        public void AddGamePadInput(Buttons buttonType, bool repeatActionOnHold)
        {
            if (gamePadInputs.ContainsKey(buttonType)) {
                gamePadInputs[buttonType] = repeatActionOnHold;
                return;
            }

            gamePadInputs.Add(buttonType, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified touch area to this <see cref="Input"/>.
        /// </summary>
        /// <param name="touchArea">The touch area to add to this <see cref="Input"/>.</param>
        /// <param name="repeatActionOnHold">
        /// A value indicating whether or not holding down <paramref name="touchArea"/> repeats
        /// the same action until it is released.
        /// </param>
        public void AddTapInput(Rectangle touchArea, bool repeatActionOnHold)
        {
            if (tapInputs.ContainsKey(touchArea)) {
                tapInputs[touchArea] = repeatActionOnHold;
                return;
            }

            tapInputs.Add(touchArea, repeatActionOnHold);
        }

        /// <summary>
        /// Adds the specified sliding <see cref="Direction"/> to this <see cref="Input"/>.
        /// </summary>
        /// <param name="direction">
        /// The sliding <see cref="Direction"/> to add to this <see cref="Input"/>.
        /// </param>
        /// <param name="slideDistance">
        /// Specifies how far to slide the finger to trigger an action.
        /// </param>
        public void AddSlideInput(Direction direction, float slideDistance)
        {
            if (slideInputs.ContainsKey(direction)) {
                slideInputs[direction] = slideDistance;
                return;
            }

            slideInputs.Add(direction, slideDistance);
        }

        /// <summary>
        /// Adds the specified <b>GestureType</b> to this <see cref="Input"/>, in
        /// the specified touch area.
        /// </summary>
        /// <param name="gestureType">One of the <b>GestureType</b> values.</param>
        /// <param name="touchArea">The touch area affected by this <see cref="Input"/>.</param>
        public void AddGestureInput(GestureType gestureType, Rectangle touchArea)
        {
            TouchPanel.EnabledGestures |= gestureType;
            gestureInputs.Add(gestureInputs.Count, new GestureDefinition(gestureType, touchArea));
            if (gestureType == GestureType.Pinch) { IsPinchGestureAvailable = true; }
        }

        /// <summary>
        /// Adds the specified tilt <see cref="Direction"/> to this <see cref="Input"/>.
        /// </summary>
        /// <param name="direction">One of the <see cref="Direction"/> values.</param>
        /// <param name="tiltThreshold">The tilt threshold for this <see cref="Input"/>.</param>
        public void AddAccelerometerInput(Direction direction, float tiltThreshold)
        {
#if WINDOWS_PHONE
            if (!Input.isAccelerometerStarted) {
                try {
                    Input.accelerometerSensor.Start();
                    Input.isAccelerometerStarted = true;
                } catch (AccelerometerFailedException e) {
                    Input.isAccelerometerStarted = false;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            accelerometerInputs.Add(direction, tiltThreshold);
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Removes the specified keyboard key from this <see cref="Input"/>.
        /// </summary>
        /// <param name="key">The key to remove from this <see cref="Input"/>.</param>
        public void RemoveKeyboardInput(Keys key)
        {
            keyboardInputs.Remove(key);
        }

        /// <summary>
        /// Removes the specified game pad button from this <see cref="Input"/>.
        /// </summary>
        /// <param name="buttonType">One of the <b>Buttons</b> value.</param>
        public void RemoveGamePadInput(Buttons buttonType)
        {
            gamePadInputs.Remove(buttonType);
        }

        /// <summary>
        /// Removes the specified touch area from this <see cref="Input"/>.
        /// </summary>
        /// <param name="touchArea">The touch area to remove from this <see cref="Input"/>.</param>
        public void RemoveTapInput(Rectangle touchArea)
        {
            tapInputs.Remove(touchArea);
        }

        /// <summary>
        /// Removes the specified sliding <see cref="Direction"/> from this <see cref="Input"/>.
        /// </summary>
        /// <param name="direction">
        /// The sliding <see cref="Direction"/> to remove from this <see cref="Input"/>.
        /// </param>
        public void RemoveSlideInput(Direction direction)
        {
            slideInputs.Remove(direction);
        }

        /// <summary>
        /// Removes the specified <b>GestureType</b> from this <see cref="Input"/>, in
        /// the specified touch area.
        /// </summary>
        /// <param name="gestureType">One of the <b>GestureType</b> values.</param>
        /// <param name="touchArea">The touch area to remove from this <see cref="Input"/>.</param>
        public void RemoveGestureInput(GestureType gestureType, Rectangle touchArea)
        {
            TouchPanel.EnabledGestures &= ~gestureType;

            foreach (KeyValuePair<int,GestureDefinition> gestureInput in gestureInputs) {
                if (gestureInput.Value.GestureType == gestureType && gestureInput.Value.GestureArea == touchArea) {
                    if (gestureType == GestureType.Pinch) { IsPinchGestureAvailable = false; }
                    gestureInputs.Remove(gestureInput.Key);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the specified tilt <see cref="Direction"/> from this <see cref="Input"/>.
        /// </summary>
        /// <param name="direction">One of the <see cref="Direction"/> values.</param>
        public void RemoveAccelerometerInput(Direction direction)
        {
#if WINDOWS_PHONE
            accelerometerInputs.Remove(direction);

            if (accelerometerInputs.Count == 0 && Input.isAccelerometerStarted) {
                try {
                    Input.accelerometerSensor.Stop();
                    Input.isAccelerometerStarted = false;
                } catch (AccelerometerFailedException e) {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Returns the first touch position in the specified <b>TouchCollection</b> that
        /// matches one of the supported states.
        /// </summary>
        /// <param name="touchCollection">
        /// The <b>TouchCollection</b> that contains the available touch locations.
        /// </param>
        /// <returns>
        /// The touch position, if any; otherwise, <see langword="null"/>.
        /// </returns>
        private Vector2? GetTouchPosition(TouchCollection touchCollection)
        {
            foreach (TouchLocation touchLocation in touchCollection) {
                switch (touchLocation.State) {
                    case TouchLocationState.Moved:
                    case TouchLocationState.Pressed:
                        return touchLocation.Position;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether or not the game pad controller based
        /// on the specified <b>PlayerIndex</b> is connected.
        /// </summary>
        /// <param name="playerIndex">The <b>PlayerIndex</b> the game pad controller is based on.</param>
        /// <returns>
        /// <see langword="true"/> if the game pad controller is connected;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        static public bool IsConnected(PlayerIndex playerIndex)
        {
            return Input.currGamePadState[playerIndex].IsConnected;
        }

        /// <summary>
        /// Returns a value indicating whether or not this <see cref="Input"/> is pressed.
        /// </summary>
        /// <param name="playerIndex">
        /// In case of game pad input, the <b>PlayerIndex</b> the game pad controller
        /// is based on, otherwise ignored.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="Input"/> is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool IsPressed(PlayerIndex playerIndex)
        {
            return IsPressed(playerIndex, null);
        }

        /// <summary>
        /// Returns a value indicating whether or not this <see cref="Input"/> is pressed
        /// within the specified touch area.
        /// </summary>
        /// <param name="playerIndex">
        /// In case of game pad input, the <b>PlayerIndex</b> the game pad controller
        /// is based on, otherwise ignored.
        /// </param>
        /// <param name="touchArea">The touch area.</param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="Input"/> is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool IsPressed(PlayerIndex playerIndex, Rectangle? touchArea)
        {
            if (IsKeyboardInputPressed())           { return true; }
            if (IsGamePadInputPressed(playerIndex)) { return true; }
            if (IsTapInputPressed())                { return true; }
            if (IsSlideInputPressed())              { return true; }
            if (IsGestureInputPressed(touchArea))   { return true; }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not a keyboard key is pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a keyboard key is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool IsKeyboardInputPressed()
        {
            foreach (Keys key in keyboardInputs.Keys) {
                if (keyboardInputs[key]) {
                    // Repeat action on hold
                    if (Input.currKeyboardState.IsKeyDown(key)
                        && !Input.prevKeyboardState.IsKeyDown(key)) {
                        return true;
                    }
                } else {
                    // Only accept new presses
                    if (Input.currKeyboardState.IsKeyDown(key)) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not an input of the game pad
        /// controller based on the specified <b>PlayerIndex</b> is pressed.
        /// </summary>
        /// <param name="playerIndex">The <b>PlayerIndex</b> the game pad input is based on.</param>
        /// <returns>
        /// <see langword="true"/> if a game pad controller input is pressed;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool IsGamePadInputPressed(PlayerIndex playerIndex)
        {
            foreach (Buttons buttonType in gamePadInputs.Keys) {
                if (gamePadInputs[buttonType]) {
                    // Repeat action on hold
                    if (Input.currGamePadState[playerIndex].IsButtonDown(buttonType)
                        && !Input.prevGamePadState[playerIndex].IsButtonDown(buttonType)) {
                        return true;
                    }
                } else {
                    // Only accept new presses
                    if (Input.currGamePadState[playerIndex].IsButtonDown(buttonType)) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not a tap input is pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a tap input is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool IsTapInputPressed()
        {
            foreach (Rectangle touchArea in tapInputs.Keys) {
                if (tapInputs[touchArea]) {
                    // Repeat action on hold
                    if (touchArea.Intersects(TouchRectangle) && PreviousTouchPosition == null) {
                        return true;
                    }
                } else {
                    // Only accept new presses
                    if (touchArea.Intersects(TouchRectangle)) {
                        return true;
                    }
                } 
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not a slide input is pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a slide input is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool IsSlideInputPressed()
        {
            if (CurrentTouchPosition == null || PreviousTouchPosition == null) {
                return false;
            }

            foreach (Direction slideDirection in slideInputs.Keys) {
                switch (slideDirection) {
                    case Direction.Up:
                        if (CurrentTouchPosition.Value.Y + slideInputs[slideDirection] < PreviousTouchPosition.Value.Y) {
                            return true;
                        }
                        break;
                    case Direction.Down:
                        if (CurrentTouchPosition.Value.Y - slideInputs[slideDirection] > PreviousTouchPosition.Value.Y) {
                            return true;
                        }
                        break;
                    case Direction.Left:
                        if (CurrentTouchPosition.Value.X + slideInputs[slideDirection] < PreviousTouchPosition.Value.X) {
                            return true;
                        }
                        break;
                    case Direction.Right:
                        if (CurrentTouchPosition.Value.X - slideInputs[slideDirection] > PreviousTouchPosition.Value.X) {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not a gesture input is pressed.
        /// </summary>
        /// <param name="gestureArea">The gesture area.</param>
        /// <returns>
        /// <see langword="true"/> if a gesture input is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool IsGestureInputPressed(Rectangle? gestureArea)
        {
            currGestureDefinition = null;

            if (Input.detectedGestures.Count == 0) {
                return false;
            }

            // Check to see if any of the gestures defined in the gestureInputs
            // dictionary have been performed and detected
            foreach (GestureDefinition gestureInput in gestureInputs.Values) {
                foreach (GestureDefinition detectedGesture in Input.detectedGestures) {
                    if (detectedGesture.GestureType == gestureInput.GestureType) {
                        // If a Rectangle area to check against has been passed in, then
                        // use that one, otherwise use the one originally defined
                        Rectangle areaToCheck = gestureInput.GestureArea;
                        if (gestureArea != null) { areaToCheck = gestureArea.Value; }

                        // Gestures are considered detected when they are made in the
                        // area where the user was interested in input (they intersect)
                        // or when they do not provide a position (e.g. Flick)
                        if (detectedGesture.GestureArea.Intersects(areaToCheck) || detectedGesture.GestureArea.IsEmpty) {
                            if (currGestureDefinition == null) {
                                currGestureDefinition = new GestureDefinition(detectedGesture.GestureSample);
                            } else {
                                // Some gestures like FreeDrag and Flick are registered many
                                // times in a single update frame, and since there is only one
                                // variable to store the gesture info, add on any additional
                                // gesture values so there is a combination of all the gesture
                                // information in currentGesture
                                currGestureDefinition.Delta += detectedGesture.Delta;
                                currGestureDefinition.Delta2 += detectedGesture.Delta2;
                                currGestureDefinition.Position += detectedGesture.Position;
                                currGestureDefinition.Position2 += detectedGesture.Position2;
                            }
                        }
                    }
                }
            }

            return currGestureDefinition != null;
        }

        /// <summary>
        /// Returns a value indicating whether or not an accelerometer input is
        /// pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if an accelerometer input is pressed; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        private bool IsAccelerometerInputPressed()
        {
#if WINDOWS_PHONE
            foreach (KeyValuePair<Direction, float> input in accelerometerInputs) {
                switch (input.Key) {
                    case Direction.Up:
                        if (Math.Abs(Input.currentAccelerometerReading.Y) > input.Value
                            && Input.currentAccelerometerReading.Y < 0) {
                            return true;
                        }
                        break;
                    case Direction.Down:
                        if (Math.Abs(Input.currentAccelerometerReading.Y) > input.Value
                            && Input.currentAccelerometerReading.Y > 0) {
                            return true;
                        }
                        break;
                    case Direction.Left:
                        if (Math.Abs(Input.currentAccelerometerReading.X) > input.Value
                            && Input.currentAccelerometerReading.X < 0) {
                            return true;
                        }
                        break;
                    case Direction.Right:
                        if (Math.Abs(Input.currentAccelerometerReading.X) > input.Value
                            && Input.currentAccelerometerReading.X > 0) {
                            return true;
                        }
                        break;
                }
            }

            return false;
#else
            throw new NotSupportedException();
#endif
        }
        #endregion
    }
}
