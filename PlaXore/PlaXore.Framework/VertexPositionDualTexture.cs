#region Header
//+ <source name="VertexPositionDualTexture.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace PlaXore.GameFramework
{
    public struct VertexPositionDualTexture : IVertexType
    {
        #region Fields
        public Vector3 Position;
        public Vector2 Coordinate0;
        public Vector2 Coordinate1;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
        );
        #endregion

        #region Constructors
        public VertexPositionDualTexture(Vector3 position, Vector2 coordinate0, Vector2 coordinate1)
        {
            Position = position;
            Coordinate0 = coordinate0;
            Coordinate1 = coordinate1;
        }
        #endregion

        #region Properties
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
        #endregion
    }
}
