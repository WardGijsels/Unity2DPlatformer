using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;

    private void Start()
    {
        // Initialiseer dropdown options
        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(new List<string> { "Normal", "Hardcore" });

        // Gebruik standaard Unity class PlayerPrefs om de index om te halen
        int savedDifficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0); // Default Normal
        difficultyDropdown.value = savedDifficulty;

        // Voeg een listener toe die wordt aangeroepen wanneer de waarde van de dropdown verandert
        difficultyDropdown.onValueChanged.AddListener(OnDifficultyChange);
    }

    public void OnDifficultyChange(int index)
    {
        Debug.Log("Difficulty changed to index: " + index);
        PlayerPrefs.SetInt("SelectedDifficulty", index);
        PlayerPrefs.Save();
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}