using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public AudioClip winSound;
    private AudioSource audioSource;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        scoreText.text = "Score: " + finalScore;

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }
}