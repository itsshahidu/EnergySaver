using UnityEngine;

public class Appliance : MonoBehaviour
{
    public bool isOn = false;

    [Header("Timing")]
    public float minTimeToTurnOn = 2f;
    public float maxTimeToTurnOn = 6f;

    [Header("Energy")]
    public float energyPerSecond = 5f;

    [Header("Sound")]
    public AudioClip clickSound;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private GameManager gameManager;
    private Color offColor = Color.white;
    private Color onColor = new Color(1f, 0.85f, 0.3f);

    private float timer;
    private float nextTurnOnTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        SetState(false);
        ScheduleNextTurnOn();
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsPaused()) return;

        if (!isOn)
        {
            timer += Time.deltaTime;
            if (timer >= nextTurnOnTime)
            {
                TurnOn();
            }
        }
    }

    void ScheduleNextTurnOn()
    {
        timer = 0f;
        nextTurnOnTime = Random.Range(minTimeToTurnOn, maxTimeToTurnOn);
    }

    public void TurnOn()
    {
        isOn = true;
        SetState(true);
    }

    public void TurnOff()
    {
        isOn = false;
        SetState(false);
        ScheduleNextTurnOn();

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void SetState(bool on)
    {
        spriteRenderer.color = on ? onColor : offColor;
    }

    void OnMouseDown()
    {
        if (gameManager != null && gameManager.IsPaused()) return;

        if (isOn)
        {
            TurnOff();
        }
    }
}