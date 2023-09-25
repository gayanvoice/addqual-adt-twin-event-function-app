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
using AddQualADTTwinEventFunctionApp.Root;
using Azure;

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
                    BasicDigitalTwin urCobotBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URCobot", digitalTwinsClient: digitalTwinsClient);
                    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                    URCobotTwinModel existingURCobotTwinModel = URCobotTwinModel.GetFromBasicDigitalTwin(urCobotBasicDigitalTwin);
                    URCobotTwinModel newURCobotTwinModel = URCobotTwinModel.GetFromExistingDigitalTwin(urCobotModel: urCobotModel);
                    if (existingURCobotTwinModel.Equals(newURCobotTwinModel))
                    {
                        log.LogInformation("URCobot Twin Is Not Updated!");
                    }
                    else
                    {
                        log.LogInformation("URCobot Twin Is Updated!");
                        JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                        azureJsonPatchDocument.AppendAdd("/ActualQJointPosition", newURCobotTwinModel.ActualQJointPosition);
                        azureJsonPatchDocument.AppendAdd("/IsMoving", newURCobotTwinModel.IsMoving);
                        await digitalTwinsClient.UpdateDigitalTwinAsync("URCobot", azureJsonPatchDocument);
                    }

                }
                else if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    BasicDigitalTwin urGripperBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URGripper", digitalTwinsClient: digitalTwinsClient);
                    URGripperModel urGripperModel = JsonConvert.DeserializeObject<URGripperModel>(eventGridEvent.Data.ToString());
                    URGripperTwinModel existingURGripperTwinModel = URGripperTwinModel.GetFromBasicDigitalTwin(urGripperBasicDigitalTwin);
                    URGripperTwinModel newURGripperTwinModel = URGripperTwinModel.GetFromExistingDigitalTwin(
                        urGripperModel: urGripperModel, existingURGripperTwinModel: existingURGripperTwinModel);
                    if (existingURGripperTwinModel.Equals(newURGripperTwinModel))
                    {
                        log.LogInformation("URGripper Twin Is Not Updated!");
                    }
                    else
                    {
                        log.LogInformation("URGripper Twin Is Updated!");
                        JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                        azureJsonPatchDocument.AppendAdd("/IsOpen", newURGripperTwinModel.IsOpen);
                        azureJsonPatchDocument.AppendAdd("/Position", newURGripperTwinModel.Position);
                        await digitalTwinsClient.UpdateDigitalTwinAsync("URGripper", azureJsonPatchDocument);
                    }
                    
                }
            }
        }
        private static async Task<BasicDigitalTwin> GetBasicDigitalTwinAsync(
          string twinId, DigitalTwinsClient digitalTwinsClient)
        {
            Response<BasicDigitalTwin> twinResponse = await digitalTwinsClient.GetDigitalTwinAsync<BasicDigitalTwin>(twinId);
            return twinResponse.Value;
        }
    }
}