#region Header
//+ <source name="HighScoreEntry.cs" language="C#" begin="25-Mar-2012">
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
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Represents a high score entry in a <see cref="HighScoreTable"/>.
    /// </summary>
    public class HighScoreEntry : IComparer<HighScoreEntry>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreEntry"/> class.
        /// </summary>
        internal HighScoreEntry()
        {
            Name = "";
            Score = 0;
            DateTime = DateTime.MinValue;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of this <see cref="HighScoreEntry"/>.
        /// </summary>
        /// <value>
        /// The name of this <see cref="HighScoreEntry"/>.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the score of this <see cref="HighScoreEntry"/>.
        /// </summary>
        /// <value>
        /// The score of this <see cref="HighScoreEntry"/>.
        /// </value>
        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time of this <see cref="HighScoreEntry"/>.
        /// </summary>
        /// <value>
        /// The time of this <see cref="HighScoreEntry"/>.
        /// </value>
        public DateTime DateTime
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="HighScoreEntry"/> instances for equality.
        /// </summary>
        /// <param name="highScoreEntry1">The first <see cref="HighScoreEntry"/> to compare.</param>
        /// <param name="highScoreEntry2">The second <see cref="HighScoreEntry"/> to compare.</param>
        /// <returns>
        /// 0 if <paramref name="highScoreEntry1"/> and <paramref name="highScoreEntry2"/> are equal;
        /// a value less than 0 if <paramref name="highScoreEntry1"/> is less than <paramref name="highScoreEntry2"/>;
        /// a value greater than 0 if <paramref name="highScoreEntry1"/> is greater than <paramref name="highScoreEntry2"/>.
        /// </returns>
        public int Compare(HighScoreEntry highScoreEntry1, HighScoreEntry highScoreEntry2)
        {
            if (highScoreEntry1.Score < highScoreEntry2.Score) {
                return 1;
            } else if (highScoreEntry1.Score > highScoreEntry2.Score) {
                return -1;
            } else {
                if (highScoreEntry1.DateTime < highScoreEntry2.DateTime) {
                    return -1;
                } else if (highScoreEntry1.DateTime > highScoreEntry2.DateTime) {
                    return 1;
                } else {
                    return 0;
                }
            }
        }
        #endregion
    }
}