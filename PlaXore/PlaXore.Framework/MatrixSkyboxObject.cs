#region Header
//+ <source name="MatrixSkyboxObject.cs" language="C#" begin="25-Mar-2012">
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
using System;
using System.Runtime.Serialization;
#endregion

namespace PlaXore.GameFramework
{
    [DataContract]
    public class MatrixSkyboxObject : MatrixObject
    {
        #region Fields
        private static VertexPositionColorTexture[] vertices;
        private static VertexBuffer vertexBuffer;

        internal bool Rendered = false;
        #endregion

        #region Constructors
        public MatrixSkyboxObject()
            : base()
        {
        }

        public MatrixSkyboxObject(GameHost gameHost, Texture2D texture, Vector3 position, Vector3 scale)
            : base(gameHost)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
        }
        #endregion

        #region Properties
        private VertexPositionColorTexture[] Vertices
        {
            get {
                if (MatrixSkyboxObject.vertices == null) {
                    CreateVertices();
                }

                return MatrixSkyboxObject.vertices;
            }
        }

        private VertexBuffer VertexBuffer
        {
            get {
                if (MatrixSkyboxObject.vertexBuffer == null) {
                    MatrixSkyboxObject.vertexBuffer = new VertexBuffer(GameHost.GraphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.WriteOnly);
                    MatrixSkyboxObject.vertexBuffer.SetData(Vertices);
                }

                return MatrixSkyboxObject.vertexBuffer;
            }
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetIdentity();

            if (GameHost.Camera != null) {
                ApplyTransformation(Matrix.CreateTranslation(GameHost.Camera.Transformation.Translation));
            }

            ApplyStandardTransformations();
            Rendered = false;
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            PrepareEffect(effect);

            bool lightingEnabled = ((BasicEffect) effect).LightingEnabled;
            ((BasicEffect) effect).LightingEnabled = false;

            DepthStencilState depthState = effect.GraphicsDevice.DepthStencilState;
            effect.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            effect.GraphicsDevice.SetVertexBuffer(VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertices.Length / 3);
            }

            if (lightingEnabled) {
                ((BasicEffect) effect).LightingEnabled = true;
            }

            effect.GraphicsDevice.DepthStencilState = depthState;
            Rendered = true;
        }

        private void CreateVertices()
        {
            Color thisColor = Color.Black;
            MatrixSkyboxObject.vertices = new VertexPositionColorTexture[24];

            int i = 0;
            // Front face.
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Right face.
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, 0.5f, 0.5f);
            // Back face.
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(0.5f, -0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            // Left face.
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, -0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, 0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, 0.5f, -0.5f);
            MatrixSkyboxObject.vertices[i++].Position = new Vector3(-0.5f, -0.5f, 0.5f);

            i = 0;
            // Front face.
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.001f);
            // Right face.
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.25f, 0.001f);
            // Back face.
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.5f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            // Left face.
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(1f, 0.999f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(1f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(0.75f, 0.001f);
            MatrixSkyboxObject.vertices[i++].TextureCoordinate = new Vector2(1f, 0.999f);

            for (i = 0; i < MatrixSkyboxObject.vertices.Length; i++) {
                MatrixSkyboxObject.vertices[i].Color = Color.White;
            }
        }
        #endregion
    }
}
