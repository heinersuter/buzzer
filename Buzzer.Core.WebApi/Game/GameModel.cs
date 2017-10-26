using System.Collections.Generic;

namespace Buzzer.Core.WebApi.Game
{
    public class GameModel
    {
        public string Name { get; set; }

        public IEnumerable<string> Users { get; set; } = new List<string>();

        public string Winner { get; set; }
    }
}
