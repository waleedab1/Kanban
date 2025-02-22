﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        internal User(string email, string nickname)
        {
            this.Email = email;
            this.Nickname = nickname;
        }

        internal User(BusinessLayer.UserPackage.User u)
        {
            this.Email = u.getEmail();
            this.Nickname = u.getNickname();
            
        }
        

    }
}
