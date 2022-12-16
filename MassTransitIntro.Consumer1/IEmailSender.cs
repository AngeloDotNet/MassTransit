using System.Threading.Tasks;

namespace MassTransitIntro.Consumer1
{
    public interface IEmailSender
    {
        Task SendAsync(Email email);
    }
}
