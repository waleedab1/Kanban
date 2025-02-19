using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class BoardDalController : DalController
    {
        private const string BoardsTableName = "Board";
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BoardDalController() : base(BoardsTableName) { }

        /// Converts records from the Board table in the database to BoardDTO object
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetString(0), reader.GetInt32(1));
            return result;
        }

        /// Selecting all the boards in the Database file and converting all the boards to list of BoardDTO
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }

        /// Inserting a new Board to the table in the Database
        public override bool Insert(DTO board)
        {
            BoardDTO b = (BoardDTO)board;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO [{BoardsTableName}] ([{BoardDTO.BoardEmail}] ,[{BoardDTO.BoardTaskId}]) VALUES (@emailVal,@taskidVal)";

                    SQLiteParameter EmailParam = new SQLiteParameter(@"emailVal", b.Email);
                    SQLiteParameter TaskIDParam = new SQLiteParameter(@"taskidVal", b.TaskId);


                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(TaskIDParam);


                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Inserting Board Failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        //This Function is responsible for getting all guests in a certain board
        public List<string> SelectBoardGuests(string HostEmail)
        {
            List<string> Guests = new List<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT GuestEmail FROM [Guest] WHERE [HostEmail] = @HostVal";

                SQLiteParameter HostParam = new SQLiteParameter(@"HostVal", HostEmail);
                command.Parameters.Add(HostParam);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Prepare();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Guests.Add(dataReader.GetString(0));
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
            return Guests;
        }

        //This function is responsible for inserting a guest in a board
        public bool InsertGuest(string Email, string GuestEmail)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO [Guest] ([HostEmail] ,[GuestEmail]) VALUES (@HostVal,@GuestVal)";

                    SQLiteParameter HostParam = new SQLiteParameter(@"HostVal", Email);
                    SQLiteParameter GuestParam = new SQLiteParameter(@"GuestVal", GuestEmail);

                    command.Parameters.Add(HostParam);
                    command.Parameters.Add(GuestParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Inserting Guest Failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool DeleteGuests()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    command.CommandText = $"DELETE FROM [Guest]";
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
    }
}
