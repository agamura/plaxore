#region Header
//+ <source name="AccelerometerHelperReadingEventArgs.cs" language="C#" begin="24-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace PlaXore.ShakeGestures.Sensors
{
    /// <summary>
    /// Provides data for the <see cref="AccelerometerHelper.ReadingChanged"/> event.
    /// </summary>
    public class AccelerometerHelperReadingEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets or sets unfiltered accelerometer data coming directly from sensor.
        /// </summary>
        /// <value>
        /// Raw, unfiltered accelerometer data (acceleration vector in all three
        /// dimensions) coming directly from sensor.
        /// </value>
        /// <remarks>
        /// <see cref="AccelerometerHelperReadingEventArgs.RawAcceleration"/> is
        /// required for updating rapidly reacting UI.
        /// </remarks>
        public Simple3DVector RawAcceleration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Filtered accelerometer data using a combination of a
        /// low-pass and threshold triggered high-pass on each axis.
        /// </summary>
        /// <value>
        /// Filtered accelerometer data using a combination of a low-pass and
        /// threshold triggered high-pass on each axis to elimate the majority
        /// of the sensor low amplitude noise while trending very quickly to
        /// large offsets - not perfectly smooth signal in that case, providing
        /// a very low latency.
        /// </value>
        /// <remarks>
        /// <see cref="AccelerometerHelperReadingEventArgs.OptimalyFilteredAcceleration"/>
        /// is ideal for quickly reacting UI updates.</remarks>
        public Simple3DVector OptimalyFilteredAcceleration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets filtered accelerometer data using a 1 Hz first-order low-pass
        /// on each axis.
        /// </summary>
        /// <value>
        /// Filtered accelerometer data using a 1 Hz first-order low-pass on each
        /// axis to elimate the main sensor noise while providing a medium latency.
        /// </value>
        /// <remarks>
        /// <see cref="AccelerometerHelperReadingEventArgs.LowPassFilteredAcceleration"/>
        /// can be used for moderatly reacting UI updates requiring a very smooth signal.
        /// </remarks>
        public Simple3DVector LowPassFilteredAcceleration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets filtered and temporally averaged accelerometer data using
        /// an arithmetic mean of the last 25 optimaly-filtered samples.
        /// </summary>
        /// <value>
        /// Filtered and temporally averaged accelerometer data using an arithmetic
        /// mean of the last 25 optimaly-filtered samples, so over 500ms at 50Hz on
        /// each axis, to virtually eliminate most sensor noise. 
        /// </value>
        /// <remarks>
        /// <see cref="AccelerometerHelperReadingEventArgs.AverageAcceleration"/>
        /// provides a very stable reading but it has also a very high latency
        /// and cannot be used for rapidly reacting UI.
        /// </remarks>
        public Simple3DVector AverageAcceleration
        {
            get;
            set;
        }
        #endregion
    }
}
