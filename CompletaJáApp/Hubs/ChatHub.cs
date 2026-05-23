using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CompletaJaApp.Hubs
{
    public class ChatHub : Hub
    {
        // Agora recebemos o remetenteId diretamente da tela (JavaScript)
        public async Task EnviarMensagem(string remetenteId, string usuarioIdDestino, string mensagem)
        {
            // Repassamos o remetenteId exato para todo o mundo
            await Clients.All.SendAsync("ReceberMensagem", remetenteId, mensagem);
        }
    }
}