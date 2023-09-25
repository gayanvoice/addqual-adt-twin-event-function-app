using Azure.DigitalTwins.Core;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URGripperTwinModel
    {
        public bool IsOpen{ get; set; }
        public bool IsInvoked{ get; set; }
        public static URGripperTwinModel GetFromBasicDigitalTwin(BasicDigitalTwin basicDigitalTwin)
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
                            bool boolValue = bool.Parse(stringValue);
                            if (property.Equals("IsInvoked")) urGripperTwinModel.IsInvoked = boolValue;
                            if (property.Equals("IsOpen")) urGripperTwinModel.IsOpen = boolValue;

                        }
                    }
                }
            }
            return urGripperTwinModel;
        }
        public static URGripperTwinModel GetFromExistingDigitalTwin(URGripperModel urGripperModel)
        {
            URGripperTwinModel urGripperTwinModel = new URGripperTwinModel();
            if (urGripperModel.data.POS < 10) urGripperTwinModel.IsOpen = true;
            else urGripperTwinModel.IsOpen = false;
            urGripperTwinModel.IsInvoked = false;
            return urGripperTwinModel;
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || GetType() != obj.GetType())
        //        return false;

        //    URGripperTwinModel other = (URGripperTwinModel) obj;
        //    return IsOpen == other.IsOpen && Position == other.Position;
        //}

        //public override int GetHashCode()
        //{
        //    return (IsOpen, Position).GetHashCode();
        //}
    }
}