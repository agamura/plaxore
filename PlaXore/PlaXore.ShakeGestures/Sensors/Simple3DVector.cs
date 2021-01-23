#region Header
//+ <source name="Simple3DVector.cs" language="C#" begin="24-Mar-2012">
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

namespace PlaXore.ShakeGestures.Sensors
{
    /// <summary>
    /// Represents a 3D vector.
    /// </summary>
    public class Simple3DVector
    {
        #region Fields
        private double? magnitude = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Simple3DVector"/> class.
        /// </summary>
        public Simple3DVector()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Simple3DVector"/> class
        /// with the specified coordinates.
        /// </summary>
        /// <param name="x">The the x-axis coordinate.</param>
        /// <param name="y">The the y-axis coordinate.</param>
        /// <param name="z">The the z-axis coordinate.</param>
        public Simple3DVector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Simple3DVector"/> class
        /// with the specified vector.
        /// </summary>
        /// <param name="vector">A <see cref="Simple3DVector"/> instance.</param>
        public Simple3DVector(Simple3DVector vector)
        {
            if (vector != null) {
                X = vector.X;
                Y = vector.Y;
                Z = vector.Z;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the x-axis coordinate.
        /// </summary>
        /// <value>The the x-axis coordinate.</value>
        public double X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the y-axis coordinate.
        /// </summary>
        /// <value>The the y-axis coordinate.</value>
        public double Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the z-axis coordinate.
        /// </summary>
        /// <value>The the z-axis coordinate.</value>
        public double Z
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the magnitude of this <see cref="Simple3DVector"/>.
        /// </summary>
        public double Magnitude
        {
            get {
                if (magnitude == null) {
                    magnitude = Math.Sqrt(X * X + Y * Y + Z * Z);
                }
                return magnitude.Value;
            }
        }
        #endregion

        #region operators
        /// <summary>
        /// Compares the specified <see cref="Simple3DVector"/> instances for equality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Simple3DVector"/> to compare.</param>
        /// <param name="vector2">The Second <see cref="Simple3DVector"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elements of <paramref name="vector1"/> and
        /// <paramref name="vector2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Simple3DVector vector1, Simple3DVector vector2)
        {
            if (Object.ReferenceEquals(vector1, vector2)) {
                return true;
            }

            if (((object) vector1 == null) || ((object) vector2 == null)) {
                return false;
            }

            return (vector1.X == vector2.X)
                && (vector1.Y == vector2.Y)
                && (vector1.Z == vector2.Z);
        }

        /// <summary>
        /// Compares the specified <see cref="Simple3DVector"/> instances for inequality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Simple3DVector"/> to compare.</param>
        /// <param name="vector2">The Second <see cref="Simple3DVector"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elements of <paramref name="vector1"/> and
        /// <paramref name="vector2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Simple3DVector vector1, Simple3DVector vector2)
        {
            return !(vector1 == vector2);
        }

         /// <summary>
        /// Adds the values of one <see cref="Simple3DVector"/> to the values of
        /// another <see cref="Simple3DVector"/>.
        /// </summary>
        /// <param name="vector1">
        /// The <see cref="Simple3DVector"/> on the left side of the operator.
        /// </param>
        /// <param name="vector2">
        /// The <see cref="Simple3DVector"/> on the right side of the operator.
        /// </param>
        /// <returns>A <see cref="Simple3DVector"/> that is the result of the operation.</returns>>
        public static Simple3DVector operator +(Simple3DVector vector1, Simple3DVector vector2)
        {
            return new Simple3DVector(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        }

        /// <summary>
        /// Subtracts the values of one <see cref="Simple3DVector"/> from the values of
        /// another <see cref="Simple3DVector"/>.
        /// </summary>
        /// <param name="vector1">
        /// The <see cref="Simple3DVector"/> on the left side of the operator.
        /// </param>
        /// <param name="vector2">
        /// The <see cref="Simple3DVector"/> on the right side of the operator.
        /// </param>
        /// <returns>A <see cref="Simple3DVector"/> that is the result of the operation.</returns>
        public static Simple3DVector operator -(Simple3DVector vector1, Simple3DVector vector2)
        {
            return new Simple3DVector(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
        }

        /// <summary>
        /// Multiplies the values of one <see cref="Simple3DVector"/> with the values of
        /// another <see cref="Simple3DVector"/>.
        /// </summary>
        /// <param name="vector1">
        /// The <see cref="Simple3DVector"/> on the left side of the operator.
        /// </param>
        /// <param name="vector2">
        /// The <see cref="Simple3DVector"/> on the right side of the operator.
        /// </param>
        /// <returns>A <see cref="Simple3DVector"/> that is the result of the operation.</returns>
        public static Simple3DVector operator *(Simple3DVector vector1, Simple3DVector vector2)
        {
            return new Simple3DVector(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
        }

        /// <summary>
        /// Multiplies the values of one <see cref="Simple3DVector"/> by the specified
        /// factor.
        /// </summary>
        /// <param name="vector">
        /// The <see cref="Simple3DVector"/> on the left side of the operator.
        /// </param>
        /// <param name="factor">
        /// The factor on the right side of the operator.
        /// </param>
        /// <returns>A <see cref="Simple3DVector"/> that is the result of the operation.</returns>
        public static Simple3DVector operator *(Simple3DVector vector, double factor)
        {
            return new Simple3DVector(vector.X * factor, vector.Y * factor, vector.Z * factor);
        }

        /// <summary>
        /// Divides the values of one <see cref="Simple3DVector"/> by the specified
        /// quotient.
        /// </summary>
        /// <param name="vector">
        /// The <see cref="Simple3DVector"/> on the left side of the operator.
        /// </param>
        /// <param name="quotient">
        /// The quotient on the right side of the operator.
        /// </param>
        /// <returns>A <see cref="Simple3DVector"/> that is the result of the operation.</returns>
        public static Simple3DVector operator /(Simple3DVector vector, double quotient)
        {
            return new Simple3DVector(vector.X / quotient, vector.Y / quotient, vector.Z / quotient);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is a
        /// <see cref="Simple3DVector"/> and whether it contains the same elements as
        /// this <see cref="Simple3DVector"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Simple3DVector"/>
        /// and contains the same elements as this <see cref="Simple3DVector"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Simple3DVector)) {
                return false;
            }

            return (bool) (this == (Simple3DVector) obj);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Simple3DVector"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="Simple3DVector"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Simple3DVector"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Simple3DVector"/>.</returns>
        public override string ToString()
        {
            return (String.Format("({0:0.000},{1:0.000},{2:0.000})", X, Y, Z));
        }
        #endregion
    }
}
