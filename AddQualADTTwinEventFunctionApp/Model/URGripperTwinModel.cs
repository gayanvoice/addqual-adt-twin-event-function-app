using Azure.DigitalTwins.Core;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URGripperTwinModel
    {
        public int Position { get; set; }
        public bool IsOpen{ get; set; }
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
                            if (property.Equals("Position")) urGripperTwinModel.Position = int.Parse(stringValue);
                            if (property.Equals("IsOpen")) urGripperTwinModel.IsOpen = bool.Parse(stringValue);

                        }
                    }
                }
            }
            return urGripperTwinModel;
        }
        public static URGripperTwinModel GetFromExistingDigitalTwin(
            URGripperModel urGripperModel, 
            URGripperTwinModel existingURGripperTwinModel)
        {
            URGripperTwinModel urGripperTwinModel = new URGripperTwinModel();
            if (existingURGripperTwinModel.Position == urGripperModel.data.POS)
            {
                if (urGripperModel.data.POS < 10) urGripperTwinModel.IsOpen = true;
                else urGripperTwinModel.IsOpen = false;
            }
            urGripperTwinModel.Position = urGripperModel.data.POS;
            return urGripperTwinModel;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            URGripperTwinModel other = (URGripperTwinModel) obj;
            return IsOpen == other.IsOpen && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return (IsOpen, Position).GetHashCode();
        }
    }
}