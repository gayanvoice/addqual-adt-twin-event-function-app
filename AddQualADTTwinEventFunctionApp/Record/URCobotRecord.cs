using System.Collections.Generic;

namespace AddQualADTTwinEventFunctionApp.Root
{
    public record URCobotRecord
    (
        string id,
        string timestamp,
        List<double> target_q,
        List<double> target_qd,
        List<double> target_qdd,
        List<double> target_current,
        List<double> target_moment,
        List<double> actual_current,
        List<double> actual_q,
        List<double> actual_qd,
        List<double> joint_control_output,
        List<double> actual_tcp_force,
        List<double> joint_temperatures,
        List<int> joint_mode,
        List<double> tool_accelerometer,
        double speed_scaling,
        double actual_momentum,
        double actual_main_voltage,
        double actual_robot_voltage,
        double actual_robot_current,
        List<double> actual_joint_voltage,
        int runtime_state,
        int robot_mode,
        int safety_mode,
        int analog_io_types,
        double io_current,
        int tool_mode,
        int tool_output_voltage,
        double tool_output_current
    );
}