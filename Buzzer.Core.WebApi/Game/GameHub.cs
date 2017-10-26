using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Buzzer.Core.WebApi.Game
{
    public class GameHub : Hub
    {
        public Task Send(GameModel game)
        {
            Console.WriteLine("Broadcasting a game!");
            return Clients.All.InvokeAsync("Send", game);
        }
    }
}
