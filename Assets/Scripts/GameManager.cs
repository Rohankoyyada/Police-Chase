using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI kill_Text;
    public GameObject gameOverPanel;
    public GameObject startGamePanel;
    public Timer gameTimer;
    public EnemySpawner spawner;

    [Header("Wave Scaling Settings")]
    public float timePerEnemy = 90f; // 1.5 minutes (90s) per enemy
    public int enemiesToSpawn = 1;

    [Header("Game State")]
    public int kill_Count = 0;
    private int enemiesKilledInWave = 0;
    private bool gameNotStarted = true;

    void Awake() { instance = this; }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        StartNewWave(true); // First wave
        StartedGameUi(gameNotStarted);
    }

    public void StartedGameUi(bool gameState)
    {
        startGamePanel.SetActive(gameState);
    }

    public void StartGameButtonInput()
    {
        gameNotStarted = false;
        StartedGameUi(gameNotStarted);
    }

    public void StartNewWave(bool isFirstWave)
    {
        enemiesKilledInWave = 0;

        // --- TIME CALCULATION LOGIC ---
        float extraTime = enemiesToSpawn * timePerEnemy;
        float currentTimeLeft = isFirstWave ? 0 : gameTimer.remaining_Time;
        float totalNewTime = extraTime + currentTimeLeft;

        gameTimer.ResetTimer(totalNewTime);
        // ------------------------------

        spawner.SpawnWave(enemiesToSpawn);
    }

    public void AddKill()
    {
        kill_Count++;
        enemiesKilledInWave++;
        kill_Text.text = kill_Count.ToString();

        if (enemiesKilledInWave >= enemiesToSpawn)
        {
            enemiesToSpawn++; // Next wave will have one more enemy
            StartNewWave(false);
        }
    }

    public void OnTimeExpired()
    {
        NPCEscapeController[] activeEnemies = Object.FindObjectsByType<NPCEscapeController>(FindObjectsSortMode.None);
        foreach (var enemy in activeEnemies)
        {
            enemy.TriggerExplosionForce();
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}