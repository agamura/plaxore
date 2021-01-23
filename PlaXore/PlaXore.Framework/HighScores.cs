#region Header
//+ <source name="HighScores.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
#if WINDOWS_PHONE
using System.IO;
using System.IO.IsolatedStorage;
#endif
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Provides functionality for dealing with high scores.
    /// </summary>
    public class HighScores
    {
        #region Fields
        private GameHost gameHost;
        private Dictionary<string, HighScoreTable> highScoreTables;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HighScores"/> class with
        /// the specified game host.
        /// </summary>
        /// <param name="gameHost">The game host.</param>
        internal HighScores(GameHost gameHost)
        {
            this.gameHost = gameHost;
            FileName = "HighScores.dat";

            Clear();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the file that stores the high scores of this
        /// <see cref="HighScores"/> instance on the local storage.
        /// </summary>
        /// <value>
        /// The name of the file that stores the high scores of this
        /// <see cref="HighScores"/> instance on the local storage.
        /// </value>
        public string FileName
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes a new <see cref="HighScoreTable"/> with the specified name
        /// and size.
        /// </summary>
        /// <param name="name">The name of the <see cref="HighScoreTable"/>.</param>
        /// <param name="size">The size of the <see cref="HighScoreTable"/>.</param>
        public void InitializeTable(string name, int size)
        {
            InitializeTable(name, size, "");
        }

        /// <summary>
        /// Initializes a new <see cref="HighScoreTable"/> with the specifed name,
        /// size, and description.
        /// </summary>
        /// <param name="name">The name of the <see cref="HighScoreTable"/>.</param>
        /// <param name="size">The size of the <see cref="HighScoreTable"/>.</param>
        /// <param name="description">The description of the <see cref="HighScoreTable"/>.</param>
        public void InitializeTable(string name, int size, string description)
        {
            if (!highScoreTables.ContainsKey(name)) {
                highScoreTables.Add(name, new HighScoreTable(size, description));
            }
        }

        /// <summary>
        /// Returns the <see cref="HighScoreTable"/> identified by the specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="HighScoreTable"/> to retrieve.</param>
        /// <returns>
        /// The <see cref="HighScoreTable"/> identified by <paramref name="name"/>.
        /// </returns>
        public HighScoreTable GetTable(string name)
        {
            if (highScoreTables.ContainsKey(name)) {
                return highScoreTables[name];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Clears this <see cref="HighScores"/> instance.
        /// </summary>
        public void Clear()
        {
            if (highScoreTables == null) {
                highScoreTables = new Dictionary<string, HighScoreTable>();
            }

            foreach (HighScoreTable table in highScoreTables.Values) {
                table.Clear();
            }
        }

        /// <summary>
        /// Loads the high scores from the local storage into this <see cref="HighScores"/>
        /// instance.
        /// </summary>
        public void Load()
        {
            string fileContent;

            try {
                Clear();

#if WINDOWS_PHONE
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                    if (!store.FileExists(FileName)) {
                        return;
                    }

                    using (StreamReader streamReader = new StreamReader(store.OpenFile(FileName, FileMode.Open))) {
                        fileContent = streamReader.ReadToEnd();
                    }
                }
#else
                if (!System.IO.File.Exists(FileName)) {
                    return;
                }

                fileContent = System.IO.File.ReadAllText(FileName);
#endif
                XDocument xDocument = XDocument.Parse(fileContent);
                var result = from node in xDocument.Root.Descendants("entry")
                             select new {
                                 TableName = node.Parent.Parent.Element("name").Value,
                                 Name = node.Element("name").Value,
                                 Score = node.Element("score").Value,
                                 DateTime = node.Element("datetime").Value
                             };

                HighScoreTable table;
                foreach (var node in result) {
                    table = GetTable(node.TableName);
                    if (table != null)
                        table.AddEntry(node.Name, int.Parse(node.Score), new DateTime(Int64.Parse(node.DateTime)));
                }
            } catch {
                Clear();
            }
        }

        /// <summary>
        /// Saves the high scores in this <see cref="HighScores"/> instance into
        /// the local storage.
        /// </summary>
        public void Save()
        {
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            HighScoreTable table;

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("highscores");

            foreach (string tableName in highScoreTables.Keys) {
                table = highScoreTables[tableName];

                xmlWriter.WriteStartElement("table");
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(tableName);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("entries");

                foreach (HighScoreEntry entry in table.Entries) {
                    if (entry.DateTime != DateTime.MinValue) {
                        xmlWriter.WriteStartElement("entry");
                        xmlWriter.WriteStartElement("score");
                        xmlWriter.WriteString(entry.Score.ToString());
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(entry.Name);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteStartElement("datetime");
                        xmlWriter.WriteString(entry.DateTime.Ticks.ToString());
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Dispose();

#if WINDOWS_PHONE
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                using (StreamWriter streamWriter = new StreamWriter(store.CreateFile(FileName))) {
                    streamWriter.Write(stringBuilder.ToString());
                }
            }
#else
            System.IO.File.WriteAllText(FileName, stringBuilder.ToString());
#endif
        }
        #endregion
    }
}
