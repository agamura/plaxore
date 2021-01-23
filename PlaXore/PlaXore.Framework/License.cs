#region Header
//+ <source name="License.cs" language="C#" begin="24-Jun-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Marketplace;
#endif
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Provides functionality for determining whether the game is running under
    /// a trial license and in case for buying the full license.
    /// </summary>
    public class License
    {
        #region Fields
#if WINDOWS_PHONE
        private LicenseInformation licenseInformation;
#endif
        private bool isTrial;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the full <see cref="License"/> is bought.
        /// </summary>
        public event EventHandler<EventArgs> Bought;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="License"/> class.
        /// </summary>
        public License()
        {
#if DEBUG && WINDOWS_PHONE
            isTrial = true;
#else
            isTrial = false;
#endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether or not the game is running under a
        /// trial license.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the game is running under a trial license;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsTrial
        {
            get {
#if WINDOWS_PHONE
                if (licenseInformation == null) {
                    licenseInformation = new LicenseInformation();
#if !DEBUG
                    isTrial = licenseInformation.IsTrial();
#endif
                }
#endif
                return isTrial;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lets the <see cref="LocalGamer"/> buy the game.
        /// </summary>
        public void Buy()
        {
#if WINDOWS_PHONE
#if DEBUG
            isTrial = false;
#else
            licenseInformation = null;
            var launcher = new MarketplaceDetailTask();
            launcher.Show();
#endif
#endif
            OnBought(null);
        }

        /// <summary>
        /// Raises the <see cref="License.Bought"/> event.
        /// </summary>
        /// <param name="args">Always <see langword="null"/>.</param>
        /// <remarks>
        /// The <see cref="License.Bought"/> method also allows derived classes
        /// to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnBought(EventArgs args)
        {
            if (Bought != null) {
                Bought(this, args);
            }
        }
        #endregion
    }
}
