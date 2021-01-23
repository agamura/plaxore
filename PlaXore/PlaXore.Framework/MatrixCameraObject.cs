#region Header
//+ <source name="MatrixCameraObject.cs" language="C#" begin="25-Mar-2012">
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
    public class MatrixCameraObject : MatrixObject
    {
        #region Fields
        private Vector3 lastChaseCamDelta = new Vector3(0, 0, 1);
        #endregion

        #region Constructors
        public MatrixCameraObject()
            : base()
        {
        }

        public MatrixCameraObject(GameHost game)
            : base(game)
        {
            ChaseDistance = 1;
            ChaseElevation = 0.1f;
        }
        #endregion

        #region Properties
        [System.Runtime.Serialization.DataMember]
        public Vector3 LookAtTarget
        {
            get;
            set;
        }

        public MatrixModelObject ChaseObject
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public float ChaseDistance
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public float ChaseElevation
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ChaseObject == null) {
                SetIdentity();
                ApplyStandardTransformations();
                return;
            }

            Vector3 delta = Position - ChaseObject.Position;
            delta.Normalize();

            if (delta == Vector3.Zero) {
                delta = lastChaseCamDelta;
            } else {
                lastChaseCamDelta = delta;
            }

            SetIdentity();
            ApplyTransformation(Matrix.CreateTranslation(ChaseObject.Position));

            if (ChaseDistance != 0) {
                ApplyTransformation(Matrix.CreateTranslation(delta * ChaseDistance));
                ApplyTransformation(Matrix.CreateTranslation(0, ChaseElevation, 0));
            } else {
                ApplyTransformation(Matrix.CreateTranslation(delta * 0.01f));
            }

            LookAtTarget = ChaseObject.Position;
            Position = ChaseObject.Position;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            ((BasicEffect) effect).View = Matrix.CreateLookAt(Transformation.Translation, LookAtTarget, Transformation.Up);
        }
        #endregion
    }
}
