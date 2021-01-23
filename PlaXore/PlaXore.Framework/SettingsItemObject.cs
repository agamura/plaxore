#region Header
//+ <source name="SettingsItemObject.cs" language="C#" begin="25-Mar-2012">
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
    public class SettingsItemObject : TextObject
    {
        #region Fields
        private int valueIndex;
        #endregion

        #region Constructors
        public SettingsItemObject(GameHost gameHost, Vector2 position, SpriteFont font, float scale, SettingsManager settingsManager, string name, string title, string defaultValue, string[] values)
            : base(gameHost)
        {
            Position = position;
            Scale = new Vector2(scale);
            Font = font;
            Name = name;
            Title = title;
            Values = values;

            valueIndex = GetValueIndex(settingsManager.GetValue(name, defaultValue));

            SetText();
        }
        #endregion

        #region Properties
        public string SelectedValue
        {
            get { return Values[valueIndex]; }
            set { valueIndex = GetValueIndex(value); }
        }

        public string Name
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string[] Values
        {
            get;
            set;
        }
        #endregion

        #region Methods
        private int GetValueIndex(string value)
        {
            for (int i = 0; i < Values.Length; i++) {
                if (Values[i] == value) {
                    return i;
                }
            }

            return 0;
        }

        public void SelectNextValue()
        {
            valueIndex++;

            if (valueIndex >= Values.Length) {
                valueIndex = 0;
            }

            SetText();
        }

        private void SetText()
        {
            Text = Title + ": " + SelectedValue;
        }
        #endregion
    }
}
