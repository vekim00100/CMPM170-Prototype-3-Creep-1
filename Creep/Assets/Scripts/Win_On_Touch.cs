// ...existing code...
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_On_Touch : MonoBehaviour
{
    public GameObject winScreen; 
    public string targetTag = "Enemy"; 
    public float touchRadius = 0.8f; 
    public bool useDistanceFallback = false;

    void Start()
    {
        if (winScreen == null)
            winScreen = GameObject.Find("WinScreen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GlobalState.GameEnded) return;

        if (other.CompareTag(targetTag) || CompareTag(targetTag))
        {
            Debug.Log("Win_On_Touch: Touch detected -> Win");
            TriggerWin();
        }
    }

    void Update()
    {
        if (GlobalState.GameEnded || !useDistanceFallback) return;

        // Fallback distance check: finds first enemy with tag if this is player
        GameObject enemy = GameObject.FindWithTag(targetTag);
        if (enemy == null) return;

        if ((enemy.transform.position - transform.position).sqrMagnitude <= touchRadius * touchRadius)
        {
            Debug.Log("Win_On_Touch: Distance fallback -> Win");
            TriggerWin();
        }
        // Allow restart with R when an ending is reached
        if (GlobalState.GameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void TriggerWin()
    {
        GlobalState.GameEnded = true;
        Time.timeScale = 0f;
        // Pause any enemy footstep audio sources on game end (robust: pauses AudioSources under enemy objects)
        GlobalState.PauseEnemyFootsteps();

        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
        else
            Debug.LogWarning("Win_On_Touch: winScreen not assigned and not found.");
    }



    private void RestartGame()
    {
        // Restore time and state, then reload the active scene
        Time.timeScale = 1f;
        GlobalState.GameEnded = false;

        if (winScreen != null)
            winScreen.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
// ...existing code...
