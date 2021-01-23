#region Header
//+ <source name="AssemblyInfo.cs" language="C#" begin="28-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Reflection;
using System.Runtime.InteropServices;
#endregion

namespace PlaXore
{
    /// <summary>
    /// Provides functionality for reading assembly attributes.
    /// </summary>
    public sealed class AssemblyInfo
    {
        #region Fields
        /// <summary>
        /// Gets the globally unique identifier that identifies the assembly.
        /// </summary>
        /// <value>
        /// The globally unique identifier that identifies the assembly.
        /// </value>
        public readonly Guid Guid;

        /// <summary>
        /// Gets the name of the product the assembly belongs to.
        /// </summary>
        /// <value>The name of the product the assembly belongs to.</value>
        public readonly string Product;

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        public readonly string Title;

        /// <summary>
        /// Gets the assembly description.
        /// </summary>
        /// <value>The assembly description.</value>
        public readonly string Description;

        /// <summary>
        /// Gets the name of the company that implemented the assembly.
        /// </summary>
        /// <value>The name of the company that implemented the assembly.</value>
        public readonly string Company;

        /// <summary>
        /// Gets the copyright information for the assembly.
        /// </summary>
        /// <value>The copyright information for the assembly.</value>
        public readonly string Copyright;

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public readonly Version Version;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class.
        /// </summary>
        /// <remarks>
        /// By default assembly attributes are read from the calling assembly.
        /// </remarks>
        public AssemblyInfo()
            : this(PortableAssembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class
        /// with the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to read attributes from.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembly"/> is <see langword="null"/>.
        /// </exception>
        public AssemblyInfo(Assembly assembly)
        {
            if (assembly == null) { throw new ArgumentNullException("assembly"); }
            Assembly = assembly;

            foreach (object attribute in assembly.GetCustomAttributes(false.GetType())) {
                if (attribute.GetType() == typeof(GuidAttribute)) {
                    Guid = new Guid((attribute as GuidAttribute).Value);
                } else if (attribute.GetType() == typeof(AssemblyTitleAttribute)) {
                    Title = (attribute as AssemblyTitleAttribute).Title;
                } else if (attribute.GetType() == typeof(AssemblyProductAttribute)) {
                    Product = (attribute as AssemblyProductAttribute).Product;
                } else if (attribute.GetType() == typeof(AssemblyDescriptionAttribute)) {
                    Description = (attribute as AssemblyDescriptionAttribute).Description;
                } else if (attribute.GetType() == typeof(AssemblyCompanyAttribute)) {
                    Company = (attribute as AssemblyCompanyAttribute).Company;
                } else if (attribute.GetType() == typeof(AssemblyCopyrightAttribute)) {
                    Copyright = (attribute as AssemblyCopyrightAttribute).Copyright;
                }
            }

            // AssemblyVersionAttribute does not return any value, therefore version
            // information has to be retrieved from assembly's full name
            Version = new Version(assembly.FullName.Split('=')[1].Split(',')[0]);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the assembly this <see cref="AssemblyInfo"/> refers to.
        /// </summary>
        /// <value>
        /// The assembly this <see cref="AssemblyInfo"/> refers to.
        /// </value>
        public Assembly Assembly
        {
            get;
            private set;
        }
        #endregion
    }
}
