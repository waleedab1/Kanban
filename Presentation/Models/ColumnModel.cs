using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Models
{
    public class ColumnModel : NotifiableModelObject
    {
        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        private string UserEmail { get; set; }
        private int columnOrdinal;
        public int ColumnOrdinal { get { return columnOrdinal; } set { columnOrdinal = value; } }
        private string name;
        public string Name 
        { 
            get { return name; } 
            set 
            {
                try
                {
                    controller.ChangeColumnName(UserEmail, columnOrdinal, value);
                    name = value;
                    RaisePropertyChanged("Name");
                }
                catch(Exception e)
                {
                    MessageBox.Show("Failed to change name. " + e.Message);
                }
            } 
        }

        private int limit;
        public int Limit
        {
            get { return limit; }
            set
            {
                try
                {
                    controller.LimitColumnTasks(UserEmail, ColumnOrdinal, value);
                    limit = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to change limit. " + e.Message);

                }
            }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableForwardTask = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }

        private bool _enableForwardTask = false;
        public bool EnableForwardTask
        {
            get => _enableForwardTask;
            set
            {
                _enableForwardTask = value;
                RaisePropertyChanged("EnableForwardTask");
            }
        }

        internal ColumnModel(Column c, BackendController controller, string UserEmail, int columnOrdinal) : base(controller)
        {
            this.name = c.Name;
            this.limit = c.Limit;
            this.UserEmail = UserEmail;
            this.ColumnOrdinal = columnOrdinal;
            tasks = new ObservableCollection<TaskModel>(c.Tasks.Select(t => new TaskModel(t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.emailAssignee, controller, UserEmail, ColumnOrdinal)).ToList());
            Tasks.CollectionChanged += HandleChange;
        }
        //removes a tasks from a column
        public void RemoveTask()
        {
            //original
            controller.DeleteTask(UserEmail, SelectedTask.ColumnOrdinal, SelectedTask.Id);
            Tasks.Remove(SelectedTask);
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e) { }
    }
}
