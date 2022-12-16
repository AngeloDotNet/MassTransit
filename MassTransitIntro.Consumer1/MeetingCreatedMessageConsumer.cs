using MassTransit;
using MassTransitIntro.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransitIntro.Consumer1
{
    public class MeetingCreatedMessageConsumer : IConsumer<MeetingCreatedMessage>
    {
        private const string MeetingAppEmail = "meeting-app@example.com";
        private readonly IEmailSender _emailSender;

        public MeetingCreatedMessageConsumer(IEmailSender mailSender)
        {
            _emailSender = mailSender;
        }

        public async Task Consume(ConsumeContext<MeetingCreatedMessage> context)
        {
            var meeting = context.Message;

            if (IsMeetingAlreadyEnded(meeting))
            {
                return;
            }

            var emails = CreateEmails(meeting);
            await SendEmails(emails);
        }

        private static IEnumerable<Email> CreateEmails(MeetingCreatedMessage meeting)
        {
            return meeting.Participants.Select(participant => new Email(
                MeetingAppEmail,
                new List<string> { participant },
                $"{meeting.Creator} invited you to a new meeting.",
                $"{meeting.Creator} invited you to the meeting {meeting.Name} that will start on {meeting.StartsOn:g} and will end on {meeting.EndsOn:g}."
            )).ToList();
        }

        private async Task SendEmails(IEnumerable<Email> emails)
        {
            foreach (var email in emails)
            {
                await _emailSender.SendAsync(email);
            }
        }

        private static bool IsMeetingAlreadyEnded(MeetingCreatedMessage meeting) =>
            DateTimeOffset.UtcNow >= meeting.EndsOn;
    }
}