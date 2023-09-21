using System.Collections.Generic;

namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URCobotModel
    {
        public DataModel data { get; set; }
        public string dataschema { get; set; }
        public string contenttype { get; set; }
        public string traceparent { get; set; }
        public class DataModel
        {
            public List<double> TargetQ { get; set; }
            public List<double> TargetQd { get; set; }
            public List<double> TargetQdd { get; set; }
            public List<double> TargetCurrent { get; set; }
            public List<double> TargetMoment { get; set; }
            public List<double> ActualCurrent { get; set; }
            public List<double> ActualQ { get; set; }
            public List<double> ActualQd { get; set; }
            public List<double> JointControlOutput { get; set; }
            public List<double> ActualTCPForce { get; set; }
            public List<double> JointTemperatures { get; set; }
            public List<int> JointMode { get; set; }
            public List<double> ToolAccelerometer { get; set; }
            public double SpeedScaling { get; set; }
            public double ActualMomentum { get; set; }
            public double ActualMainVoltage { get; set; }
            public double ActualRobotVoltage { get; set; }
            public double ActualRobotCurrent { get; set; }
            public List<double> ActualJointVoltage { get; set; }
            public int RuntimeState { get; set; }
            public int RobotMode { get; set; }
            public int SafetyMode { get; set; }
            public int AnalogIoTypes { get; set; }
            public double IoCurrent { get; set; }
            public int ToolMode { get; set; }
            public int ToolOutputVoltage { get; set; }
            public double ToolOutputCurrent { get; set; }
        }
    }
}