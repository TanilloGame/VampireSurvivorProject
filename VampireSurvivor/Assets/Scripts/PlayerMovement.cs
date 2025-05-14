using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float accelerationTime = 0.2f;
    public float decelerationTime = 0.2f;
    public float turnSmoothTime = 0.1f;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 velocitySmoothing;
    private float turnSmoothVelocity;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider;
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentHealth = maxHealth;
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider)
            healthSlider.value = currentHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        OnDeath?.Invoke();
        // A�adir respawn o l�gica de Game Over aqu�
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Calcula velocidad objetivo y suavizado
        Vector3 targetVelocity = inputDir * moveSpeed;
        float smoothTime = Vector3.Dot(velocity, targetVelocity) > 0 ? accelerationTime : decelerationTime;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref velocitySmoothing, smoothTime);

        // Aplica velocidad al Rigidbody (mantiene la componente Y actual)
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        // Rotaci�n suave con Rigidbody
        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion rot = Quaternion.Euler(0f, angle, 0f);
            rb.MoveRotation(rot);
        }
    }
}
