using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.Models
{
    public class BoardModel : NotifiableModelObject
    {
        private ObservableCollection<ColumnModel> columns;

        public ObservableCollection<ColumnModel> Columns
        {
            get { return columns; }
            set { columns = value; RaisePropertyChanged("Columns"); }
        }

        public string emailCreator { get; set; }
        private string UserEmail;
        internal BoardModel(List<ColumnModel> columns, string emailCreator, string UserEmail, BackendController controller) : base(controller)
        {
            this.UserEmail = UserEmail;
            Columns = new ObservableCollection<ColumnModel>(columns);
            this.emailCreator = emailCreator;
            Columns.CollectionChanged += HandleChange;
        }

        public void AddTask(string title, string description, DateTime dueDate)
        {
            TaskModel added = controller.AddTask(UserEmail, title, description, dueDate);
            if(added != null)
            {
                Columns[0].Tasks.Add(added);
            }
        }

        public void AddColumn(string columnOrdinal, string name)
        {
            ColumnModel added = controller.AddColumn(UserEmail,int.Parse(columnOrdinal), name);
            if (added != null)
            {
                Columns.Insert(int.Parse(columnOrdinal),added);
                for (int i = 0; i < Columns.Count; i++)
                {
                    Columns[i].ColumnOrdinal = i;
                }
            }
        }
       //this function removes a colun from the board and moves all of its tasks to the column to its right if it is the leftmost column if it can handle all the tasks by checking it's limit, else it moves
       //all the tasks to the column on its left if it candle both the tasks by checking it's limit, if we have only 2 columns in the board, wr cant remove any column at this point
        public void RemoveColumn(ColumnModel c)
        {
          int num = c.ColumnOrdinal;
          controller.RemoveColumn(UserEmail, num);
          Columns = new ObservableCollection<ColumnModel>(controller.GetBoard(UserEmail).Columns);
          Columns.CollectionChanged += HandleChange;
        }
        public void logout(string email)
        {
            controller.Logout(email);
        }
        //this function move a column with the given columnOrdinal in the parameter to it's left, unless it's the leftmost column

        public void MoveColmnLeft(int ordinal)
        {
            ColumnModel added = controller.MoveColumnLeft(UserEmail, ordinal);

            if(added != null)
            {
                Columns = new ObservableCollection<ColumnModel>(controller.GetBoard(UserEmail).Columns);
                Columns.CollectionChanged += HandleChange;
            }
        }
        //this function move a column with the given columnOrdinal in the parameter to it's right, unless it's the rightmost column
        public void MoveColumnRight(int ordinal)
        {
           ColumnModel added = controller.MoveColumnRight(UserEmail, ordinal );
            
            if(added!=null)
            {
                Columns = new ObservableCollection<ColumnModel>(controller.GetBoard(UserEmail).Columns);
                Columns.CollectionChanged += HandleChange;
            }
        }

        //in this code we can move a task from one column to another column which is on it's right, unless it's in the rightmost column
        public void advanceTask(int columnOrdinal, int id)
        {
            try
            {
                controller.AdvanceTask(UserEmail, columnOrdinal, id);
                for(int i=0; i<Columns[columnOrdinal].Tasks.Count; i++)
                {
                    if(Columns[columnOrdinal].Tasks.ElementAt(i).Id == id)
                    {
                        TaskModel t = Columns[columnOrdinal].Tasks.ElementAt(i);
                        Columns[columnOrdinal].Tasks.RemoveAt(i);
                        Columns[columnOrdinal + 1].Tasks.Add(t);
                        t.ColumnOrdinal = columnOrdinal + 1;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            Columns.CollectionChanged += HandleChange;

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e) { }


        //this code sorts all the tasks in the prigram by it's due date
        public void SortTasks()
        {
            foreach (ColumnModel c in Columns)
            {
                List<TaskModel> myList = c.Tasks.OrderBy(x => x.DueDate).ToList();
                c.Tasks.Clear();
                foreach (TaskModel t in myList)
                {
                    c.Tasks.Add(t);
                }
            }
        }
    }
}
