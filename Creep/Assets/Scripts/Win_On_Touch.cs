// ...existing code...
using UnityEngine;

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
    }

    private void TriggerWin()
    {
        GlobalState.GameEnded = true;
        Time.timeScale = 0f;
        if (winScreen != null)
            winScreen.SetActive(true);
        else
            Debug.LogWarning("Win_On_Touch: winScreen not assigned and not found.");
    }
}
// ...existing code...
