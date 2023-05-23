using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public class Playlist
    {
        public Playlist(int number, int playlistId, string playlistName, int creatorId, string creatorName, List<Core.User>? users)
        {
            Number = number;
            Users = users;
            PlaylistId = playlistId;
            PlaylistName = playlistName;
            CreatorId = creatorId;
            CreatorName = creatorName;
        }
        public List<Core.User>? Users { get; }
        public int Number { get; }
        public int PlaylistId { get; }
        public string PlaylistName { get; }
        public int CreatorId { get; }
        public string CreatorName { get; }

        public static async Task Create(string name, int creatorId)
        {
            if (name == null)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            await Data.Playlist.Create(name, creatorId);
        }
        public static async Task<List<Playlist>> GetAllUsers()
        {
            var dataPlaylists = await Data.Playlist.GetAllUsers();
            return dataPlaylists
                .GroupBy(p => new { p.PlaylistId, p.CreatorId })
                .Select((g, index) => new Playlist
                    (
                        index + 1,
                        g.Key.PlaylistId,
                        g.First().PlaylistName,
                        g.Key.CreatorId,
                        g.First().CreatorName,
                        g.Select((playlist, index) => new User(index + 1, (int)playlist.UserId, playlist.UserName, playlist.UserMail)).ToList()
                        )
                    ).ToList();
        }
        public static async Task<List<Playlist>> GetAll()
        {
            var dataPlaylists = await Data.Playlist.GetAll();
            return dataPlaylists
                .Select((playlist, index) => new Playlist
                    (
                        index + 1,
                        playlist.PlaylistId,
                        playlist.PlaylistName,
                        playlist.CreatorId,
                        playlist.CreatorName,
                        null
                    )
                ).ToList();
        }
        public static async Task DeleteById(int id) => await Data.Playlist.DeleteById(id);
        public static async Task DeleteByNumber(int number)
        {
            var playlists = await GetAll();
            var id = playlists.Where((playlist) => playlist.Number == number).Select(playlists => playlists.PlaylistId).First();
            await Data.Playlist.DeleteById(id);
        }
        public static async Task UpdateNameById(string name, int id)
        {
            if (name == null)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            await Data.Playlist.UpdateNameById(name, id);
        }
        public static async Task UpdateNameByNumber(string name, int number)
        {
            if (name == null)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            var playlists = await GetAll();
            var id = playlists.Where((playlist) => playlist.Number == number).Select(playlists => playlists.PlaylistId).First();
            await Data.Playlist.UpdateNameById(name, id);
        }
    }
}
