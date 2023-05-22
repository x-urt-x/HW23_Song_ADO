using System.Text.RegularExpressions;

namespace Core
{
    public class User
    {
        public static void Create(string name, string mail)
        {
            if (name == null)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            if (mail == null)
            {
                throw new ArgumentException("mail cant be null");
            }
            if (mail.Length > 100)
            {
                throw new ArgumentException("mail too long");
            }
            if (!Regex.IsMatch(mail, @"^([\w!#$%&‘*+—/=?^_`{|}~]+)([\w!#$%&‘*+—\./=?^_`{|}~]*?)(?(2)(?<!\.)|)@\w{2,}(\.\w+)+$"))
            {
                throw new ArgumentException("mail is not valid");
            }
            try
            {
                Data.User.Create(name, mail);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}