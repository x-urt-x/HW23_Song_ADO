using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public class Playlist
    {
        public static void Create(string name, int creatorId)
        {
            if (name == null)
            {
                throw new ArgumentException("name cant be null");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("name too long");
            }
            try
            {
                Data.Playlist.Create(name, creatorId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
