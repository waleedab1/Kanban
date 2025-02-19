using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class TaskDalController : DalController
    {
        private const string TaskTableName = "Task";
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TaskDalController() : base(TaskTableName) { }

        //Converts records from the Task table in the database to TaskDTO object
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3),
                  reader.GetString(4), reader.GetString(5), reader.IsDBNull(6) ? null : reader.GetString(6), reader.GetString(7));
            return result;

        }
        /// Selecting all the tasks with a specified user(email) and columnname(column) in the Database file and converting all the tasks to list of TaskDTO
        public List<TaskDTO> SelectUserTasks(string email, string ColumnName)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM [{TaskTableName}] WHERE ([{TaskDTO.BoardEmail}] = @emailVal AND [{TaskDTO.TaskColumnName}] = @ColumnNameVal)";

                SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", email);
                SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnNameVal", ColumnName);

                command.Parameters.Add(emailParam);
                command.Parameters.Add(ColumnNameParam);

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
            return results.Cast<TaskDTO>().ToList();
        }
        public bool Delete(DTO task)
        {
            TaskDTO c = (TaskDTO)task;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                { /// Delete from TaskTable where taskId= 
                    command.CommandText = $"DELETE FROM [{TaskTableName}] WHERE [{TaskDTO.TaskId}] =@idVal AND [{TaskDTO.AssigneeEmail}]=@emailVal" ;
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", c._id);
                    SQLiteParameter idParam1 = new SQLiteParameter(@"emailVal", c.Assignee);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(idParam1);

                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Deleting Task Failed!");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// Inserting a new Task to the table in the Database
        public override bool Insert(DTO task)
        {

            TaskDTO t = (TaskDTO)task;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO [{TaskTableName}] ([{TaskDTO.BoardEmail}],[{TaskDTO.AssigneeEmail}],[{TaskDTO.TaskColumnName}],[{TaskDTO.TaskId}],[{TaskDTO.TaskCreationTime}],[{TaskDTO.TaskTitle}],[{TaskDTO.TaskDescription}],[{TaskDTO.TaskDueDate}]) " +
                        $"VALUES(@emailVal,@assigneeVal,@ColumnNameVal,@IdVal,@creationTimeVal,@titleVal,@descVal,@dueDateVal)";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", t.Email);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", t.Assignee);
                    SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnNameVal", t.ColumnName);
                    SQLiteParameter IdParam = new SQLiteParameter(@"IdVal", t.Id);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", t.CreationTime);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", t.Title);
                    SQLiteParameter descParam = new SQLiteParameter(@"descVal", t.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", t.DueDate);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(ColumnNameParam);
                    command.Parameters.Add(IdParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descParam);
                    command.Parameters.Add(dueDateParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    logger.Error("Inserting Task Failed");
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
