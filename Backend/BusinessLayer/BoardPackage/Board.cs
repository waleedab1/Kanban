using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{

    class Board : PersistedObject<BoardDTO>
    {
        private List<Column> Columns;
        private int TaskId;
        private string Email;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Board(string email)
        {

            this.Columns = new List<Column>();
            Columns.Add(new Column("backlog",email, 0));
            Columns.Add(new Column("in progress",email, 1));
            Columns.Add(new Column("done",email, 2));
            TaskId = 1;
            this.Email = email;
        }

        public Board(BoardDTO b)
        {
            TaskId = b.TaskId;
            Email = b.Email;
            ColumnDalController ColumnDalController = new ColumnDalController();
            List<ColumnDTO> ListColumnDTO = ColumnDalController.SelectUserColumns(Email);
            Columns = new List<Column>();
            foreach (ColumnDTO col in ListColumnDTO)
            {
                Columns.Add(new Column(col));
            }
        }

        //this constructor is for use in tests
        public Board(string email, List<Column> columns)
        {
            this.Email = email;
            this.Columns = columns;
            this.TaskId = 1;
        }

        public List<string> ColumnNames()
        {
            List<String> ColumnNames = new List<string>();
            foreach (Column c in Columns)
                ColumnNames.Add(c.name);
            return ColumnNames;
        }

        public string email { get { return Email; } }
        public int taskid { get { return TaskId; } set { this.TaskId = value; } }

        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal > Columns.Count - 1 | columnOrdinal < 0)
            {
                logger.Warn("no such column");
                throw new Exception("No such column!");
            }
            logger.Info("getting column");
            return Columns[columnOrdinal];
        }

        //first we check if the tasks that already exists in the column with the given columnOrdinal is greater than the setLimit we received in the parameter,
        //if it is, then we throw an exception
        //if its not, we change the limit of the column with the given columnOrdinal to be the given setLimit
        public void LimitColumnTasks(string email, int columnOrdinal, int Setlimit)
        {
            if (!Email.Equals(email)) { throw new Exception("Only Host Can Limit A Column"); }
            if (GetColumn(columnOrdinal).tasks.Count > Setlimit)
            {
                logger.Warn("The Number of Tasks Surpasses The Wanted Limit");
                throw new Exception("The Number of Tasks Surpasses The Wanted Limit!");
            }
            GetColumn(columnOrdinal).limit = Setlimit;
            logger.Info("Tasks in Column: " + columnOrdinal + " - has been limited");
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if(newName == null) { throw new Exception("A column name can't be null"); }
            if (!Email.Equals(email)) { throw new Exception("Only the Host can change a column's name."); }
            if (ColumnNames().Contains(newName))
                throw new Exception("Column name already exists.");
            Columns[columnOrdinal].Update("Name", newName);
            Columns[columnOrdinal].name = newName;
        }

        //this function is used in RemoveColumn function, and its only purpose is to move tasks from one column to another
        public void MoveTasksAfterDelete(int ColumnOrdinal)
        {
            if (ColumnOrdinal == 0)
            {
                for (int i = 0; i < Columns[0].tasks.Count; i++)
                {
                    Columns[1].AddTask(Columns[0].tasks[i]);
                    Columns[0].tasks[i].Update("ColumnName", Columns[1].name);
                }
                logger.Info("Tasks has been moved to its right column");
            }
            else
            {
                for (int i = 0; i < Columns[ColumnOrdinal].tasks.Count; i++)
                {
                    Columns[ColumnOrdinal - 1].AddTask(Columns[ColumnOrdinal].tasks[i]);
                    Columns[ColumnOrdinal].tasks[i].Update("ColumnName", Columns[ColumnOrdinal - 1].name);
                }
                logger.Info("Tasks has been moved to its left column");

            }

        }

        //first we check if the given columnOrdinal is out of the wanted range
        //We throw an exception if our numberof columns is 2, since it is the minimum number of columns
        //we check if the column we will move the tasks to will be able to contain them
        //if we want to remove the first column we move it's tasks to  the second
        //else we move the tasks to the previous column
        //after moving tasks we delete the column
        public void RemoveColumn(string email, int ColumnOrdinal)
        {
            if (!Email.Equals(email)) { throw new Exception("Only Host Can Remove A Column"); }
            if (ColumnOrdinal < 0 | ColumnOrdinal > Columns.Count - 1)
            {
                logger.Warn("columnOrdinal is invalid");
                throw new Exception("invalid index");
            }

            if (Columns.Count == 2)
            {
                logger.Warn("Error, min number of columns is 2, here after we delete , we end up with 1 COLUMN!!!");
                throw new Exception("Error, min number of columns is 2, here after we delete , we end up with 1 COLUMN!!!");
            }
            if (ColumnOrdinal == 0)
            {
                if (Columns[1].limit < Columns[0].tasks.Count + Columns[1].tasks.Count)
                {
                    logger.Warn("cant delete this column. because it's right column cant handle both of the 2 columns's tasks ");
                    throw new Exception("error occured while trying to delete column: its RIGHT column CANT handle both of 2 columns tasks");
                }
            }
            else
            {
                if (Columns[ColumnOrdinal - 1].limit < Columns[ColumnOrdinal].tasks.Count + Columns[ColumnOrdinal - 1].tasks.Count)
                {
                    logger.Warn("cant delete this column. because it's left column cant handle both of the 2 columns's tasks ");
                    throw new Exception("error occured while trying to delete column: its LEFT column CANT handle both of 2 columns tasks");
                }
            }
            MoveTasksAfterDelete(ColumnOrdinal);
            Columns.RemoveAt(ColumnOrdinal);
            logger.Info("column has been removed");
        }

        /// <summary>
        /// <param name="columnOrdinal"></param>
        /// we check if the given columnOrdinal is out of the required range, if so we throw an exception
        /// now we check if the column we are trying to move to the right is the rightmost column, if so we throw an exception,
        /// if nothing failed and we didnt throw any exception, we move the column with the given columnOrdinal with the column on its right, 
        /// while also switching these 2 columns's columnOrdinal with one another
        public void MoveColumnRight(string email, int columnOrdinal)
        {
            if (!Email.Equals(email)) { throw new Exception("Only Host Can Move A Column"); }
            if (columnOrdinal < 0 || columnOrdinal > Columns.Count - 1)
            {
                logger.Warn("columnOrdinal is invalid");
                throw new Exception("invalid ColumnOrdinal!");
            }

            if (columnOrdinal == Columns.Count - 1)
                throw new Exception("Can't Move the RightMost Column Right!");
            Columns[columnOrdinal].Update("Ordinal", columnOrdinal + 1);
            Columns[columnOrdinal + 1].Update("Ordinal", columnOrdinal);
            Columns.Insert(columnOrdinal, Columns[columnOrdinal+1]);
            Columns.RemoveAt(columnOrdinal + 2);
            logger.Info("move column right has succeeded");
        }

        /// <summary>
        /// <param name="columnOrdinal"></param>
        /// we check if the given columnOrdinal is out of the required range, if so we throw an exception
        /// now we check if the column we are trying to move to the left is the leftmost column, if so we throw an exception,
        /// if nothing failed and we didnt throw any exception, we move the column with the given columnOrdinal with the column on its left, 
        /// while also switching these 2 columns's columnOrdinal with one another
        public void MoveColumnLeft(string email, int columnOrdinal)
        {
            if (!Email.Equals(email)) { throw new Exception("Only Host Can Limit A Column"); }
            if (columnOrdinal < 0 || columnOrdinal > Columns.Count - 1)
            {
                logger.Warn("columnOrdinal is invalid");
                throw new Exception("invalid ColumnOrdinal!");
            }

            if (columnOrdinal == 0)
            {
                logger.Warn("columnOrdinal is invalid");
                throw new Exception("Can't Move the LeftMost Column Left!");
            }

            Columns[columnOrdinal].Update("Ordinal", columnOrdinal - 1);
            Columns[columnOrdinal - 1].Update("Ordinal", columnOrdinal);
            Columns.Insert(columnOrdinal - 1, Columns[columnOrdinal]);
            Columns.RemoveAt(columnOrdinal + 1);
            logger.Info("move column right has succeeded");
        }

        /// <summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="name"></param>
        ///first we check if the columnname length is greater than 15 ,if so we throw an exception
        ///then we check if the columnOrdinal is negative or larger than the columns's count, if so we throw an exception
        ///after that we check if there is another column with the same column name we got in our function parameters, if so we throw an exception
        ///and then.. if nothing failed, we add a new column with the name and columnOrdianl we got in the parameters, we add them with the Email
        public Column AddColumn(string email, int columnOrdinal, string name)
        {
            if (!Email.Equals(email)) { throw new Exception("Only Host Can Add A Column"); }
            if (name.Length > 15)
            {
                logger.Warn("Entered Name Surpasses Max Length!");
                throw new Exception("Entered Name Surpasses Max Length!");
            }
            if (columnOrdinal < 0 || columnOrdinal > Columns.Count)   
            {
                logger.Warn("column ordinal is invalid");
                throw new Exception("column ordinal is invalid");
            }
            if (ColumnNames().Contains(name))
            {
                logger.Warn("column name is already taken");
                throw new Exception("column name already exists");
            }
            Column c = new Column(name, Email, columnOrdinal);
            Columns.Insert(columnOrdinal, c);
            logger.Info("column has been added");
            return c;
        }

        public void Insert()
        {
            ToDalObject().Insert();
            Columns[0].ToDalObject().Insert();
            Columns[1].ToDalObject().Insert();
            Columns[2].ToDalObject().Insert();
        }

        //Updates Board's Data
        public void Update(string AttributeName, object value)
        {
            ToDalObject().Update(AttributeName, value);
        }

        //Updates the order of all columns
        public void Update()
        {
            int index = 0;
            foreach (Column c in Columns)
            {
                c.Update("Ordinal", index);
                index++;
            }
        }

        //Insert A guest in the board
        public void InsertGuest(string GuestEmail)
        {
            ToDalObject().InsertGuest(GuestEmail);
        }

        public BoardDTO ToDalObject()
        {
            BoardDTO Board = new BoardDTO(Email, TaskId);
            return Board;
        }
    }

}

