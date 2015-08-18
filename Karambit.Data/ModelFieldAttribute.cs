using System;

namespace Karambit.Data
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ModelFieldAttribute : Attribute
    {
        #region Fields
        private string name;
        private bool primary;
        #endregion

        #region Properties
        public string Name {
            get {
                return name;
            } set {
                this.name = value;
            }
        }

        public bool Primary {
            get {
                return primary;
            }  set {
                this.primary = value;
            }
        }
        #endregion

        #region Constructors
        public ModelFieldAttribute() { }

        public ModelFieldAttribute(string name) {
            this.name = name;
        }
        #endregion
    }
}
