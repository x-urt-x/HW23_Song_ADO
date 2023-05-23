using System.Text.RegularExpressions;

namespace Core
{
    public class User
    {
        public User(int number, int id, string name, string mail)
        {
            Number = number;
            Id = id;
            Name = name;
            Mail = mail;
        }
        public int Number { get; }
        public int Id { get; }
        public string Name { get; }
        public string Mail { get; }

        public static async Task Create(string name, string mail)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            if (mail == string.Empty)
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
            await Data.User.Create(name, mail);
        }
        public static async Task<List<User>> GetAll()
        {
            var dataUsers = await Data.User.GetAll();
            return dataUsers.Select((user, index) => new User(index + 1, user.Id, user.Name, user.Mail)).ToList(); //в бд авто индексы с пробелами из за удаленных записей
        }
        public static async Task<User> GetByMail(string mail)
        {
            if (mail == string.Empty)
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
            var dataUser = await Data.User.GetByMail(mail);
            return new User(1, dataUser.Id, dataUser.Name, dataUser.Mail);
        }

        public static async Task DeleteByMail(string mail)
        {
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
                await Data.User.DeleteByMail(mail);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public static async Task DeleteById(int id) => await Data.User.DeleteById(id);
        public static async Task DeleteByNumber(int number)
        {
            var users = await GetAll();
            var id = users.Where((user) => user.Number == number).Select(user => user.Id).First();
            await Data.User.DeleteById(id);
        }
        public static async Task UpdateNameByMail(string mail, string name)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            if (mail == string.Empty)
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
            await Data.User.UpdateNameByMail(name, mail);
        }
        public static async Task UpdateNameById(string name, int id)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            await Data.User.UpdateNameById(name, id);
        }
        public static async Task UpdateNameByNumber(string name, int number)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            var users = await GetAll();
            var id = users.Where((user) => user.Number == number).Select(user => user.Id).First();
            await Data.User.UpdateNameById(name, id);
        }
    }
}