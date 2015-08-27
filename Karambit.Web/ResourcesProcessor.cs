using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karambit.Web
{
    internal class ResourcesProcessor
    {
        #region Fields
        private static Dictionary<int, string> cachedStatus = null;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the status strings.
        /// </summary>
        /// <value>The status strings.</value>
        public static Dictionary<int, string> StatusStrings {
            get {
                if (cachedStatus == null) {
                    cachedStatus = JsonConvert.DeserializeObject<Dictionary<int, string>>(Resources.status);
                }

                return cachedStatus;
            }
        }
        #endregion
    }
}
