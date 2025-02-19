using Presentation.Models;
using Presentation.ViewModel;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWin.xaml
    /// </summary>
    public partial class BoardWin : Window
    {
        BoardViewModel vm;
        public BoardWin(UserModel user)
        {
            InitializeComponent();
             this.DataContext = vm;
            this.DataContext = new BoardViewModel(user);
            vm = (BoardViewModel)this.DataContext;
            
        }

        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            new AddTaskWin(vm.board).Show();
        }

        private void SelectColumn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColumnList.SelectedIndex = -1;
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel CM = (ColumnModel)((Button)sender).DataContext;
            vm.SelectedColumn = CM;
            vm.RemoveTask();
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel CM = (ColumnModel)((Button)sender).DataContext;
            vm.SelectedColumn = CM;
            vm.advanceTask();
        }


        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            vm.SortTasks();
        }

        private void addColumn_Click(object sender, RoutedEventArgs e)
        {
            new AddColumnWin(vm.board).Show();
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel CM = (ColumnModel)((Button)sender).DataContext;
            vm.SelectedColumn = CM;
            vm.RemoveColumn();
        }
        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel CM = (ColumnModel)((Button)sender).DataContext;
            vm.SelectedColumn = CM;
            vm.MoveColumnRight();
        }

        private void MoveColumnLeft_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel CM = (ColumnModel)((Button)sender).DataContext;
            vm.SelectedColumn = CM;
            vm.MoveColumnLeft();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            vm.logout();
            LoginWin view = new LoginWin();
            view.Show();
            this.Close();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            vm.filterTasksByTitleOrDescription();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            vm.ResetFilter();
        }
    }
}
