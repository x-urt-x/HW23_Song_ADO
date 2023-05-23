using Microsoft.VisualBasic.FileIO;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;
using System.Xml.Linq;

namespace Data
{
    public class User
    {
        public User(int id, string name, string mail)
        {
            Id = id;
            Name = name;
            Mail = mail;
        }
        public int Id { get; }
        public string Name { get; }
        public string Mail { get; }

        private readonly static string conectionString = "Server=localhost;Port=3306;Database=Song_Service;Uid=root;Pwd=password;";
        public static async Task Create(string name, string mail)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "INSERT INTO user (name, mail) VALUES (@name, @mail)";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("mail", mail);
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    if (ex.Message.Contains("Duplicate entry"))
                    {
                        throw new ArgumentException("not unique mail");
                    }
                }
            }
        }
        public static async Task<List<User>> GetAll()
        {
            var users = new List<User>();
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "SELECT * FROM USER";
                var command = new MySqlCommand(query, connection);
                var reader = command.ExecuteReader();
                try
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User(
                            reader.GetInt32("Id"),
                            reader.GetString("name"),
                            reader.GetString("mail")
                            ));
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
                return users;
            }
        }
        public static async Task<User> GetByMail(string mail)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "SELECT * FROM USER WHERE mail=@mail";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("mail", mail);
                var reader = command.ExecuteReader();
                try
                {
                    await reader.ReadAsync();
                    return new User(
                        reader.GetInt32("Id"),
                        reader.GetString("name"),
                        reader.GetString("mail")
                        );
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
        }
        public static async Task DeleteByMail(string mail)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "DELETE FROM `song_service`.`user` WHERE `mail` = @mail;";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("mail", mail);

                await command.ExecuteNonQueryAsync();
            }
        }
        public static async Task DeleteById(int id)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "DELETE FROM `song_service`.`user` WHERE `id` = @id;";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("id", id);

                await command.ExecuteNonQueryAsync();
            }
        }
        public static async Task UpdateNameByMail(string name, string mail)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "UPDATE `song_service`.`user` SET `name` = @name WHERE `mail`=@mail);";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("mail", mail);
                command.Parameters.AddWithValue("name", name);

                await command.ExecuteNonQueryAsync();
            }
        }
        public static async Task UpdateNameById(string name, int id)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "UPDATE `song_service`.`user` SET `name` = @name WHERE `id`=@id;";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("id", id);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}