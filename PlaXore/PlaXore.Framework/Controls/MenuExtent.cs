#region Header
//+ <source name="MenuExtent.cs" language="C#" begin="1-May-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace PlaXore.GameFramework.Controls
{
    /// <summary>
    /// Represents a <see cref="Menu"/> extent, and provides functionality for
    /// initializing, incrementing, and decrementing it.
    /// </summary>
    public struct MenuExtent
    {
        #region fields
        private int current;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuExtent"/> structure
        /// with the specified initial value.
        /// </summary>
        /// <param name="menu">The initial <see cref="MenuExtent"/> value.</param>
        public MenuExtent(int value)
        {
            current = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuExtent"/> structure
        /// with the width or height of the specified <see cref="Menu"/>, according
        /// to <see cref="Menu.Orientation"/>.
        /// </summary>
        /// <param name="menu">A <see cref="Menu"/> instance.</param>
        public MenuExtent(Menu menu)
        {
            current = menu.Orientation == Orientation.Horizontal
                ? menu.SourceRectangle.Width
                : menu.SourceRectangle.Height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current <see cref="MenuExtent"/> value.
        /// </summary>
        /// <value>The current <see cref="MenuExtent"/> value.</value>
        public int Current
        {
            get { return current; }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> structures for equality.
        /// </summary>
        /// <param name="menuExtent1">The first <see cref="MenuExtent"/> to compare.</param>
        /// <param name="menuExtent2">The Second <see cref="MenuExtent"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="menuExtent1"/> and
        /// <paramref name="menuExtent2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return Equals(menuExtent1, menuExtent2);
        }

        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> and value for equality.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to compare.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/>
        /// and <paramref name="value"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(MenuExtent menuExtent, int value)
        {
            return Equals(menuExtent, value);
        }

        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> structures for inequality.
        /// </summary>
        /// <param name="menuExtent1">The first <see cref="MenuExtent"/> to compare.</param>
        /// <param name="menuExtent2">The Second <see cref="MenuExtent"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="menuExtent1"/> and
        /// <paramref name="menuExtent2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return !Equals(menuExtent1, menuExtent2);
        }

        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> and value for inequality.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to compare.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/> and
        /// <paramref name="value"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(MenuExtent menuExtent, int value)
        {
            return !Equals(menuExtent, value);
        }

        /// <summary>
        /// Adds one <see cref="MenuExtent"/> to another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator +(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            MenuExtent menuExtent = new MenuExtent();
            menuExtent.current = menuExtent1.current + menuExtent2.current;

            return menuExtent;
        }

        /// <summary>
        /// Adds a value to a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator +(MenuExtent menuExtent, int value)
        {
            return menuExtent + new MenuExtent(value);
        }

        /// <summary>
        /// Adds the width or height of a <see cref="Menu"/> to a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menu">The <see cref="Menu"/> on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator +(MenuExtent menuExtent, Menu menu)
        {
            return menuExtent + new MenuExtent(menu);
        }

        /// <summary>
        /// Subtracts one <see cref="MenuExtent"/> from another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator -(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            MenuExtent menuExtent = new MenuExtent();

            if (menuExtent1.current < menuExtent2.current) {
                menuExtent.current = 0;
            } else {
                menuExtent.current = menuExtent1.current - menuExtent2.current;
            }

            return menuExtent;
        }

        /// <summary>
        /// Subtracts a value from a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator -(MenuExtent menuExtent, int value)
        {
            return menuExtent - new MenuExtent(value);
        }

        /// <summary>
        /// Subtracts the width or height of a <see cref="Menu"/> from a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menu">The <see cref="Menu"/> on the right side of the operator.</param>
        /// <returns>A <see cref="MenuExtent"/> that is the result of the operation.</returns>
        public static MenuExtent operator -(MenuExtent menuExtent, Menu menu)
        {
            return menuExtent - new MenuExtent(menu);
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is greater than another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="menuExtent1"/> is greater than
        /// <paramref name="menuExtent2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return menuExtent1.current > menuExtent2.current;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is greater than a value.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/> is
        /// greater than <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(MenuExtent menuExtent, int value)
        {
            return menuExtent.current > value;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is less than another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="menuExtent1"/> is less than
        /// <paramref name="menuExtent2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return menuExtent1.current < menuExtent2.current;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is less than a value.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/> is
        /// less than <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(MenuExtent menuExtent, int value)
        {
            return menuExtent.current < value;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is greater than or
        /// equal to another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="menuExtent1"/> is greater than or
        /// equal to <paramref name="menuExtent2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return menuExtent1.current >= menuExtent2.current;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is greater than or equal to a value.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/> is greater than
        /// or equal to <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(MenuExtent menuExtent, int value)
        {
            return menuExtent.current >= value;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is less than or equal to
        /// another <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="menuExtent1">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="menuExtent2">The <see cref="MenuExtent"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="menuExtent1"/> is less than or equal to
        /// <paramref name="menuExtent2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return menuExtent1.current <= menuExtent2.current;
        }

        /// <summary>
        /// Determines whether a <see cref="MenuExtent"/> is less than or equal to a value.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> on the left side of the operator.</param>
        /// <param name="value">The value on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="menuExtent"/> is less than
        /// or equal to <paramref name="value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(MenuExtent menuExtent, int value)
        {
            return menuExtent.current <= value;
        }

        /// <summary>
        /// Converts the specified <see cref="MenuExtent"/> implicitly to an integer.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to convert.</param>
        /// <returns>The resulting integer.</returns>
        public static implicit operator int(MenuExtent menuExtent)
        {
            return menuExtent.current;
        }

        /// <summary>
        /// Converts the specified integer explicitly to a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>The resulting <see cref="MenuExtent"/>.</returns>
        public static explicit operator MenuExtent(int value)
        {
            return new MenuExtent(value);
        }

        /// <summary>
        /// Converts the specified <see cref="MenuExtent"/> implicitly to a float.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to convert.</param>
        /// <returns>The resulting float.</returns>
        public static implicit operator float(MenuExtent menuExtent)
        {
            return (float) menuExtent.current;
        }

        /// <summary>
        /// Converts the specified float explicitly to a <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="value">The float to convert.</param>
        /// <returns>The resulting <see cref="MenuExtent"/>.</returns>
        public static explicit operator MenuExtent(float value)
        {
            return new MenuExtent(unchecked((int) value));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> structures for equality.
        /// </summary>
        /// <param name="menuExtent1">The first <see cref="MenuExtent"/> to compare.</param>
        /// <param name="menuExtent2">The second <see cref="MenuExtent"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="menuExtent1"/> and <paramref name="menuExtent2"/>
        /// have the same values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(MenuExtent menuExtent1, MenuExtent menuExtent2)
        {
            return menuExtent1.current == menuExtent2.current;
        }

        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> and value for equality.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to compare.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both the value of <paramref name="menuExtent"/> and
        /// <paramref name="value"/> have the same values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(MenuExtent menuExtent, int value)
        {
            return menuExtent.current == value;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="MenuExtent"/>
        /// and whether it has the same value as this <see cref="MenuExtent"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="MenuExtent"/> and
        /// has the same value as this <see cref="MenuExtent"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is MenuExtent)) {
                return false;
            }

            return Equals(this, (MenuExtent) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="MenuExtent"/> with this <see cref="MenuExtent"/>
        /// for equality.
        /// </summary>
        /// <param name="menuExtent">The <see cref="MenuExtent"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="menuExtent"/> has the same value as this
        /// <see cref="MenuExtent"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(MenuExtent menuExtent)
        {
            return Equals(this, menuExtent);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="MenuExtent"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="MenuExtent"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Resets this <see cref="MenuExtent"/> value to 0.
        /// </summary>
        public void Reset()
        {
            current = 0;
        }

        /// <summary>
        /// Increments this <see cref="MenuExtent"/> by the specified value.
        /// </summary>
        /// <param name="value">The value to add to this <see cref="MenuExtent"/>.</param>
        /// <returns>The incremented <see cref="MenuExtent"/> value.</returns>
        public int IncrementBy(int value)
        {
            return current += value;
        }

        /// <summary>
        /// Increments this <see cref="MenuExtent"/> by the width or height of the
        /// specified <see cref="Menu"/>, depending on <see cref="Menu.Orientation"/>.
        /// </summary>
        /// <param name="menu">
        /// The <see cref="Menu"/> of which the width or height is added to
        /// this <see cref="MenuExtent"/>.
        /// </param>
        /// <returns>The incremented <see cref="MenuExtent"/> value.</returns>
        public int IncrementBy(Menu menu)
        {
            return IncrementBy(menu.Orientation == Orientation.Horizontal
                ? menu.SourceRectangle.Width
                : menu.SourceRectangle.Height);
        }

        /// <summary>
        /// Decrements this <see cref="MenuExtent"/> by the specified value.
        /// </summary>
        /// <param name="value">The value to subtract from this <see cref="MenuExtent"/>.</param>
        /// <returns>The decremented <see cref="MenuExtent"/> value.</returns>
        public int DecrementBy(int value)
        {
            if (current < value) {
                current = 0;
            } else {
                current -= value;
            }

            return current;
        }

        /// <summary>
        /// Decrements this <see cref="MenuExtent"/> by the width or height of the
        /// specified <see cref="Menu"/>, depending on <see cref="Menu.Orientation"/>.
        /// </summary>
        /// <param name="menu">
        /// The <see cref="Menu"/> of which the width or height is subtracted from
        /// this <see cref="MenuExtent"/>.
        /// </param>
        /// <returns>The decremented <see cref="MenuExtent"/> value.</returns>
        public int DecrementBy(Menu menu)
        {
            return DecrementBy(menu.Orientation == Orientation.Horizontal
                ? menu.SourceRectangle.Width
                : menu.SourceRectangle.Height);
        }
        #endregion
    }
}
