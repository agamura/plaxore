#region Header
//+ <source name="SettingsManager.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using Windows.Storage;
#endregion

namespace PlaXore.GameFramework
{
    public class SettingsManager
    {
        #region Fields
        private GameHost gameHost;
#if WINDOWS
        private Dictionary<string, string> settings = new Dictionary<string, string>();
        private const string FileName = "Settings.dat";
#endif
        #endregion

        #region Constructors
        internal SettingsManager(GameHost gamehost)
        {
            gameHost = gamehost;
#if WINDOWS
            LoadSettings();
#endif
        }
        #endregion

        #region Methods
        public void SetValue(string settingName, string value)
        {
            settingName = settingName.ToLower();

#if WINDOWS_UWP
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.Keys.Contains(settingName)) {
                localSettings.Values[settingName] = value;
            } else {
                localSettings.Values.Add(settingName, value);
            }
#else
            if (settings.ContainsKey(settingName)) {
                settings[settingName] = value;
            } else {
                settings.Add(settingName, value);
            }

            SaveSettings();
#endif
        }

        public void SetValue(string settingName, int value)
        {
            SetValue(settingName, value.ToString());
        }

        public void SetValue(string settingName, float value)
        {
            SetValue(settingName, value.ToString());
        }

        public void SetValue(string settingName, bool value)
        {
            SetValue(settingName, value.ToString());
        }

        public void SetValue(string settingName, DateTime value)
        {
            SetValue(settingName, value.Ticks.ToString());
        }

        public string GetValue(string settingName, string defaultValue)
        {
            settingName = settingName.ToLower();

#if WINDOWS_UWP
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.Keys.Contains(settingName)) {
                return localSettings.Values[settingName].ToString();
            } else {
                return defaultValue;
            }
#else
            if (settings.ContainsKey(settingName)) {
                return settings[settingName];
            }

            return defaultValue;
#endif
        }

        public int GetValue(string settingName, int defaultValue)
        {
            return int.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        public float GetValue(string settingName, float defaultValue)
        {
            return float.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        public bool GetValue(string settingName, bool defaultValue)
        {
            return bool.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        public DateTime GetValue(string settingName, DateTime defaultValue)
        {
            return new DateTime(Int64.Parse(GetValue(settingName, defaultValue.Ticks.ToString())));
        }

        public void ClearValues()
        {
#if WINDOWS_UWP
            ApplicationData.Current.LocalSettings.Values.Clear();
#else
            settings.Clear();
            SaveSettings();
#endif
        }

        public void DeleteValue(string settingName)
        {
            settingName = settingName.ToLower();

#if WINDOWS_UWP
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.Keys.Contains(settingName)) {
                localSettings.Values.Remove(settingName);
            }
#else
            if (settings.ContainsKey(settingName)) {
                settings.Remove(settingName);
                SaveSettings();
            }
#endif
        }

#if !WINDOWS_UWP
        private void SaveSettings() {
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("settings");

            foreach (KeyValuePair<string, string> setting in settings) {
                xmlWriter.WriteStartElement("setting");
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(setting.Key);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("value");
                xmlWriter.WriteString(setting.Value);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            System.IO.File.WriteAllText(FileName, stringBuilder.ToString());
        }

        private void LoadSettings()
        {
            string settingsContent;

            try {
                settings.Clear();

                if (!System.IO.File.Exists(FileName)) {
                    return;
                }

                settingsContent = System.IO.File.ReadAllText(FileName);

                XDocument xDocument = XDocument.Parse(settingsContent);
                var result = from node in xDocument.Descendants("setting")
                             select new {
                                 Name = node.Element("name").Value.ToString(),
                                 Value = node.Element("value").Value.ToString()
                             };

                foreach (var node in result) {
                    SetValue(node.Name, node.Value);
                }
            } catch {
                settings.Clear();
            }
        }
#endif

        public void RetrieveValues()
        {
            foreach (GameObject gameObject in gameHost.GameObjects) {
                if (gameObject is SettingsItemObject) {
                    SetValue(((SettingsItemObject) gameObject).Name, ((SettingsItemObject) gameObject).SelectedValue);
                }
            }
        }
        #endregion
    }
}
