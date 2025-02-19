using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class BoardService
    {
        private BoardController BC;

        internal BoardService() 
        {
            BC = new BoardController();
        }

        public Response addBoard(string email)
        {
            try
            {
                BC.addBoard(email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        
        public Response LoadData()
        {
            try
            {
                BC.LoadAllBoards();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response DeleteData()
        {
            try
            {
                BC.DeleteBoardData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Board> GetBoard(string email)
        {
            try
            {
                Board b = new Board(BC.getBoard(email));
                return new Response<Board>(b);
            }
            catch (Exception e)
            {
                return new Response<Board>(e.Message);
            }
        }

        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            try
            {
                BC.LimitColumnTasks(email, columnOrdinal, limit);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            try
            {

                Task tsk = new Task(BC.AddTask(email, title, description, dueDate));
                return new Response<Task>(tsk);
            }
            catch (Exception e)
            {
                return new Response<Task>(e.Message);
            }
        }

        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                BC.AdvanceTask(email, columnOrdinal, taskId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                BC.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            try
            {
                BC.UpdateTaskTitle(email, columnOrdinal, taskId, title);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            try
            {
                BC.UpdateTaskDescription(email, columnOrdinal, taskId, description);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Column> GetColumn(string email, string columnName)
        {
            try
            {
                Column c = new Column(BC.GetColumn(email, columnName));
                return new Response<Column>(c);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            try
            {
                Column c = new Column(BC.GetColumn(email, columnOrdinal));
                return new Response<Column>(c);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }
        public Response RemoveColumn(string email, int columnOrdinal)
        {
            try
            {
                BC.RemoveColumn(email, columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            try
            {
                Column c = new Column(BC.AddColumn(email, columnOrdinal, Name));
                return new Response<Column>(c);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            try
            {
                BC.MoveColumnRight(email, columnOrdinal);
                Column c = new Column(BC.GetColumn(email, columnOrdinal + 1));
                return new Response<Column>(c);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            try
            {
                BC.MoveColumnLeft(email, columnOrdinal);
                Column c = new Column(BC.GetColumn(email, columnOrdinal - 1));
                return new Response<Column>(c);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response JoinExistingBoard(string email, string emailHost)
        {
            try
            {
                BC.joinExistingBoard(email, emailHost);
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                BC.AssignTask(email, columnOrdinal, taskId, emailAssignee);
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                BC.DeleteTask(email, columnOrdinal, taskId);
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            try
            {
                BC.ChangeColumnName(email, columnOrdinal, newName);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

    }
}
