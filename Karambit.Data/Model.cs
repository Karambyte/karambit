using System;

namespace Karambit.Data
{
    public abstract class Model
    {
        #region Fields
        protected object id;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Model(object id) {
            this.id = id;
        }
        #endregion
    }
}
