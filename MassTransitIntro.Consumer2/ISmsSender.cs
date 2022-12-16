using System.Threading.Tasks;

namespace MassTransitIntro.Consumer2
{
    public interface ISmsSender
    {
        Task SendAsync(Sms sms);
    }
}
