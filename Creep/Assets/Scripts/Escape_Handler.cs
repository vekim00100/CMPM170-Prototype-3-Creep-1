using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class EscapeHandler : MonoBehaviour
{
    public GameObject gotAwayScreen; 
    public string escapeTag = "EscapePoint";
    public string escapeName = "EscapeCube"; 
    public string enemyTag = "Enemy"; 

    // Fallback collider reference for bounds-check (no Rigidbody needed)
    private Collider escapeCollider;

    void Start()
    {
        if (gotAwayScreen == null)
            gotAwayScreen = GameObject.Find("GotAwayScreen");

        var col = GetComponent<Collider>();

        // Try to find escape cube by tag first, then by name
        var escapeObj = GameObject.FindWithTag(escapeTag);
        if (escapeObj == null)
            escapeObj = GameObject.Find(escapeName);

        if (escapeObj != null)
            escapeCollider = escapeObj.GetComponent<Collider>();

    }

    // Trigger can be fired on either the enemy or the cube depending on where this script is attached.
    private void OnTriggerEnter(Collider other)
    {
        if (GlobalState.GameEnded) return;


        // If this object is the enemy, other should be the escape cube.
        if (other.CompareTag(escapeTag) || other.name == escapeName)
        {
            TriggerGotAway();
            return;
        }

        // If this object is the escape cube, other should be the enemy.
        if (CompareTag(escapeTag) || name == escapeName)
        {
            if (other.CompareTag(enemyTag))
            {
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

        if (other.CompareTag(escapeTag) || other.name == escapeName)
        {
            TriggerGotAway();
            return;
        }

        if (CompareTag(escapeTag) || name == escapeName)
        {
            if (other.CompareTag(enemyTag))
            {
                TriggerGotAway();
                return;
            }
        }
    }

    void Update()
    {
        if (GlobalState.GameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (GlobalState.GameEnded) return;
        if (escapeCollider == null) return;

        // Use world-space bounds to check if this object's position is inside the escape collider
        if (escapeCollider.bounds.Contains(transform.position))
        {
            TriggerGotAway();
        }
    }

    private void TriggerGotAway()
    {
        if (GlobalState.GameEnded) return;
        GlobalState.GameEnded = true;
        Time.timeScale = 0f;
        // Pause any enemy footstep audio sources on game end
        GlobalState.PauseEnemyFootsteps();

        if (gotAwayScreen != null)
            gotAwayScreen.SetActive(true);
    }



    private void RestartGame()
    {
        Time.timeScale = 1f;
        GlobalState.GameEnded = false;

        if (gotAwayScreen != null)
            gotAwayScreen.SetActive(false);

        // Resume by reloading the scene so all state is clean
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
