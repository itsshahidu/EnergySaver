using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider energySlider;
    public TMP_Text timerText;

    [Header("Game Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public float gameDuration = 60f;

    private float timeRemaining;
    private Appliance[] allAppliances;
    private bool gameEnded = false;

    void Start()
    {
        timeRemaining = gameDuration;
        currentEnergy = 0f;
        gameEnded = false;

        allAppliances = FindObjectsOfType<Appliance>();

        energySlider.minValue = 0;
        energySlider.maxValue = maxEnergy;
    }

    void Update()
    {
        if (gameEnded) return; // stop everything once win/lose has triggered

        // Countdown timer
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0) timeRemaining = 0;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);

        // Add energy for every appliance currently ON
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

        // Check Lose condition
        if (currentEnergy >= maxEnergy)
        {
            TriggerGameOver();
            return;
        }

        // Check Win condition
        if (timeRemaining <= 0)
        {
            TriggerWin();
            return;
        }
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
}