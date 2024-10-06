using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace VOX_NS
{
    public class SqliteHelper
    {
        private static object _locker = new object();//锁对象

        private static DbProviderFactory _factory;//抽象数据工厂

        private static string _rdbstablepre;//关系数据库对象前缀
        private static string _connectionstring;//关系数据库连接字符串

        /// <summary>
        /// 关系数据库对象前缀
        /// </summary>
        public static string RDBSTablePre
        {
            get { return _rdbstablepre; }
        }

        /// <summary>
        /// 关系数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionstring; }
        }

#if DEBUG
        private static int _executecount = 0;
        private static string _executedetail = string.Empty;

        /// <summary>
        /// 数据库执行次数
        /// </summary>
        public static int ExecuteCount
        {
            get { return _executecount; }
            set { _executecount = value; }
        }

        /// <summary>
        /// 数据库执行细节
        /// </summary>
        public static string ExecuteDetail
        {
            get { return _executedetail; }
            set { _executedetail = value; }
        }

        /// <summary>
        /// 设置数据库执行细节
        /// </summary>
        /// <param name="commandText">数据库执行语句</param>
        /// <param name="startTime">数据库执行开始时间</param>
        /// <param name="endTime">数据库执行结束时间</param>
        /// <param name="commandParameters">数据库执行参数列表</param>
        /// <returns></returns>
        private static string GetExecuteDetail(string commandText, DateTime startTime, DateTime endTime, DbParameter[] commandParameters)
        {
            if (commandParameters != null && commandParameters.Length > 0)
            {
                StringBuilder paramdetails = new StringBuilder("------ExecuteDetail------\r\n");
                paramdetails.AppendFormat("CommandText:{0}\r\n", commandText);
                paramdetails.AppendFormat("ExecuteTime:{0}\r\n", endTime.Subtract(startTime).TotalMilliseconds / 1000);
                foreach (DbParameter param in commandParameters)
                {
                    if (param == null)
                        continue;

                    paramdetails.Append("---\r\n");
                    paramdetails.AppendFormat("ParameterName:{0}\r\n", param.ParameterName);
                    paramdetails.AppendFormat("DbType:{0}\r\n", param.DbType);
                    paramdetails.AppendFormat("Value:{0}\r\n", param.Value);
                    paramdetails.Append("---\r\n");
                }
                return paramdetails.Append("------ExecuteDetail------\r\n").ToString();
            }
            return string.Empty;
        }
#endif

        static SqliteHelper()
        {
            //设置数据工厂
            _factory = SQLiteFactory.Instance;
            //设置关系数据库对象前缀
            _rdbstablepre = "vox";
            //设置关系数据库连接字符串
            _connectionstring = "Data Source=sqlite.db;Version=3;";
        }

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, null);
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(cmdType, cmdText, null);
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif
            DbCommand cmd = _factory.CreateCommand();

            using (DbConnection conn = _factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                int val = cmd.ExecuteNonQuery();
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return val;
            }
        }



        public static int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(trans, cmdType, cmdText, null);
        }

        public static int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            int val = cmd.ExecuteNonQuery();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteReader

        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            return ExecuteReader(cmdType, cmdText, null);
        }

        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = ConnectionString;

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }



        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteReader(trans, cmdType, cmdText, null);
        }

        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            DbDataReader rdr = cmd.ExecuteReader();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return rdr;

        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(cmdType, cmdText, null);
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();

            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                object val = cmd.ExecuteScalar();
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return val;
            }
        }



        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(trans, cmdType, cmdText, null);
        }

        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            object val = cmd.ExecuteScalar();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteDataset

        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText)
        {
            return ExecuteDataset(cmdType, cmdText, null);
        }

        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            DbDataAdapter adapter = _factory.CreateDataAdapter();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();

#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                adapter.Fill(ds);
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                adapter.Dispose();
                conn.Close();
            }
        }



        public static DataSet ExecuteDataset(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteDataset(trans, cmdType, cmdText, null);
        }

        public static DataSet ExecuteDataset(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbDataAdapter adapter = _factory.CreateDataAdapter();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();

#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            adapter.Fill(ds);
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return ds;
        }

        #endregion

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    if (parm != null)
                    {
                        if ((parm.Direction == ParameterDirection.InputOutput || parm.Direction == ParameterDirection.Input) &&
                            (parm.Value == null))
                        {
                            parm.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parm);
                    }
                }
            }
        }

        #region  辅助方法

        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DbParameter GenerateInParam(string paramName, object value)
        {
            return GenerateParam(paramName, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 生成输出参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateOutParam(string paramName)
        {
            return GenerateParam(paramName, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 生成返回参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="sqlDbType">参数类型</param>
        /// <param name="size">类型大小</param>
        /// <returns></returns>
        public static DbParameter GenerateReturnParam(string paramName)
        {
            return GenerateParam(paramName, ParameterDirection.ReturnValue, null);
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
        public static DbParameter GenerateParam(string paramName, ParameterDirection direction, object value)
        {
            SQLiteParameter param = new SQLiteParameter(paramName);
            param.Direction = direction;
            if (direction == ParameterDirection.Input && value != null)
                param.Value = value;
            return param;
        }

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public string RunSql(string sql)
        {
            if (!IsNullOrWhiteSpace(sql))
            {
                SQLiteConnection conn = new SQLiteConnection(SqliteHelper.ConnectionString);
                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    string[] sqlList = SplitString(sql, "-sqlseparator-");
                    foreach (string item in sqlList)
                    {
                        if (!IsNullOrWhiteSpace(item))
                        {
                            try
                            {
                                SqliteHelper.ExecuteNonQuery(CommandType.Text, item);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                return ex.Message;
                            }
                        }
                    }
                }
                conn.Close();
            }
            return string.Empty;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">分隔字符串</param>
        /// <returns></returns>
        private static string[] SplitString(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
                return new string[0] { };

            if (sourceStr.IndexOf(splitStr) == -1)
                return new string[] { sourceStr };

            if (splitStr.Length == 1)
                return sourceStr.Split(splitStr[0]);
            else
                return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns></returns>
        private static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null)
            {
                return true;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
