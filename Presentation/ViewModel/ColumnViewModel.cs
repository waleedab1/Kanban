using Presentation.Models;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class ColumnViewModel : NotifiableObject
    {

        private string columnOrdinal;
        public string ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ColumnViewModel() { }

        public void AddColumn(BoardModel b)
        {
            try
            {
                b.AddColumn(columnOrdinal, name);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could Not Add Column." + e.Message);
            }
        }
    }
}
