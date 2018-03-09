using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DXFDXT.Common
{
    public class DbHelperPostSQL
    {
        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperPostSQLP来实现.
        public static string connectionString = PubConstant.ConnectionString;
        public DbHelperPostSQL()
        {
        }

        #region 公用方法
        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString()); //也可能=0
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object obj = GetSingle(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Exists(string strSql, params NpgsqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, "", cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {

                using (NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string SQLString, int Times, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(ArrayList SQLStringList, ref string resultmsg, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                NpgsqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count > 0 ? count : 1;
                }
                catch (Exception ex)
                {
                    resultmsg = ex.Message;
                    tx.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(ArrayList SQLStringList, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                NpgsqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count > 0 ? count : 1;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection);
                NpgsqlParameter myParameter = new NpgsqlParameter("@content", NpgsqlTypes.NpgsqlDbType.Text);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string SQLString, string content, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection);
                NpgsqlParameter myParameter = new NpgsqlParameter("@content", NpgsqlTypes.NpgsqlDbType.Text);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(strSQL, connection);
                NpgsqlParameter myParameter = new NpgsqlParameter("@fs", NpgsqlTypes.NpgsqlDbType.Inet);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, int Times, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回NpgsqlDataReader ( 注意：调用该方法后，一定要对NpgsqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>NpgsqlDataReader</returns>
        public static NpgsqlDataReader ExecuteReader(string strSQL, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            NpgsqlConnection connection = new NpgsqlConnection(ConnString);
            NpgsqlCommand cmd = new NpgsqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    NpgsqlDataAdapter command = new NpgsqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        public static DataSet Query(string SQLString, int Times, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    NpgsqlDataAdapter command = new NpgsqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string ConnString = "", params NpgsqlParameter[] cmdParms)
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的NpgsqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            NpgsqlParameter[] cmdParms = (NpgsqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的NpgsqlParameter[]）</param>
        public static void ExecuteSqlTran(Dictionary<object, object> SQLStringList, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    try
                    {
                        //循环
                        foreach (object myDE in SQLStringList.Keys)
                        {
                            string cmdText = myDE.ToString();
                            NpgsqlParameter[] cmdParms = (NpgsqlParameter[])SQLStringList[myDE];
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的NpgsqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            NpgsqlParameter[] cmdParms = (NpgsqlParameter[])myDE.Value;
                            foreach (NpgsqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (NpgsqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, string ConnString = "", params NpgsqlParameter[] cmdParms)
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回NpgsqlDataReader ( 注意：调用该方法后，一定要对NpgsqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>NpgsqlDataReader</returns>
        public static NpgsqlDataReader ExecuteReader(string SQLString, string ConnString = "", params NpgsqlParameter[] cmdParms)
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            NpgsqlConnection connection = new NpgsqlConnection(ConnString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                NpgsqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
            //			finally
            //			{
            //				cmd.Dispose();
            //				connection.Close();
            //			}	

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string ConnString = "", params NpgsqlParameter[] cmdParms)
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(NpgsqlCommand cmd, NpgsqlConnection conn, NpgsqlTransaction trans, string cmdText, NpgsqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (NpgsqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回NpgsqlDataReader ( 注意：调用该方法后，一定要对NpgsqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>NpgsqlDataReader</returns>
        public static NpgsqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            NpgsqlConnection connection = new NpgsqlConnection(ConnString);
            NpgsqlDataReader returnReader;
            connection.Open();
            NpgsqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;

        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                NpgsqlDataAdapter sqlDA = new NpgsqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                NpgsqlDataAdapter sqlDA = new NpgsqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// 构建 NpgsqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>NpgsqlCommand</returns>
        private static NpgsqlCommand BuildQueryCommand(NpgsqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (NpgsqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected, string ConnString = "")
        {
            ConnString = ConnString == "" ? connectionString : ConnString;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnString))
            {
                int result;
                connection.Open();
                NpgsqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 NpgsqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>NpgsqlCommand 对象实例</returns>
        private static NpgsqlCommand BuildIntCommand(NpgsqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            NpgsqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            NpgsqlParameter npp = new NpgsqlParameter();
            command.Parameters.Add(new NpgsqlParameter("ReturnValue", NpgsqlTypes.NpgsqlDbType.Integer, 4, string.Empty, ParameterDirection.ReturnValue,
                false, 0, 0, DataRowVersion.Default, null));
            return command;
        }
        public static byte[] BytesReader(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            byte[] bytes = null;

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                NpgsqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() && myReader[0].ToString() != "")
                {
                    bytes = (byte[])myReader[0];
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                connection.Close();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return null;
            }

            return bytes;
        }
        #endregion
    }
}
