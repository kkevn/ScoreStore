using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class Player
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public List<uint> GameList { get; }
        public List<uint> FriendList { get; }
        public List<uint> StreakList { get; }

        public Player()
        {

        }
    }
}
