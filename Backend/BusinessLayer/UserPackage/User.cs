using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Data.Entity.Migrations.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{



    class User : PersistedObject<UserDTO>
    {
        private string Password;
        private string Email;
        private string Nickname;
        private bool logged_in;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public User(string email, string password, string nickname)
        {
            if (!IsValidEmail(email))
                throw new Exception("Invalid Email!");
            this.Email = email;
            this.Nickname = nickname;
            this.Password = password;
            logged_in = false;
        }
        public User(UserDTO u)
        {
            this.Password = u.Password;
            this.Email = u.Email;
            this.Nickname = u.Nickname;
            logged_in = false;
        }
        public void login(string email, string password)
        {
            if (logged_in)
            {
                logger.Warn("cant login if the user is already logged in");
                throw new Exception("cant login if user is already logged in");
            }
            if (!this.Email.Equals(email))
            {
                logger.Warn("wrong email");

                throw new Exception("email mismatch");
            }
            if (!this.Password.Equals(password))
            {
                throw new Exception("password mismatch");
            }
            logged_in = true;
        }


        public void logout()
        {
            if (!logged_in)
            {
                logger.Warn("cant logout if the user is not logged in");

                throw new Exception("the user cant logout if he is not logged in");
            }
            this.logged_in = false;
        }

        public String getEmail()
        {
            return Email;
        }

        public string getNickname()
        {
            return this.Nickname;
        }

        public bool isloggedin()
        {
            return logged_in;
        }

        public void Insert()
        {
            ToDalObject().Insert();
        }

        public void Update(string AttributeName, object value)
        {
            ToDalObject().Update(AttributeName, value);
        }

        public UserDTO ToDalObject()
        {
            UserDTO dal_user = new UserDTO(Email, Nickname, Password);
            return dal_user;
        }

        //this function is responsible for checking if the entered email is in the right format
        // this code was taken from Microsoft API
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

