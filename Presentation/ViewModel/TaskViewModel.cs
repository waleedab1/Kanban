using Presentation.Models;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class TaskViewModel : NotifiableObject
    {
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string title = "";
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }

        private string dueDate = "";

        public string DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        public TaskViewModel() { }

        public void AddTask(BoardModel b)
        {
            try
            {
                b.AddTask(title, description, DateTime.Parse(dueDate)); 

            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Add Task. " + e.Message);
            }
        }
    }
}
