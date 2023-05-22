using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data
{
    public class Playlist
    {
        public static void Create(string name, int creatorId)
        {
            var conectionString = "Server=localhost;Port=3306;Database=Song_Service;Uid=root;Pwd=password;";
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "INSERT INTO playlist (name, creatorId) VALUES (@name, @creatorId)";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("creatorId", creatorId);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    if (ex.Message.Contains("a foreign key constraint fails"))
                    {
                        throw new ArgumentException("creator does not exist");
                    }
                }
            }
        }
    }
}
