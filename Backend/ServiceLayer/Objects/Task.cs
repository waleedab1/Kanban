using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly string emailAssignee;
        internal Task(int id, DateTime creationTime, string title, string description, DateTime dueDate, string emailAssignee)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.emailAssignee = emailAssignee;
        }
        internal Task(BusinessLayer.BoardPackage.Task T)
        {
            this.Id = T.TaskId;
            this.CreationTime = T.TaskCreationTime;
            this.Title = T.TaskTitle;
            this.Description = T.TaskDescription;
            this.DueDate = T.TaskDueDate;
            this.emailAssignee = T.assigneeEmail;
        }
    }
}
