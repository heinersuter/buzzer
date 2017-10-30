using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Buzzer.Core.WebApi.Game
{
    public class GameModel
    {
        public GameModel(string name)
        {
            Name = name;
            Created = DateTime.Now;
        }

        public string Name { get; }

        public ICollection<string> Users { get; set; } = new List<string>();

        public string Winner { get; set; }

        [JsonIgnore]
        public DateTime Created { get; }
    }
}
