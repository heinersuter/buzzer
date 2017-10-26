using System.Collections.Generic;

namespace Buzzer.Core.WebApi.Game
{
    public class GameModel
    {
        public GameModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ICollection<string> Users { get; set; } = new List<string>();

        public string Winner { get; set; }
    }
}
