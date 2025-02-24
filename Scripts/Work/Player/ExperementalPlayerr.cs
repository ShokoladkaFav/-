using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Швидкість переміщення

    [Header("Health and Mana")]
    public float maxHealth = 560f;
    public float currentHealth;
    public float maxMana = 315f;
    public float currentMana;
    public float healthRegen = 2f; // Регенерація здоров'я (наприклад, за секунду)
    public float manaRegen = 1f; // Регенерація мани (наприклад, за секунду)

    private Rigidbody2D rb; // Посилання на Rigidbody2D
    private Vector2 movement; // Напрямок руху

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Отримуємо Rigidbody2D

        // Ініціалізація здоров'я та мани
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        // Отримання вводу з клавіатури
        movement.x = Input.GetAxis("Horizontal"); // Вліво/вправо (A/D)
        movement.y = Input.GetAxis("Vertical"); // Вгору/вниз (W/S)

        // Регенерація здоров'я та мани
        RegenerateHealthAndMana();

        // Тестові клавіші для перевірки здоров'я/мани
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(50); // Втратити 50 HP
        if (Input.GetKeyDown(KeyCode.M)) UseMana(30); // Витратити 30 мани
    }

    private void FixedUpdate()
    {
        // Переміщення гравця
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Метод для отримання пошкоджень
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"Гравець отримав {damage} урону. Здоров'я: {currentHealth}/{maxHealth}");
        CheckIfPlayerIsDead();
    }

    // Метод для витрати мани
    public void UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            Debug.Log($"Використано {amount} мани. Мана: {currentMana}/{maxMana}");
        }
        else
        {
            Debug.Log("Не вистачає мани!");
        }
    }

    // Регенерація здоров'я та мани
    private void RegenerateHealthAndMana()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegen * Time.deltaTime;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            if (currentMana > maxMana) currentMana = maxMana;
        }
    }

    // Перевірка смерті гравця
    private void CheckIfPlayerIsDead()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Гравець помер!");
            // Тут можна викликати метод перезапуску рівня або показу екрану поразки
        }
    }
}
