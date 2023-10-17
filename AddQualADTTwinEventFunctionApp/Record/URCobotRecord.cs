using System.Collections.Generic;

namespace AddQualADTTwinEventFunctionApp.Root
{
    public record URCobotRecord
    (
        List<double> TargetQ,
    List<double> TargetQd,
    List<double> TargetQdd,
    List<double> TargetCurrent,
    List<double> TargetMoment,
    List<double> ActualCurrent,
    List<double> ActualQ,
    List<double> ActualQd,
    List<double> JointControlOutput,
    List<double> ActualTCPForce,
    List<double> JointTemperatures,
    List<int> JointMode,
    List<double> ToolAccelerometer,
    double SpeedScaling,
    double ActualMomentum,
    double ActualMainVoltage,
    double ActualRobotVoltage,
    double ActualRobotCurrent,
    List<double> ActualJointVoltage,
    int RuntimeState,
    int RobotMode,
    int SafetyMode,
    int AnalogIoTypes,
    double IoCurrent,
    int ToolMode,
    int ToolOutputVoltage,
    double ToolOutputCurrent
    );
}