using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Karambit.Data.MySql
{
    public class MySqlResult
    {
        #region Fields
        private IntPtr handle;
        private MySqlField[] fields;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of rows in the result set.
        /// If the result is buffered (not stored), this value will be zero.
        /// </summary>
        /// <value>The number of rows.</value>
        public uint Count {
            get {
                return MySqlNative.mysql_num_rows(handle);
            }
        }

        /// <summary>
        /// Gets the fields for this result set.
        /// </summary>
        /// <value>The fields.</value>
        public MySqlField[] Fields {
            get {
                if (fields == null)
                    FetchFields();

                return fields;
            }
        }

        /// <summary>
        /// Gets the underlying handle for this result.
        /// </summary>
        /// <value>The handle.</value>
        public IntPtr Handle {
            get {
                return handle;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Fetches the fields for the result set.
        /// </summary>
        private void FetchFields() {
            // fetch fields
            IntPtr fieldsArr = MySqlNative.mysql_fetch_fields(handle);
            int numFields = (int)MySqlNative.mysql_num_fields(handle);

            // create
            fields = new MySqlField[numFields];

            // fields
            for (int i = 0; i < numFields; i++) {
                // get field
                MySqlField.Field field = (MySqlField.Field)Marshal.PtrToStructure(fieldsArr + (i * Marshal.SizeOf(typeof(MySqlField.Field))), typeof(MySqlField.Field));

                // add
                fields[i] = new MySqlField(field);
            }
        }

        /// <summary>
        /// Fetches the next row in the set.
        /// </summary>
        /// <returns></returns>
        public MySqlRow Fetch() {
            if (fields == null)
                FetchFields();

            return MySqlRow.FetchFromResult(this);
        }

        /// <summary>
        /// Fetches the next row and casts it to a model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// The provided type must be a model
        /// or
        /// The provided model has no attribute
        /// or
        /// The provided model does not match the table
        /// or
        /// The model field ' + field.Name + ' has no attribute
        /// </exception>
        public T Fetch<T>() {
            // test
            if (!typeof(Model).IsAssignableFrom(typeof(T)))
                throw new Exception("The provided type must be a model");

            // get type data
            Type t = typeof(T);

            // get model attributes
            ModelAttribute modelAtt = t.GetCustomAttribute<ModelAttribute>();

            // check table
            if (modelAtt.Name.ToLower() != Fields[0].Table.ToLower())
                throw new Exception("The provided model does not match the table");

            // create
            object obj = null;

            // fetch
            MySqlRow row = Fetch();

            if (row == null)
                return default(T);

            // get fields
            foreach (FieldInfo field in t.GetFields()) {
                // get field attributes
                ModelFieldAttribute fieldAtt = field.GetCustomAttribute<ModelFieldAttribute>();

                // find mysql field
                for (int i = 0; i < Fields.Length; i++) {
                    if (Fields[i].Name == fieldAtt.Name) {
                        if (fieldAtt.Primary) {
                            obj = Activator.CreateInstance(typeof(T), row.Values[i]);
                        } else if (obj == null) {
                            throw new Exception("The model does not define a valid primary key");
                        }

                        field.SetValue(obj, row.Values[i]);
                    }
                }
            }

            return (T)obj;
        }
        #endregion

        #region Structures
        
        #endregion

        #region Constructors
        ~MySqlResult() {
            MySqlNative.mysql_free_result(handle);
        }

        internal MySqlResult(IntPtr handle) {
            this.handle = handle;
        }
        #endregion
    }
}
