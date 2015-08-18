using System;

namespace Karambit.Data.MySql
{
    public class MySqlField
    {
        #region Fields
        private Field field;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the column name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return field.name;
            }
        }

        /// <summary>
        /// Gets the table this field belongs to.
        /// </summary>
        /// <value>The table.</value>
        public string Table {
            get {
                return field.table;
            }
        }

        public string Default {
            get {
                return field.def;
            }
        }

        public string Database {
            get {
                return field.db;
            }
        }

        public int Decimals {
            get {
                return (int)field.decimals;
            }
        }

        internal MySqlType Type {
            get {
                return (MySqlType)field.type;
            }
        }
        #endregion

        #region Methods                
        /// <summary>
        /// Casts the specified string to the field's type.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public object CastFrom(string str) {
            switch (Type) {
                case MySqlType.VARCHAR:
                    return str;
                case MySqlType.VAR_STRING:
                    return str;
                case MySqlType.STRING:
                    return str;
                case MySqlType.INT24:
                    return int.Parse(str);
                case MySqlType.TINY:
                    return sbyte.Parse(str);
                case MySqlType.SHORT:
                    return int.Parse(str);
                case MySqlType.LONG:
                    return int.Parse(str);
                case MySqlType.LONGLONG:
                    return long.Parse(str);
                case MySqlType.FLOAT:
                    return float.Parse(str);
                case MySqlType.DOUBLE:
                    return double.Parse(str);
                default:
                    throw new NotSupportedException("The field's type is not supported");
            }
        }
        #endregion

        #region Structures
        /// <summary>
        /// MySQL Field Data.
        /// </summary>
        internal struct Field
        {
#pragma warning disable 0649
            public string name;                 /* Name of column */
            public string org_name;             /* Original column name, if an alias */
            public string table;                /* Table of column if column was a field */
            public string org_table;            /* Org table name, if table was an alias */
            public string db;                   /* Database for table */
            public string catalog;	      /* Catalog for table */
            public string def;                  /* Default value (set by mysql_list_fields) */
            public uint length;       /* Width of column (create length) */
            public uint max_length;   /* Max width for selected set */
            public uint name_length;
            public uint org_name_length;
            public uint table_length;
            public uint org_table_length;
            public uint db_length;
            public uint catalog_length;
            public uint def_length;
            public uint flags;         /* Div flags */
            public uint decimals;      /* Number of decimals in field */
            public uint charsetnr;     /* Character set */
            public int type; /* Type of field. See mysql_com.h for types */
            public IntPtr extension;
#pragma warning restore 0649
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlField"/> class from raw field data.
        /// </summary>
        /// <param name="field">The field.</param>
        internal MySqlField(Field field) {
            this.field = field;
        }
        #endregion
    }
}
