using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Proyecto.Hubs
{
    public class ColaC : Hub
    {
        public async Task SendQueueUpdate()
        {
            await Clients.All.SendAsync("ReceiveQueueUpdate");
        }
    }
}
