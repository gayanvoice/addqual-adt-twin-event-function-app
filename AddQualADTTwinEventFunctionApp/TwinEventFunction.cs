// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Newtonsoft.Json;
using AddQualADTTwinEventFunctionApp.Model;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AddQualADTTwinEventFunctionApp
{
    public static class TwinEventFunction
    {
        private static readonly string ADT_SERVICE_URL = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        [FunctionName("TwinEventFunction")]
        public static async Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(ADT_SERVICE_URL), credential: defaultAzureCredential);

            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(eventGridEvent.Data.ToString());
                log.LogInformation(jObject["dataschema"].ToString());

                if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                {
                    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                    Azure.JsonPatchDocument azureJsonPatchDocument = new Azure.JsonPatchDocument();
                    log.LogInformation(urCobotModel.data.ActualQ.ToArray().ToString());

                }
                else if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    URGripperModel urGripperModel = JsonConvert.DeserializeObject<URGripperModel>(eventGridEvent.Data.ToString());
                    Azure.JsonPatchDocument azureJsonPatchDocument = new Azure.JsonPatchDocument();
                    log.LogInformation(urGripperModel.data.ACT.ToString());
                    log.LogInformation(urGripperModel.data.POS.ToString());
                    //    azureJsonPatchDocument.AppendReplace("/IsActive", urGripperModel.data.ACT);
                    //    if (urGripperModel.data.POS < 10) azureJsonPatchDocument.AppendReplace("/IsOpen", true);
                    //    else azureJsonPatchDocument.AppendReplace("/IsOpen", false);
                    //    azureJsonPatchDocument.AppendReplace("/IsInvoked", false);
                    //    await digitalTwinsClient.UpdateDigitalTwinAsync("URGripper", azureJsonPatchDocument);
                }
            }
        }
    }
}
