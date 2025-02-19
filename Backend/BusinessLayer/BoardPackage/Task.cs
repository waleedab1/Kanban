using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Task : PersistedObject<TaskDTO>
    {
        private int Id;
        private DateTime CreationTime;
        private DateTime DueDate;
        private string Title;
        private string Description;
        //the following two fields are for use in the Database
        private string Email;
        private string ColumnName;
        private string AssigneeEmail;
        //Logger
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Magic Numbers
        private const int MAX_TITLE = 50;
        private const int MAX_DESC = 300;

        public Task(int Id, DateTime CreationTime, string Title, string Description, DateTime DueDate,string email, string ColumnName, string hostemail)
        {
            if (Title.Length > MAX_TITLE)
            {
                logger.Warn("BoardController: adding task to the user with email:" + email + "has failed");
                throw new Exception("Illegal Title!");
            }
            if (Description != null && Description.Length > MAX_DESC)
            {
                logger.Warn("BoardController: adding task to the user with email:" + email + "has failed");
                throw new Exception("Illegal Description!");
            }
            if (DateTime.Compare(DateTime.Now, DueDate) > 0)
            {
                logger.Warn("BoardController: adding task to the user with email:" + email + "has failed");
                throw new Exception("Can't Set the Due Date To Be a Past Date!");
            }
            this.Id = Id;
            this.CreationTime = CreationTime;
            this.DueDate = DueDate;
            this.Title = Title;
            this.Description = Description;
            this.ColumnName = ColumnName;
            this.Email = hostemail;
            this.AssigneeEmail = email;
        }

        public Task(TaskDTO t)
        {
            this.Id = t.Id;
            this.CreationTime = DateTime.Parse(t.CreationTime);
            this.DueDate = DateTime.Parse(t.DueDate);
            this.Title = t.Title;
            this.Description = t.Description;
            this.Email = t.Email;
            this.ColumnName = t.ColumnName;
            this.AssigneeEmail = t.Assignee;
        }

        //parameterlesss constructor for testing
        public Task() { }

        public int TaskId { get { return Id; } }
        public string TaskTitle { get { return Title; } }
        public string TaskDescription { get { return Description; } }
        public DateTime TaskCreationTime { get { return CreationTime; } }
        public DateTime TaskDueDate { get { return DueDate; } }
        public string columnName { get { return ColumnName; } set { ColumnName = value;} }
        public string assigneeEmail { get { return AssigneeEmail; } set { AssigneeEmail = value; } }

        public void EditTitle(string title, string email)
        {
            if (!AssigneeEmail.Equals(email)) { throw new Exception("Only Assignee Can Edit His Task"); }
            if (title.Length > MAX_TITLE | title.Equals(""))
            {
                logger.Warn("illegal title");
                throw new Exception("Illegal Title!");
            }
            this.Title = title;
            logger.Info("title has been successfully edited");

        }
        public void EditDueDate(DateTime dDate, string email)
        {
            if (!AssigneeEmail.Equals(email)) { throw new Exception("Only Assignee Can Edit His Task"); }
            if (DateTime.Compare(CreationTime, dDate) > 0)
            {
                logger.Warn("Can't Set the Due Date To Be a Past Date");
                throw new Exception("Can't Set the Due Date To Be a Past Date!");
            }
            this.DueDate = dDate;
            logger.Info("DateTime has been successfully edited");
        }

        public void EditDescription(string description, string email)
        {
            if (!AssigneeEmail.Equals(email)) { throw new Exception("Only Assignee Can Edit His Task"); }
            if (description != null && description.Length > MAX_DESC)
            {
                logger.Warn("illegal description");
                throw new Exception("Illegal Description!");
            }
            this.Description = description;
            logger.Info("description has been successfully edited");
        }
        
        public void AssignTask(string newEmail)
        {
            assigneeEmail = newEmail;
            Update("Assignee", this.AssigneeEmail);
        }

        //inserts the task in the database
        public void Insert()
        {
            ToDalObject().Insert();
        }

        //updates a given value
        public virtual void Update(string AttributeName, object value)
        {
            ToDalObject().Update(AttributeName, value);
        }

        //deletes the Task from the database
        public void Delete()
        {
            ToDalObject().Delete();
        }

        public TaskDTO ToDalObject()
        {
            TaskDTO newTask = new TaskDTO(Email, AssigneeEmail, ColumnName, Id, CreationTime.ToString(), Title, Description, DueDate.ToString());
            return newTask;
        }
    }
}
