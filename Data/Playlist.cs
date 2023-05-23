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
        public Playlist(int playlistId, string playlistName, int creatorId, string creatorName, int? userId = null, string? userName = null, string? userMail = null)
        {
            UserId = userId;
            UserName = userName;
            UserMail = userMail;
            PlaylistId = playlistId;
            PlaylistName = playlistName;
            CreatorId = creatorId;
            CreatorName = creatorName;
        }
        public int? UserId { get; }
        public string? UserName { get; }
        public string? UserMail { get; }
        public int PlaylistId { get; }
        public string PlaylistName { get; }
        public int CreatorId { get; }
        public string CreatorName { get; }

        private readonly static string conectionString = "Server=localhost;Port=3306;Database=Song_Service;Uid=root;Pwd=password;";
        public static async Task Create(string name, int creatorId)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "INSERT INTO playlist (name, creatorId) VALUES (@name, @creatorId)";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("creatorId", creatorId);
                try
                {
                    await command.ExecuteNonQueryAsync();
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
        public static async Task<List<Playlist>> GetAllUsers()
        {
            var playlists = new List<Playlist>();
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "SELECT `AddUsers`.`id` AS userId, `AddUsers`.`name` AS userName,`AddUsers`.`mail` AS userMail, `playlist`.`Id` AS playlistId, `playlist`.`name` AS playlistName, `playlist`.`CreatorId` , `CreatorUsers`.`name` AS creatorName FROM song_service.playlist" +
                    " JOIN `user-playlist` ON `playlist`.`Id` = `user-playlist`.`PlaylistId`" +
                    " JOIN `user` AS AddUsers ON `user-playlist`.`UserId` = `AddUsers`.`Id`" +
                    " JOIN `user` AS CreatorUsers ON `playlist`.`CreatorId` = `CreatorUsers`.`Id`;";
                var command = new MySqlCommand(query, connection);
                var reader = command.ExecuteReader();
                try
                {
                    while (await reader.ReadAsync())
                    {
                        playlists.Add(new Playlist(
                            reader.GetInt32("playlistId"),
                            reader.GetString("playlistName"),
                            reader.GetInt32("creatorId"),
                            reader.GetString("creatorName"),
                            reader.GetInt32("userId"),
                            reader.GetString("userName"),
                            reader.GetString("userMail")
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
                return playlists;
            }
        }
        public static async Task<List<Playlist>> GetAll()
        {
            var playlists = new List<Playlist>();
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "SELECT `playlist`.`Id` AS playlistId, `playlist`.`name` AS playlistName, `playlist`.`CreatorId` , `CreatorUsers`.`name` AS creatorName FROM song_service.playlist" +
                    " JOIN `user` AS CreatorUsers ON `playlist`.`CreatorId` = `CreatorUsers`.`Id`;";
                var command = new MySqlCommand(query, connection);
                var reader = command.ExecuteReader();
                try
                {
                    while (await reader.ReadAsync())
                    {
                        playlists.Add(new Playlist(
                            reader.GetInt32("playlistId"),
                            reader.GetString("playlistName"),
                            reader.GetInt32("creatorId"),
                            reader.GetString("creatorName")
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
                return playlists;
            }
        }
        public static async Task DeleteById(int id)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "DELETE FROM `song_service`.`playlist` WHERE `id` = @id;";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("id", id);

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw ex;
                }
            }
        }
        public static async Task UpdateNameById(string name, int id)
        {
            using (var connection = new MySqlConnection(conectionString))
            {
                connection.Open();
                var query = "UPDATE `song_service`.`playlist` SET `name` = @name WHERE `id`=@id;";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("name", name);

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
