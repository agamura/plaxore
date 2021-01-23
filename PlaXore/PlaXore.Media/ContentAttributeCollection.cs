#region Header
//+ <source name="ContentAttributeCollection.cs" language="C#" begin="29-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Represents a collection of <see cref="ContentAttribute"/> instances.
    /// <see cref="ContentAttributeCollection"/> cannot be inherited.
    /// </summary>
    /// <remarks>
    /// Items in a <see cref="ContentAttributeCollection"/> can be modified, but
    /// cannot be added or removed.
    /// </remarks>
    public sealed class ContentAttributeCollection : IDictionary<ContentAttributeId, ContentAttribute>
    {
        #region Fields
        private IDictionary<ContentAttributeId, ContentAttribute> dictionary;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttributeCollection"/> class.
        /// </summary>
        internal ContentAttributeCollection()
        {
            ContentAttributeId[] ids = {
                ContentAttributeId.Codec,
                ContentAttributeId.CodecVersion,
                ContentAttributeId.FrameFormat,
                ContentAttributeId.FrameHeight,
                ContentAttributeId.FrameWidth,
                ContentAttributeId.FrameQuality,
                ContentAttributeId.FrameRate,
                ContentAttributeId.FrameCount,
                ContentAttributeId.AudioFormat,
                ContentAttributeId.AudioBlockCount,
                ContentAttributeId.AudioChannels, 
                ContentAttributeId.AudioSampleRate,
                ContentAttributeId.Duration,
                ContentAttributeId.Title,
                ContentAttributeId.Copyright,
                ContentAttributeId.Timestamp
            };

            dictionary = new Dictionary<ContentAttributeId, ContentAttribute>(ids.Length);

            foreach (ContentAttributeId id in ids) {
                dictionary.Add(id, null);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of elements in this <see cref="ContentAttributeCollection"/>.
        /// </summary>
        /// <value>The number of elements in this <see cref="ContentAttributeCollection"/>.</value>
        public int Count
        {
            get { return dictionary.Count; }
        }

        /// <summary>
        /// Gets the dictionary wrapped by this <see cref="ContentAttributeCollection"/>.
        /// </summary>
        /// <value>The dictionary wrapped by this <see cref="ContentAttributeCollection"/>.</value>
        internal IDictionary<ContentAttributeId, ContentAttribute> Dictionary
        {
            get { return dictionary; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ContentAttributeCollection"/> is read-only.
        /// </summary>
        /// <value>Always <see langword="true"/>.</value>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ContentAttribute"/> with the specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContentAttribute"/> to get or set.</param>
        /// <returns>The <see cref="ContentAttribute"/> with the specified id.</returns>
        public ContentAttribute this[ContentAttributeId id]
        {
            get { return dictionary[id]; }
            set { dictionary[id] = value; }
        }

        /// <summary>
        /// Gets a collection containing the keys in this <see cref="ContentAttributeCollection"/>.
        /// </summary>
        /// <value>The keys in this <see cref="ContentAttributeCollection"/>.</value>
        public ICollection<ContentAttributeId> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values in this <see cref="ContentAttributeCollection"/>.
        /// </summary>
        /// <value>The values in this <see cref="ContentAttributeCollection"/>.</value>
        public ICollection<ContentAttribute> Values
        {
            get { return dictionary.Values; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Throws a <c>NotSupportedException</c> exception.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContentAttribute"/> to add to this <see cref="ContentAttributeCollection"/>.</param>
        /// <param name="contentAttribute">The <see cref="ContentAttribute"/> to add to this <see cref="ContentAttributeCollection"/>.</param>
        /// <exception cref="NotSupportedException">
        /// Attempted to add a <see cref="ContentAttribute"/> to this <see cref="ContentAttributeCollection"/>.
        /// </exception>
        public void Add(ContentAttributeId id, ContentAttribute contentAttribute)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a <c>NotSupportedException</c> exception.
        /// </summary>
        /// <param name="item">The item to add to this <see cref="ContentAttributeCollection"/>.</param>
        /// <exception cref="NotSupportedException">
        /// Attempted to add an item to this <see cref="ContentAttributeCollection"/>.
        /// </exception>
        public void Add(KeyValuePair<ContentAttributeId, ContentAttribute> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a <c>NotSupportedException</c> exception.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Attempted to clear this <see cref="ContentAttributeCollection"/>.
        /// </exception>
        public void Clear()
        {
            throw new NotSupportedException();
        }
                
        /// <summary>
        /// Determines whether this <see cref="ContentAttributeCollection"/> contains the
        /// specified item.
        /// </summary>
        /// <param name="item">The item to locate in this <see cref="ContentAttributeCollection"/>.</param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="ContentAttributeCollection"/> contains
        /// the specified item; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(KeyValuePair<ContentAttributeId, ContentAttribute> item)
        {
            return dictionary.Contains(item);
        }

        /// <summary>
        /// Determines whether this <see cref="ContentAttributeCollection"/> contains the specified id.
        /// </summary>
        /// <param name="id">The id to locate in this <see cref="ContentAttributeCollection"/>.</param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="ContentAttributeCollection"/> contains the specified
        /// id; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ContainsKey(ContentAttributeId id)
        {
            return dictionary.ContainsKey(id);
        }

        /// <summary>
        /// Copies this <see cref="ContentAttributeCollection"/> into the specified
        /// array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(KeyValuePair<ContentAttributeId, ContentAttribute>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Throws a <c>NotSupportedException</c> exception.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContentAttribute"/> to remove.</param>
        /// <returns>Always throws a <c>NotSupportedException</c> exception.</returns>
        /// <exception cref="NotSupportedException">
        /// Attempted to remove a <see cref="ContentAttribute"/> from this
        /// <see cref="ContentAttributeCollection"/>.
        /// </exception>
        public bool Remove(ContentAttributeId id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a <c>NotSupportedException</c> exception.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Always throws a <c>NotSupportedException</c> exception.</returns>
        /// <exception cref="NotSupportedException">
        /// Attempted to remove an item from this <see cref="ContentAttributeCollection"/>.
        /// </exception>
        public bool Remove(KeyValuePair<ContentAttributeId, ContentAttribute> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to get the <see cref="ContentAttribute"/> with the specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContentAttribute"/> to get.</param>
        /// <param name="contentAttribute">The <see cref="ContentAttribute"/> to get.</param>
        /// <returns><see langword="true"/> if this <see cref="ContentAttributeCollection"/>
        /// contains an <see cref="ContentAttribute"/> with the specified id; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool TryGetValue(ContentAttributeId id, out ContentAttribute contentAttribute)
        {
            return dictionary.TryGetValue(id, out contentAttribute);
        }

        /// <summary>
        /// Returns an enumerator that iterates through this <see cref="ContentAttributeCollection"/>.
        /// </summary>
        /// <returns>An enumerator for this <see cref="ContentAttributeCollection"/>.</returns>
        public IEnumerator<KeyValuePair<ContentAttributeId, ContentAttribute>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) dictionary).GetEnumerator();
        }
        #endregion
    }
}
