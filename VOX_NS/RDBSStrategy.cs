using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace VOX_NS
{
    public class RDBSStrategy
    {
        #region  辅助方法

        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DbParameter GenerateInParam(string paramName, SqlDbType sqlDbType, int size, object value)
        {
            return GenerateParam(paramName, sqlDbType, size, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 生成输出参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateOutParam(string paramName, SqlDbType sqlDbType, int size)
        {
            return GenerateParam(paramName, sqlDbType, size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 生成返回参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateReturnParam(string paramName, SqlDbType sqlDbType, int size)
        {
            return GenerateParam(paramName, sqlDbType, size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="direction">方向</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DbParameter GenerateParam(string paramName, SqlDbType sqlDbType, int size, ParameterDirection direction, object value)
        {
            SqlParameter param = new SqlParameter(paramName, sqlDbType, size);
            param.Direction = direction;
            if (direction == ParameterDirection.Input && value != null)
                param.Value = value;
            return param;
        }

        #endregion

        #region  Sqlite辅助方法

        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DbParameter GenerateSqliteInParam(string paramName, DbType sqlDbType, int size, object value)
        {
            return GenerateSqliteParam(paramName, sqlDbType, size, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 生成输出参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateSqliteOutParam(string paramName, DbType sqlDbType, int size)
        {
            return GenerateSqliteParam(paramName, sqlDbType, size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 生成返回参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateSqliteReturnParam(string paramName, DbType sqlDbType, int size)
        {
            return GenerateSqliteParam(paramName, sqlDbType, size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="direction">方向</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DbParameter GenerateSqliteParam(string paramName, DbType dbType, int size, ParameterDirection direction, object value)
        {
            SQLiteParameter param = new SQLiteParameter(paramName, dbType, size);
            param.Direction = direction;
            if (direction == ParameterDirection.Input && value != null)
                param.Value = value;
            return param;
        }

        #endregion
    }
}
