﻿#region Header
//+ <source name="ListChangedType.cs" language="C#" begin="21-Nov-2011">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2011">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore
{
    /// <summary>
    /// Specifies how an <see cref="ObservableList&lt;T&gt;"/> has changed.
    /// </summary>
    public enum ListChangedType
    {
        /// <summary>
        /// The <see cref="ObservableList&lt;T&gt;"/> has been cleared.
        /// </summary>
        Cleared = 1,

        /// <summary>
        /// An item has been added to the <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        ItemAdded = 2,

        /// <summary>
        /// An item has been removed from the <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        ItemRemoved = 3,

        /// <summary>
        /// An item in the <see cref="ObservableList&lt;T&gt;"/> has changed.
        /// </summary>
        ItemChanged = 4
    }
}
