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
using Microsoft.Azure.Cosmos;

namespace AddQualADTTwinEventFunctionApp
{
    public static class TwinEventFunction
    {
        private static readonly string ADT_SERVICE_URL = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");
        private static readonly string cosmosUri = Environment.GetEnvironmentVariable("COSMOS_URI");
        private static readonly string cosmosKey = Environment.GetEnvironmentVariable("COSMOS_KEY");
        [FunctionName("TwinEventFunction")]
        public static async Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
            DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(endpoint: new Uri(ADT_SERVICE_URL), credential: defaultAzureCredential);
            //CosmosClient cosmosClient = new CosmosClient(accountEndpoint: cosmosUri, authKeyOrResourceToken: cosmosKey);
            //Database cobotDatabase = await cosmosClient.CreateDatabaseIfNotExistsAsync(id: "AddQualCobotTelemetryDatabase");

            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(eventGridEvent.Data.ToString());
                log.LogInformation(jObject["dataschema"].ToString());
                if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                {
                    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                    log.LogInformation(JsonConvert.SerializeObject(urCobotModel));
                    //CobotRecord cobotRecord = new(
                    //        id: EncryptionHelper.MD5Encryption(dateTime.ToString()),
                    //        deviceId: "Cobot",
                    //        timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                    //        elapsedTime: rootModel.Data.Patch.Find(patch => patch.Path.Contains("/ElapsedTime")).Value);
                    //Container cobotContainer = cobotDatabase.GetContainer(id: "cobotContainer");
                    //CobotRecord cobotRecordItem = await cobotContainer.CreateItemAsync<CobotRecord>(
                    //    item: cobotRecord,
                    //    partitionKey: new PartitionKey("Cobot"));
                    //log.LogInformation(JsonConvert.SerializeObject(cobotRecordItem, Formatting.Indented));

                }
            }
        }
    }
}