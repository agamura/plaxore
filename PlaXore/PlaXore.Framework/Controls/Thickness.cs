#region Header
//+ <source name="Thickness.cs" language="C#" begin="28-Jul-2012">
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

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents the thickness of a frame around a <see cref="Control"/>.
    /// </summary>
    public struct Thickness
    {
        #region Fields
        private int left;
        private int top;
        private int right;
        private int bottom;

        /// <summary>
        /// Represents an undefined <see cref="Thickness"/>.
        /// </summary>
        public static readonly Thickness Undefined;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure
        /// with the specified uniform width applied to all four sides of the
        /// bounding <see cref="Control"/>.
        /// </summary>
        /// <param name="uniformWidth">
        /// The uniform width applied to all four sides of the bounding
        /// <see cref="Control"/>.
        /// </param>
        public Thickness(int uniformWidth)
        {
            left = top = right = bottom = uniformWidth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure
        /// with the specified widths applied to each side of the bounding
        /// <see cref="Control"/>.
        /// </summary>
        /// <param name="left">
        /// The width of the left side of the bounding <see cref="Control"/>, in pixels.
        /// </param>
        /// <param name="top">
        /// The width of the upper side of the bounding <see cref="Control"/>, in pixels.
        /// </param>
        /// <param name="right">
        /// The width of the right side of the bounding <see cref="Control"/>, in pixels.
        /// </param>
        /// <param name="bottom">
        /// The width of the lower side of the bounding <see cref="Control"/>, in pixels.
        /// </param>
        public Thickness(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Initializes <see cref="Thickness.Undefined"/>.
        /// </summary>
        static Thickness()
        {
            Thickness.Undefined = new Thickness(Int32.MinValue);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the width of the left side of the bounding <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The width of the left side of the bounding <see cref="Control"/>, in pixels.
        /// </value>
        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Gets or sets the width of the upper side of the bounding <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The width of the uppper side of the bounding <see cref="Control"/>, in pixels.
        /// </value>
        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets or sets the width of the right side of the bounding <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The width of the right side of the bounding <see cref="Control"/>, in pixels.
        /// </value>
        public int Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// Gets or sets the width of the lower side of the bounding <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// The width of the lower side of the bounding <see cref="Control"/>, in pixels.
        /// </value>
        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Thickness"/> structures for equality.
        /// </summary>
        /// <param name="thickness1">The first <see cref="Thickness"/> to compare.</param>
        /// <param name="thickness2">The Second <see cref="Thickness"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if all of the widths applied by <paramref name="thickness1"/>
        /// and <paramref name="thickness2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Thickness thickness1, Thickness thickness2)
        {
            return Equals(thickness1, thickness2);
        }

        /// <summary>
        /// Compares the specified <see cref="Thickness"/> structures for inequality.
        /// </summary>
        /// <param name="thickness1">The first <see cref="Thickness"/> to compare.</param>
        /// <param name="thickness2">The Second <see cref="Thickness"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="thickness1"/> and <paramref name="thickness2"/>
        /// have at least one different width; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Thickness thickness1, Thickness thickness2)
        {
            return !Equals(thickness1, thickness2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Thickness"/> structures for equality.
        /// </summary>
        /// <param name="thickness1">The first <see cref="Thickness"/> to compare.</param>
        /// <param name="thickness2">The Second <see cref="Thickness"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if all of the widths applied by <paramref name="thickness1"/>
        /// and <paramref name="thickness2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Thickness thickness1, Thickness thickness2)
        {
            return thickness1.left == thickness2.left
                && thickness1.top == thickness2.top
                && thickness1.right == thickness2.right
                && thickness1.bottom == thickness2.bottom;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Thickness"/> and
        /// whether it contains the same widths as this <see cref="Thickness"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Thickness"/> and
        /// contains the same widths as this <see cref="Thickness"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Thickness)) {
                return false;
            }

            return Equals(this, (Thickness) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Thickness"/> with this <see cref="Thickness"/>
        /// for equality.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Thickness"/> contains the 
        /// same widths as this <see cref="Thickness"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Thickness thickness)
        {
            return Equals(this, thickness);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Thickness"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Thickness"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Thickness"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Thickness"/>.</returns>
        public override string ToString()
        {
            return "(" + left + ", " + top + ", " + right + ", " + bottom + ")";
        }
        #endregion
    }
}
