using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject spottedCanvas;

    public bool isSpotted { get; private set; }

    float baseFixedDeltaTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            baseFixedDeltaTime = Time.fixedDeltaTime;
            if (spottedCanvas != null) spottedCanvas.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Spotted()
    {
        if (isSpotted) return;
        isSpotted = true;

        if (spottedCanvas != null)
            spottedCanvas.SetActive(true);

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unspotted()
    {
        if (!isSpotted) return;
        isSpotted = false;

        if (spottedCanvas != null)
            spottedCanvas.SetActive(false);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}