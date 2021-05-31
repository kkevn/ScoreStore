using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }

        public Game()
        {
            
        }
    }
}
