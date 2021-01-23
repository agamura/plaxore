#region Header
//+ <source name="AccelerometerHelper.cs" language="C#" begin="24-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Diagnostics;
using Microsoft.Devices.Sensors;
#endregion

namespace PlaXore.ShakeGestures.Sensors
{
    /// <summary>
    /// Provides functionality for filtering and locally calibrating accelerometer sensor data.
    /// </summary>
    public sealed class AccelerometerHelper : IDisposable
    {
        #region Fields
        private const int SamplesCount = 25;
        private const double LowPassFilterCoef = 0.1;
        private const double NoiseMaxAmplitude = 0.05;
        private const double MaxCalibrationTiltAngle = 20.0 * Math.PI / 180.0;
        private const double MaxStabilityTiltDeltaAngle = 0.5 * Math.PI / 180.0;
        private const string AccelerometerCalibrationKeyName = "AccelerometerCalibration";

        private static volatile AccelerometerHelper singletonInstance;
        private static Object syncRoot = new Object();
        private static double maxCalibrationOffset = Math.Sin(MaxCalibrationTiltAngle);
        private static double maxStabilityDeltaOffset = Math.Sin(MaxStabilityTiltDeltaAngle);

        private Accelerometer sensor;
        private int deviceStableCount;
        private bool isInitialized = false;
        private Simple3DVector[] sampleBuffer = new Simple3DVector[SamplesCount];
        private Simple3DVector prevLowPassOutput;
        private Simple3DVector prevOptimalFilterOutput;
        private Simple3DVector sampleSum = new Simple3DVector(0.0 * SamplesCount, 0.0 * SamplesCount, -1.0 * SamplesCount);
        private int sampleIndex;
        private Simple3DVector averageAcceleration;
        private bool isActive = false;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when new raw and processed accelerometer data is available.
        /// </summary>
        /// <remarks>Occurs every 20ms.</remarks>
        public event EventHandler<AccelerometerHelperReadingEventArgs> ReadingChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerHelper"/> class.
        /// </summary>
        private AccelerometerHelper()
        {
            sensor = new Accelerometer();

            if (sensor == null) {
                NoAccelerometer = true;
            } else {
                NoAccelerometer = (sensor.State == SensorState.NotSupported);
            }

            sensor = null;
            sampleIndex = 0;
            ZeroAccelerationCalibrationOffset = AccelerometerCalibrationPersisted;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets a singleton instance of the <see cref="AccelerometerHelper"/> class.
        /// </summary>
        /// <value>
        /// A singleton instance of the <see cref="AccelerometerHelper"/> class.
        /// </value>
        public static AccelerometerHelper Instance
        {
            get {
                if (AccelerometerHelper.singletonInstance == null) {
                    lock (AccelerometerHelper.syncRoot) {
                        if (AccelerometerHelper.singletonInstance == null) {
                            AccelerometerHelper.singletonInstance = new AccelerometerHelper();
                        }
                    }
                }

                return AccelerometerHelper.singletonInstance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the device is stable.
        /// </summary>
        /// <value>
        /// <see langword="true"/> it the device is stable - i.e. no movements
        /// for about 0.5 seconds; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDeviceStable
        {
            get {
                return (deviceStableCount >= SamplesCount);
            }
        }

        /// <summary>
        /// Gets or sets the calibration setting Key.
        /// </summary>
        /// <value>The calibration setting key.</value>
        private static Simple3DVector AccelerometerCalibrationPersisted
        {
            get {
                double x = ApplicationSettingsHelper.TryGetValueWithDefault<double>(AccelerometerCalibrationKeyName + "X", 0);
                double y = ApplicationSettingsHelper.TryGetValueWithDefault<double>(AccelerometerCalibrationKeyName + "Y", 0);
                return new Simple3DVector(x, y, 0);
            }
            set {
                bool isUpdated = ApplicationSettingsHelper.AddOrUpdateValue(AccelerometerCalibrationKeyName + "X", value.X);
                isUpdated |= ApplicationSettingsHelper.AddOrUpdateValue(AccelerometerCalibrationKeyName + "Y", value.Y);

                if (isUpdated) {
                    ApplicationSettingsHelper.Save();
                }
            }
        }

        /// <summary>
        /// Gets or sets the calibration data of the accelerometer.
        /// </summary>
        /// <value>The calibration data of the accelerometer.</value>
        public Simple3DVector ZeroAccelerationCalibrationOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the device has no
        /// accelerometer.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the device has no accelerometer;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool NoAccelerometer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the accelerometer is
        /// active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the accelerometer is active; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool IsActive
        {
            get { return isActive; }
            set {
                if (!NoAccelerometer) {
                    if (value) {
                        if (!isActive) { StartAccelerometer(); }
                    } else {
                        if (isActive) { StopAccelerometer(); }
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Releases any sensor resource.
        /// </summary>
        public void Dispose()
        {
            if (sensor != null) {
                sensor.Dispose();
            }
        }

        /// <summary>
        /// Returns a value indicating whether or not the calibration of the sensor
        /// would work along a particular set of axis because the device is stable enough
        /// or not inclined beyond reasonable.
        /// </summary>
        /// <param name="xAxis"><see langword="true"/> to check stability on the x-axis.</param>
        /// <param name="yAxis"><see langword="true"/> to check stability on the y-axis.</param>
        /// <returns>
        /// <see langword="true"/> if all of the checked axis were stable enough
        /// or not too inclined; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanCalibrate(bool xAxis, bool yAxis)
        {
            bool retval = false;

            lock (sampleBuffer) {
                if (IsDeviceStable) {
                    double accelerationMagnitude = 0;

                    if (xAxis) {
                        accelerationMagnitude += averageAcceleration.X * averageAcceleration.X;
                    }

                    if (yAxis) {
                        accelerationMagnitude += averageAcceleration.Y * averageAcceleration.Y;
                    }

                    accelerationMagnitude = Math.Sqrt(accelerationMagnitude);

                    if (accelerationMagnitude <= AccelerometerHelper.maxCalibrationOffset) {
                        retval = true;
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// Calibrates the accelerometer on the x-axis and/or the y-axis and save
        /// data to isolated storage.
        /// </summary>
        /// <param name="xAxis"><see langword="true"/> to calibrate on the x-axis.</param>
        /// <param name="yAxis"><see langword="true"/> to calibrate on the y-axis.</param>
        /// <returns>
        /// <see langword="true"/> if the accelerometer was successfully calibrated;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Calibrate(bool xAxis, bool yAxis)
        {
            bool retval = false;

            lock (sampleBuffer) {
                if (CanCalibrate(xAxis, yAxis)) {
                    ZeroAccelerationCalibrationOffset =
                        new Simple3DVector(
                            xAxis ? -averageAcceleration.X : ZeroAccelerationCalibrationOffset.X,
                            yAxis ? -averageAcceleration.Y : ZeroAccelerationCalibrationOffset.Y,
                            0);

                    AccelerometerCalibrationPersisted = ZeroAccelerationCalibrationOffset;
                    retval = true;
                }
            }

            return retval;
        }

        /// <summary>
        /// Initializes the accelerometer sensor and starts sampling.
        /// </summary>
        private void StartAccelerometer()
        {
            try {
                sensor = new Accelerometer();

                if (sensor != null) {
                    sensor.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(OnCurrentValueChanged);
                    sensor.Start();
                    isActive = true;
                    NoAccelerometer = false;
                } else {
                    isActive = false;
                    NoAccelerometer = true;
                }
            } catch (Exception e) {
                isActive = false;
                NoAccelerometer = true;
                Debug.WriteLine("Exception creating Accelerometer: " + e.Message);
            }
        }

        /// <summary>
        /// Stops sampling and releases the accelerometer sensor.
        /// </summary>
        private void StopAccelerometer()
        {
            try {
                if (sensor != null) {
                    sensor.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(OnCurrentValueChanged);
                    sensor.Stop();
                    sensor = null;
                    isActive = false;
                    isInitialized = false;
                }
            } catch (Exception e) {
                isActive = false;
                NoAccelerometer = true;
                Debug.WriteLine("Exception deleting Accelerometer: " + e.Message);
            }
        }

        /// <summary>
        /// Returns the first order discrete low-pass filter used to remove noise
        /// from raw accelerometer.
        /// </summary>
        /// <param name="newInputValue">New input value (latest sample).</param>
        /// <param name="prevOutputValue">The previous output value (filtered, one sampling period ago).</param>
        /// <returns>The new first order discrete low-pass filter.</returns>
        private static double GetLowPassFilter(double newInputValue, double prevOutputValue)
        {
            double newOutputValue = prevOutputValue + LowPassFilterCoef * (newInputValue - prevOutputValue);
            return newOutputValue;
        }

        /// <summary>
        /// Returns the discrete low-magnitude fast low-pass filter used to remove
        /// noise from raw accelerometer while allowing fast trending on high
        /// amplitude changes.
        /// </summary>
        /// <param name="newInputValue">New input value - latest sample.</param>
        /// <param name="prevOutputValue">The previous (n-1) output value - filtered one sampling period ago.</param>
        /// <returns>The new discrete low-magnitude fast low-pass filter.</returns>
        private static double GetFastLowAmplitudeNoiseFilter(double newInputValue, double prevOutputValue)
        {
            double newOutputValue = newInputValue;

            if (Math.Abs(newInputValue - prevOutputValue) <= NoiseMaxAmplitude) {
                newOutputValue = prevOutputValue + LowPassFilterCoef * (newInputValue - prevOutputValue);
            }

            return newOutputValue;
        }

        /// <summary>
        /// Handles the <c>CurrentValueChanged</c> event.
        /// </summary>
        /// <param name="sender">The <c>Accelerometer</c> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnCurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> args)
        {
            Simple3DVector lowPassFilteredAcceleration;
            Simple3DVector optimalFilteredAcceleration;
            Simple3DVector averagedAcceleration;
            Simple3DVector rawAcceleration = new Simple3DVector(
                args.SensorReading.Acceleration.X,
                args.SensorReading.Acceleration.Y,
                args.SensorReading.Acceleration.Z);

            lock (sampleBuffer) {
                if (!isInitialized) {
                    sampleSum = rawAcceleration * SamplesCount;
                    averageAcceleration = rawAcceleration;

                    for (int i = 0; i < SamplesCount; i++) {
                        sampleBuffer[i] = averageAcceleration;
                    }

                    prevLowPassOutput = averageAcceleration;
                    prevOptimalFilterOutput = averageAcceleration;

                    isInitialized = true;
                }

                // Low-pass filter.
                lowPassFilteredAcceleration = new Simple3DVector(
                    GetLowPassFilter(rawAcceleration.X, prevLowPassOutput.X),
                    GetLowPassFilter(rawAcceleration.Y, prevLowPassOutput.Y),
                    GetLowPassFilter(rawAcceleration.Z, prevLowPassOutput.Z));
                prevLowPassOutput = lowPassFilteredAcceleration;

                // Optimal filter.
                optimalFilteredAcceleration = new Simple3DVector(
                    GetFastLowAmplitudeNoiseFilter(rawAcceleration.X, prevOptimalFilterOutput.X),
                    GetFastLowAmplitudeNoiseFilter(rawAcceleration.Y, prevOptimalFilterOutput.Y),
                    GetFastLowAmplitudeNoiseFilter(rawAcceleration.Z, prevOptimalFilterOutput.Z));
                prevOptimalFilterOutput = optimalFilteredAcceleration;

                sampleIndex++;

                if (sampleIndex >= SamplesCount) {
                    sampleIndex = 0;
                }

                Simple3DVector vector = optimalFilteredAcceleration;
                sampleSum += vector;
                sampleSum -= sampleBuffer[sampleIndex];
                sampleBuffer[sampleIndex] = vector;

                averagedAcceleration = sampleSum / SamplesCount;
                averageAcceleration = averagedAcceleration;

                // Stablity check: if current low-pass filtered sample is deviating
                // for more than 1/100 g from average (max of 0.5 deg inclination
                // noise if device steady) then reset the stability counter.
                //
                // The calibration will be prevented until the counter is reaching
                // the sample count size (calibration enabled only if entire sampling
                // buffer is stable.
                Simple3DVector deltaAcceleration = averagedAcceleration - optimalFilteredAcceleration;

                if ((Math.Abs(deltaAcceleration.X) > AccelerometerHelper.maxStabilityDeltaOffset) ||
                    (Math.Abs(deltaAcceleration.Y) > AccelerometerHelper.maxStabilityDeltaOffset) ||
                    (Math.Abs(deltaAcceleration.Z) > AccelerometerHelper.maxStabilityDeltaOffset)) {
                    // Unstable.
                    deviceStableCount = 0;
                } else {
                    if (deviceStableCount < SamplesCount) { ++deviceStableCount; }
                }

                // Add calibrations.
                rawAcceleration += ZeroAccelerationCalibrationOffset;
                lowPassFilteredAcceleration += ZeroAccelerationCalibrationOffset;
                optimalFilteredAcceleration += ZeroAccelerationCalibrationOffset;
                averagedAcceleration += ZeroAccelerationCalibrationOffset;
            }

            if (ReadingChanged != null) {
                AccelerometerHelperReadingEventArgs readingEventArgs = new AccelerometerHelperReadingEventArgs();

                readingEventArgs.RawAcceleration = rawAcceleration;
                readingEventArgs.LowPassFilteredAcceleration = lowPassFilteredAcceleration;
                readingEventArgs.OptimalyFilteredAcceleration = optimalFilteredAcceleration;
                readingEventArgs.AverageAcceleration = averagedAcceleration;

                ReadingChanged(this, readingEventArgs);
            }
        }
        #endregion
    }
}
