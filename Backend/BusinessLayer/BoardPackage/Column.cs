using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Column : PersistedObject<ColumnDTO>
    {
        private string Name;
        private int Limit;
        private List<Task> Tasks;
        private string Email;
        private int ColumnOrdinal;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Column(string Name, string email, int columnOrdinal)
        {
            this.Name = Name;
            Limit = 100;
            this.Email = email;
            Tasks = new List<Task>();
            this.ColumnOrdinal = columnOrdinal;
        }

        public Column(ColumnDTO c)
        {
            this.Name = c.Name;
            this.Limit = c.Limit;
            ColumnOrdinal = c.Ordinal;
            Email = c.Email;
            List<Task> tasks = new List<Task>();
            TaskDalController TaskDalController = new TaskDalController();
            List<TaskDTO> ListTasksDTO = TaskDalController.SelectUserTasks(Email, Name);
            foreach (TaskDTO task in ListTasksDTO)
            {
                tasks.Add(new Task(task));
            }
            this.Tasks = tasks;
        }

        //parameterlesss constructor for testing
        public Column() { }

        public int limit { get { return Limit; } set { Limit = value; } }
        public string name { get { return Name; } set { Name = value; } }
        public List<Task> tasks { get { return Tasks; } set { Tasks = value; } }
        public int columnOrdinal { get { return ColumnOrdinal; } set { ColumnOrdinal = value; } }

        public Task GetTask(int id)
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].TaskId == id)
                {
                    logger.Info("returning task with id" + id + "!");
                    return Tasks[i];
                }
            }
            logger.Warn("Task with id"+ id+ "not found!");
            throw new Exception("Task Not found! error ");
        }

        public void AddTask(Task toAdd)
        {
            if (!Tasks.Contains(toAdd))
            {
                logger.Info("Added "+ toAdd + "!");
                Tasks.Add(toAdd);
            }
            else
            {
                logger.Warn("task already exists");
                throw new Exception("Task Already Exists!");
            }
        }

        public void RemoveTask(Task toRemove, string email)
        {
            if (email == null || toRemove == null)
            {
                throw new Exception("error");
            }
            if (!toRemove.assigneeEmail.Equals(email))
                throw new Exception("This Task Is Assigned To Someone Else!");
            if (!Tasks.Contains(toRemove))
            {
                throw new Exception("error");
            }
            Tasks.Remove(toRemove);
        }
        
        public ColumnDTO ToDalObject()
        {
            ColumnDTO Column = new ColumnDTO(Email, Name,ColumnOrdinal, Limit);
            return Column;
        }

        //inserts the column in the database
        public virtual void Insert()
        {
            ToDalObject().Insert();
        }

        //updates a given value in the column
        public virtual void Update(string AttributeName, object value)
        {
            ToDalObject().Update(AttributeName, value);
        }

        //deletes the column from the database
        public void Delete()
        {
            ToDalObject().Delete();
        }

    }
}
