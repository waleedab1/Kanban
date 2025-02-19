using Presentation.Models;
using Presentation.ViewModel;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddColumnWin.xaml
    /// </summary>
    public partial class AddColumnWin : Window
    {
        ColumnViewModel vm;
        BoardModel board;
        public AddColumnWin(BoardModel b)
        {
            this.board = b;
            InitializeComponent();
            vm = new ColumnViewModel();
            this.DataContext = vm;
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            vm.AddColumn(board);
            this.Close();
        }
    }
}
