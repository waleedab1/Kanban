using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DalController
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //starting up the connection.
        //and the file name is the db name for the Database in order to savrC:\Users\97254\Desktop\Kanban-M4-Updated (22)damnmnmn\Kanban-M4-Updated (22)herere\Kanban M4 - Gui -0-\Kanban-M3-1 (1)\Kanban\Backend\DataAccessLayer\DalController.cs
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
            //if the file does not exist, we create a new one
            if (!File.Exists(path))
            {
                string creation_query = @"CREATE TABLE 'User' (
                'Email' TEXT NOT NULL UNIQUE,
                'Nickname'  TEXT NOT NULL,
	            'Password'  TEXT,
	            PRIMARY KEY('Email')
                );
                CREATE TABLE 'Board'(
                    'Email' TEXT NOT NULL,
                    'TaskId'   INTEGER,
                    PRIMARY KEY('Email')
                );
                CREATE TABLE 'Column'(
                    'Email' TEXT NOT NULL,
                    'Name'  TEXT NOT NULL,
                    'Ordinal'    INTEGER NOT NULL,
                    'Limit'   INTEGER DEFAULT - 1,
                    PRIMARY KEY('Email','Name')
                );
                CREATE TABLE 'Task'(
                    'Email' TEXT,
                    'Assignee' TEXT NOT NULL,
                    'ColumnName'    TEXT,
                    'ID'    INTEGER,
                    'CreationTime'  TEXT,
                    'Title' TEXT NOT NULL,
                    'Description'   TEXT,
                    'DueDate'   TEXT,
                    PRIMARY KEY('Email', 'ColumnName', 'ID')
                ); 
                CREATE TABLE 'Guest'(
                    'HostEmail' TEXT NOT NULL,
                    'GuestEmail' TEXT NOT NULL,
                    PRIMARY KEY('HostEmail', 'GuestEmail')
                );";
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    command.CommandText = creation_query;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
            }
        }
       
        //generic select for all the data types from the Database
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM [{_tableName}]";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Prepare();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        //an update function to be used by all controllers
        public bool Update(string Id, string AttributeName, object AttributeValue)
        {
            var value = AttributeValue;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    command.CommandText = $"UPDATE  [{_tableName}] SET [{AttributeName}]= @AttributeVal WHERE" + Id;

                    SQLiteParameter ValueParam = new SQLiteParameter(@"AttributeVal", value);

                    command.Parameters.Add(ValueParam);

                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Updating: " + _tableName + " Failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        //Deletes all data in a certain table
        public bool DeleteData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    command.CommandText = $"DELETE FROM [{_tableName}]";
                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        //methods to be implemented by the controllers
        public abstract bool Insert(DTO Obj);
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

    }
}