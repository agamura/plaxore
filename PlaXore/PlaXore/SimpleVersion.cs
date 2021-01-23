#region Header
//+ <source name="SimpleVersion.cs" language="C#" begin="27-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Represents a simple version number consisting of a major value and a
    /// minor value.
    /// </summary>
    public struct SimpleVersion : IComparable, IComparable<SimpleVersion>, IEquatable<SimpleVersion>
    {
        #region Fields
        private uint version;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleVersion"/> class
        /// with the specified version.
        /// </summary>
        /// <param name="version">The version value.</param>
        /// <remarks>
        /// The higher bytes of <paramref name="version"/> contain the major
        /// component, while the lower bytes the minor component.
        /// </remarks>
        private SimpleVersion(uint version)
        {
            this.version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleVersion"/> class
        /// with the specified major value and minor value.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        public SimpleVersion(ushort major, ushort minor)
        {
            version = (uint) major << 16 | minor;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the value of the major component of the version number.
        /// </summary>
        /// <value>The value of the major component of the version number.</value>
        public ushort Major
        {
            get { return (ushort) (version >> 16); }
        }

        /// <summary>
        /// Gets the value of the minor component of the version number.
        /// </summary>
        /// <value>The value of the minor component of the version number.</value>
        public ushort Minor
        {
            get { return (ushort) version; }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="SimpleVersion"/> classes for equality.
        /// </summary>
        /// <param name="simpleVersion1">The first <see cref="SimpleVersion"/> to compare.</param>
        /// <param name="simpleVersion2">The second <see cref="SimpleVersion"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both major and minor version numbers of <paramref name="simpleVersion1"/>
        /// and <paramref name="simpleVersion1"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return Equals(simpleVersion1, simpleVersion2);
        }

        /// <summary>
        /// Compares the specified <see cref="SimpleVersion"/> classes for inequality.
        /// </summary>
        /// <param name="simpleVersion1">The first <see cref="SimpleVersion"/> to compare.</param>
        /// <param name="simpleVersion2">The second <see cref="SimpleVersion"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion1"/> and <paramref name="simpleVersion2"/>
        /// have different major or minor version numbers; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return !Equals(simpleVersion1, simpleVersion2);
        }

        /// <summary>
        /// Determines whether the value of a <see cref="SimpleVersion"/> is greater than
        /// the value of another <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="simpleVersion1">The <see cref="SimpleVersion"/> on the left side of the operator.</param>
        /// <param name="simpleVersion2">The <see cref="SimpleVersion"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion1"/> is greater than
        /// <paramref name="simpleVersion2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return simpleVersion1.version > simpleVersion2.version;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="SimpleVersion"/> is less than
        /// the value of another <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="simpleVersion1">The <see cref="SimpleVersion"/> on the left side of the operator.</param>
        /// <param name="simpleVersion2">The <see cref="SimpleVersion"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion1"/> is less than
        /// <paramref name="simpleVersion2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return simpleVersion1.version < simpleVersion2.version;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="SimpleVersion"/> is greater than or equal to
        /// the value of another <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="simpleVersion1">The <see cref="SimpleVersion"/> on the left side of the operator.</param>
        /// <param name="simpleVersion2">The <see cref="SimpleVersion"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion1"/> is greater than or equal to
        /// <paramref name="simpleVersion2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return simpleVersion1.version >= simpleVersion2.version;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="SimpleVersion"/> is less than or equal to
        /// the value of another <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="simpleVersion1">The <see cref="SimpleVersion"/> on the left side of the operator.</param>
        /// <param name="simpleVersion2">The <see cref="SimpleVersion"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion1"/> is less than or equal to
        /// <paramref name="simpleVersion2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return simpleVersion1.version <= simpleVersion2.version;
        }

        /// <summary>
        /// Converts the specified <see cref="SimpleVersion"/> implicitly to a version value.
        /// </summary>
        /// <param name="simpleVersion">The <see cref="SimpleVersion"/> to convert.</param>
        /// <returns>The resulting version value.</returns>
        /// <remarks>
        /// The higher bytes of the resulting version value contain the major
        /// component, while the lower bytes the minor component.
        /// </remarks>
        public static implicit operator uint(SimpleVersion simpleVersion)
        {
            return simpleVersion.version;
        }

        /// <summary>
        /// Converts the specified version value explicitly to a <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="version">The version value to convert.</param>
        /// <returns>The resulting <see cref="SimpleVersion"/>.</returns>
        /// <remarks>
        /// The higher bytes of <paramref name="version"/> contain the major
        /// component, while the lower bytes the minor component.
        /// </remarks>
        public static explicit operator SimpleVersion(uint version)
        {
            return new SimpleVersion(version);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares this <see cref="SimpleVersion"/> with the specified <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="simpleVersion">The <see cref="SimpleVersion"/> to compare.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>This <see cref="SimpleVersion"/> is less than <paramref name="simpleVersion"/>.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>This <see cref="SimpleVersion"/> is equal to <paramref name="simpleVersion"/>.</description>
        /// </item> 
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>This <see cref="SimpleVersion"/> is greater than <paramref name="simpleVersion"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(SimpleVersion simpleVersion)
        {
            if (version > simpleVersion.version) return 1;
            if (version < simpleVersion.version) return -1;
            return 0;
        }

        /// <summary>
        /// Compares this <see cref="SimpleVersion"/> with the specified object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>This <see cref="SimpleVersion"/> is less than <paramref name="obj"/>.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>This <see cref="SimpleVersion"/> is equal to <paramref name="obj"/>.</description>
        /// </item> 
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>This <see cref="SimpleVersion"/> is greater than <paramref name="obj"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo((SimpleVersion) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="SimpleVersion"/> instances for equality.
        /// </summary>
        /// <param name="simpleVersion1">The first <see cref="SimpleVersion"/> to compare.</param>
        /// <param name="simpleVersion2">The second <see cref="SimpleVersion"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="simpleVersion1"/> and <paramref name="simpleVersion2"/>
        /// have the same values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(SimpleVersion simpleVersion1, SimpleVersion simpleVersion2)
        {
            return simpleVersion1.version == simpleVersion2.version;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="SimpleVersion"/> and
        /// whether it has the same value as this <see cref="SimpleVersion"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="SimpleVersion"/> and
        /// has the same value as this <see cref="SimpleVersion"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SimpleVersion)) {
                return false;
            }

            return Equals(this, (SimpleVersion) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="SimpleVersion"/> with this <see cref="SimpleVersion"/>
        /// for equality.
        /// </summary>
        /// <param name="simpleVersion">The <see cref="SimpleVersion"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="simpleVersion"/> has the same value as this
        /// <see cref="SimpleVersion"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(SimpleVersion simpleVersion)
        {
            return Equals(this, simpleVersion);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="SimpleVersion"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="SimpleVersion"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
