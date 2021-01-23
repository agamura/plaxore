#region Header
//+ <source name="ContentAttribute.cs" language="C#" begin="28-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Represents a content attribute used by <see cref="IEncoder"/> and <see cref="IDecoder"/>
    /// implementations to embed or extract metadata, respectively.
    /// </summary>
    /// <see cref="ContentAttributeId"/>
    public class ContentAttribute : Variant
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and Boolean value.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The Boolean value to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, bool value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 8-bit integer.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 8-bit integer to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, byte value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 16-bit integer.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 16-bit integer to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, short value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 32-bit integer.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 32-bit integer to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, int value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 64-bit integer.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 64-bit integer to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, long value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 32-bit floating point number.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 32-bit floating point number to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, float value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and 64-bit floating point number.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The 64-bit floating point number to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, double value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and character string.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The character string to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, string value)
            : base(value)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAttribute"/> class with
        /// the specified id and byte array.
        /// </summary>
        /// <param name="id">One of the <see cref="ContentAttributeId"/> values.</param>
        /// <param name="value">The byte array to be represented by this <see cref="ContentAttribute"/>.</param>
        public ContentAttribute(ContentAttributeId id, byte[] value)
            : base(value)
        {
            Id = id;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the id of this <see cref="ContentAttribute"/>.
        /// </summary>
        /// <value>One of the <see cref="ContentAttributeId"/> values.</value>
        public ContentAttributeId Id
        {
            get;
            private set;
        }
        #endregion
    }
}
