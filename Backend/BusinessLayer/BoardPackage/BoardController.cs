using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class BoardController
    {
        private Dictionary<string, List<string>> Guests;
        private Dictionary<string, Board> Hostmap;
        private BoardDalController BoardData;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController()
        {
            Hostmap = new Dictionary<string, Board>();
            BoardData = new BoardDalController();
            Guests = new Dictionary<string, List<string>>();
        }

        public void LoadAllBoards()
        {
            List<BoardDTO> boards = BoardData.SelectAllBoards();
            foreach (BoardDTO b in boards)
            {
                Hostmap.Add(b.Email, new Board(b));
                Guests[b.Email] = BoardData.SelectBoardGuests(b.Email);
            }
        }

        /* Deleting the data from the following tables:
         * Board, Column, Task */
        public void DeleteBoardData()
        {
            ColumnDalController c = new ColumnDalController();
            TaskDalController t = new TaskDalController();
            bool rs1 = BoardData.DeleteData() && BoardData.DeleteGuests();
            bool rs2 = c.DeleteData();
            bool rs3 = t.DeleteData();
            Hostmap = new Dictionary<string, Board>();
            Guests = new Dictionary<string, List<string>>();
        }

        public Board getBoard(string email)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            string host = null;
            if (Hostmap.ContainsKey(email))
                host = email;
            else
                host = searchForHost(email);
            logger.Info("BoardController: returning board for user : " + email + "!");
            return Hostmap[host];
        }

        private string searchForHost(string email)
        {
            for (int i = 0; i < Guests.Count; i++)
                if (Guests.ElementAt(i).Value.Contains(email))
                    return Guests.ElementAt(i).Key;
            throw new Exception("A host for the current email was not found!");
        }

        public void addBoard(string email)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            if (Hostmap.ContainsKey(email))
            {
                logger.Warn("BoardController: " + email + " is in use!");
                throw new Exception("email already exist");
            }
            Board board = new Board(email);
            Hostmap.Add(email, board);
            Guests.Add(email, new List<string>());
            board.Insert();//inserting board in database
            logger.Info("BoardController: adding board for user : " + email + " succeeded!");
        }

        public void joinExistingBoard(string Guestemail, string Hostemail)
        {
            Check_if_Illegal_Input("HostEmail", Hostemail);
            Check_if_Illegal_Input("GuestEmail", Guestemail);
            Hostemail = Hostemail.ToLower();
            Guestemail = Guestemail.ToLower();
            Board b = getBoard(Hostemail);
            Guests[Hostemail].Add(Guestemail);
            b.InsertGuest(Guestemail);//inserting the guest in database
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Check_if_Illegal_Input("Name", newName);
            getBoard(email.ToLower()).ChangeColumnName(email, columnOrdinal, newName);
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Check_if_Illegal_Input("Email", email);
            Check_if_Illegal_Input("Title", title);
            //getting task
            email = email.ToLower();
            Board b = getBoard(email);
            Task t = b.GetColumn(columnOrdinal).GetTask(taskId);
            //check if task is done
            Check_if_Task_Done(taskId, columnOrdinal, b.ColumnNames().Count - 1, "Update Title of");
            t.EditTitle(title, email);
            t.Update("Title", title);//updating task Title in database
            logger.Info("BoardController: updating the title of task: " + taskId + ", in column: " + columnOrdinal + " succeeded!!");
        }

        public void UpdateTaskDueDate(string email, int columnOrd, int taskId, DateTime dDate)
        {
            Check_if_Illegal_Input("Email", email);
            //getting task
            email = email.ToLower();
            Board b = getBoard(email);
            Task t = b.GetColumn(columnOrd).GetTask(taskId);
            //checking if task is done
            Check_if_Task_Done(taskId, columnOrd, b.ColumnNames().Count - 1, "Update DueDate of");
            t.EditDueDate(dDate, email);
            t.Update("DueDate", dDate.ToString());//updating task DueDate in database
            logger.Info("BoardController: updating duedate of task: " + taskId + ", in column: " + columnOrd + " succeeded!!");
        }

        public void UpdateTaskDescription(string email, int columnOrd, int taskId, string descrip)
        {
            Check_if_Illegal_Input("Email", email);
            //getting task
            email = email.ToLower();
            Board b = getBoard(email);
            Task t = b.GetColumn(columnOrd).GetTask(taskId);
            //checking if task is done
            Check_if_Task_Done(taskId, columnOrd, b.ColumnNames().Count - 1, "Update Description of");
            t.EditDescription(descrip, email);
            t.Update("Description", descrip); //updating task Description in database
            logger.Info("BoardController: updating the description of task: " + taskId + ", in column: " + columnOrd + " succeeded!!");
        }

        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            Check_if_Illegal_Input("Email", email);
            Check_if_Illegal_Input("Title", title);
            email = email.ToLower();
            Board b = getBoard(email);
            //check if there is space for the task
            Check_if_Column_Is_Full(b.GetColumn(0).tasks.Count, b.GetColumn(0).limit, "Add", email);
            //Adding Task
            int id = b.taskid;
            Task toAdd = new Task(id, DateTime.Now, title, description, dueDate, email, b.GetColumn(0).name, b.email);
            b.taskid = id + 1;
            b.GetColumn(0).AddTask(toAdd);
            toAdd.Insert();//inserting task in database
            b.Update("TaskId", id + 1);//updating Task Counter in database
            logger.Info("BoardController: adding task to the user with email:" + email + "has succeeded!!");
            return toAdd;
        }

        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Check_if_Illegal_Input("Email", email);
            //limiting task
            email = email.ToLower();
            getBoard(email).LimitColumnTasks(email, columnOrdinal, limit);
            getBoard(email).GetColumn(columnOrdinal).Update("Limit", limit);//updating column's Limit
            logger.Info("limit column tasks has succeeded");
        }

        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Check_if_Illegal_Input("Email", email);
            //getting task
            email = email.ToLower();
            Board b = getBoard(email);
            Column fromColumn = b.GetColumn(columnOrdinal);
            Task toAdvance = fromColumn.GetTask(taskId);
            //checking if task can be advanced
            Check_if_Task_Done(taskId, columnOrdinal, b.ColumnNames().Count - 1, "Advance");

            Column ToColumn = b.GetColumn(columnOrdinal + 1);
            //checking if there is enough space to add the task
            Check_if_Column_Is_Full(ToColumn.tasks.Count, ToColumn.limit, "Advance", email);

            //advancing task
            fromColumn.RemoveTask(toAdvance, email);
            ToColumn.AddTask(toAdvance);
            toAdvance.Update("ColumnName", ToColumn.name);//updating task's ColumnName in database
            logger.Info("BoardController: advancing task to the user with email:" + email + "has succeeded!!");
        }

        public Column GetColumn(string email, string columnName)
        {
            Check_if_Illegal_Input("Email", email);
            Check_if_Illegal_Input("ColumnName", columnName);
            email = email.ToLower();
            Board b = getBoard(email);
            return b.GetColumn(b.ColumnNames().IndexOf(columnName));
        }

        public Column GetColumn(string email, int columnOrdinal)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            Board b = getBoard(email);
            return b.GetColumn(columnOrdinal);
        }

        public void RemoveColumn(string email, int columnOrdinal)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            Board b = getBoard(email);
            Column c = b.GetColumn(columnOrdinal);
            b.RemoveColumn(email, columnOrdinal);
            c.Delete();//deleting column
            b.Update();//updating board Order
        }
        public Column AddColumn(string email, int columnOrdinal, string name)
        {
            Check_if_Illegal_Input("Email", email);
            Check_if_Illegal_Input("Name", name);
            email = email.ToLower();
            Board b = getBoard(email);
            Column c = b.AddColumn(email, columnOrdinal, name);
            c.Insert();//inserting column
            b.Update();//updating board Order
            return c;
        }
        public void MoveColumnLeft(string email, int columnOrdinal)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            Board b = getBoard(email);
            b.MoveColumnLeft(email, columnOrdinal);
        }
        public void MoveColumnRight(string email, int columnOrdinal)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            Board b = getBoard(email);
            b.MoveColumnRight(email, columnOrdinal);
        }
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Check_if_Illegal_Input("Email", email);
            Check_if_Illegal_Input("EmailAssignee", emailAssignee);
            email = email.ToLower();
            if (!Hostmap.ContainsKey(email))
                throw new Exception("No such Host!");
            if (!Guests[email].Contains(emailAssignee))
                throw new Exception("No Such User In This Board!");
            Hostmap[email].GetColumn(columnOrdinal).GetTask(taskId).AssignTask(emailAssignee);
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Check_if_Illegal_Input("Email", email);
            email = email.ToLower();
            Column c = getBoard(email).GetColumn(columnOrdinal);
            Task t = c.GetTask(taskId);
            c.RemoveTask(t, email);
            t.Delete();//delete task from database
        }

        //private functions responsible for checking input and throwing exceptions.
        private void Check_if_Illegal_Input(string Attribute, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                logger.Warn("invalid " + Attribute);
                throw new Exception("Invalid Parmeter: " + Attribute);
            }
        }

        private void Check_if_Task_Done(int TaskID, int ColumnOrd, int LastColumn, string Attribute)
        {
            if (ColumnOrd == LastColumn)
            {
                logger.Warn("BoardController: " + Attribute + " task: " + TaskID + ", in column: " + ColumnOrd + " failed!!");
                throw new Exception("Can't " + Attribute + " a finished task!");
            }
        }

        private void Check_if_Column_Is_Full(int numOfTasks, int limit, string Attribute, string email)
        {
            if (numOfTasks == limit)
            {
                logger.Warn("BoardController: " + Attribute + " task to the user with email:" + email + "has failed");
                throw new Exception("Can't " + Attribute + "! Column Limit Has Been Reached!");
            }
        }
    }
}
