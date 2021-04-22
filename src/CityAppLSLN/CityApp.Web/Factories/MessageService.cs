using System;
using System.Threading.Tasks;
using CityApp.Web.Interfaces;
using CityApp.Web.Models;
using CityApp.Web.Settings;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Factories
{
    public class MessageService : IMessageService
    {
        private readonly DaprClient daprClient;
        private readonly ILogger<MessageService> logger;
        private readonly string stateStore;

        public MessageService(DaprClient daprClient,
            ILogger<MessageService> logger,
            IOptions<WebSettings> webSettingsValue)
        {
            this.daprClient = daprClient;
            this.logger = logger;
            stateStore = webSettingsValue.Value.StateStore;
        }

        public async Task<bool> SetMessageAsync(MessageModel model)
        {
            logger.LogInformation($"Setting message for user {model.CityUserId}");
            var currentModel = await GetMessagesAsync(model.CityUserId)
                               ?? new MessageModel {CityUserId = model.CityUserId, Messages = model.Messages};
            try
            {
                logger.LogInformation("Saving state {data");
                await daprClient.SaveStateAsync(stateStore, model.CityUserId.ToString(), currentModel);
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
            try
            {
                logger.LogInformation($"Getting data for specific {cityUserId}");
                var messageModel = await daprClient.GetStateAsync<MessageModel>(stateStore, cityUserId.ToString());
                if (messageModel == null)
                {
                    logger.LogInformation("No data was found in the system");
                    return null;
                }

                logger.LogInformation($"Data received: {messageModel.Messages.Count} messages for user {messageModel.CityUserId}");
                return messageModel;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return null;
            }
        }
    }
}