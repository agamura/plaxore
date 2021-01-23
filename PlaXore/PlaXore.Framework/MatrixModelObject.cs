#region Header
//+ <source name="MatrixModelObject.cs" language="C#" begin="25-Mar-2012">
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
#endregion

namespace PlaXore.GameFramework
{
    [System.Runtime.Serialization.DataContract]
    public class MatrixModelObject : MatrixObject
    {
        #region Fields
        private Model model;
        private string name;
        #endregion

        #region Constructors
        public MatrixModelObject()
            : base()
        {
        }

        public MatrixModelObject(GameHost gameHost)
            : base(gameHost)
        {
        }

        public MatrixModelObject(GameHost gameHost, Vector3 position, Model model)
            : this(gameHost)
        {
            Position = position;
            Model = model;
        }
        #endregion

        #region Properties
        public Model Model
        {
            get {
                if (model == null && !String.IsNullOrEmpty(name) && GameHost != null) {
                    model = GameHost.Models[name];
                }

                return model;
            }
            set {
                if (model != value) {
                    model = value;
                    name = GameHost.GetContentObjectName<Model>(GameHost.Models, value);
                }
            }
        }

        [System.Runtime.Serialization.DataMember]
        public virtual string Name
        {
            get { return name; }
            set {
                if (name != value) {
                    name = value;
                    model = null;
                }
            }
        }

        public override Texture2D Texture
        {
            get {
                if (base.Texture != null) {
                    return base.Texture;
                }

                if (Model == null || Model.Meshes.Count == 0 || Model.Meshes[0].MeshParts.Count == 0) {
                    return null;
                }

                return ((BasicEffect) Model.Meshes[0].MeshParts[0].Effect).Texture;
            }
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetIdentity();
            ApplyStandardTransformations();
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            PrepareEffect(effect);
            DrawModel((BasicEffect) effect);
        }

        protected virtual void DrawModel(BasicEffect effect)
        {
            if (Model == null) { return; }

            Matrix initialWorld = effect.World;
            Matrix[] boneTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in Model.Meshes) {
                effect.World = boneTransforms[mesh.ParentBone.Index] * effect.World;

                foreach (ModelMeshPart meshPart in mesh.MeshParts) {
                    if (base.Texture != null) {
                        SetEffectTexture(effect, base.Texture);
                    } else {
                        SetEffectTexture(effect, ((BasicEffect) meshPart.Effect).Texture);
                    }

                    effect.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                    effect.GraphicsDevice.Indices = meshPart.IndexBuffer;

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }

            effect.World = initialWorld;
        }
        #endregion
    }
}
