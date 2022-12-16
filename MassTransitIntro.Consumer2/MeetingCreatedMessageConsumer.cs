using MassTransit;
using MassTransitIntro.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransitIntro.Consumer2
{
    public class MeetingCreatedMessageConsumer : IConsumer<MeetingCreatedMessage>
    {
        private const string MeetingAppPhoneNumber = "1234567890";
        private static readonly Random Generator = new();

        private readonly ISmsSender _smsSender;

        public MeetingCreatedMessageConsumer(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public async Task Consume(ConsumeContext<MeetingCreatedMessage> context)
        {
            var meeting = context.Message;

            if (IsMeetingAlreadyEnded(meeting))
            {
                return;
            }

            var smsList = CreateSmsList(meeting);
            await SendSmsList(smsList);
        }

        private static IEnumerable<Sms> CreateSmsList(MeetingCreatedMessage meeting)
        {
            return meeting.Participants.Select(_ => new Sms(
                MeetingAppPhoneNumber,
                GenerateRandomPhoneNumber(),
                $"{meeting.Creator} invited you to the meeting {meeting.Name} that will start on {meeting.StartsOn:g} and will end on {meeting.EndsOn:g}.")
            ).ToList();
        }

        private static string GenerateRandomPhoneNumber() =>
            Enumerable.Range(0, 10).Aggregate(string.Empty, (accumulator, _) => accumulator = string.Concat(accumulator, Generator.Next(10)));

        private async Task SendSmsList(IEnumerable<Sms> smsList)
        {
            foreach (var sms in smsList)
            {
                await _smsSender.SendAsync(sms);
            }
        }

        private static bool IsMeetingAlreadyEnded(MeetingCreatedMessage meeting) =>
            DateTimeOffset.UtcNow >= meeting.EndsOn;
    }
}