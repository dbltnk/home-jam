using UnityEngine;

public static class LegacyInput
{
    public static bool ReleaseTriggered => Input.GetKeyDown(KeyCode.E);
    public static bool JumpPressed => Input.GetKey(KeyCode.Space);
    public static bool JumpWasReleasedThisFrame => Input.GetKeyUp(KeyCode.Space);
    public static bool RestartTriggered => Input.GetKey(KeyCode.R);

    public static bool MouseWheelUp => Input.mouseScrollDelta.y > 0;
    public static bool MouseWheelDown => Input.mouseScrollDelta.y < 0;
}
