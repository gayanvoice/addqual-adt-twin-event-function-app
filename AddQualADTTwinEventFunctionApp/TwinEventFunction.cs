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
                    azureJsonPatchDocument.AppendAdd("/IsActive", urGripperModel.data.ACT);

                    if (urGripperModel.data.ACT == 1) azureJsonPatchDocument.AppendAdd("/IsActive", true);
                    else azureJsonPatchDocument.AppendAdd("/IsActive", false);

                    if (urGripperModel.data.POS < 10) azureJsonPatchDocument.AppendAdd("/IsOpen", true);
                    else azureJsonPatchDocument.AppendAdd("/IsOpen", false);

                    azureJsonPatchDocument.AppendAdd("/IsInvoked", false);

                    await digitalTwinsClient.UpdateDigitalTwinAsync("URGripper", azureJsonPatchDocument);
                }
            }
        }
    }
}

//[
//  {
//    "op": "add",
//    "path": "/CobotJointPosition",
//    "value": {
//      "Base": 1,
//      "Shoulder": 2,
//      "Elbow": 3,
//      "Wrist1": 4,
//      "Wrist2": 5,
//      "Wrist3": 6
//    }
//  },
//  {
//    "op": "add",
//    "path": "/IsFreeDriveModeEnabled",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsInvoked",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsPaused",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsPlay",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsPowerOn",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsProtectiveStopUnlocked",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsSafetyPopupClosed",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/IsTeachModeEnabled",
//    "value": true
//  },
//  {
//    "op": "add",
//    "path": "/PopupText",
//    "value": "454"
//  }
//]