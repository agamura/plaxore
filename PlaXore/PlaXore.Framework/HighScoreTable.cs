#region Header
//+ <source name="HighScoreTable.cs" language="C#" begin="25-Mar-2012">
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
    /// Represents a collection of <see cref="HighScoreEntry"/> instances.
    /// </summary>
    public class HighScoreTable
    {
        #region Fields
        private List<HighScoreEntry> entries;
        private int size;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreTable"/> class
        /// with the specified size and description.
        /// </summary>
        /// <param name="size">The size of the <see cref="HighScoreTable"/>.</param>
        /// <param name="description">The description of the <see cref="HighScoreTable"/>.</param>
        internal HighScoreTable(int size, string description)
        {
            this.size = size;
            Description = description;

            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="HighScoreEntry"/> instances in this 
        /// <see cref="HighScoreTable"/> as a read-only collection.
        /// </summary>
        /// <value>
        /// The <see cref="HighScoreEntry"/> instances in this 
        /// <see cref="HighScoreTable"/> as a read-only collection.
        /// </value>
        public System.Collections.ObjectModel.ReadOnlyCollection<HighScoreEntry> Entries
        {
            get { return entries.AsReadOnly(); }
        }

        /// <summary>
        /// Gets or sets the description of this <see cref="HighScoreTable"/>.
        /// </summary>
        /// <value>
        /// The description of this <see cref="HighScoreTable"/>.
        /// </value>
        public string Description
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clears this <see cref="HighScoreTable"/>.
        /// </summary>
        public void Clear()
        {
            entries = new List<HighScoreEntry>();

            for (int i = 0; i < size; i++) {
                entries.Add(new HighScoreEntry());
            }
        }

        /// <summary>
        /// Creates a new <see cref="HighScoreEntry"/> and adds it to this
        /// <see cref="HighScoreTable"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="HighScoreEntry"/>.</param>
        /// <param name="score">The score of the <see cref="HighScoreEntry"/>.</param>
        /// <returns>
        /// The <see cref="HighScoreEntry"/> added to this <see cref="HighScoreTable"/>.
        /// </returns>
        public HighScoreEntry AddEntry(string name, int score)
        {
            return AddEntry(name, score, DateTime.UtcNow);
        }

        /// <summary>
        /// Creates a new <see cref="HighScoreEntry"/> and adds it to this
        /// <see cref="HighScoreTable"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="HighScoreEntry"/>.</param>
        /// <param name="score">The score of the <see cref="HighScoreEntry"/>.</param>
        /// <param name="dateTime">The time of the <see cref="HighScoreEntry"/>.</param>
        /// <returns>
        /// The <see cref="HighScoreEntry"/> added to this <see cref="HighScoreTable"/>.
        /// </returns>
        public HighScoreEntry AddEntry(string name, int score, DateTime dateTime)
        {
            HighScoreEntry entry = new HighScoreEntry();
            entry.Name = name;
            entry.Score = score;
            entry.DateTime = dateTime;

            entries.Add(entry);
            entries.Sort(new HighScoreEntry());

            if (entries.Count > size) {
                entries.RemoveAt(size);
            }

            if (entries.Contains(entry)) {
                return entry;
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified score qualifies.
        /// </summary>
        /// <param name="score">The score to evaluate.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="score"/> qualifies; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool ScoreQualifies(int score)
        {
            return (score > entries[size - 1].Score);
        }
        #endregion
    }
}