using Azure.DigitalTwins.Core;
using Newtonsoft.Json;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URCobotTwinModel
    {
        public bool IsMoving { get; set; }
        public JointPositionModel? ActualQJointPosition { get; set; }
        public static URCobotTwinModel GetFromBasicDigitalTwin(BasicDigitalTwin basicDigitalTwin)
        {
            URCobotTwinModel urCobotTwinModel = new URCobotTwinModel();
            foreach (string property in basicDigitalTwin.Contents.Keys)
            {
                if (basicDigitalTwin.Contents.TryGetValue(property, out object value))
                {
                    if (value is not null)
                    {
                        if (property.Equals("ActualQJointPosition"))
                        {
                            JointPositionModel actualQJointPositionModel = JsonConvert.DeserializeObject<JointPositionModel>(value.ToString());
                            if (actualQJointPositionModel != null) urCobotTwinModel.ActualQJointPosition = actualQJointPositionModel;
                        }
                        if (property.Equals("IsMoving"))
                        {
                            if (value is not null)
                            {
                                string? stringValue = value.ToString();
                                if (stringValue is not null)
                                {
                                    bool booleanValue = bool.Parse(stringValue);
                                    if (property.Equals("IsMoving")) urCobotTwinModel.IsMoving = booleanValue;
                                }
                            }
                        }
                    }
                }
            }
            return urCobotTwinModel;
        }
        public static URCobotTwinModel GetFromExistingDigitalTwin(URCobotModel urCobotModel)
        {
            JointPositionModel actualQJointPositionModel = JointPositionModel.GetDegreesOfActualQ(urCobotModel);
            JointPositionModel targetQJointPositionModel = JointPositionModel.GetDegreesOfTargetQ(urCobotModel);
            URCobotTwinModel urCobotTwinModel = new URCobotTwinModel();
            urCobotTwinModel.ActualQJointPosition = actualQJointPositionModel;
            if (actualQJointPositionModel.Equals(targetQJointPositionModel)) urCobotTwinModel.IsMoving = false;
            else urCobotTwinModel.IsMoving = true;
            return urCobotTwinModel;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            URCobotTwinModel other = (URCobotTwinModel)obj;

            return IsMoving == other.IsMoving
                && ActualQJointPosition == other.ActualQJointPosition;
        }

        public override int GetHashCode()
        {
            return (IsMoving, ActualQJointPosition).GetHashCode();
        }
    }
}
