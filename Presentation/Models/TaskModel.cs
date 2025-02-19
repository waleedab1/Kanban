using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.Models
{
    public class TaskModel : NotifiableModelObject
    {
        private string UserEmail;
        private int columnOrdinal;
        public int ColumnOrdinal { get { return columnOrdinal; } set { columnOrdinal = value; } }
        private int id;
        public int Id { get { return id; } }
        private DateTime creationTime;
        public DateTime CreationTime { get { return creationTime; } }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                try
                {
                    controller.UpdateTaskTitle(UserEmail, ColumnOrdinal, Id, value);
                    title = value;
                    RaisePropertyChanged("Title");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to update task title. " + e.Message);
                }
            }
        }
        private string description;
        public string Descritpion
        {
            get { return description; }
            set
            {
                try
                {
                    controller.UpdateTaskDescription(UserEmail, ColumnOrdinal, Id, value);
                    description = value;
                    RaisePropertyChanged("Descritpion");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to update task description. " + e.Message);
                }
            }
        }

        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                try
                {
                    controller.UpdateTaskDueDate(UserEmail, ColumnOrdinal, Id, value);
                    dueDate = value;
                    CalculatePercentage();
                    RaisePropertyChanged("DueDate");
                    RaisePropertyChanged("TaskBackground");
                }
                catch(Exception e)
                {
                    MessageBox.Show("Failed to update task due date. " + e.Message);
                }
            }
        }

        private string emailAssignee;
        public string EmailAssignee
        {
            get { return emailAssignee; }
            set
            {
                try
                {
                    controller.AssignTask(UserEmail, ColumnOrdinal, Id, value);
                    emailAssignee = value;
                    RaisePropertyChanged("EmailAssignee");
                }
                catch(Exception e)
                {
                    MessageBox.Show("Failed to update task assignee. " + e.Message);
                }
            }
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        private SolidColorBrush taskBackground;
        public SolidColorBrush TaskBackground
        {
            get {
                CalculatePercentage();
                return taskBackground; 
            }
            set { taskBackground = value; RaisePropertyChanged("TaskBackground"); }
        }

        public SolidColorBrush TaskBorder
        {
            get { 
                if (UserEmail.Equals(emailAssignee))
                    return new SolidColorBrush(Colors.Blue);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }
        //this code helps us calculate the percentage of the times that is over since the creation time of the task
        //so we can use this code to give specific colors to the tasks as requested
        public void CalculatePercentage()
        {
            var total = (dueDate - creationTime).TotalSeconds;
            var percentage = (int)(DateTime.Now - creationTime).TotalSeconds * 100 / total;
            int result = (int)percentage;
            if (result >= 100)
                taskBackground = new SolidColorBrush(Colors.Red);
            else if (result >= 75)
                taskBackground = new SolidColorBrush(Colors.Orange);
            else
                taskBackground = new SolidColorBrush(Colors.LightGreen);
        }

        internal TaskModel(int id, DateTime creationTime, string title, string description, DateTime dueDate, string emailAssignee, BackendController controller, string UserEmail, int columnOrdinal) : base(controller)
        {
            this.id = id;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.emailAssignee = emailAssignee;
            this.UserEmail = UserEmail;
            this.columnOrdinal = columnOrdinal;
        }
    }
}
