using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace DevelPlatform.OneCEUtils.V8PasswordEjector
{
    public class PasswordEjector
    {
        #region ServiceVars // Служебные переменные

        private static string V8USERS_ORIGINAL_TABLE_NAME = "V8USERS";
        private static string USERS_PARAMS_CONGIG_NAME = "users.usr";
        private static Byte V8USERS_NEW_ASCII_BEGIN_BYTE = 72;
        private static Byte V8USERS_NEW_UNICODE_BEGIN_BYTE = 72;
        private static Byte USERS_PARAMS_ORIGINAL_UNICODE_BEGIN_BYTE = 117;
        private static Byte USERS_PARAMS_NEW_BEGIN_BYTE = 1;

        #endregion

        #region ResetPasswordForFileInfobase // Сброс и восстановление учетных записей в файловой базе данных

        public static void ResetFileBaseUsers(string InfobasePath)
        {
            Encoding tableV8UsersEncoding;
            long resTable = -1;
            long resParam = -1;
            try
            {
                GetFileInfobaseTablesAdresses(out resTable, out resParam, InfobasePath, V8USERS_ORIGINAL_TABLE_NAME, out tableV8UsersEncoding);

                bool tableExist = !(resTable < 0);
                bool paramExist = !(resParam < 0);
                if (tableExist && paramExist)
                {
                    using (var stream = new FileStream(InfobasePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = resTable;
                        if(tableV8UsersEncoding == Encoding.ASCII)
                            stream.WriteByte(V8USERS_NEW_ASCII_BEGIN_BYTE);
                        else
                            stream.WriteByte(V8USERS_NEW_UNICODE_BEGIN_BYTE);

                        stream.Position = resParam;
                        stream.WriteByte(USERS_PARAMS_NEW_BEGIN_BYTE);
                    }
                }
                else
                {
                    throw new TableNotFoundException("Не найдена таблица пользователей и/или параметров аутентификации!");
                }
            }
            catch (TableNotFoundException ex)
            {
                throw new Exception("Некорректный формат базы данных", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }

        // Изменяем байты в файле базы данных
        // Параметры: 1. "searchTable" - массив байт для поиска таблицы "v8users"
        //            2. "searchParam" - массив байт для поиска записи параметров "users.usr"
        //            3. "PathDataBase" - путь к файлу информационной базы
        //            4. "newByteTable" - новое значение байта для таблицы "v8users"
        //            5. "newByteParam" - новое значение байта для записи параметров "users.usr"
        private static void ChangeDataBaseFile(byte[] searchTable, byte[] searchParam,
                                               string PathDataBase, byte newByteTable, byte newByteParam)
        {
            try
            {
                Byte[] searchIn = File.ReadAllBytes(PathDataBase);
                int resTable = PasswordReseterHelper.ByteSearch(searchIn, searchTable, 0);
                int resParam = PasswordReseterHelper.ByteSearch(searchIn, searchParam, 0);
                bool tableExist = !(resTable < 0);
                bool paramExist = !(resParam < 0);
                if (tableExist && paramExist)
                {
                    using (var stream = new FileStream(PathDataBase, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = resTable;
                        stream.WriteByte(newByteTable);
                        stream.Position = resParam;
                        stream.WriteByte(newByteParam);
                    }
                }
                else
                {
                    throw new Exception("Не найдена таблица пользователей и/или параметров аутентификации!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }


        private static void GetFileInfobaseTablesAdresses(out long resTable, out long resParam, string path, string V8USERS, out Encoding tableV8UsersEncoding)
        {
            int buflen = 8388608;
            tableV8UsersEncoding = null;

            resTable = -1;
            resParam = -1;
            bool sucV8 = false;
            bool sucUs = false;
            long n = 0;
            {
                using (FileStream fsSource = new FileStream(path,
                           FileMode.Open, FileAccess.Read))
                {
                    char firstUpperSymbolOfTableName = V8USERS.ToUpper()[0];
                    char firstLowerSymbolOfTableName = V8USERS.ToLower()[0];
                    string upperTableName = V8USERS.ToUpper();
                    string lowerTableName = V8USERS.ToLower();

                    byte[] bytes = new byte[buflen];
                    long numBytesToRead = (long)fsSource.Length;
                    long numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        if ((sucV8 == true) & (sucUs == true)) break;

                        n = (long)fsSource.Read(bytes, 0, buflen);

                        for (int i = 0; i < buflen; i++)
                        { 
                            if (bytes[i] == Convert.ToByte(firstUpperSymbolOfTableName)
                                || bytes[i] == Convert.ToByte(firstLowerSymbolOfTableName))
                            {
                                if (buflen - i > 13)
                                {
                                    if (Encoding.Unicode.GetString(bytes, i, 14).ToUpper().Contains(upperTableName))
                                    {
                                        resTable = numBytesRead + (long)i;
                                        sucV8 = true;
                                        tableV8UsersEncoding = Encoding.Unicode;
                                    } else if(Encoding.ASCII.GetString(bytes, i, 14).ToUpper().Contains(upperTableName))
                                    {
                                        resTable = numBytesRead + (long)i;
                                        sucV8 = true;
                                        tableV8UsersEncoding = Encoding.ASCII;
                                    }
                                }
                                else
                                {
                                    long SP = numBytesRead;
                                    byte[] temp = new byte[14];
                                    int j = 0;
                                    for (int k = i; k < buflen; k++)
                                    {
                                        temp[j] = bytes[k];
                                        j++;
                                    }
                                    int max = 14 - j;
                                    for (int k = 0; k < max; k++)
                                    {
                                        temp[j] = (byte)fsSource.ReadByte();
                                        j++;
                                        numBytesRead += 1;
                                        numBytesToRead -= 1;
                                    }
                                    if (Encoding.Unicode.GetString(temp, 0, 14) == V8USERS)
                                    {
                                        resTable = SP + (long)i;
                                        sucV8 = true;
                                    }
                                }

                            }

                            if (bytes[i] == USERS_PARAMS_ORIGINAL_UNICODE_BEGIN_BYTE)
                            {
                                if (buflen - i > 17)
                                {
                                    if (Encoding.Unicode.GetString(bytes, i, 18) == USERS_PARAMS_CONGIG_NAME)
                                    {
                                        resParam = numBytesRead + (long)i - 3;
                                        sucUs = true;
                                    }
                                }
                                else 
                                {
                                    long SP = numBytesRead;
                                    byte[] temp = new byte[18];
                                    int j = 0;
                                    for (int k = i; k < buflen; k++) 
                                    {
                                        temp[j] = bytes[k];
                                        j++;
                                    }
                                    int max = 18 - j;
                                    for (int k = 0; k < max; k++)
                                    {
                                        temp[j] = (byte)fsSource.ReadByte(); 
                                        j++;
                                        numBytesRead += 1;
                                        numBytesToRead -= 1;
                                    }
                                    if (Encoding.Unicode.GetString(temp, 0, 18) == "users.usr")
                                    {
                                        resParam = SP + (long)i - 3;
                                        sucUs = true;
                                    }
                                }
                            }
                        }

                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                }
            }
        }

        #endregion

        #region ResetPasswordForServerInfobase // Сброс учетных записей в серверной базе данных

        public static void RecoveryServerInfobaseUsers(string connectionString, SupportedDBMS dbms = SupportedDBMS.MSSQL)
        {
            if (dbms == SupportedDBMS.MSSQL)
            {
                RecoveryServerBaseUsersForMSSQLSERVER(connectionString);
            }
            else if (dbms == SupportedDBMS.PostgreSQL)
            {
                RecoveryServerBaseUsersForPOSTGRESQL(connectionString);
            }
            else
            {
                throw new Exception(string.Format("СУБД \"{0}\" не поддерживается!", dbms));
            }
        }

        public static void ResetServerInfobaseUsers(string connectionString, SupportedDBMS dbms = SupportedDBMS.MSSQL)
        {
            if (dbms == SupportedDBMS.MSSQL)
            {
                ResetServerBaseUsersForMSSQLSERVER(connectionString);
            }
            else if (dbms == SupportedDBMS.PostgreSQL)
            {
                ResetServerBaseUsersForPOSTGRESQL(connectionString);
            }
        }

        private static void ResetServerBaseUsersForMSSQLSERVER(string connStr)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "sp_rename 'v8users', 'h8users'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "UPDATE Params Set FileName = 'husers.usr' Where FileName = 'users.usr'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }
        private static void RecoveryServerBaseUsersForMSSQLSERVER(string connStr)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            bool TableExist = false;
                            using (var cmdDelSelect = conn.CreateCommand())
                            {
                                cmdDelSelect.CommandText = "select * from sysobjects Where name = 'v8users'";
                                cmdDelSelect.Transaction = transaction;
                                var reader = cmdDelSelect.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    TableExist = true;
                                }
                                reader.Close();
                            }
                            if (TableExist) 
                            {               
                                using (var cmdDel = conn.CreateCommand())
                                {
                                    cmdDel.CommandText = "DROP TABLE v8users";
                                    cmdDel.Transaction = transaction;
                                    cmdDel.ExecuteNonQuery();
                                }
                            }
                            using (var cmdDel = conn.CreateCommand())
                            {   
                                cmdDel.CommandText = "DELETE FROM Params WHERE filename='users.usr'";
                                cmdDel.Transaction = transaction;
                                cmdDel.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {   
                                cmd.CommandText = "sp_rename 'h8users', 'v8users'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {  
                                cmd.CommandText = "UPDATE Params Set FileName = 'users.usr' Where FileName = 'husers.usr'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }

        private static void ResetServerBaseUsersForPOSTGRESQL(string connStr)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var cmdGetTabIndex = conn.CreateCommand())
                            {
                                List<string> listIndex = new List<string>();

                                cmdGetTabIndex.CommandText = "SELECT i.relname as INDEX_NAME, " +
                                                              "idx.indrelid::regclass::text " +
                                                              "FROM   pg_index as idx " +
                                                              "JOIN   pg_class as i " +
                                                              "ON     i.oid = idx.indexrelid " +
                                                              "JOIN   pg_am as am " +
                                                              "ON     i.relam = am.oid " +
                                                              "WHERE idx.indrelid::regclass::text = 'v8users'";
                                cmdGetTabIndex.Transaction = transaction;
                                var reader = cmdGetTabIndex.ExecuteReader();
                                while (reader.Read())
                                {
                                    string nameIndex = reader.GetString(0);
                                    listIndex.Add(nameIndex);
                                }
                                reader.Close();
                                foreach (var nameIndex in listIndex)
                                {
                                    using (var renameIndex = conn.CreateCommand())
                                    {
                                        renameIndex.CommandText = "ALTER INDEX " + nameIndex + " RENAME TO H" + nameIndex + ";";
                                        renameIndex.Transaction = transaction;
                                        renameIndex.ExecuteNonQuery();
                                    }
                                }
                            }

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "ALTER TABLE v8users RENAME TO h8users;";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "UPDATE Params Set FileName = 'husers.usr' Where FileName = 'users.usr'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }
        private static void RecoveryServerBaseUsersForPOSTGRESQL(string connStr)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            bool TableExist = false;
                            using (var cmdDelSelect = conn.CreateCommand())
                            {
                                cmdDelSelect.CommandText = "Select * From pg_tables where tablename = 'v8users'";
                                cmdDelSelect.Transaction = transaction;
                                var reader = cmdDelSelect.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    TableExist = true;
                                }
                                reader.Close();
                            }
                            if (TableExist)
                            {
                                using (var cmdDel = conn.CreateCommand())
                                {
                                    cmdDel.CommandText = "DROP TABLE v8users";
                                    cmdDel.Transaction = transaction;
                                    cmdDel.ExecuteNonQuery();
                                }
                            }

                            using (var cmdDel = conn.CreateCommand())
                            {
                                cmdDel.CommandText = "DELETE FROM Params WHERE filename='users.usr'";
                                cmdDel.Transaction = transaction;
                                cmdDel.ExecuteNonQuery();
                            }

                            using (var cmdGetTabIndex = conn.CreateCommand())
                            {
                                List<string> listIndex = new List<string>();

                                cmdGetTabIndex.CommandText = "SELECT i.relname as INDEX_NAME, " +
                                                              "idx.indrelid::regclass::text " +
                                                              "FROM   pg_index as idx " +
                                                              "JOIN   pg_class as i " +
                                                              "ON     i.oid = idx.indexrelid " +
                                                              "JOIN   pg_am as am " +
                                                              "ON     i.relam = am.oid " +
                                                              "WHERE idx.indrelid::regclass::text = 'h8users'";
                                cmdGetTabIndex.Transaction = transaction;
                                var reader = cmdGetTabIndex.ExecuteReader();
                                while (reader.Read())
                                {
                                    string nameIndex = reader.GetString(0);
                                    listIndex.Add(nameIndex);
                                }
                                reader.Close();
                                foreach (var nameIndex in listIndex)
                                {
                                    using (var renameIndex = conn.CreateCommand())
                                    {
                                        renameIndex.CommandText = "ALTER INDEX " + nameIndex + " RENAME TO " + nameIndex.Substring(1, nameIndex.Length - 1) + ";";
                                        renameIndex.Transaction = transaction;
                                        renameIndex.ExecuteNonQuery();
                                    }
                                }
                            }

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "ALTER TABLE h8users RENAME TO v8users";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "UPDATE Params Set FileName = 'users.usr' Where FileName = 'husers.usr'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }

        public enum SupportedDBMS
        {
            MSSQL,
            PostgreSQL
        }

        #endregion

        #region Service

        public class TableNotFoundException : Exception
        {
            public TableNotFoundException(string Message)
                : base(Message)
            {

            }
        }

        private class PasswordReseterHelper
        {
            public static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
            {
                int found = -1;
                bool matched = false;
                if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
                {
                    for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                    {
                        if (searchIn[i] == searchBytes[0])
                        {
                            if (searchIn.Length > 1)
                            {
                                matched = true;
                                for (int y = 1; y <= searchBytes.Length - 1; y++)
                                {
                                    if (searchIn[i + y] != searchBytes[y])
                                    {
                                        matched = false;
                                        break;
                                    }
                                }
                                if (matched)
                                {
                                    found = i;
                                    break;
                                }
                            }
                            else
                            {
                                found = i;
                                break;
                            }

                        }
                    }
                }
                return found;
            }
        }

        #endregion
    }
}
