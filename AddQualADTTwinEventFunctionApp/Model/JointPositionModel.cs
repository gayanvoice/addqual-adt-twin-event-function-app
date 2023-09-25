using System;

namespace AddQualADTTwinEventFunctionApp.Model

{
    public class JointPositionModel
    {
        public double Base { get; set; }
        public double Shoulder { get; set; }
        public double Elbow { get; set; }
        public double Wrist1 { get; set; }
        public double Wrist2 { get; set; }
        public double Wrist3 { get; set; }

        public static JointPositionModel GetDegreesOfActualQ(URCobotModel urCobotModel)
        {
            JointPositionModel jointPositionModel = new JointPositionModel();
            jointPositionModel.Base = convertToDegrees(urCobotModel.data.ActualQ[0]);
            jointPositionModel.Shoulder = convertToDegrees(urCobotModel.data.ActualQ[1]);
            jointPositionModel.Elbow = convertToDegrees(urCobotModel.data.ActualQ[2]);
            jointPositionModel.Wrist1 = convertToDegrees(urCobotModel.data.ActualQ[3]);
            jointPositionModel.Wrist2 = convertToDegrees(urCobotModel.data.ActualQ[4]);
            jointPositionModel.Wrist3 = convertToDegrees(urCobotModel.data.ActualQ[5]);
            return jointPositionModel;
        }
        public static JointPositionModel GetDegreesOfTargetQ(URCobotModel urCobotModel)
        {
            JointPositionModel jointPositionModel = new JointPositionModel();
            jointPositionModel.Base = convertToDegrees(urCobotModel.data.TargetQ[0]);
            jointPositionModel.Shoulder = convertToDegrees(urCobotModel.data.TargetQ[1]);
            jointPositionModel.Elbow = convertToDegrees(urCobotModel.data.TargetQ[2]);
            jointPositionModel.Wrist1 = convertToDegrees(urCobotModel.data.TargetQ[3]);
            jointPositionModel.Wrist2 = convertToDegrees(urCobotModel.data.TargetQ[4]);
            jointPositionModel.Wrist3 = convertToDegrees(urCobotModel.data.TargetQ[5]);
            return jointPositionModel;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            JointPositionModel other = (JointPositionModel) obj;

            return Base == other.Base 
                && Shoulder == other.Shoulder 
                && Elbow == Elbow
                && Wrist1 == Wrist1
                && Wrist2 == Wrist2
                && Wrist3 == Wrist3;
        }

        public override int GetHashCode()
        {
            return (Base, Shoulder, Elbow, Wrist1, Wrist2, Wrist3).GetHashCode();
        }
        private static double convertToDegrees(double radians)
        {
            return Math.Round(radians * 180 / Math.PI, 2);
        }
    }
}