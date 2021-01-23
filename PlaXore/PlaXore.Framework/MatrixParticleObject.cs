#region Header
//+ <source name="MatrixParticleObject.cs" language="C#" begin="25-Mar-2012">
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
    [System.Runtime.Serialization.DataContract]
    public abstract class MatrixParticleObject : MatrixObject
    {
        #region Constructors
        public MatrixParticleObject()
            : base()
        {
        }

        public MatrixParticleObject(GameHost gameHost)
            : base(gameHost)
        {
            IsActive = true;
        }

        public MatrixParticleObject(GameHost gameHost, Texture2D texture, Vector3 position, Vector3 scale)
            : this(gameHost)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
        }
        #endregion

        #region Properties
        [System.Runtime.Serialization.DataMember]
        public bool IsActive
        {
            get;
            set;
        }

        public override bool WriteToPhoneState
        {
            get { return false; }
        }
        #endregion
    }
}
