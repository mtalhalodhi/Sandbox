using Godot;

public static class Settings
{
    public static bool ShowDebugData => (bool)ProjectSettings.GetSetting("global/ShowDebugData");
}
