#region Header
//+ <source name="PortableAssembly.cs" language="C#" begin="14-Mar-2016">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2016">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Reflection;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Represents an assembly, which is a reusable, versionable, and self-describing building block
    /// of a common language runtime application.
    /// </summary>
    public class PortableAssembly
    {
        #region Methods
        /// <summary>
        /// Returns the assembly of the method that invoked the currently executing method.
        /// </summary>
        /// <returns>
        /// The Assembly object of the method that invoked the currently executing method.
        /// </returns>
        public static Assembly GetCallingAssembly()
        {
            return (Assembly)typeof(Assembly).GetTypeInfo()
                .GetDeclaredMethod("GetCallingAssembly")
                .Invoke(null, new object[0]);
        }

        /// <summary>
        /// Returns the assembly that contains the code that is currently executing.
        /// </summary>
        /// <returns>
        /// The assembly that contains the code that is currently executing.
        /// </returns>
        public static Assembly GetExecutingAssembly()
        {
            return (Assembly)typeof(Assembly).GetTypeInfo()
                .GetDeclaredMethod("GetExecutingAssembly")
                .Invoke(null, new object[0]);
        }
        #endregion
    }
}
