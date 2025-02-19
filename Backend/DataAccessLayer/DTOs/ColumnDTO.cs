using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class ColumnDTO : DTO
    {
        public const string ColumnName = "Name";
        public const string ColumnOrdinal = "Ordinal";
        public const string ColumnLimit = "Limit";
        public const string ColumnEmail = "Email";
        private int _limit;
        private string _name;
        private int _Ordinal;
        private string _email;

        public string ID { get { return " " + "[" + ColumnEmail + "]" + " = " + "'" + _email + "'" + " AND " + "[" + ColumnName + "]" + "=" + "'" + _name + "'"; } }
        public string Name { get { return _name; } }
        public int Ordinal { get { return _Ordinal; } }
        public string Email { get { return _email; } }
        public int Limit { get { return _limit; } set { _limit = value; } }

        public ColumnDTO(string email, string name,  int Ordinal, int limit) : base(new ColumnDalController())
        {
            _name = name;
            _limit = limit;
            _email = email;
            _Ordinal = Ordinal;
 
        }

        public void Insert()
        {
            Insert(this);
        }

        public void Delete()
        {
            Delete(this);
        }

        public void Update(string AttributeName, object AttributeValue)
        {
            Update(ID, AttributeName, AttributeValue);
        }

    }
}
