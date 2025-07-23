using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator animator;
    private Collider playerCollider;

    public Text healthText;
    public Image healthBarFill;

    public MonoBehaviour playerMovementScript;

    // 애니메이터 상태 관리 스크립트 참조 (Inspector 할당 or GetComponent)
    private ThirdPersonController playerController;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        if (playerController == null)
        {
            playerController = GetComponent<ThirdPersonController>();
        }

        UpdateHealthUI();
    }

    void Update()
    {
        Debug.Log(currentHealth);
    }

    void HandleShielding()
    {
        bool shielding = playerController != null && playerController.IsShielding;

        if (playerCollider != null)
            playerCollider.enabled = !shielding;

        if (playerMovementScript != null)
            playerMovementScript.enabled = !shielding;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        HandleShielding();

        if (playerController != null && playerController.IsShielding)
        {
            Debug.Log("쉴드가 활성화되어 피해를 막음");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

        if (animator != null)
            animator.SetBool("Die", true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;
    }
}
