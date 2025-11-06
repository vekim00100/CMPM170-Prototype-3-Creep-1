// ...existing code...
using UnityEngine;

public static class GlobalState
{
    public static Vector3 LastPlayerPosition = Vector3.positiveInfinity;
    public static bool SpottedTriggered = false;

    // New global flag to prevent multiple end-state triggers
    public static bool GameEnded = false;
}
