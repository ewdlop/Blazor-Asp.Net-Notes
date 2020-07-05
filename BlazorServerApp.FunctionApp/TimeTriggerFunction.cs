using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BlazorServerApp.FunctionApp
{
    public static class TimeTriggerFunction
    {
        [FunctionName("TimeTriggerFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log) {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
