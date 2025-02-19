using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class UserModel : NotifiableModelObject
    {
        private string email;
        public string Email { get { return email; } }
        private string nickname;
        public string Nickname { get { return nickname; } }

        internal UserModel(string email, string nickname, BackendController controller) : base(controller)
        {
            this.email = email;
            this.nickname = nickname;
        }

        public BoardModel GetBoard(string email)
        {
            return controller.GetBoard(email);
        }
    }
}
