using MassTransit;
using MassTransitIntro.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace MassTransitIntro.Publisher.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBus _bus;

        public IndexModel(IBus bus)
        {
            _bus = bus;
        }

        [BindProperty] public string Title { get; set; }

        [BindProperty] public string Creator { get; set; }

        [BindProperty] public string Participants { get; set; }

        [BindProperty] public DateTimeOffset StartsOn { get; set; }

        [BindProperty] public DateTimeOffset EndsOn { get; set; }

        [TempData] public bool? MessageSent { get; set; }

        public void OnGet()
        {
            var now = DateTimeOffset.Now;
            Title ??= "Meeting";
            Creator ??= "Angelo";
            Participants ??= "marco@example.com,giulio@example.com,emanuele@example.com";
            StartsOn = now - new TimeSpan(0, 0, 0, now.Second, now.Millisecond);
            EndsOn = StartsOn.AddMinutes(30);
        }

        public async Task<IActionResult> OnPostCreateMeetingAsync()
        {

            var message = new MeetingCreatedMessage(Title, Creator, Participants?.Split(','), StartsOn, EndsOn);
            await SendMessage(message);
            return Page();
        }

        private async Task SendMessage(MeetingCreatedMessage message)
        {
            try
            {
                await _bus.Publish(message);
                MessageSent = true;
            }
            catch
            {
                MessageSent = false;
            }
        }
    }
}