using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class UserDalController : DalController
    {
        private const string UsersTableName = "User";
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserDalController() : base(UsersTableName)
        {

        }
        /// Selecting all the Users in the Database file and converting all the Users to list of UserDTO
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }

        //Converts records from the User table in the database to UserDTO object
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            return result;
        }
        /// Inserting a new User to the table in the Database
        public override bool Insert(DTO user)
        {

            UserDTO u = (UserDTO)user;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    command.CommandText = $"INSERT INTO [{UsersTableName}] ([{UserDTO.EmailColumnName}],[{UserDTO.NickNameColumnName}],[{UserDTO.PassColumnName}])" +
                         $"VALUES(@Email,@Nickname,@Password)";

                    SQLiteParameter EmailParam = new SQLiteParameter(@"Email", u.Email);
                    SQLiteParameter NicknameParam = new SQLiteParameter(@"Nickname", u.Nickname);
                    SQLiteParameter PasswordParam = new SQLiteParameter(@"Password", u.Password);

                    command.Parameters.Add(EmailParam);
                    command.Parameters.Add(NicknameParam);
                    command.Parameters.Add(PasswordParam);
                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Inserting User Failed");
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