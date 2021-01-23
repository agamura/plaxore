#region Header
//+ <source name="MatrixObject.cs" language="C#" begin="25-Mar-2012">
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
    public class MatrixObject : GameObject
    {
        #region Fields
        private Texture2D texture;
        private string textureName;
        private Texture2D texture2;
        private string texture2Name;
        #endregion

        #region Constructors
        public MatrixObject()
            : base()
        {
        }

        public MatrixObject(GameHost gameHost)
            : base(gameHost)
        {
            Transformation = Matrix.Identity;
            Scale = Vector3.One;
            ObjectColor = Color.White;
            SpecularColor = Color.Black;
            SpecularPower = 1;
            EmissiveColor = Color.Black;
            EnvironmentMapAmount = 1;
            EnvironmentMapSpecular = Color.Black;
            FresnelFactor = 0;
        }

        public MatrixObject(GameHost gameHost, Vector3 position)
            : this(gameHost)
        {
            Position = position;
        }

        public MatrixObject(GameHost gameHost, Vector3 position, Texture2D texture)
            : this(gameHost, position)
        {
            Texture = texture;
        }
        #endregion

        #region Properties
        public virtual Texture2D Texture
        {
            get {
                if (texture == null && !String.IsNullOrEmpty(textureName) && GameHost != null) {
                    texture = GameHost.Textures[textureName];
                }

                return texture;
            }
            set {
                if (texture != value) {
                    texture = value;
                    textureName = GameHost.GetContentObjectName<Texture2D>(GameHost.Textures, value);
                }
            }
        }

        [DataMember]
        public virtual string TextureName
        {
            get { return textureName; }
            set {
                if (textureName != value) {
                    textureName = value;
                    texture = null;
                }
            }
        }

        public virtual Texture2D Texture2
        {
            get {
                if (texture2 == null && !String.IsNullOrEmpty(texture2Name) && GameHost != null) {
                    texture2 = GameHost.Textures[texture2Name];
                }

                return texture2;
            }
            set {
                if (texture2 != value) {
                    texture2 = value;
                    texture2Name = GameHost.GetContentObjectName<Texture2D>(GameHost.Textures, value);
                }
            }
        }

        [DataMember]
        public virtual string Texture2Name
        {
            get { return texture2Name; }
            set {
                if (texture2Name != value) {
                    texture2Name = value;
                    texture2 = null;
                }
            }
        }

        [DataMember]
        public virtual Matrix Transformation
        {
            get;
            set;
        }

        [DataMember]
        public virtual float PositionX
        {
            get;
            set;
        }

        [DataMember]
        public virtual float PositionY
        {
            get;
            set;
        }

        [DataMember]
        public virtual float PositionZ
        {
            get;
            set;
        }

        [DataMember]
        public virtual float AngleX
        {
            get;
            set;
        }

        [DataMember]
        public virtual float AngleY
        {
            get;
            set;
        }

        [DataMember]
        public virtual float AngleZ
        {
            get;
            set;
        }

        [DataMember]
        public virtual float ScaleX
        {
            get;
            set;
        }

        [DataMember]
        public virtual float ScaleY
        {
            get;
            set;
        }

        [DataMember]
        public virtual float ScaleZ
        {
            get;
            set;
        }

        [DataMember]
        public virtual Color ObjectColor
        {
            get;
            set;
        }

        [DataMember]
        public virtual Color SpecularColor
        {
            get;
            set;
        }

        [DataMember]
        public virtual float SpecularPower
        {
            get;
            set;
        }

        [DataMember]
        public virtual Color EmissiveColor
        {
            get;
            set;
        }

        [DataMember]
        public virtual float EnvironmentMapAmount
        {
            get;
            set;
        }

        [DataMember]
        public virtual Color EnvironmentMapSpecular
        {
            get;
            set;
        }

        [DataMember]
        public virtual float FresnelFactor
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get { return new Vector3(PositionX, PositionY, PositionZ); }
            set {
                PositionX = value.X;
                PositionY = value.Y;
                PositionZ = value.Z;
            }
        }

        public Vector3 Angle
        {
            get { return new Vector3(AngleX, AngleY, AngleZ); }
            set {
                AngleX = value.X;
                AngleY = value.Y;
                AngleZ = value.Z;
            }
        }

        public Vector3 Scale
        {
            get { return new Vector3(ScaleX, ScaleY, ScaleZ); }
            set {
                ScaleX = value.X;
                ScaleY = value.Y;
                ScaleZ = value.Z;
            }
        }
        #endregion

        #region Methods
        public virtual void Draw(GameTime gameTime, Effect effect)
        { 
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
        }

        public override bool IsPointInObject(Vector2 point)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            return false;
        }

        public void CalculateVertexNormals(VertexPositionNormalTexture[] vertices)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            short[] indices = new short[vertices.Length];

            for (short i = 0; i < indices.Length; i++) {
                indices[i] = i;
            }

            CalculateVertexNormals(vertices, indices);
        }

        public void CalculateVertexNormals(VertexPositionNormalTexture[] vertices, short[] indices)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }

            Vector3 vectora;
            Vector3 vectorb;
            Vector3 normal;

            for (int index = 0; index < indices.Length; index += 3) {
                vectora = vertices[index + 2].Position - vertices[index + 1].Position;
                vectorb = vertices[index + 1].Position - vertices[index + 0].Position;
                normal = Vector3.Cross(vectora, vectorb);

                normal.Normalize();

                vertices[index].Normal = normal;
                vertices[index + 1].Normal = normal;
                vertices[index + 2].Normal = normal;
            }
        }

        protected void ApplyStandardTransformations()
        {
            Matrix result = Transformation;

            if (PositionX != 0 || PositionY != 0 || PositionZ != 0) {
                result = Matrix.CreateTranslation(Position) * result;
            }

            if (AngleX != 0) {
                result = Matrix.CreateRotationX(AngleX) * result;
            }

            if (AngleY != 0) {
                result = Matrix.CreateRotationY(AngleY) * result;
            }

            if (AngleZ != 0) {
                result = Matrix.CreateRotationZ(AngleZ) * result;
            }

            if (ScaleX != 1 || ScaleY != 1 || ScaleZ != 1) {
                result = Matrix.CreateScale(Scale) * result;
            }

            Transformation = result;
        }

        protected void SetIdentity()
        {
            Transformation = Matrix.Identity;
        }

        protected void ApplyTransformation(Matrix transformation)
        {
            Transformation = transformation * Transformation;
        }

        protected void PrepareEffect(Effect effect)
        {
            if (effect is BasicEffect) {
                PrepareEffect((BasicEffect) effect);
                return;
            }

            if (effect is AlphaTestEffect) {
                PrepareEffect((AlphaTestEffect) effect);
                return;
            }

            if (effect is DualTextureEffect) {
                PrepareEffect((DualTextureEffect) effect);
                return;
            }

            if (effect is EnvironmentMapEffect) {
                PrepareEffect((EnvironmentMapEffect) effect);
                return;
            }

            throw new NotSupportedException("Cannot prepare effects of type '" + effect.GetType().Name + "', not currently implemented.");
        }

        protected void PrepareEffect(BasicEffect effect)
        {
            SetEffectTexture(effect, Texture);

            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.SpecularColor = SpecularColor.ToVector3();
            effect.SpecularPower = SpecularPower;
            effect.EmissiveColor = EmissiveColor.ToVector3();
            effect.Alpha = (float) ObjectColor.A / 255f;
            effect.World = Transformation;
        }

        protected void PrepareEffect(AlphaTestEffect effect)
        {
            if (effect.Texture != Texture) {
                effect.Texture = Texture;
            }

            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.Alpha = (float) ObjectColor.A / 255f;
            effect.World = Transformation;
        }

        protected void PrepareEffect(DualTextureEffect effect)
        {
            if (effect.Texture != Texture) {
                effect.Texture = Texture;
            }

            if (effect.Texture2 != Texture2) {
                effect.Texture2 = Texture2;
            }

            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.Alpha = (float) ObjectColor.A / 255f;
            effect.World = Transformation;
        }

        protected void PrepareEffect(EnvironmentMapEffect effect)
        {
            if (effect.Texture != Texture && Texture != null) {
                effect.Texture = Texture;
            }

            effect.DiffuseColor = ObjectColor.ToVector3();
            effect.EmissiveColor = EmissiveColor.ToVector3();
            effect.Alpha = (float) ObjectColor.A / 255f;

            if (effect.Texture != null) {
                effect.EnvironmentMapAmount = EnvironmentMapAmount;
                effect.EnvironmentMapSpecular = EnvironmentMapSpecular.ToVector3();
                effect.FresnelFactor = FresnelFactor;
            } else {
                effect.EnvironmentMapAmount = 1f;
                effect.EnvironmentMapSpecular = Vector3.Zero;
                effect.FresnelFactor = 0f;
            }

            effect.World = Transformation;
        }

        protected void SetEffectTexture(BasicEffect effect, Texture2D texture)
        {
            if (texture == null) {
                effect.TextureEnabled = false;
            } else {
                effect.TextureEnabled = true;
                if (texture != effect.Texture)
                    effect.Texture = texture;
            }
        }
        #endregion
    }
}
