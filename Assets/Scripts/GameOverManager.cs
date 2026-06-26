using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public AudioClip alarmSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && alarmSound != null)
        {
            audioSource.PlayOneShot(alarmSound);
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }
}