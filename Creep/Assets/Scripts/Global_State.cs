// ...existing code...
using UnityEngine;

public static class GlobalState
{
    public static Vector3 LastPlayerPosition = Vector3.positiveInfinity;
    public static bool SpottedTriggered = false;

    // New global flag to prevent multiple end-state triggers
    public static bool GameEnded = false;

    // Pause any AudioSources that belong to enemy GameObjects (on the same object or in parents/children).
    // This is more robust than relying on a single serialized AudioSource field on the enemy.
    public static void PauseEnemyFootsteps()
    {
        // Use UnityEngine.Object.FindObjectsOfType to find all audio sources in the scene.
        var sources = Object.FindObjectsOfType<AudioSource>();
        foreach (var src in sources)
        {
            if (src == null) continue;

            // Try to find an EnemyMovement on the same GameObject or in its parents/children.
            var em = src.GetComponent<EnemyMovement>() ?? src.GetComponentInParent<EnemyMovement>() ?? src.GetComponentInChildren<EnemyMovement>();
            if (em != null)
            {
                // Pause if it's playing (safe to call even if already paused/stopped)
                if (src.isPlaying)
                    src.Pause();
            }
        }
    }
}
