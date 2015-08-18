using System;

namespace Karambit.Data.MySql
{
    internal enum MySqlType
    {
        DECIMAL, 
        TINY,
        SHORT,
        LONG,
        FLOAT, 
        DOUBLE,
        NULL, 
        TIMESTAMP,
        LONGLONG,
        INT24,
        DATE,
        TIME,
        DATETIME, 
        YEAR,
        NEWDATE, 
        VARCHAR,
        BIT,
        TIMESTAMP2,
        DATETIME2,
        TIME2,
        NEWDECIMAL = 246,
        ENUM = 247,
        SET = 248,
        TINY_BLOB = 249,
        MEDIUM_BLOB = 250,
        LONG_BLOB = 251,
        BLOB = 252,
        VAR_STRING = 253,
        STRING = 254,
        GEOMETRY = 255
    }
}
