using Karambit.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Karambit.IO
{
    /// <summary>
    /// A class representing an Ini file.
    /// </summary>
    public sealed class IniFile
    {
        #region Fields
        private string path = null;
        private Dictionary<string, Dictionary<string, string>> sections = new Dictionary<string, Dictionary<string, string>>(new CaseInsensitiveEqualityComparer());
        #endregion

        #region Properties
        /// <summary>
        /// Gets the source, the filename or stream type.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source {
            get {
                return (path[0] == ']') ? path : Path.GetFileName(path);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the specified key in the provided section to the requested value.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string section, string key, string value) {
            // add section
            if (!sections.ContainsKey(section))
                sections.Add(section, new Dictionary<string, string>(new CaseInsensitiveEqualityComparer()));

            // set key
            sections[section][key] = value;
        }

        /// <summary>
        /// Gets the value from the specified section and provided key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="def">The default value.</param>
        /// <returns></returns>
        public string Get(string section, string key, string def) {
            if (!sections.ContainsKey(section))
                return def;
            else if (!sections[section].ContainsKey(key))
                return def;
            else
                return sections[section][key];
        }

        /// <summary>
        /// Gets the value from the specified section and provided key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Get(string section, string key) {
            return Get(section, key, null);
        }

        /// <summary>
        /// Saves the ini file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream) {
            // writer
            StreamWriter writer = new StreamWriter(stream);

            // loop
            foreach (KeyValuePair<string, Dictionary<string, string>> section in sections) {
                // section
                writer.WriteLine("[" + section.Key + "]");

                // key/values
                foreach (KeyValuePair<string, string> kv in section.Value) {
                    writer.WriteLine(kv.Key + " = " + kv.Value);
                }

                // flush section
                writer.Flush();
            }
        }

        /// <summary>
        /// Saves the ini file to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(string path) {
            // path
            this.path = path;

            // stream
            using (FileStream fs = new FileStream(path, FileMode.Create)) {
                Save(fs);
            }
        }

        /// <summary>
        /// Loads the ini file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Load(Stream stream) {
            // source
            if (!(stream is FileStream))
                path = "[" + stream.GetType().Name + "]";
            
            // reader
            StreamReader reader = new StreamReader(stream);

            // variables
            string line = reader.ReadLine();
            string section = "Root";
            int lineNum = 1;

            // loop
            while (line != null) {
                // trim
                line = line.Trim();

                if (line[0] == ';' || line == "") { // comment/newline
                    goto next;
                } else if (line[0] == '[') { // section
                    if (line[line.Length - 1] != ']')
                        throw new Exception("The section is not terminated on line " + Source + ":" + lineNum);

                    section = line.Substring(1, line.Length - 2);

                    if (!sections.ContainsKey(section))
                        sections.Add(section, new Dictionary<string,string>(new CaseInsensitiveEqualityComparer()));
                } else { // key/value
                    if (line.IndexOf('=') == -1)
                        throw new Exception("The format is invalid " + Source + ":" + lineNum);

                    // process key/value
                    string[] kv = line.Split('=');
                    kv[0] = kv[0].Trim();
                    kv[1] = kv[1].Trim();

                    // add
                    Set(section, kv[0], kv[1]);
                }

                // next
            next:
                line = reader.ReadLine();
                lineNum++;
            }
        }

        /// <summary>
        /// Loads the ini file from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Load(string path) {
            // path
            this.path = path;

            // stream
            using (FileStream fs = new FileStream(path, FileMode.Open)) {
                Load(fs);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        public IniFile() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class and loads from the specified file.
        /// </summary>
        /// <param name="path">The path.</param>
        public IniFile(string path) {
            Load(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class and loads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public IniFile(Stream stream) {
            Load(stream);
        }
        #endregion
    }
}
