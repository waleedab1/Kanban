using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class UserDTO : DTO
    {
        public const string EmailColumnName = "Email";
        public const string NickNameColumnName = "Nickname";
        public const string PassColumnName = "Password";

        private string _email;
        private string _nickname;
        private string _pass;

        public string ID { get { return " " + "[" + EmailColumnName + "]" + " = " + "'" + Email + "'"; } }

        public string Email { get { return _email; } set { _email = value; } }
        public string Nickname { get { return _nickname; } set { _nickname = value; } }
        public string Password { get { return _pass; } set { _pass = value; } }

        public UserDTO(string Email, string Nickname, string Password) : base(new UserDalController())
        {
            _email = Email;
            _nickname = Nickname;
            _pass = Password;
        }

        public void Insert()
        {
            Insert(this);
        }

        public void Update(string AttributeName, object AttributeValue)
        {
            Update(ID, AttributeName, AttributeValue);
        }
    }
}
