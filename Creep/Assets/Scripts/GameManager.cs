using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Lives")]
    [SerializeField] int startingLives = 3;
    public int Lives { get; private set; }

    [Header("Optional UI")]
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject gameOverPanel;
    [Header("Spam Protection")]
    [SerializeField] float catchCooldown = 1f;
    float lastCatchTime = -999f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Lives = startingLives;
        UpdateUI();
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void PlayerCaught()
    {
        if (Time.time - lastCatchTime < catchCooldown) return; // avoid rapid multi-hits
        lastCatchTime = Time.time;

        Lives--;
        UpdateUI();

        if (Lives <= 0)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

    void UpdateUI()
    {
        if (livesText) livesText.text = $"Lives: {Lives}";
    }
}
