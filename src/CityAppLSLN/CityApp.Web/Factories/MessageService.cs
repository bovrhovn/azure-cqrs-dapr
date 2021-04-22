using System;
using System.Net.Http;
using System.Threading.Tasks;
using CityApp.Web.Interfaces;
using CityApp.Web.Models;
using CityApp.Web.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CityApp.Web.Factories
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient client;
        private readonly ILogger<MessageService> logger;
        private string stateStore;

        public MessageService(HttpClient client, ILogger<MessageService> logger, IOptions<WebSettings> webSettingsValue)
        {
            this.client = client;
            this.logger = logger;
            stateStore = webSettingsValue.Value.StateStore;
            client.BaseAddress = new Uri(webSettingsValue.Value.StateManagementUrl, UriKind.RelativeOrAbsolute);
        }

        public async Task<bool> SetMessageAsync(MessageModel model)
        {
            logger.LogInformation($"Setting message for user {model.CityUserId}");
            var currentModel = await GetMessagesAsync(model.CityUserId)
                               ?? new MessageModel {CityUserId = model.CityUserId, Messages = model.Messages};
            try
            {
                var data = JsonConvert.SerializeObject(currentModel);
                logger.LogInformation("Saving state {data");
                await client.PostAsync(stateStore, new StringContent(data));
                logger.LogInformation("State succefully stored");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public async Task<MessageModel> GetMessagesAsync(int cityUserId)
        {
            logger.LogInformation($"Retrieving message for user {cityUserId}");
            var url = $"{stateStore}/{cityUserId}";
            try
            {
                var responseMessage = await client.GetStringAsync(url);
                logger.LogInformation($"Message received {responseMessage}");
                if (string.IsNullOrEmpty(responseMessage))
                {
                    logger.LogInformation("No data has been returned");
                    return null;
                }
                var result = JsonConvert.DeserializeObject<MessageModel>(responseMessage);
                return result;
            }
            catch (Exception e)
            {
                logger.LogInformation("There has been error " + e.Message);
                return null;
            }
        }
    }
}