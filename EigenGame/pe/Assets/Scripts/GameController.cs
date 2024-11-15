using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverScreen;

    public TextMeshProUGUI survivedText;
    private int survivedLevelsCount;

    public static event Action onReset;

    [Header("Level handling")]
    public GameObject player;

    public List<GameObject> levels;
    private int currentLevelIndex;
    public List<Transform> playerSpawnPoints;
    private Coroutine levelTransitionCoroutine;

    [Header("Gem Collection")]
    public TextMeshProUGUI gemCountText;

    private int gemCount = 0;
    private Coroutine gemCollectionCoroutine;
    private Gem[] gems; // Alle gems in een level

    [Header("Difficulty Settings")]
    public int selectedDifficulty = 0; // 0 for Normal, 1 for Hardcore

    public static GameController instance;

    [Header("Highscore")]
    public TextMeshProUGUI highScoreText;

    private int highScore;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);

        //Interact.OnHoldComplete += LoadNextLevel;
        gems = FindObjectsOfType<Gem>();

        selectedDifficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0); // Default Difficulty is Normal
        SetDifficulty(selectedDifficulty);

        highScore = PlayerPrefs.GetInt("HighScore", 0); // Default is 0 als er nog geen highscore is
    }

    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = difficulty;
        // Update PlayerHealth max health gebaseerd op difficulty
        if (PlayerHealth.instance != null)
        {
            PlayerHealth.instance.maxHealth = (selectedDifficulty == 0) ? 3 : 1;
            PlayerHealth.instance.ResetHealth(); // ResetHealth na aanpassen van Difficulty
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);

        survivedText.text = "You survived " + survivedLevelsCount + " Level";
        if (survivedLevelsCount != 1) survivedText.text += "s";

        // Kijk of er een nieuwe highScore is
        if (survivedLevelsCount > highScore)
        {
            highScore = survivedLevelsCount;
            PlayerPrefs.SetInt("HighScore", highScore); // Save de nieuwe highScore
        }

        // Update high score text in het game over screen
        highScoreText.text = "High Score: " + highScore + " Level";
        if (highScore != 1) highScoreText.text += "s";
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        survivedLevelsCount = 0;
        gemCount = 0;
        UpdateGemUI();

        foreach (Gem gem in gems)
        {
            gem.ResetGem();
        }

        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        // Defensive coding
        if (playerSpawnPoints != null && playerSpawnPoints.Count > currentLevelIndex)
        {
            // De spelers positie terug zetten op het begin van het level
            player.transform.position = playerSpawnPoints[currentLevelIndex].position;
        }
    }

    public void LoadNextLevel()
    {
        // Zorgt ervoor dat je niet naar meerdere levels te gelijk gestuurd wordt
        if (levelTransitionCoroutine != null)
            return;

        levelTransitionCoroutine = StartCoroutine(TransitionToNextLevel());
    }

    private IEnumerator TransitionToNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;

        // Deactivate current level
        Debug.Log("Deactivating level: " + levels[currentLevelIndex].name);
        levels[currentLevelIndex].SetActive(false);

        // Activate next level
        Debug.Log("Activating level: " + levels[nextLevelIndex].name);
        levels[nextLevelIndex].SetActive(true);

        // Verplaats de player naar de spawnpoint van het volgende level
        player.transform.position = playerSpawnPoints[nextLevelIndex].position;

        currentLevelIndex = nextLevelIndex;
        survivedLevelsCount++;

        // Gebruik Yield return zodat de functie niet meerdere keren getriggerd kan worden
        yield return new WaitForSeconds(0.5f);

        levelTransitionCoroutine = null; // Reset de Couroutine
        Debug.Log("Level transition complete to: " + levels[nextLevelIndex].name);
    }

    public void IncreaseGemCount()
    {
        if (gemCollectionCoroutine != null)
            return;

        gemCollectionCoroutine = StartCoroutine(GemCountUpdated());
    }

    public IEnumerator GemCountUpdated()
    {
        gemCount++;
        UpdateGemUI();

        // Gebruik Yield return zodat de functie niet meerdere keren getriggerd kan worden
        yield return new WaitForSeconds(0.5f);

        gemCollectionCoroutine = null;
    }

    private void UpdateGemUI()
    {
        gemCountText.text = gemCount.ToString();
    }
}