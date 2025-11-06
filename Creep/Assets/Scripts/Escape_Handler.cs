using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EscapeHandler : MonoBehaviour
{
    public GameObject gotAwayScreen; // assign in inspector or name it "GotAwayScreen"
    public string escapeTag = "EscapePoint"; // tag for the invisible cube
    public string escapeName = "EscapeCube"; // fallback by name
    public string enemyTag = "Enemy"; // tag for the enemy

    // Fallback collider reference for bounds-check (no Rigidbody needed)
    private Collider escapeCollider;

    void Start()
    {
        if (gotAwayScreen == null)
            gotAwayScreen = GameObject.Find("GotAwayScreen");

        var col = GetComponent<Collider>();
        // Debug.Log($"EscapeHandler.Start on '{name}': hasCollider={col!=null}, isTrigger={col?.isTrigger}");
        // Debug.Log($"EscapeHandler.Start: hasRigidbody={GetComponent<Rigidbody>()!=null}");

        // Try to find escape cube by tag first, then by name
        var escapeObj = GameObject.FindWithTag(escapeTag);
        if (escapeObj == null)
            escapeObj = GameObject.Find(escapeName);

        if (escapeObj != null)
            escapeCollider = escapeObj.GetComponent<Collider>();

        // Debug.Log($"EscapeHandler.Start: foundEscape={escapeObj!=null}, hasEscapeCollider={escapeCollider!=null}");
    }

    // Trigger can be fired on either the enemy or the cube depending on where this script is attached.
    private void OnTriggerEnter(Collider other)
    {
        if (GlobalState.GameEnded) return;

        // Debug.Log($"EscapeHandler.OnTriggerEnter: '{name}' collided with '{other.name}' (tag={other.tag})");

        // If this object is the enemy, other should be the escape cube.
        if (other.CompareTag(escapeTag) || other.name == escapeName)
        {
            // Debug.Log("EscapeHandler: Enemy entered escape trigger -> GotAway");
            TriggerGotAway();
            return;
        }

        // If this object is the escape cube, other should be the enemy.
        if (CompareTag(escapeTag) || name == escapeName)
        {
            if (other.CompareTag(enemyTag))
            {
                // Debug.Log("EscapeHandler: Escape cube detected enemy -> GotAway (trigger)");
                TriggerGotAway();
                return;
            }
        }
    }

    // Fallback for non-trigger colliders if you prefer collision-based detection.
    private void OnCollisionEnter(Collision collision)
    {
        if (GlobalState.GameEnded) return;

        var other = collision.collider;
        // Debug.Log($"EscapeHandler.OnCollisionEnter: '{name}' hit '{other.name}' (tag={other.tag})");

        if (other.CompareTag(escapeTag) || other.name == escapeName)
        {
            // Debug.Log("EscapeHandler: Enemy collided with escape -> GotAway");
            TriggerGotAway();
            return;
        }

        if (CompareTag(escapeTag) || name == escapeName)
        {
            if (other.CompareTag(enemyTag))
            {
                // Debug.Log("EscapeHandler: Escape cube detected enemy -> GotAway (collision)");
                TriggerGotAway();
                return;
            }
        }
    }

    // Bounds-check fallback so trigger works even without Rigidbody setup
    void Update()
    {
        if (GlobalState.GameEnded) return;
        if (escapeCollider == null) return;

        // Use world-space bounds to check if this object's position is inside the escape collider
        if (escapeCollider.bounds.Contains(transform.position))
        {
            // Debug.Log($"EscapeHandler.Update: '{name}' is inside escape bounds -> GotAway");
            TriggerGotAway();
        }
    }

    private void TriggerGotAway()
    {
        if (GlobalState.GameEnded) return;
        GlobalState.GameEnded = true;
        Time.timeScale = 0f;
        if (gotAwayScreen != null)
            gotAwayScreen.SetActive(true);
        // else
        //     Debug.LogWarning("EscapeHandler: gotAwayScreen not assigned and not found.");
    }
}
