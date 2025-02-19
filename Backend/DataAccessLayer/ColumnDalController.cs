using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class ColumnDalController : DalController
    {
        private const string ColumnsTableName = "Column";
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ColumnDalController() : base(ColumnsTableName) { }

        //Converts records from the Column table in the database to ColumnDTO object
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
            return result;
        }
        /// Selecting all columns with a specifit user(email) from the Database and after that convert them to a list of ColumnDTOs in order to load the data
        public List<ColumnDTO> SelectUserColumns(string email)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM [{ColumnsTableName}] WHERE [{ColumnDTO.ColumnEmail}] = @emailVal ORDER BY [{ColumnDTO.ColumnOrdinal}]";

                SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", email);
                command.Parameters.Add(emailParam);
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

            return results.Cast<ColumnDTO>().ToList();
        }

        /// Inserting Column in the table in the Database
        public override bool Insert(DTO column)
        {

            ColumnDTO c = (ColumnDTO)column;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO [{ColumnsTableName}] ([{ColumnDTO.ColumnEmail}],[{ColumnDTO.ColumnName}],[{ColumnDTO.ColumnOrdinal}],[{ColumnDTO.ColumnLimit}]) " +
                        $"VALUES (@emailVal, @nameVal, @OrdinalVal, @limitVal)";

                    SQLiteParameter EmailParam = new SQLiteParameter(@"emailVal", c.Email);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", c.Name);
                    SQLiteParameter OrdinalParam = new SQLiteParameter(@"OrdinalVal", c.Ordinal);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", c.Limit);


                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(OrdinalParam);
                    command.Parameters.Add(limitParam);


                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Inserting Column Failed!");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// Deleting column from the table in the Database
        public bool Delete(DTO column)
        {
            ColumnDTO c = (ColumnDTO)column;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    command.CommandText = $"DELETE FROM [{ColumnsTableName}] WHERE [{ColumnDTO.ColumnEmail}] =@emailVal AND [{ColumnDTO.ColumnName}] =@nameVal";

                    SQLiteParameter columnEmailParam = new SQLiteParameter(@"emailVal", c.Email);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"nameVal", c.Name);

                    command.Parameters.Add(columnEmailParam);
                    command.Parameters.Add(columnNameParam);


                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Deleting Column Failed!");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
    }
}

