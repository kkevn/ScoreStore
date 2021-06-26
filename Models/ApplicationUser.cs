using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string GameList { get; set; }
        public string FriendList { get; set; }
        public string StreakList { get; set; }
    }
}
