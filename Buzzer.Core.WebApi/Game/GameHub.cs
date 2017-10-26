using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Buzzer.Core.WebApi.Game
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }
        public Task Send(GameModel game)
        {
            _logger.LogDebug($"Notifying all clients with game {game.Name}.");
            return Clients.All.InvokeAsync(game.Name, game);
        }
    }
}
