using MySql.Data.MySqlClient;

namespace Data
{
    public class User
    {
        public static void Create(string name, string mail) 
        {
            var conectionString = "Server=localhost;Port=3306;Database=Song_Service;Uid=root;Pwd=password;";
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "INSERT INTO user (name, mail) VALUES (@name, @mail)";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("mail", mail);
                try
                {
                    command.ExecuteNonQuery();
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
    }
}