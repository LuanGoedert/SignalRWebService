using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRClr.Server.Services
{
    public class MessageService
    {
        private HubConnection _hubConnection;
        private List<Message> _messages = new List<Message>();
        private Message _message = new Message();

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/messageHub"))
                .Build();

            _hubConnection.On<Message>("ReceiveMessage",
                (message) => {
                    _messages.Add(message);
                    StateHasChanged();
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
