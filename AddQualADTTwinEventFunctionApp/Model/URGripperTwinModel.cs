using Azure.DigitalTwins.Core;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URGripperTwinModel
    {
        public int Position { get; set; }
        public bool IsOpen{ get; set; }
        public static URGripperTwinModel Get(BasicDigitalTwin basicDigitalTwin)
        {
            URGripperTwinModel urGripperTwinModel = new URGripperTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        string stringValue = value.ToString();
                        if (stringValue is not null)
                        {
                            if (property.Equals("Position")) urGripperTwinModel.Position = int.Parse(stringValue);
                            if (property.Equals("IsOpen")) urGripperTwinModel.IsOpen = bool.Parse(stringValue);

                        }
                    }
                }
            }
            return urGripperTwinModel;
        }
    }
}