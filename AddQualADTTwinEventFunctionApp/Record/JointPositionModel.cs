using AddQualADTTwinEventFunctionApp.Model;
using System;
using System.Runtime.CompilerServices;

namespace AddQualADTTwinEventFunctionApp.Root
{
    public class JointPositionModel
    {
        public double Base { get; set; }
        public double Shoulder { get; set; }
        public double Elbow { get; set; }
        public double Wrist1 { get; set; }
        public double Wrist2 { get; set; }
        public double Wrist3 { get; set; }

        public static JointPositionModel GetDegrees(URCobotModel urCobotModel)
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
        private static double convertToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
    }
}