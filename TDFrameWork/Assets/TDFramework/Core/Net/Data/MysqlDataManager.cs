using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;


namespace TDFramework.Net.Mysql
{
    public class DatabaseManager 
    {
        protected static string m_databaseIP = "localhost";         //IP地址
        protected static string m_databasePort = "3306";            //端口号

        protected static string m_userID = "root";                  //MySQL数据库用户名
        protected static string m_password = "root1";              //MySQL登陆密码
        protected static string m_databaseName = "game";   //链接的数据库的库名


        protected static string m_connectionString;                 // 数据库连接字符串


        /// <summary>
        /// 测试是否链接上数据库
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            bool isConnected = true;
            //发送数据库连接字段 创建连接通道
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                try
                {
                    //打开连接通道
                    connection.Open();
                }
                catch (MySqlException E)
                {
                    //如果有异常 则连接失败
                    isConnected = false;
                    throw new Exception(E.Message);
                }
                finally
                {
                    //关闭连接通道
                    connection.Close();
                }
            }

            return isConnected;
        }

        /// <summary>
        /// 利用字符串组拼方式来编写数据库的连接
        /// </summary>
        public static void Init()
        {
            m_connectionString = string.Format("Server = {0}; port = {1}; Database = {2}; User ID = {3}; Password = {4}; Pooling=true; Charset = utf8;", m_databaseIP, m_databasePort, m_databaseName, m_userID, m_password);
        }


        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="cols">字段名</param>
        /// <param name="colType">字段类型</param>
        /// <returns></returns>
        public static int CreateTable(string name, string[] cols, string[] colType)
        {
            if (cols.Length != colType.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }

            string query = "CREATE TABLE " + name + " (" + cols[0] + " " + colType[0];

            for (int i = 1; i < cols.Length; ++i)
            {
                query += ", " + cols[i] + " " + colType[i];
            }

            query += ")";

            return ExecuteNonQuery(query);
        }


        /// <summary>
        /// 创建表默认ID自动增长
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="cols">字段名</param>
        /// <param name="colType">字段类型</param>
        /// <returns></returns>
        public static int CreateTableAutoID(string name, string[] cols, string[] colType)
        {
            if (cols.Length != colType.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }

            string query = "CREATE TABLE " + name + " (" + cols[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";

            for (int i = 1; i < cols.Length; ++i)
            {
                query += ", " + cols[i] + " " + colType[i];
            }

            query += ", PRIMARY KEY (" + cols[0] + ")" + ")";

            return ExecuteNonQuery(query);
        }

        /// <summary>
        /// 插入一条数据，包括所有，不适用自动累加ID。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";

            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }

            query += ")";

            return ExecuteNonQuery(query);
        }

        /// <summary>
        /// 插入部分ID
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int InsertInto(string tableName, string[] cols, string[] values)
        {
            if (cols.Length != values.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }

            string query = "INSERT INTO " + tableName + " (" + cols[0];
            for (int i = 1; i < cols.Length; ++i)
            {
                query += ", " + cols[i];
            }

            query += ") VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }

            query += ")";

            return ExecuteNonQuery(query);
        }

        /// <summary>
        /// 当指定字段满足一定条件时，更新指定字段的数据
        /// 例如更新在user这个表中字段名为userAccount的值等于10086时，将对应userPwd字段的值改成newMd5SumPassword
        /// ("users", new string[] { "userPwd" }, new string[] { newMd5SumPassword }, "userAccount", "10086")
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段</param>
        /// <param name="colsValues">字段值</param>
        /// <param name="selectKey">指定的字段</param>
        /// <param name="selectValue">指定字段满足的条件</param>
        /// <returns></returns>
        public static int UpdateInto(string tableName, string[] cols, string[] colsValues, string selectKey, string selectValue)
        {
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + "'" + colsValues[0] + "'";

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + "'" + colsValues[i] + "'";
            }

            query += " WHERE " + selectKey + " = " + selectValue;

            return ExecuteNonQuery(query);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段</param>
        /// <param name="colsValues">字段值</param>
        /// <returns></returns>
        public static int Delete(string tableName, string[] cols, string[] colsValues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsValues[0];

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += " or " + cols[i] + " = " + colsValues[i];
            }
            return ExecuteNonQuery(query);
        }

        /// <summary>
        /// 查询指定字段数据中满足条件的
        /// DataSet内存中的数据库，DataSet是不依赖于数据库的独立数据集合,是一种不包含表头的纯数据文件
        /// 有条件的查询，查询在users这个表当中，只需要字段名为userAccount，userPwd，userName，ID这几个字段对应的数据，
        /// 满足条件为 userAccount对应的value=account， userPwd对应的value=md5Password；
        /// ("users", new string[] { "userAccount", "userPwd", "userName", "ID" }, new string[] { "userAccount", "userPwd" }, new string[] { "=", "=" }, new string[] { account, md5Password });
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="items">字段名</param>
        /// <param name="cols">字段名</param>
        /// <param name="operations">条件运算符</param>
        /// <param name="values">满足的条件值</param>
        /// <returns></returns>
        public static DataSet SelectWhere(string tableName, string[] items, string[] cols, string[] operations, string[] values)
        {
            if (cols.Length != operations.Length || operations.Length != values.Length)
            {
                throw new Exception("col.Length != operation.Length != values.Length");
            }

            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {
                query += ", " + items[i];
            }

            query += " FROM " + tableName + " WHERE " + cols[0] + operations[0] + "'" + values[0] + "' ";

            for (int i = 1; i < cols.Length; ++i)
            {
                query += " AND " + cols[i] + operations[i] + "'" + values[i] + "' ";
            }

            return ExecuteQuery(query);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="items">字段名</param>
        /// <returns></returns>
        public static DataSet Select(string tableName, string[] items)
        {
            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {
                query += ", " + items[i];
            }

            query += " FROM " + tableName;

            return ExecuteQuery(query);
        }

        /// <summary>
        /// 查询所有字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static DataSet Select(string tableName)
        {
            string query = "SELECT * FROM " + tableName;
            return ExecuteQuery(query);
        }

        #region 执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数。用于Update、Insert和Delete
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryByTime(string SQLString, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        public static void ExecuteNonQueryTran(ArrayList SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(m_connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (MySqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string SQLString, string content)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                MySqlParameter myParameter = new MySqlParameter("@content", MySqlDbType.Text);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySqlException E)
                {
                    throw new Exception(E.Message);
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
        public static object ExecuteScalar(string SQLString, string content)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                MySqlParameter myParameter = new MySqlParameter("@content", MySqlDbType.Text);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((System.Object.Equals(obj, null)) || (System.Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (MySqlException E)
                {
                    throw new Exception(E.Message);
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
        public static int ExecuteNonQueryInsertImg(string strSQL, byte[] fs)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                MySqlParameter myParameter = new MySqlParameter("@fs", MySqlDbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySqlException E)
                {
                    throw new Exception(E.Message);
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
        public static object GetSingle(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((System.Object.Equals(obj, null)) || (System.Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader(使用该方法切记要手工关闭MySqlDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string strSQL)
        {
            MySqlConnection connection = new MySqlConnection(m_connectionString);
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                MySqlDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw new Exception(e.Message);
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            // cmd.Dispose();
            // connection.Close();
            //}
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteQuery(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(SQLString, connection);
                    da.Fill(ds);
                }
                catch (MySqlException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string SQLString, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// 获取SQL查询记录条数
        /// </summary>
        /// <param name="sqlstr">SQL语句</param>
        /// <returns></returns>
        public static int GetRowsNum(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    return ds.Tables[0].Rows.Count;
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion 执行简单SQL语句

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string SQLString, params System.Object[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的 Object[]）</param>
        public static void ExecuteNonQueryTran(Hashtable SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(m_connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            System.Object[] cmdParms = (System.Object[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params System.Object[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((System.Object.Equals(obj, null)) || (System.Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader (使用该方法切记要手工关闭MySqlDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string SQLString, params System.Object[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(m_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                MySqlDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw new Exception(e.Message);
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            // cmd.Dispose();
            // connection.Close();
            //}
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet ExcuteQuery(string SQLString, params System.Object[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, System.Object[] cmdParms)
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
                foreach (MySqlParameter parameter in cmdParms)
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

        #endregion 执行带参数的SQL语句

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程  (使用该方法切记要手工关闭MySqlDataReader和连接)
        /// 手动关闭不了，所以少用，MySql.Data组组件还没解决该问题
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader RunProcedure(string storedProcName, System.Object[] parameters)
        {
            MySqlConnection connection = new MySqlConnection(m_connectionString);
            MySqlDataReader returnReader;
            connection.Open();
            MySqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader();
            //Connection.Close(); 不能在此关闭，否则，返回的对象将无法使用
            return returnReader;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, System.Object[] parameters, string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                MySqlDataAdapter sqlDA = new MySqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, System.Object[] parameters, string tableName, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                MySqlDataAdapter sqlDA = new MySqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public static void RunProcedureNull(string storedProcName, System.Object[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                connection.Open();
                MySqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// 执行存储过程，返回第一行第一列的数据
        /// </summary>
        /// <param name="CommandText">T-SQL语句；例如："pr_shell 'dir *.exe'"或"select * from sysobjects where xtype=@xtype"</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>返回第一行第一列，用Convert.To{Type}把类型转换为想要的类型</returns>
        public object ExecuteScaler(string storedProcName, System.Object[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(m_connectionString))
            {
                object returnObjectValue;
                connection.Open();
                MySqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                returnObjectValue = command.ExecuteScalar();
                connection.Close();
                return returnObjectValue;
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName, System.Object[] parameters)
        {
            MySqlCommand command = new MySqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParameter parameter in parameters)
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
        /// 创建 MySqlCommand 对象实例(用来返回一个整数值)
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>MySqlCommand 对象实例</returns>
        private static MySqlCommand BuildIntCommand(MySqlConnection connection, string storedProcName, System.Object[] parameters)
        {
            MySqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new MySqlParameter("ReturnValue",
                MySqlDbType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        #endregion 存储过程操作
    }
}

