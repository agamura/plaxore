#region Header
//+ <source name="GameRandomHelper.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using PlaXore.Utilities;
using System;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Helper class for generating pseudo-random numbers and derivates.
    /// </summary>
    public class GameRandomHelper : RandomUtility
    {
        #region Methods
        /// Returns a vector with two random components.
        /// </summary>
        /// <returns>
        /// A vector with two components greater than or equal to zero, and less
        /// than <b>Single.MaxValue</b>.
        /// </returns>
        public static Vector2 GetRandomVector2()
        {
            return new Vector2(GetRandomFloat(0f, Single.MaxValue), GetRandomFloat(0f, Single.MaxValue));
        }

        /// Returns a vector with two random components less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random components to be generated.
        /// <paramref name="maxValue"/> must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// A vector with two components greater than or equal to zero, and less
        /// than <paramref name="maxValue"/>.
        /// </returns>
        public static Vector2 GetRandomVector2(float maxValue)
        {
            return new Vector2(GetRandomFloat(0f, maxValue), GetRandomFloat(0f, maxValue));
        }

        /// <summary>
        /// Returns a vector with two random components within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random components returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random components returned.</param>
        /// <returns>
        /// A vector with two components greater than or equal to <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        public static Vector2 GetRandomVector2(float minValue, float maxValue)
        {
            return new Vector2(GetRandomFloat(minValue, maxValue), GetRandomFloat(minValue, maxValue));
        }

        /// Returns a vector with three random components.
        /// </summary>
        /// <returns>
        /// A vector with three components greater than or equal to zero, and less
        /// than <b>Single.MaxValue</b>.
        /// </returns>
        public static Vector3 GetRandomVector3()
        {
            return new Vector3(
                GetRandomFloat(0f, Single.MaxValue),
                GetRandomFloat(0f, Single.MaxValue),
                GetRandomFloat(0f, Single.MaxValue));
        }

        /// Returns a vector with three random components less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random components to be generated.
        /// <paramref name="maxValue"/> must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// A vector with three components greater than or equal to zero, and less
        /// than <paramref name="maxValue"/>.
        /// </returns>
        public static Vector3 GetRandomVector3(float maxValue)
        {
            return new Vector3(
                GetRandomFloat(0f, maxValue),
                GetRandomFloat(0f, maxValue),
                GetRandomFloat(0f, maxValue));
        }

        /// <summary>
        /// Returns a vector with three random components within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random components returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random components returned.</param>
        /// <returns>
        /// A vector with three components greater than or equal to <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        public static Vector3 GetRandomVector3(float minValue, float maxValue)
        {
            return new Vector3(
                GetRandomFloat(minValue, maxValue),
                GetRandomFloat(minValue, maxValue),
                GetRandomFloat(minValue, maxValue));
        }

        /// <summary>
        /// Returns a random color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetRandomColor()
        {
            return new Color(
                new Vector3(
                    GetRandomFloat(0.25f, 1f),
                    GetRandomFloat(0.25f, 1f),
                    GetRandomFloat(0.25f, 1f)));
        }
        #endregion
    }
}
