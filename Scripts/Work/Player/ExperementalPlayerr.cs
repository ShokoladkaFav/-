using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // �������� ����������

    [Header("Health and Mana")]
    public float maxHealth = 560f;
    public float currentHealth;
    public float maxMana = 315f;
    public float currentMana;
    public float healthRegen = 2f; // ����������� ������'� (���������, �� �������)
    public float manaRegen = 1f; // ����������� ���� (���������, �� �������)

    private Rigidbody2D rb; // ��������� �� Rigidbody2D
    private Vector2 movement; // �������� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� Rigidbody2D

        // ����������� ������'� �� ����
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        // ��������� ����� � ���������
        movement.x = Input.GetAxis("Horizontal"); // ����/������ (A/D)
        movement.y = Input.GetAxis("Vertical"); // �����/���� (W/S)

        // ����������� ������'� �� ����
        RegenerateHealthAndMana();

        // ������ ������ ��� �������� ������'�/����
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(50); // �������� 50 HP
        if (Input.GetKeyDown(KeyCode.M)) UseMana(30); // ��������� 30 ����
    }

    private void FixedUpdate()
    {
        // ���������� ������
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // ����� ��� ��������� ����������
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"������� ������� {damage} �����. ������'�: {currentHealth}/{maxHealth}");
        CheckIfPlayerIsDead();
    }

    // ����� ��� ������� ����
    public void UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            Debug.Log($"����������� {amount} ����. ����: {currentMana}/{maxMana}");
        }
        else
        {
            Debug.Log("�� ������� ����!");
        }
    }

    // ����������� ������'� �� ����
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

    // �������� ����� ������
    private void CheckIfPlayerIsDead()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("������� �����!");
            // ��� ����� ��������� ����� ����������� ���� ��� ������ ������ �������
        }
    }
}
