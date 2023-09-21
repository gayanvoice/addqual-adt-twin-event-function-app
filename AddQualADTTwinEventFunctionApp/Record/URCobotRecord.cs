namespace AddQualADTTwinEventFunctionApp.Root
{
    public record URCobotRecord
    (
        bool IsPaused,
        bool IsPlay,
        bool IsSafetyPopupClosed,
        bool IsPowerOn,
        bool IsFreeDriveModeEnabled,
        bool IsTeachModeEnabled,
        bool IsInvoked,
        string PopupText
    );
}