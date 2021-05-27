using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class GameModel
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }

        public GameModel()
        {

        }
    }
}
