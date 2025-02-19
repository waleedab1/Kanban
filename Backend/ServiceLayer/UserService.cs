using System;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer

{
    class UserService
    {

        private UserController uc;

        internal UserService()
        {
            uc = new UserController();
        }
        public Response LoadData()
        {
            try
            {
                uc.LoadAllUsers();
                return new Response();
            } 
            catch (Exception e)
            {
                return new Response (e.Message);
            }
        }

        public Response DeleteData()
        {
            try
            {
                uc.DeleteUserData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<User> login(string email, string password)
        {
            try
            {
                uc.Login(email, password);
                User user = new User(uc.getUser(email));
                return new Response<User>(user);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }

        }

        public Response Logout(string email)
        {
            try
            {
                uc.logout(email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
   
        public Response register(string email, string password, string nickname)
        {
            try
            {
                uc.Register(email, password, nickname);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response validateLoggedIn(string email)
        {
            try
            {
                uc.ValidateLoggedIn(email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

    }
}
