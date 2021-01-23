#region Header
//+ <source name="ShakeGestureHelper.cs" language="C#" begin="23-Mar-2012">
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
using System.Threading;
using PlaXore.ShakeGestures.Sensors;
#endregion

namespace PlaXore.ShakeGestures
{
    /// <summary>
    /// Provides functionality for dealing with shake gestures.
    /// </summary>
    public class ShakeGesturesHelper
    {
        #region Fields
        private const double DefaultShakeMagnitudeWithoutGravitationThreshold = 0.2;
        private const int DefaultStillCounterThreshold = 20;
        private const double DefaultStillMagnitudeWithoutGravitationThreshold = 0.02;
        private const int DefaultMaximumStillVectorsNeededForAverage = 20;
        private const int DefaultMinimumStillVectorsNeededForAverage = 5;
        private const int DefaultMinimumShakeVectorsNeededForShake = 10;
        private const double DefaultWeakMagnitudeWithoutGravitationThreshold = 0.2;
        private const int DefaultMinimumRequiredMovesForShake = 3;

        private static volatile ShakeGesturesHelper singletonInstance;
        private static Object syncRoot = new Object();
        private Simple3DVector lastStillVector = new Simple3DVector(0, -1, 0);
        private bool isInShakeState = false;
        private int stillCounter = 0;
        private List<Simple3DVector> shakeSignal = new List<Simple3DVector>();
        private int[] shakeHistogram = new int[3];
        private LinkedList<Simple3DVector> stillSignal = new LinkedList<Simple3DVector>();
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a new shake gesture has been identified.
        /// </summary>
        public event EventHandler<ShakeGestureEventArgs> ShakeGesture;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the magnitude threshold above which a vector is
        /// considered a shake vector.
        /// </summary>
        /// <value>
        /// The magnitude threshold above which a vector (after reducing gravitation)
        /// is considered a shake vector.
        /// </value>
        public double ShakeMagnitudeWithoutGravitationThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of consecutive still vectors required to stop
        /// a shake signal.
        /// </summary>
        /// <value>
        /// The amount of consecutive still vectors required to stop a shake signal.
        /// </value>
        public int StillCounterThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum allowed magnitude for a still vector to be
        /// considered to the average. The last still vector is averaged out of
        /// still vectors that are under this bound.
        /// </summary>
        /// <value>
        /// The maximum allowed magnitude (after reducing gravitation) for a still
        /// vector to be considered to the average.
        /// </value>
        public double StillMagnitudeWithoutGravitationThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of still vectors needed to create a
        /// still vector average instead of averaging the entire still signal.
        /// </summary>
        /// <value>
        /// The maximum amount of still vectors needed to create a still vector
        /// average instead of averaging the entire still signal.
        /// </value>
        public int MaximumStillVectorsNeededForAverage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum amount of still vectors needed to create a
        /// still vector average.
        /// </summary>
        /// <value>
        /// The minimum amount of still vectors needed to create a still vector
        /// average.
        /// </value>
        /// <remarks>
        /// Without enough vectors the average will not be stable and thus ignored.
        /// </remarks>
        public int MinimumStillVectorsNeededForAverage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of shake vectors needed that has been classified
        /// the same to recognize a shake.
        /// </summary>
        /// <value>
        /// The amount of shake vectors needed that has been classified the same to
        /// recognize a shake.
        /// </value>
        public int MinimumShakeVectorsNeededForShake
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the magnitude threshold below which shake vectors are
        /// not considered for gesture classification.
        /// </summary>
        /// <value>
        /// The magnitude threshold below which shake vectors are not considered
        /// for gesture classification.
        /// </value>
        public double WeakMagnitudeWithoutGravitationThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of moves required to get a shake signal.
        /// </summary>
        /// <value>
        /// The amount of moves required to get a shake signal.
        /// </value>
        public int MinimumRequiredMovesForShake
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a singleton instance of the <see cref="ShakeGesturesHelper"/> class.
        /// </summary>
        /// <value>
        /// A singleton instance of the <see cref="ShakeGesturesHelper"/> class.
        /// </value>
        public static ShakeGesturesHelper Instance
        {
            get {
                if (ShakeGesturesHelper.singletonInstance == null) {
                    lock (ShakeGesturesHelper.syncRoot) {
                        if (ShakeGesturesHelper.singletonInstance == null) {
                            ShakeGesturesHelper.singletonInstance = new ShakeGesturesHelper();
                        }
                    }
                }

                return ShakeGesturesHelper.singletonInstance;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the accelerometer
        /// handled by this <see cref="ShakeGesturesHelper"/> is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the accelerometer is active;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsActive
        {
            get { return AccelerometerHelper.Instance.IsActive; }
            set { AccelerometerHelper.Instance.IsActive = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ShakeGesturesHelper"/> class.
        /// </summary>
        private ShakeGesturesHelper()
        {
            ShakeMagnitudeWithoutGravitationThreshold = DefaultShakeMagnitudeWithoutGravitationThreshold;
            StillCounterThreshold = DefaultStillCounterThreshold;
            StillMagnitudeWithoutGravitationThreshold = DefaultStillMagnitudeWithoutGravitationThreshold;
            MinimumStillVectorsNeededForAverage = DefaultMinimumStillVectorsNeededForAverage;
            MaximumStillVectorsNeededForAverage = DefaultMaximumStillVectorsNeededForAverage;
            MinimumShakeVectorsNeededForShake = DefaultMinimumShakeVectorsNeededForShake;
            WeakMagnitudeWithoutGravitationThreshold = DefaultWeakMagnitudeWithoutGravitationThreshold;
            MinimumRequiredMovesForShake = DefaultMinimumRequiredMovesForShake;

            // Register for acceleromter input.
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Simulates the specified shake type.
        /// </summary>
        /// <param name="shakeType">One of the <see cref="ShakeType"/> values.</param>
        public void Simulate(ShakeType shakeType)
        {
            bool isActivePrevState = IsActive;

            ThreadPool.QueueUserWorkItem(
                (o) => {
                    IsActive = false;
                    Simulation.CallTo = OnAccelerometerHelperReadingChanged;
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    switch (shakeType) {
                        case ShakeType.X:
                            Simulation.SimulateShakeX();
                            break;

                        case ShakeType.Y:
                            Simulation.SimulateShakeY();
                            break;

                        case ShakeType.Z:
                            Simulation.SimulateShakeZ();
                            break;
                    }

                    Simulation.CallTo = null;
                    IsActive = isActivePrevState;
                });
        }

        /// <summary>
        /// Handles the <see cref="AccelerometerHelper.ReadingChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="AccelerometerHelper"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs args)
        {
            Simple3DVector currentVector = args.OptimalyFilteredAcceleration;
            bool isShakeMagnitude = (Math.Abs(lastStillVector.Magnitude - currentVector.Magnitude) > ShakeMagnitudeWithoutGravitationThreshold);

            if ((!isInShakeState) && (isShakeMagnitude)) {
                isInShakeState = true;

                ClearShakeSignal();
                ProcessStillSignal();
                AddVectorToShakeSignal(currentVector);
            } else if ((!isInShakeState) && (!isShakeMagnitude)) {
                AddVectorToStillSignal(currentVector);
            } else if ((isInShakeState) && (isShakeMagnitude)) {
                AddVectorToShakeSignal(currentVector);

                stillCounter = 0;

                if (ProcessShakeSignal()) {
                    ClearShakeSignal();
                }
            } else if ((isInShakeState) && (!isShakeMagnitude)) {
                AddVectorToShakeSignal(currentVector);

                stillCounter++;

                if (stillCounter > StillCounterThreshold) {
                    stillSignal.Clear();

                    for (int i = 0; i < StillCounterThreshold; ++i) {
                        int currentSampleIndex = shakeSignal.Count - StillCounterThreshold + i;
                        AddVectorToStillSignal(currentVector);
                    }

                    shakeSignal.RemoveRange(shakeSignal.Count - StillCounterThreshold, StillCounterThreshold);
                    isInShakeState = false;
                    stillCounter = 0;

                    if (ProcessShakeSignal()) {
                        ClearShakeSignal();
                    }
                }
            }
        }

        /// <summary>
        /// Adds a vector to the still signal.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        private void AddVectorToStillSignal(Simple3DVector vector)
        {
            stillSignal.AddFirst(vector);

            if (stillSignal.Count > 2 * MaximumStillVectorsNeededForAverage) {
                stillSignal.RemoveLast();
            }
        }

        /// <summary>
        /// Adds a vector to the shake signal.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        private void AddVectorToShakeSignal(Simple3DVector vector)
        {
            Simple3DVector currentVectorWithoutGravitation = vector - lastStillVector;
            shakeSignal.Add(currentVectorWithoutGravitation);

            if (currentVectorWithoutGravitation.Magnitude < WeakMagnitudeWithoutGravitationThreshold) {
                return;
            }

            ShakeType vectorShakeType = ClassifyVectorShakeType(currentVectorWithoutGravitation);
            shakeHistogram[(int) vectorShakeType]++;
        }

        /// <summary>
        /// Clears the shake signal.
        /// </summary>
        private void ClearShakeSignal()
        {
            shakeSignal.Clear();
            Array.Clear(shakeHistogram, 0, shakeHistogram.Length);
        }

        /// <summary>
        /// Calculates the average still vector.
        /// </summary>
        private void ProcessStillSignal()
        {
            Simple3DVector sumVector = new Simple3DVector(0, 0, 0);
            int count = 0;

            foreach (Simple3DVector currentStillVector in stillSignal) {
                bool isStillMagnitude = (Math.Abs(lastStillVector.Magnitude - currentStillVector.Magnitude) < StillMagnitudeWithoutGravitationThreshold);

                if (isStillMagnitude) {
                    sumVector += currentStillVector;

                    if (++count >= MaximumStillVectorsNeededForAverage) {
                        break;
                    }
                }
            }

            if (count >= MinimumStillVectorsNeededForAverage) {
                lastStillVector = sumVector / count;
            }
        }

        /// <summary>
        /// Classifies the vector shake type.
        /// </summary>
        /// <param name="vector">The vector for which to classify the shake type.</param>
        private ShakeType ClassifyVectorShakeType(Simple3DVector vector)
        {
            double absX = Math.Abs(vector.X);
            double absY = Math.Abs(vector.Y);
            double absZ = Math.Abs(vector.Z);

            if ((absX >= absY) & (absX >= absZ)) {
                return ShakeType.X;
            }

            if ((absY >= absX) & (absY >= absZ)) {
                return ShakeType.Y;
            }

            return ShakeType.Z;
        }

        /// <summary>
        /// Processes the shake signal according to the shake histogram.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the shake signal was processed successfully;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool ProcessShakeSignal()
        {
            int xCount = shakeHistogram[0];
            int yCount = shakeHistogram[1];
            int zCount = shakeHistogram[2];

            ShakeType? shakeType = null;

            if ((xCount >= yCount) & (xCount >= zCount) & (xCount >= MinimumShakeVectorsNeededForShake)) {
                shakeType = ShakeType.X;
            } else if ((yCount >= xCount) & (yCount >= zCount) & (yCount >= MinimumShakeVectorsNeededForShake)) {
                shakeType = ShakeType.Y;
            } else if ((zCount >= xCount) & (zCount >= yCount) & (zCount >= MinimumShakeVectorsNeededForShake)) {
                shakeType = ShakeType.Z;
            }

            if (shakeType != null) {
                int countSignsChanges = CountSignChanges(shakeType.Value);

                if (countSignsChanges < MinimumRequiredMovesForShake) {
                    shakeType = null;
                }
            }

            if (shakeType != null) {
                EventHandler<ShakeGestureEventArgs> localShakeGesture = ShakeGesture;

                if (localShakeGesture != null) {
                    localShakeGesture(this, new ShakeGestureEventArgs(shakeType.Value));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Counts the shakes of the shake signal.
        /// </summary>
        /// <param name="shakeType">One of the <see cref="ShakeType"/> values.</param>
        private int CountSignChanges(ShakeType shakeType)
        {
            int countSignsChanges = 0;
            int currentSign = 0;
            int? prevSign = null;

            for (int i = 0; i < shakeSignal.Count; ++i) {
                switch (shakeType) {
                    case ShakeType.X:
                        currentSign = Math.Sign(shakeSignal[i].X);
                        break;

                    case ShakeType.Y:
                        currentSign = Math.Sign(shakeSignal[i].Y);
                        break;

                    case ShakeType.Z:
                        currentSign = Math.Sign(shakeSignal[i].Z);
                        break;
                }

                if (currentSign == 0) { continue; }
                if (prevSign == null) { prevSign = currentSign; }
                if (currentSign != prevSign) { ++countSignsChanges; }

                prevSign = currentSign;
            }

            return countSignsChanges;
        }
        #endregion
    }
}
