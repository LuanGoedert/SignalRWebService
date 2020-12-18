using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRClr.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HubConnection = Microsoft.AspNetCore.SignalR.Client.HubConnection;

namespace SignalRClr.Server.Services
{
    public class MessageService
    {
        public HubConnection _hubConnection;
        public List<Message> _messages = new List<Message>();
        public Message _message = new Message();

        public async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder().Build();

            _hubConnection.On<Message>("ReceiveMessage",
                (message) => {
                    _messages.Add(message);
                });
            await _hubConnection.StartAsync();
        }

        public async Task SendMessage()
        {
            await _hubConnection.SendAsync("SendMessage", _message);
            _message = new Message();
        }

        public void Dispose()
        {
            _ = _hubConnection?.DisposeAsync();

        }
    }
}
