using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator animator;
    private Collider playerCollider;

    public UnityEngine.UI.Text DeathText;
    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Image healthBarFill;

    public MonoBehaviour playerMovementScript;

    private ThirdPersonController playerController;

    private float lastHitTime = 0f;
    public float damageCooldown = 0.5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        if (playerController == null)
        {
            playerController = GetComponent<ThirdPersonController>();
        }

        if (DeathText != null)
            DeathText.gameObject.SetActive(false);

        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead)
        {
            if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭 감지
            {
                // 씬 재시작 전 timeScale 1로 복구
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }


    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0 || isDead) return;

        if (Time.time - lastHitTime < damageCooldown)
            return;
        lastHitTime = Time.time;


        if (playerController != null && playerController.IsShielding)
            return;

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
        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}";

        if (healthBarFill != null)
            healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

        isDead = true;

        if (DeathText != null)
        {
            DeathText.text = "You Die. Left Click To Restart";
            DeathText.gameObject.SetActive(true);
        }

        if (animator != null)
            animator.SetBool("Die", true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;

        // 3.5초 기다리도록 코루틴 실행
        StartCoroutine(WaitAndPause(3.5f));
    }

    private System.Collections.IEnumerator WaitAndPause(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // 애니메이션 재생은 시간이 충분히 경과했으므로 게임 일시정지
        Time.timeScale = 0f;
    }
}
