using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider energySlider;
    public TMP_Text timerText;
    public GameObject pausePanel;

    [Header("Game Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public float gameDuration = 30f;

    private float timeRemaining;
    private Appliance[] allAppliances;
    private bool gameEnded = false;
    private bool isPaused = false;

    void Start()
    {
        timeRemaining = gameDuration;
        currentEnergy = 0f;
        gameEnded = false;
        isPaused = false;

        allAppliances = FindObjectsOfType<Appliance>();

        energySlider.minValue = 0;
        energySlider.maxValue = maxEnergy;

        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        // Allow toggling pause with the Escape key too, as a convenience
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            TogglePause();
        }

        if (gameEnded || isPaused) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0) timeRemaining = 0;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);

        foreach (Appliance appliance in allAppliances)
        {
            if (appliance.isOn)
            {
                currentEnergy += appliance.energyPerSecond * Time.deltaTime;
            }
        }

        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        if (currentEnergy < 0) currentEnergy = 0;

        energySlider.value = currentEnergy;

        if (currentEnergy >= maxEnergy)
        {
            TriggerGameOver();
            return;
        }

        if (timeRemaining <= 0)
        {
            TriggerWin();
            return;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log("TogglePause called. isPaused is now: " + isPaused + " | pausePanel is null? " + (pausePanel == null));
        if (pausePanel != null) pausePanel.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void TriggerWin()
    {
        gameEnded = true;
        PlayerPrefs.SetInt("FinalScore", Mathf.FloorToInt(gameDuration));
        SceneManager.LoadScene("WinScreen");
    }

    void TriggerGameOver()
    {
        gameEnded = true;
        SceneManager.LoadScene("GameOverScreen");
    }

    public void ReduceEnergyByPercent(float percent)
    {
        currentEnergy -= maxEnergy * (percent / 100f);
        if (currentEnergy < 0) currentEnergy = 0;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}