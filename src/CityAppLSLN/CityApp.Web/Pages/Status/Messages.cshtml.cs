using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Web.Interfaces;
using CityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Status
{
    [Authorize]
    public class MessagesPageModel : PageModel
    {
        private readonly ILogger<MessagesPageModel> logger;
        private readonly IMessageService messageService;
        private readonly IUserDataContext userDataContext;

        public MessagesPageModel(ILogger<MessagesPageModel> logger, IMessageService messageService, IUserDataContext userDataContext)
        {
            this.logger = logger;
            this.messageService = messageService;
            this.userDataContext = userDataContext;
        }

        public async Task OnGetAsync()
        {
            logger.LogInformation("Messages page loaded");
            MessageModel = await messageService.GetMessagesAsync(userDataContext.GetCurrentUser().CityUserId);
            logger.LogInformation($"Loaded messages {MessageModel.Messages.Count}");
        }

        [BindProperty]
        public MessageModel MessageModel { get; set; }
    }
}