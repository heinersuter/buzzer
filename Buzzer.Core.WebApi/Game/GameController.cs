using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Buzzer.Core.WebApi.Game
{
    [Route("api/games")]
    public class GameController : Controller
    {
        private static readonly List<GameModel> Games = new List<GameModel>();
        private readonly IHubContext<GameHub> _hubContext;

        public GameController(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public IEnumerable<GameModel> Get()
        {
            Console.WriteLine("Get all games was called");
            return Games;
        }

        [HttpGet("{name}")]
        public GameModel Get(string name)
        {
            Console.WriteLine($"Get game was called for name '${name}'");
            return Games.FirstOrDefault(model => model.Name == name);
        }

        [HttpPut]
        public IActionResult Put([FromBody]GameModel game)
        {
            Console.WriteLine($"Put game was called with game name '${game.Name}'");
            var existingGame = Games.FirstOrDefault(model => model.Name == game.Name);
            if (existingGame != null)
            {
                if (existingGame.Winner != null && game.Winner != null)
                {
                    return BadRequest();
                }
                Games.Remove(existingGame);
            }
            Games.Add(game);

            _hubContext.Clients.All.InvokeAsync("Send", game);

            return Ok();
        }
    }
}
