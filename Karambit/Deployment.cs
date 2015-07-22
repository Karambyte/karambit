using System;

namespace Karambit
{
    public enum Deployment
    {
        /// <summary>
        /// Deployed as production, full debugging and error information.
        /// </summary>
        Production,

        /// <summary>
        /// Deployed as release, minimal debugging and error information.
        /// </summary>
        Release
    }
}
