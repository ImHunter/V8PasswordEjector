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
        private static string V8USERS_NEW_TABLE_NAME = "H8USERS";
        private static string USERS_PARAMS_CONGIG_NAME = "users.usr";
        private static Byte V8USERS_ORIGINAL_ASCII_BEGIN_BYTE = 118;
        private static Byte V8USERS_ORIGINAL_UNICODE_BEGIN_BYTE = 86;
        private static Byte V8USERS_NEW_ASCII_BEGIN_BYTE = 72;
        private static Byte V8USERS_NEW_UNICODE_BEGIN_BYTE = 72;

        private static Byte USERS_PARAMS_ORIGINAL_UNICODE_BEGIN_BYTE = 117;
        private static Byte USERS_PARAMS_ORIGINAL_BEGIN_BYTE = 0;
        private static Byte USERS_PARAMS_NEW_BEGIN_BYTE = 1;

        #endregion

        #region ResetPasswordForFileInfobase // Сброс и восстановление учетных записей в файловой базе данных

        // Сброс учетных записей для файловой базы
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
                    // Используя класс "FileStream" открываем файл для записи
                    using (var stream = new FileStream(InfobasePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        // Записываем новые значения байт в найденные позиции в файле
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
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка", ex);
            }
        }

        // Восстановление учетных записей для файловой базы
        public static void RecoveryFileBaseUsers(string InfobasePath)
        {
            Encoding tableV8UsersEncoding;
            long resTable = -1;
            long resParam = -1;
            try
            {
                GetFileInfobaseTablesAdresses(out resTable, out resParam, InfobasePath, V8USERS_NEW_TABLE_NAME, out tableV8UsersEncoding);
                // Значения байт, на которые будут изменены значения в файле
                // Вызов метода модификации файла
                bool tableExist = !(resTable < 0);
                bool paramExist = !(resParam < 0);
                if (tableExist && paramExist)
                {
                    // Используя класс "FileStream" открываем файл для записи
                    using (var stream = new FileStream(InfobasePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        // Записываем новые значения байт в найденные позиции в файле
                        stream.Position = resTable;

                        if (tableV8UsersEncoding == Encoding.ASCII)
                            stream.WriteByte(V8USERS_ORIGINAL_ASCII_BEGIN_BYTE);
                        else
                            stream.WriteByte(V8USERS_ORIGINAL_UNICODE_BEGIN_BYTE);                        

                        stream.Position = resParam;
                        stream.WriteByte(USERS_PARAMS_ORIGINAL_BEGIN_BYTE);
                    }
                }
                else
                {
                    throw new TableNotFoundException("Не найдена таблица пользователей и/или параметров аутентификации!");
                }
            }
            catch (TableNotFoundException ex)
            {
                throw ex;
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
                // Получаем массив байт файла информационной базы "1Cv8.1CD"
                Byte[] searchIn = File.ReadAllBytes(PathDataBase);
                // Ищем заданные последовательности байт
                int resTable = PasswordReseterHelper.ByteSearch(searchIn, searchTable, 0);
                int resParam = PasswordReseterHelper.ByteSearch(searchIn, searchParam, 0);
                // Определяем флаги о результатах поиска
                bool tableExist = !(resTable < 0);
                bool paramExist = !(resParam < 0);
                if (tableExist && paramExist)
                {
                    // Используя класс "FileStream" открываем файл для записи
                    using (var stream = new FileStream(PathDataBase, FileMode.Open, FileAccess.ReadWrite))
                    {
                        // Записываем новые значения байт в найденные позиции в файле
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
            //  try
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
                        //начинаем искать
                        for (int i = 0; i < buflen; i++)
                        {
                            //Ищем V8Users 
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
                                else //если это конец буфера, проверяем вручную
                                {
                                    long SP = numBytesRead;
                                    byte[] temp = new byte[14];
                                    int j = 0;
                                    for (int k = i; k < buflen; k++)    //Копируем из того, что удалось считать
                                    {
                                        temp[j] = bytes[k];
                                        j++;
                                    }
                                    int max = 14 - j;
                                    for (int k = 0; k < max; k++)
                                    {
                                        temp[j] = (byte)fsSource.ReadByte();    //добавляем остальное
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
                            //Ищем users.usr
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
                                else //если это конец буфера, проверяем вручную
                                {
                                    long SP = numBytesRead;
                                    byte[] temp = new byte[18];
                                    int j = 0;
                                    for (int k = i; k < buflen; k++)    //Копируем из того, что удалось считать
                                    {
                                        temp[j] = bytes[k];
                                        j++;
                                    }
                                    int max = 18 - j;
                                    for (int k = 0; k < max; k++)
                                    {
                                        temp[j] = (byte)fsSource.ReadByte();    //добавляем остальное
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
                        // Break when the end of the file is reached.
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

        // Сбрасываем учетный записи информационной базы для серверной базы
        public static void RecoveryServerInfobaseUsers(string connectionString, SupportedDBMS dbms = SupportedDBMS.MSSQL)
        {
            // MSSQL
            if (dbms == SupportedDBMS.MSSQL)
            {
                RecoveryServerBaseUsersForMSSQLSERVER(connectionString);
            }
            // PostgreSQL
            else if (dbms == SupportedDBMS.PostgreSQL)
            {
                RecoveryServerBaseUsersForPOSTGRESQL(connectionString);
            }
            // IBM DB2
            else if (dbms == SupportedDBMS.IBMDB2)
            {
                RecoveryServerBaseUsersForIBMDB2(connectionString);
            }
            // Oracle Database
            else if (dbms == SupportedDBMS.OracleDatabase)
            {
                throw new Exception("Поддержка \"OracleDatabase\" временно отключена!");
                //RecoveryServerBaseUsersForOracleDatabase(connectionString);
            }
            else
            {
                throw new Exception(string.Format("СУБД \"{0}\" не поддерживается!", dbms));
            }
        }

        // Сбрасываем учетный записи информационной базы для серверной базы
        public static void ResetServerInfobaseUsers(string connectionString, SupportedDBMS dbms = SupportedDBMS.MSSQL)
        {
            // MSSQL
            if (dbms == SupportedDBMS.MSSQL)
            {
                ResetServerBaseUsersForMSSQLSERVER(connectionString);
            }
            // PostgreSQL
            else if (dbms == SupportedDBMS.PostgreSQL)
            {
                ResetServerBaseUsersForPOSTGRESQL(connectionString);
            }
            // IBM DB2
            else if (dbms == SupportedDBMS.IBMDB2)
            {
                ResetServerBaseUsersForIBMDB2(connectionString);
            }
            // Oracle Database
            else if (dbms == SupportedDBMS.OracleDatabase)
            {
                throw new Exception("Поддержка \"OracleDatabase\" временно отключена!");
                //ResetServerBaseUsersForOracleDatabase(connectionString);
            }
        }

        // Сброс и восстановление учетных записей для MSSQLSERVER
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
                                // Переименовываем таблицу, чтобы платформа не нашла список пользователей
                                cmd.CommandText = "sp_rename 'v8users', 'h8users'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {
                                // Переименовываем значение "FileName" для записи с параметрами учетных записей
                                // таблице "Params"
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
                            bool TableExist = false; // Определяем наличие таблицы "v8users" в базе данных
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
                            if (TableExist) // Если таблица "v8users" существует - удаляем ее для
                            {               // восстановления старой таблицы
                                using (var cmdDel = conn.CreateCommand())
                                {
                                    cmdDel.CommandText = "DROP TABLE v8users";
                                    cmdDel.Transaction = transaction;
                                    cmdDel.ExecuteNonQuery();
                                }
                            }
                            using (var cmdDel = conn.CreateCommand())
                            {   // Удаляем параметры пользователей из таблицы "Params"
                                cmdDel.CommandText = "DELETE FROM Params WHERE filename='users.usr'";
                                cmdDel.Transaction = transaction;
                                cmdDel.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {   // Переименовываем сохраненную ранее таблицу "h8users" в "v8users"
                                cmd.CommandText = "sp_rename 'h8users', 'v8users'";
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = conn.CreateCommand())
                            {   // Возвращаем исходное значение "FileName" для записи параметров пользователей
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

        // Сброс и восстановление учетных записей для POSTGRESQL
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
                            // Переименовываем индексы таблицы
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

                            // Восстанавливаем учетные записи

                            // Переименовываем индексы таблицы
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

        // Сброс и восстановление учетных записей для IBMDB2
        private static void ResetServerBaseUsersForIBMDB2(string connStr)
        {
            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Получаем имя SQL-таблицы с учетными записями пользователей
                            string TabUserName = "";              // Исходное имя таблицы
                            string newTabUserName = "";           // Новое имя таблицы
                            using (var cmdGetTabName = conn.CreateCommand())
                            {
                                cmdGetTabName.CommandText = "SELECT SYNONYM_NAME, TABLE_NAME FROM SYSPUBLIC.\"ALL_SYNONYMS\"" +
                                                            " WHERE SYNONYM_NAME = 'V8USERS';";
                                cmdGetTabName.Transaction = transaction;
                                var reader = cmdGetTabName.ExecuteReader();
                                if (reader.Read())
                                {
                                    TabUserName = reader.GetString(1);
                                    newTabUserName = "H" + TabUserName;
                                }
                                reader.Close();
                            }
                            if (!(TabUserName == ""))
                            {
                                // Переименовываем индексы таблицы
                                using (var cmdGetTabIndex = conn.CreateCommand())
                                {
                                    cmdGetTabIndex.CommandText = "SELECT INDEX_NAME FROM SYSPUBLIC.\"USER_INDEXES\" Where TABLE_NAME = '" + TabUserName + "'" +
                                                                "AND INDEX_NAME NOT LIKE 'SQL%';";
                                    cmdGetTabIndex.Transaction = transaction;
                                    var reader = cmdGetTabIndex.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        string nameIndex = reader.GetString(0);
                                        using (var renameIndex = conn.CreateCommand())
                                        {
                                            renameIndex.CommandText = "RENAME INDEX " + nameIndex + " TO H" + nameIndex + ";";
                                            renameIndex.Transaction = transaction;
                                            renameIndex.ExecuteNonQuery();
                                        }
                                    }
                                    reader.Close();
                                }
                                // Переименовываем таблицу с учетными записями
                                using (var renameUserTable = conn.CreateCommand())
                                {
                                    renameUserTable.CommandText = "RENAME " + TabUserName + " TO " + newTabUserName + ";";
                                    renameUserTable.Transaction = transaction;
                                    renameUserTable.ExecuteNonQuery();
                                }
                                // Удаляем старый алиас "V8USERS" и создаем новый "HV8USERS" для переименованной ранее таблицы
                                using (var delUserAlias = conn.CreateCommand())
                                {
                                    delUserAlias.CommandText = "DROP ALIAS V8USERS; CREATE ALIAS HV8USERS FOR " + newTabUserName;
                                    delUserAlias.Transaction = transaction;
                                    delUserAlias.ExecuteNonQuery();
                                }
                                // Изменяем запись параметров пользователей в таблице "Params", чтобы она не учитывалась при запуске
                                using (var renameUserParam = conn.CreateCommand())
                                {
                                    renameUserParam.CommandText = "UPDATE PARAMS SET FILENAME = 'husers.usr' WHERE FILENAME = 'users.usr'";
                                    renameUserParam.Transaction = transaction;
                                    renameUserParam.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("Ошибка: не найдена таблица \"V8USERS\"! Проверьте права доступа и корректность структуры базы данных!");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Непредвиденная ошибка", ex);
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
        private static void RecoveryServerBaseUsersForIBMDB2(string connStr)
        {
            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Получаем имя SQL-таблицы с учетными записями пользователей, сохраненной ранее
                            string recTabUserName = "";
                            using (var cmdGetTabName = conn.CreateCommand())
                            {
                                cmdGetTabName.CommandText = "SELECT SYNONYM_NAME, TABLE_NAME FROM SYSPUBLIC.\"ALL_SYNONYMS\"" +
                                                            " WHERE SYNONYM_NAME = 'HV8USERS';";
                                cmdGetTabName.Transaction = transaction;
                                var reader = cmdGetTabName.ExecuteReader();
                                if (reader.Read())
                                {
                                    recTabUserName = reader.GetString(1);
                                }
                                reader.Close();
                            }
                            // Удаляем данные о созданных учетных записях, если имеется таблица с инф. для восстановления
                            if (!(recTabUserName == ""))
                            {
                                // Получаем данные по таблицам и индексам, которые необходимо удалить перед восстановлением пользователей
                                string TabUserName = "";
                                using (var cmdGetTabName = conn.CreateCommand())
                                {
                                    cmdGetTabName.CommandText = "SELECT SYNONYM_NAME, TABLE_NAME FROM SYSPUBLIC.\"ALL_SYNONYMS\"" +
                                                                " WHERE SYNONYM_NAME = 'V8USERS';";
                                    cmdGetTabName.Transaction = transaction;
                                    var reader = cmdGetTabName.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        TabUserName = reader.GetString(1);
                                    }
                                    reader.Close();
                                }
                                if (!(TabUserName == ""))
                                {
                                    // Удаляем индексы найденной таблицы
                                    using (var cmdGetTabIndex = conn.CreateCommand())
                                    {
                                        cmdGetTabIndex.CommandText = "SELECT INDEX_NAME FROM SYSPUBLIC.\"USER_INDEXES\" Where TABLE_NAME = '" + TabUserName + "'" +
                                                                    "AND INDEX_NAME NOT LIKE 'SQL%';";
                                        cmdGetTabIndex.Transaction = transaction;
                                        var reader = cmdGetTabIndex.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            string nameIndex = reader.GetString(0);
                                            using (var renameIndex = conn.CreateCommand())
                                            {
                                                renameIndex.CommandText = "DROP INDEX " + nameIndex + ";";
                                                renameIndex.Transaction = transaction;
                                                renameIndex.ExecuteNonQuery();
                                            }
                                        }
                                        reader.Close();
                                    }
                                    // Удаляем таблицу и ее алиас, а также удаляем запись с параметрами уч. записей
                                    using (var delUserTableParams = conn.CreateCommand())
                                    {
                                        delUserTableParams.CommandText = "DROP TABLE " + TabUserName + ";" +
                                                                         "DROP ALIAS V8USERS;" +
                                                                         "DELETE FROM Params WHERE filename='users.usr';";
                                        delUserTableParams.Transaction = transaction;
                                        delUserTableParams.ExecuteNonQuery();
                                    }
                                }

                                // Восстанавливаем учетные записи

                                // Переименовываем индексы таблицы
                                using (var cmdGetTabIndex = conn.CreateCommand())
                                {
                                    cmdGetTabIndex.CommandText = "SELECT INDEX_NAME FROM SYSPUBLIC.\"USER_INDEXES\" Where TABLE_NAME = '" + recTabUserName + "'" +
                                                                "AND INDEX_NAME NOT LIKE 'SQL%';";
                                    cmdGetTabIndex.Transaction = transaction;
                                    var reader = cmdGetTabIndex.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        string nameIndex = reader.GetString(0);
                                        using (var renameIndex = conn.CreateCommand())
                                        {
                                            renameIndex.CommandText = "RENAME INDEX " + nameIndex + " TO " + nameIndex.Substring(1, nameIndex.Length - 1) + ";";
                                            renameIndex.Transaction = transaction;
                                            renameIndex.ExecuteNonQuery();
                                        }
                                    }
                                    reader.Close();
                                }
                                // Переименовываем таблицы с учетными записями
                                using (var renameUserTable = conn.CreateCommand())
                                {
                                    renameUserTable.CommandText = "RENAME " + recTabUserName + " TO " + recTabUserName.Substring(1, recTabUserName.Length - 1) + ";";
                                    renameUserTable.Transaction = transaction;
                                    renameUserTable.ExecuteNonQuery();
                                }
                                // Удаляем старый алиас "HV8USERS" и создаем новый "V8USERS" для восстановления уч. записей
                                using (var delUserAlias = conn.CreateCommand())
                                {
                                    delUserAlias.CommandText = "DROP ALIAS HV8USERS; CREATE ALIAS V8USERS FOR " + recTabUserName.Substring(1, recTabUserName.Length - 1);
                                    delUserAlias.Transaction = transaction;
                                    delUserAlias.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("Ошибка: не найдена сохраненная ранее таблица \"HV8USERS\"! Проверьте права доступа и корректность структуры базы данных!");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Непредвиденная ошибка", ex);
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

        // Сброс и восстановление учетных записей для ORACLEDATABASE
        private static void ResetServerBaseUsersForOracleDatabase(string connStr)
        {
            //try
            //{
            //    using (var conn = new OracleConnection(connStr))
            //    {
            //        conn.Open();
            //        using (var transaction = conn.BeginTransaction())
            //        {
            //            try
            //            {
            //                string nameBase = textBoxDataBaseName.Text;
            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.CommandText = "ALTER TABLE " + nameBase + ".v8users RENAME TO h8users";
            //                    cmd.Transaction = transaction;
            //                    cmd.ExecuteNonQuery();
            //                }

            //                using (var cmd = conn.CreateCommand())
            //                {
            //                    cmd.CommandText = "UPDATE " + nameBase + ".Params Set FileName = 'husers.usr' Where FileName LIKE 'users.usr%'";
            //                    cmd.Transaction = transaction;
            //                    cmd.ExecuteNonQuery();
            //                }
            //                transaction.Commit();
            //                MessageBox.Show("Операция успешно выполнена!");
            //            }
            //            catch (Exception e)
            //            {
            //                transaction.Rollback();
            //                MessageBox.Show("Ошибка: " + e.Message);
            //            }
            //        }
            //        conn.Close();
            //        conn.Dispose();
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Ошибка: " + e.Message);
            //}
        }
        private static void RecoveryServerBaseUsersForOracleDatabase(string connStr)
        {
            //try
            //{
            //    using (var conn = new OracleConnection(connStr))
            //    {
            //        conn.Open();
            //        using (var transaction = conn.BeginTransaction())
            //        {
            //            try
            //            {
            //                string nameBase = textBoxDataBaseName.Text;
            //                // Получаем имя SQL-таблицы с учетными записями пользователей, сохраненной ранее
            //                bool recTabUserExist = false;
            //                using (var cmdGetTabName = conn.CreateCommand())
            //                {
            //                    cmdGetTabName.CommandText = "SELECT table_name FROM dba_tables dt WHERE table_name = 'H8USERS' AND owner = '" + nameBase.ToUpper() + "'";
            //                    cmdGetTabName.Transaction = transaction;
            //                    var reader = cmdGetTabName.ExecuteReader();
            //                    if (reader.Read())
            //                    {
            //                        recTabUserExist = true;
            //                    }
            //                    reader.Close();
            //                }
            //                // Удаляем данные о созданных учетных записях, если имеется таблица с инф. для восстановления
            //                if (recTabUserExist)
            //                {
            //                    bool TableExist = false;
            //                    using (var cmdDelSelect = conn.CreateCommand())
            //                    {
            //                        cmdDelSelect.CommandText = "SELECT table_name FROM dba_tables dt WHERE table_name = 'V8USERS' AND owner = '" + nameBase.ToUpper() + "'";
            //                        cmdDelSelect.Transaction = transaction;
            //                        var reader = cmdDelSelect.ExecuteReader();
            //                        if (reader.HasRows)
            //                        {
            //                            TableExist = true;
            //                        }
            //                        reader.Close();
            //                    }
            //                    if (TableExist)
            //                    {
            //                        using (var cmdDel = conn.CreateCommand())
            //                        {
            //                            cmdDel.CommandText = "DROP TABLE " + nameBase + ".v8users CASCADE CONSTRAINTS";
            //                            cmdDel.Transaction = transaction;
            //                            cmdDel.ExecuteNonQuery();
            //                        }
            //                    }

            //                    using (var cmdDel = conn.CreateCommand())
            //                    {
            //                        cmdDel.CommandText = "DELETE FROM " + nameBase + ".Params WHERE filename='users.usr'";
            //                        cmdDel.Transaction = transaction;
            //                        cmdDel.ExecuteNonQuery();
            //                    }

            //                    using (var cmd = conn.CreateCommand())
            //                    {
            //                        cmd.CommandText = "ALTER TABLE " + nameBase + ".h8users RENAME TO v8users";
            //                        cmd.Transaction = transaction;
            //                        cmd.ExecuteNonQuery();
            //                    }

            //                    using (var cmd = conn.CreateCommand())
            //                    {
            //                        cmd.CommandText = "UPDATE " + nameBase + ".Params Set FileName = 'users.usr' Where FileName LIKE 'husers.usr%'";
            //                        cmd.Transaction = transaction;
            //                        cmd.ExecuteNonQuery();
            //                    }
            //                    transaction.Commit();
            //                    MessageBox.Show("Операция успешно выполнена!");
            //                }
            //                else
            //                {
            //                    transaction.Rollback();
            //                    MessageBox.Show("Ошибка: не найдена сохраненная ранее таблица \"HV8USERS\"! Проверьте права доступа и корректность структуры базы данных!");
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                transaction.Rollback();
            //                MessageBox.Show("Ошибка: " + e.Message);
            //            }
            //        }
            //        conn.Close();
            //        conn.Dispose();
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Ошибка: " + e.Message);
            //}
        }

        public enum SupportedDBMS
        {
            MSSQL,
            PostgreSQL,
            IBMDB2,
            OracleDatabase
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
            // Поиск массива байт в другом массиве байт
            public static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
            {
                int found = -1;
                bool matched = false;
                //only look at this if we have a populated search array and search bytes with a sensible start
                if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
                {
                    //iterate through the array to be searched
                    for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                    {
                        //if the start bytes match we will start comparing all other bytes
                        if (searchIn[i] == searchBytes[0])
                        {
                            if (searchIn.Length > 1)
                            {
                                //multiple bytes to be searched we have to compare byte by byte
                                matched = true;
                                for (int y = 1; y <= searchBytes.Length - 1; y++)
                                {
                                    if (searchIn[i + y] != searchBytes[y])
                                    {
                                        matched = false;
                                        break;
                                    }
                                }
                                //everything matched up
                                if (matched)
                                {
                                    found = i;
                                    break;
                                }

                            }
                            else
                            {
                                //search byte is only one bit nothing else to do
                                found = i;
                                break; //stop the loop
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
