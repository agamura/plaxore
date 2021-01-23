#region Header
//+ <source name="ShakeGestureEventArgs.cs" language="C#" begin="23-Mar-2012">
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

namespace PlaXore.ShakeGestures
{
    /// <summary>
    /// Provides data for the <see cref="ShakeGesturesHelper.ShakeGesture"/> event.
    /// </summary>
    public class ShakeGestureEventArgs : EventArgs
    {
        #region Fields
        private ShakeType shakeType;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShakeGestureEventArgs"/>
        /// class with the specified <see cref="ShakeType"/>.
        /// </summary>
        /// <param name="shakeType">One of the <see cref="ShakeType"/> values.</param>
        public ShakeGestureEventArgs(ShakeType shakeType)
        {
            this.shakeType = shakeType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="ShakeType"/>.
        /// </summary>
        /// <value>One of the <see cref="ShakeType"/> values.</value>
        public ShakeType ShakeType
        {
            get { return shakeType; }
        }
        #endregion
    }
}
