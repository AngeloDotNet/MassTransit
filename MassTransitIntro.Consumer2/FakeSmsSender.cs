using System;
using System.Threading.Tasks;

namespace MassTransitIntro.Consumer2
{
    public class FakeSmsSender : ISmsSender
    {
        public Task SendAsync(Sms sms)
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine($"Sent SMS from {sms.SenderPhoneNumber} to {sms.ReceiverPhoneNumber}.");
            Console.WriteLine($"Content -> {sms.Content}");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            return Task.CompletedTask;
        }
    }
}
