using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class Scores
    {
        public int Id { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string StreakList { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public Scores()
        {
            Wins = Losses = 0;
        }

        public Scores(String userId, int gameId)
        {
            UserId = userId;
            GameId = gameId;
            Wins = Losses = 0;
        }
    }
}
