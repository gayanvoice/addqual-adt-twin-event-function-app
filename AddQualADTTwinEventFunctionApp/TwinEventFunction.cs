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
using System.Security.Cryptography;
using System.Text;

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
            CosmosClient cosmosClient = new CosmosClient(accountEndpoint: cosmosUri, authKeyOrResourceToken: cosmosKey);
            Database cobotDatabase = await cosmosClient.CreateDatabaseIfNotExistsAsync(id: "AddQualCobotTelemetryDatabase");

            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(eventGridEvent.Data.ToString());
                log.LogInformation(jObject["dataschema"].ToString());
                if (jObject["dataschema"].ToString().Equals("dtmi:com:AddQual:Factory:ScanBox:Cobot:URCobot;1"))
                {
                    URCobotModel urCobotModel = JsonConvert.DeserializeObject<URCobotModel>(eventGridEvent.Data.ToString());
                    log.LogInformation(JsonConvert.SerializeObject(urCobotModel));
                    DateTime dateTime = DateTime.Now;
                    URCobotRecord urCobotRecord = new(
                            id: MD5Encryption(dateTime.ToString()),
                            timestamp: dateTime.ToString("yyyyMMddHHmmssffff"),
                            target_q: urCobotModel.data.TargetQ,
                            target_qd: urCobotModel.data.TargetQd);
                    Container cobotContainer = cobotDatabase.GetContainer(id: "cobotContainer");
                    URCobotRecord cobotRecordItem = await cobotContainer.CreateItemAsync<URCobotRecord>(
                        item: urCobotRecord,
                        partitionKey: new PartitionKey("Cobot"));
                    log.LogInformation(JsonConvert.SerializeObject(cobotRecordItem, Formatting.Indented));

                }
            }
        }
        public static string MD5Encryption(string encryptionText)
        {

            MD5 md5 = MD5.Create();
            byte[] array = Encoding.UTF8.GetBytes(encryptionText);
            array = md5.ComputeHash(array);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in array)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}