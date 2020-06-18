using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class QnABot : ActivityHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public QnABot(IConfiguration configuration, IHttpClientFactory httpClientFactory) {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            var httpClient = _httpClientFactory.CreateClient();

            var qnaMaker = new QnAMaker(new QnAMakerEndpoint {
                KnowledgeBaseId = _configuration["QnAKnowledgebaseId"],
                EndpointKey = _configuration["QnAEndpointKey"],
                Host = _configuration["QnAEndpointHostName"]
            },null,httpClient);

            var options = new QnAMakerOptions { Top = 1 };

            var response = await qnaMaker.GetAnswersAsync(turnContext, options);
            if (response != null && response.Length > 0) {
                await turnContext.SendActivityAsync(MessageFactory.Text(response[0].Answer), cancellationToken);
            } else {
                await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
            }
        }
    }
}
