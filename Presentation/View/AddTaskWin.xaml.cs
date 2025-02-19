using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.Models;
using Presentation.ViewModel;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddTaskWin.xaml
    /// </summary>
    public partial class AddTaskWin : Window
    {
        TaskViewModel vm;
        BoardModel board;
        public AddTaskWin(BoardModel b)
        {
            this.board = b;
            InitializeComponent();
            vm = new TaskViewModel();
            this.DataContext = vm;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            vm.AddTask(board);
            this.Close();
        }
    }
}
