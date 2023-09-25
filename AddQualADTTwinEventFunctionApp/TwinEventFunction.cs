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
                    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                    JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                    JointPositionModel actualQJointPositionModel = JointPositionModel.GetDegreesOfActualQ(urCobotModel);
                    JointPositionModel targetQJointPositionModel = JointPositionModel.GetDegreesOfTargetQ(urCobotModel);
                    azureJsonPatchDocument.AppendAdd("/ActualQJointPosition", actualQJointPositionModel);
                    if (actualQJointPositionModel.Equals(targetQJointPositionModel)) azureJsonPatchDocument.AppendAdd("/IsMoving", false);
                    else azureJsonPatchDocument.AppendAdd("/IsMoving", true);
                    await digitalTwinsClient.UpdateDigitalTwinAsync("URCobot", azureJsonPatchDocument);
                }
                else if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URGripper;1"))
                {
                    BasicDigitalTwin urGripperBasicDigitalTwin = await GetBasicDigitalTwinAsync(twinId: "URGripper", digitalTwinsClient: digitalTwinsClient);
                    URGripperTwinModel urGripperTwinModel = URGripperTwinModel.Get(urGripperBasicDigitalTwin);
                    URGripperModel urGripperModel = JsonConvert.DeserializeObject<URGripperModel>(eventGridEvent.Data.ToString());
                    JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                    if (urGripperTwinModel.Position == urGripperModel.data.POS)
                    {
                        if (urGripperModel.data.POS < 10) azureJsonPatchDocument.AppendAdd("/IsOpen", true);
                        else azureJsonPatchDocument.AppendAdd("/IsOpen", false);
                    }
                    azureJsonPatchDocument.AppendAdd("/Position", urGripperModel.data.POS);
                    await digitalTwinsClient.UpdateDigitalTwinAsync("URGripper", azureJsonPatchDocument);
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