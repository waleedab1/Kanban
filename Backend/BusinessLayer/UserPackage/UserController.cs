using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        private Dictionary<string, User> users;
        private UserDalController UserData;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int Online = 0;
        //Magic Numbers
        private const int MAX_PASS = 25;
        private const int MIN_PASS = 5;

        public UserController()
        {
            users = new Dictionary<string, User>();
            UserData = new UserDalController();
        }
        
        public void LoadAllUsers()
        {
            List<UserDTO> list = UserData.SelectAllUsers();
            foreach(UserDTO u in list)
                users.Add(u.Email, new User(u));
            logger.Info("loaded all users");
        }

        //Delete the following table's data: User
        public void DeleteUserData()
        {
            bool rs = UserData.DeleteData();
            users = new Dictionary<string, User>();
        }

        //if the email is null we throw an exception,if the dictionary doesnt contain the email we throw an exception 
        //if the user with the given email is already logged in we throw an exception
        public void Login(string email, string password)
        {
            if (email == null)
            {
                logger.Warn("email is null");
                throw new Exception("email is null");
            }
            string tmp = email.ToLower();
            if (!users.ContainsKey(tmp))
            {
                logger.Warn("UserController: this email: " + email + " not found!");
                throw new Exception("email not found in the list of registered users's emails");
            }
            if (users[tmp].isloggedin())
            {
                logger.Warn("user is already logged in");
                throw new Exception("this user is already logged in");
            }
            if (Online > 0)
            {
                logger.Warn("cant allow more than 1 user to login at the same time");
                throw new Exception("cant allow more than 1 user to login in at the same time");
            }
            users[tmp].login(tmp, password);
            Online++;
            logger.Info("UserController: user with this email: " + email + "has successfully logged in!");
        }

        //if the email is null or the dictionay contains the email, we throw an exception
        //if the user with given email is not logged in, we throw an exception
        public void logout(string email)
        {
            if (email == null)
            {
                logger.Warn("email is null");
                throw new Exception("email is null");
            }
            string tmp = email.ToLower();
            if (!users.ContainsKey(tmp))
            {
                logger.Warn("UserController: this email: " + email + " not found!");
                throw new Exception("email does not exist");
            }
            if (!users[tmp].isloggedin())
            {
                logger.Warn("UserController: cant logout if the user is not logged in!");
                throw new Exception("cant logout if the user is not logged in");
            }
            users[tmp].logout();
            Online--;
            logger.Info("UserController: user with this email: " + email + "has successfully logged out!");
        }

        // if the password length is les than 5 or greater than 25 we throw an exception
        //if the password does not contain any number or any upper case letter or any lower case letter we throw an exception
        public void validatePassword(string password)
        {
            bool hasNumber = false;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            if (password.Length < MIN_PASS || password.Length > MAX_PASS)
            {
                logger.Warn("password size is less than 5 or more than 25");
                throw new Exception("Password length is less than 5 or more than 25");
            }
            for (int i = 0; i < password.Length; i++)
            {
                if (!hasNumber)
                    if (password[i] >= '0' && password[i] <= '9')
                        hasNumber = true;
                if (!hasLowerCaseLetter)
                    if (password[i] >= 'a' && password[i] <= 'z')
                        hasLowerCaseLetter = true;

                if (!hasUpperCaseLetter)
                    if (password[i] >= 'A' && password[i] <= 'Z')
                        hasUpperCaseLetter = true;
            }
            if (!hasLowerCaseLetter)
            {
                logger.Warn("password doesn't have any Lower Case Letter");
                throw new Exception("Password doesnt contain any lower case letter");
            }
            if (!hasUpperCaseLetter)
            {
                logger.Warn("password doesn't have any Upper Case Letter");
                throw new Exception("Password doesnt contain any upper case letter");
            }
            if (!hasNumber)
            {
                logger.Warn("password doesn't have any number");
                throw new Exception("password doesnt contain any number");
            }
            logger.Info("password fits all requirements and has successfully been used");
        }

        //if the email is null or the password is null, we throw an exception
        //if the dictionary contain the email, we throw an exception
        //we check if the password is valid b calling its function 
        //we throw an exception if the nickname is null or empty
        public void Register(string email, string password, string nickname)
        {
            //validating user's register info
            if (email == null || password == null)
            {
                logger.Warn("invalid parameters");
                throw new Exception("Invalid Parmeters!");
            }
            string tmp = email.ToLower();
            if (users.ContainsKey(tmp))
            {
                logger.Warn("UserController: attempt to register a new user with this email:" + email + "has failed because the email is already used by someone else.");
                throw new Exception("email already used");
            }
            validatePassword(password);
            if (nickname == null)
            {
                logger.Warn("nickname is null");
                throw new Exception("invalid nickname");
            }
            if (nickname.Length == 0)
            {
                logger.Warn("empty nickname");
                throw new Exception("invalid nickname");
            }
            //Adding User
            User user = new User(tmp, password, nickname);
            users.Add(tmp, user);
            user.Insert();
            logger.Info("UserController: a new user has successfully registered with this email:" + email + "!");
        }

        // if the email is null or the dictionary of users doesnt contain the given email we throw an exception
        //else we return the user with the given email
        public User getUser(string email)
        {
            if (email == null)
            {
                logger.Warn("email is null");
                throw new Exception("email is null");
            }
            string tmp = email.ToLower();
            if (!users.ContainsKey(tmp))
            {
                logger.Warn("UserController: failed to return a user because the given email is not used by any user.");
                throw new Exception("email does not exist");
            }
            logger.Info("UserController; the user with the email:" + email + "has successfully been returned!");
            return users[tmp];
        }

        //if the email is null or the dictionary does not contain the email , we throw an exception
        public void ValidateLoggedIn(string email)
        {
            if (email == null)
            {
                logger.Warn("email is null");
                throw new Exception("email is null");
            }
            string tmp = email.ToLower();

            if (!users.ContainsKey(tmp))
            {
                logger.Warn("UserController: the user with the email:" + email + "is not logged in because he hasn't been registered yet.");
                throw new Exception("no such user!");
            }
            if (!users[tmp].isloggedin())
            {
                logger.Warn("UserController: the user with the email:" + email + "is not logged in at this moment.");
                throw new Exception("user is not logged in");
            }
            logger.Info("UserController: the user with the email:" + email + "is logged in right  now!");
        }
    }
}

