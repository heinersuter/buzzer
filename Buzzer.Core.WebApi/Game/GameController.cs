using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Buzzer.Core.WebApi.Game
{
    [Route("api/games")]
    public class GameController : Controller
    {
        private static readonly List<GameModel> Games = new List<GameModel>();

        private readonly IHubContext<GameHub> _hubContext;
        private readonly ILogger<GameController> _logger;

        public GameController(IHubContext<GameHub> hubContext, ILogger<GameController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<GameModel> Get()
        {
            _logger.LogDebug("Get all games was called.");
            return Games;
        }

        [HttpGet("{gameName}")]
        public GameModel Get(string gameName)
        {
            _logger.LogDebug($"Get game was called for '${gameName}.'");
            return Games.FirstOrDefault(model => string.Equals(model.Name, gameName, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPut("{gameName}")]
        public IActionResult EnsureGameExists(string gameName)
        {
            var existingGame = Games.FirstOrDefault(model => string.Equals(model.Name, gameName, StringComparison.OrdinalIgnoreCase));
            if (existingGame == null)
            {
                var game = new GameModel(gameName);
                Games.Add(game);
                _logger.LogDebug($"{nameof(EnsureGameExists)}: Game created {gameName}.");
                Notify(game);
                return Ok(game);
            }
            _logger.LogDebug($"{nameof(EnsureGameExists)}: Game alredy existing {gameName}.");
            return Ok(existingGame);
        }

        [HttpPost("{gameName}/users/{userName}")]
        public IActionResult AddUser(string gameName, string userName)
        {
            var existingGame = Games.FirstOrDefault(model => string.Equals(model.Name, gameName, StringComparison.OrdinalIgnoreCase));
            if (existingGame == null)
            {
                _logger.LogError($"{nameof(AddUser)}: Game {gameName} does not exist. User {userName} cannot be added.");
                return NotFound($"Game {gameName} does not exist.");
            }
            if (existingGame.Users.Any(name => string.Equals(name, userName, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"{nameof(AddUser)}: User {userName} does already exist in game {gameName}.");
                return BadRequest($"User {userName} does already exist in game {gameName}.");
            }
            existingGame.Users.Add(userName);
            _logger.LogDebug($"{nameof(AddUser)}: User {userName} added to game {gameName}.");
            Notify(existingGame);
            return Ok(existingGame);
        }

        [HttpPost("{gameName}/winner/{userName}")]
        public IActionResult TryToWin(string gameName, string userName)
        {
            var existingGame = Games.FirstOrDefault(model => string.Equals(model.Name, gameName, StringComparison.OrdinalIgnoreCase));
            if (existingGame == null)
            {
                _logger.LogError($"{nameof(TryToWin)}: Game {gameName} does not exist. User {userName} cannot win.");
                return NotFound($"Game {gameName} does not exist.");
            }
            if (existingGame.Users.All(name => !string.Equals(name, userName, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"{nameof(TryToWin)}: Game {gameName} does not contain a user {userName}.");
                return NotFound($"User {userName} does not exist in game {gameName}.");
            }
            if (existingGame.Winner != null)
            {
                _logger.LogDebug($"{nameof(TryToWin)}: Game {gameName} has alredy a winner {existingGame.Winner}. User {userName} cannot win.");
                return Ok(existingGame);
            }
            existingGame.Winner = userName;
            _logger.LogDebug($"{nameof(TryToWin)}: User {userName} won the game {gameName}. Congratulation!");
            Notify(existingGame);
            return Ok(existingGame);
        }

        [HttpDelete("{gameName}/winner/{userName}")]
        public IActionResult Reset(string gameName, string userName)
        {
            var existingGame = Games.FirstOrDefault(model => string.Equals(model.Name, gameName, StringComparison.OrdinalIgnoreCase));
            if (existingGame == null)
            {
                _logger.LogError($"{nameof(Reset)}: Game {gameName} does not exist. User {userName} cannot reset the game.");
                return NotFound($"Game {gameName} does not exist.");
            }
            if (existingGame.Users.All(name => !string.Equals(name, userName, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"{nameof(Reset)}: Game {gameName} does not contain a user {userName}.");
                return NotFound($"User {userName} does not exist in game {gameName}.");
            }
            if (!string.Equals(existingGame.Winner, userName, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError($"{nameof(Reset)}: Game {gameName} does not contain a user {userName}.");
                return NotFound($"The game {gameName} has another winner: {existingGame.Winner}.");
            }
            existingGame.Winner = null;
            _logger.LogDebug($"{nameof(Reset)}: User {userName} resetted the {gameName}.");
            Notify(existingGame);
            return Ok(existingGame);
        }

        private void Notify(GameModel game)
        {
            _hubContext.Clients.All.InvokeAsync(game.Name, game);
        }
    }
}
