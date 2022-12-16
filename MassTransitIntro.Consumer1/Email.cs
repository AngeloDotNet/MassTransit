using System.Collections.Generic;

namespace MassTransitIntro.Consumer1
{
    public record Email(string From, IEnumerable<string> To, string Subject, string Body);
}