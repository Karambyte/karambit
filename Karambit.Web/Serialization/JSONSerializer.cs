using Karambit.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karambit.Web.Serialization
{
    public class JSONSerializer : Serializer
    {
        #region Fields
        private SerializerFormat format;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public SerializerFormat Format {
            get {
                return format;
            }
            set {
                this.format = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deserializes the object from the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public override T Deserialize<T>(Stream stream) {
            // create reader
            StreamReader reader = new StreamReader(stream);

            // deserializable 
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        /// <summary>
        /// Serializes the object to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public override void Serialize(Stream stream, object obj) {
            // create writer
            StreamWriter writer = new StreamWriter(stream);

            // serializable
            string json = JsonConvert.SerializeObject(obj, (format == SerializerFormat.Minimized) ? Formatting.None : Formatting.Indented);

            // write
            writer.Write(json);
            writer.Flush();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONSerializer"/> class.
        /// </summary>
        public JSONSerializer()
            : this(SerializerFormat.Minimized) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONSerializer"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public JSONSerializer(SerializerFormat format) {
            this.format = format;
        }
        #endregion
    }
}
