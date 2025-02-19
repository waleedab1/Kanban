using Presentation.Models;
using Presentation.View;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        public BoardModel board { get; private set; }
        public UserModel user { get; private set; }
        public BackendController controller { get; private set; }

        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public string Title { get; private set; }

        public BoardViewModel(UserModel user)
        {
            this.user = user;
            this.controller = user.controller;
            board = user.GetBoard(user.Email);
            Title = "Kanban board for " + board.emailCreator+" "  + " - "+ "Logged in as " + user.Email;
        }
        //this function removes a colun from the board and moves all of its tasks to the column to its right if it is the leftmost column if it can handle all the tasks by checking it's limit, 
        //else it moves all the tasks to the column on its left if it candle both the tasks by checking it's limit, if we have only 2 columns in the board, wr cant remove any column at this point
        public void RemoveColumn()
        {
            try
            {
                board.RemoveColumn(SelectedColumn);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Remove Column. " + e.Message);
            }
        }

        public void RemoveTask()
        {
            try
            {
                if (SelectedColumn == null)
                    MessageBox.Show("You need to select the column the task is in");
                else
                {
                    SelectedColumn.RemoveTask();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Remove Task. " + e.Message);
            }
        }
        //this function move a column with the given columnOrdinal in the parameter to it's right, unless it's the rightmost column
        public void MoveColumnRight()
        {
            try
            {
                board.MoveColumnRight(SelectedColumn.ColumnOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show("failed to move the column to the right. " + e.Message);
            }
        }
        //this function move a column with the given columnOrdinal in the parameter to it's left, unless it's the leftmost column
        public void MoveColumnLeft()
        {
            try
            {
                board.MoveColmnLeft(SelectedColumn.ColumnOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show("failed to move the column to the left. " + e.Message);
            }
        }

        public void advanceTask()
        {
            try
            {
                if (SelectedColumn == null)
                    MessageBox.Show("You need to select the column the task is in");
                else if (SelectedColumn.SelectedTask == null)
                    MessageBox.Show("You need to select the task");
                else
                {
                    board.advanceTask(SelectedColumn.ColumnOrdinal, SelectedColumn.SelectedTask.Id);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to advance task. " + e.Message);
            }
        }

        public void logout()
        {
            try
            {
                
                board.logout(user.Email);
            }
            catch (Exception e)
            {
                MessageBox.Show("logout failed " + e.Message);
            }
        }

        public void SortTasks()
        {

                board.SortTasks();
            
        }
        //by calling this function, we get all the tasks in all columns which their title or description contain the given letters
        public void filterTasksByTitleOrDescription()
        {
            foreach (ColumnModel column in board.Columns)
            {
                foreach (TaskModel task in column.Tasks)
                {
                    if (task.Descritpion.Contains(SearchText) || task.Title.Contains(SearchText))
                    {
                        task.IsVisible = true;
                    }
                    else
                    {
                        task.IsVisible = false;
                    }
                }
            }
                
        }
        public void ResetFilter()
        {
            foreach (ColumnModel column in board.Columns)
            {
                foreach (TaskModel task in column.Tasks)
                {
                    task.IsVisible = true;
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");
            }
        }
     
    }
}
