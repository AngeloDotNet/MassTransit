using System;
using System.Collections.Generic;

namespace MassTransitIntro.Messages
{
    public record MeetingCreatedMessage(string Name, string Creator, IEnumerable<string> Participants, DateTimeOffset StartsOn, DateTimeOffset EndsOn);
}
