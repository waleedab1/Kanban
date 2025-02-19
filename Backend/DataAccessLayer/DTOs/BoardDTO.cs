using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class BoardDTO : DTO
    {
        public const string BoardEmail = "Email";
        public const string BoardTaskId = "TaskId";
        private string _Email;
        private int _TaskId;

        public string ID { get { return " " + "[" + BoardEmail + "]" + " = " + "'" + _Email + "'"; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public int TaskId { get { return _TaskId; } set { _TaskId = value; } }

        public BoardDTO(string email, int taskId) : base(new BoardDalController())
        {
            this._Email = email;
            this._TaskId = taskId;
        }

        public void Insert()
        {
            Insert(this);
        }

        public void InsertGuest(string GuestEmail)
        {
            InsertGuest(Email, GuestEmail);
        }

        public void Update(string AttributeName,object AttributeValue)
        {
            Update(ID, AttributeName, AttributeValue);
        }
    }
}
