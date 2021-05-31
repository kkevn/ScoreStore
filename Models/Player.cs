using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<uint> GameList { get; }
        public string GameList { get; set; }
        //public List<uint> FriendList { get; }
        public string FriendList { get; set; }
        //public List<uint> StreakList { get; }
        public string StreakList { get; set; }

        public Player()
        {

        }
    }
}
