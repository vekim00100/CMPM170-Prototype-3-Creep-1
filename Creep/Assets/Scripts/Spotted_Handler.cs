using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class Spotted_Handler : MonoBehaviour
{
    public GameObject spottedScreen; // assign your Canvas in inspector or name it "SpottedScreen"
    public float positionThreshold = 0.2f; // increased sensitivity to avoid jitter

    FieldOfView fov;
    Transform player;

    // Per-instance baseline (do NOT use the global baseline for detection)
    private Vector3 baselinePosition = Vector3.zero;
    private bool baselineSet = false;

    void Start()
    {
        fov = GetComponent<FieldOfView>();
        if (fov == null) enabled = false;
        else player = fov.playerRef ? fov.playerRef.transform : null;

        if (spottedScreen == null)
            spottedScreen = GameObject.Find("SpottedScreen");

        baselineSet = false;
    }

    void Update()
    {
        if (GlobalState.SpottedTriggered) return;
        if (fov == null || player == null) return;

        if (fov.canSeePlayer)
        {
            // If we just started seeing the player, record their position as the baseline and do not trigger yet.
            if (!baselineSet)
            {
                baselineSet = true;
                baselinePosition = player.position;
                return;
            }

            // Use squared distance to avoid sqrt and compare with squared threshold
            float sqrDist = (player.position - baselinePosition).sqrMagnitude;
            float sqrThreshold = positionThreshold * positionThreshold;

            if (sqrDist > sqrThreshold)
            {
                GlobalState.SpottedTriggered = true;
                Time.timeScale = 0f; // freeze game

                if (spottedScreen != null)
                    spottedScreen.SetActive(true);
                else
                    Debug.LogWarning("Spotted_Handler: spottedScreen not assigned and not found.");
            }

            // Do NOT update baseline while still seen — keep baseline until lost or triggered.
        }
        else
        {
            // Lost sight -> reset baseline so next time we see the player we set a new baseline.
            baselineSet = false;
            baselinePosition = Vector3.zero;
        }
    }
}
