using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karambit.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ModelAttribute : Attribute
    {
        #region Fields
        private string name;
        #endregion

        #region Properties
        public string Name {
            get {
                return name;
            } set {
                this.name = value;
            }
        }
        #endregion

        #region Constructors
        public ModelAttribute() { }

        public ModelAttribute(string name) {
            this.name = name;
        }
        #endregion
    }
}
