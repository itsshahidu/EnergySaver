using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    [Header("Bonus Settings")]
    public float energyReductionPercent = 20f;

    [Header("Timing")]
    public float appearInterval = 20f;
    public float visibleDuration = 4f;

    [Header("Spawn Area")]
    public float minX = -4f;
    public float maxX = 4f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Sound")]
    public AudioClip appearSound;
    public AudioClip collectSound;

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private AudioSource audioSource;
    private float appearTimer;
    private float visibleTimer;
    private bool isVisible = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        appearTimer = appearInterval;
        SetVisible(false);
    }

    void Update()
    {
        if (gameManager != null && gameManager.IsPaused()) return;

        if (!isVisible)
        {
            appearTimer -= Time.deltaTime;
            if (appearTimer <= 0f)
            {
                ShowPanel();
            }
        }
        else
        {
            visibleTimer -= Time.deltaTime;
            if (visibleTimer <= 0f)
            {
                HidePanel();
            }
        }
    }

    void ShowPanel()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        transform.position = new Vector3(randomX, randomY, 0f);

        SetVisible(true);
        isVisible = true;
        visibleTimer = visibleDuration;

        PlaySound(appearSound);
    }

    void HidePanel()
    {
        SetVisible(false);
        isVisible = false;
        appearTimer = appearInterval;
    }

    void SetVisible(bool visible)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = visible;
        if (boxCollider != null) boxCollider.enabled = visible;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.ReduceEnergyByPercent(energyReductionPercent);
        }
        PlaySound(collectSound);
        HidePanel();
    }
}