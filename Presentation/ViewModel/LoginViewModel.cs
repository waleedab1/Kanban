using Presentation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        private SolidColorBrush messageBackground;
        public SolidColorBrush MessageBackground
        {
            get { return messageBackground; }
            set { messageBackground = value; RaisePropertyChanged("MessageBackground"); }
        }


        private string hostEmail;
        public string HostEmail
        {
            get => hostEmail;
            set
            {
                hostEmail = value;
                RaisePropertyChanged("HostEmail");
            }
        }

        private string nickName;
        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                RaisePropertyChanged("Nickname");
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public LoginViewModel()
        {
            controller = new BackendController();
        }

        public UserModel Login()
        {
            Message = "";
            try
            {
                return controller.Login(email, password);
            }
            catch (Exception e)
            {
                MessageBackground = new SolidColorBrush(Colors.Red);
                Message = e.Message;
                return null;
            }
        }

        public void Register()
        {
            Message = "";
            try
            {
                controller.Register(email, password, nickName);
                MessageBackground = new SolidColorBrush(Colors.Green);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                MessageBackground = new SolidColorBrush(Colors.Red);
                Message = e.Message;
            }
        }

        public void Register(string HostEmail)
        {
            Message = "";
            try
            {
                controller.Register(email, password, nickName, HostEmail);
                MessageBackground = new SolidColorBrush(Colors.Green);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                MessageBackground = new SolidColorBrush(Colors.Red);
                Message = e.Message;
            }
        }
    }
}
