using Azure.DigitalTwins.Core;
using Newtonsoft.Json;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URCobotTwinModel
    {
        public bool IsInvoked { get; set; }
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
                        if (property.Equals("IsInvoked"))
                        {
                            if (value is not null)
                            {
                                string? stringValue = value.ToString();
                                if (stringValue is not null)
                                {
                                    bool booleanValue = bool.Parse(stringValue);
                                    if (property.Equals("IsInvoked")) urCobotTwinModel.IsInvoked = booleanValue;
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
            if (actualQJointPositionModel.Equals(targetQJointPositionModel)) urCobotTwinModel.IsInvoked = false;
            else urCobotTwinModel.IsInvoked = false;
            return urCobotTwinModel;
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || GetType() != obj.GetType())
        //        return false;

        //    URCobotTwinModel other = (URCobotTwinModel)obj;

        //    return IsMoving == other.IsMoving
        //        && ActualQJointPosition.Base == other.ActualQJointPosition.Base
        //        && ActualQJointPosition.Shoulder == other.ActualQJointPosition.Shoulder
        //        && ActualQJointPosition.Elbow == other.ActualQJointPosition.Elbow
        //        && ActualQJointPosition.Wrist1 == other.ActualQJointPosition.Wrist1
        //        && ActualQJointPosition.Wrist2 == other.ActualQJointPosition.Wrist2
        //        && ActualQJointPosition.Wrist3 == other.ActualQJointPosition.Wrist3;
        //}

        //public override int GetHashCode()
        //{
        //    return (IsMoving, ActualQJointPosition).GetHashCode();
        //}
    }
}
