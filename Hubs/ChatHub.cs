using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace demochatsignal.Hubs
{
    public class ChatHub : Hub
    {

        // Almacena los usuarios conectados (ConnectionId -> Nombre de Usuario)
        private static ConcurrentDictionary<string, string> _connectedUsers = new();

        // Método para registrar al usuario
        public async Task RegisterUser(string userName)
        {
            // Almacenar el usuario en la lista
            _connectedUsers[Context.ConnectionId] = userName;

            // Notificar a todos los clientes que un usuario se unió
            await Clients.All.SendAsync("UserConnected", userName);
        }

        // Método para enviar un mensaje
        public async Task SendMessage(string message)
        {
            // Verificar si el usuario está registrado
            if (_connectedUsers.TryGetValue(Context.ConnectionId, out var userName))
            {
                // Enviar el mensaje a todos los clientes
                await Clients.All.SendAsync("ReceiveMessage", userName, message);
            }
        }

        // Método que se ejecuta cuando un usuario se desconecta
        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            // Eliminar al usuario de la lista
            if (_connectedUsers.TryRemove(Context.ConnectionId, out var userName))
            {
                // Notificar a todos los clientes que el usuario salió
                await Clients.All.SendAsync("UserDisconnected", userName);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
