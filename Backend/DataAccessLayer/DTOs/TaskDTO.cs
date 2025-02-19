using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class TaskDTO : DTO
    {
        public const string TaskId = "ID";
        public const string TaskCreationTime = "CreationTime";
        public const string TaskTitle = "Title";
        public const string TaskDescription = "Description";
        public const string TaskDueDate = "DueDate";
        public const string TaskColumnName = "ColumnName";
        public const string BoardEmail = "Email";
        public const string AssigneeEmail = "Assignee";

        public int _id;
        private string _creationtime;
        private string _title;
        private string _desc;
        private string _duedate;
        private string _Email;
        private string _ColumnName;
        private string _Assignee;

        public string ID { get { return " " + "[" + BoardEmail + "]" + " = " + "'" + _Email + "'" + " AND " + "[" + TaskId + "]" + " = " + "'" + _id + "'"; } }
        public string Email { get {return _Email;} }
        public string Assignee { get { return _Assignee; } }
        public string ColumnName { get { return _ColumnName; } }
        public int Id { get { return _id; } set { _id = value; } }
        public string CreationTime { get { return _creationtime; } set { _creationtime = value;} }
        public string Title { get {return _title; } set { _title = value;} }
        public string Description { get { return _desc; } set { _desc = value;} }
        public string DueDate { get { return _duedate; } set { _duedate = value;} }
        

        public TaskDTO(string email, string assigneeEmail, string cname,int task_id, string creation_date, string title, string desc, string due_date) : base(new TaskDalController())
        {
            _id = task_id;
            _title = title;
            _desc = desc;
            _creationtime = creation_date;
            _duedate = due_date;
            _ColumnName = cname;
            _Email = email;
            _Assignee = assigneeEmail;
        }

        public void Insert()
        {
            Insert(this);
        }

        public void Delete()
        {
            DeleteTask(this);
        }

        public void Update(string AttributeName, object AttributeValue)
        {
            Update(ID, AttributeName, AttributeValue);
        }

    }
}
