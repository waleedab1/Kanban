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
using IntroSE.Kanban.Backend;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.Build.Tasks;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for LoginWin.xaml
    /// </summary>
    public partial class LoginWin : Window
    {
        LoginViewModel vm;

        public LoginWin()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            this.vm = (LoginViewModel)DataContext;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = vm.Login();
            if (u != null)
            {
                BoardWin boardView = new BoardWin(u);
                boardView.Show();
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(vm.HostEmail))
                vm.Register();
            else
                vm.Register(vm.HostEmail);
        }

        
    }
}
    

