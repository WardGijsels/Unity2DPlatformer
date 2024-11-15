using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health logic")]
    public int maxHealth = 3;

    private int CurrentHealth;

    [Header("UI")]
    private SpriteRenderer spriteRenderer;

    public TextMeshProUGUI healthText;

    [Header("Death")]
    public float deathZoneY = -10f;

    public static event Action OnPlayerDied;

    [Header("instance")]
    public static PlayerHealth instance;

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetHealth();

        HealthPowerUp.onHealthPickUp += HealthBoost;
        GameController.onReset += ResetHealth;
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

    private void HealthBoost(int healthAmount)
    {
        StartCoroutine(HealthPowerUpBoost(healthAmount));
        UpdateHealthText();
    }

    private IEnumerator HealthPowerUpBoost(int healthAmount)
    {
        CurrentHealth += healthAmount;
        // Zorgt ervoor dat de functie niet 3 keer opgeroepen wordt
        yield return new WaitForSeconds(2f);
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        UpdateHealthText();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check of de player in de death zone zit
        if (transform.position.y < deathZoneY)
        {
            // Speler sterft
            PlayerDies();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        StartCoroutine(PlayerDamaged(damage));
        StartCoroutine(FlashRed());
        Debug.Log(CurrentHealth);
        UpdateHealthText();

        if (CurrentHealth <= 0)
        {
            // Game over
            PlayerDies();
        }
    }

    // Flash player rood als je gehit wordt
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator PlayerDamaged(int damage)
    {
        CurrentHealth -= damage;
        SoundEffectManager.Play("PlayerDamage");
        // Zorgt ervoor dat de player niet meerdere keren binnen een seconde gehit
        yield return new WaitForSeconds(2f);
    }

    public void UpdateHealthText()
    {
        healthText.text = CurrentHealth.ToString();
    }

    public void PlayerDies()
    {
        OnPlayerDied.Invoke();
        ResetHealth();
    }
}