using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Status")]
    public float maxHealth = 100f;
    private float currentHealth;

    public float damage = 10f;
    public float moveSpeed = 3.5f;

    [Header("References")]
    public Transform player;
    public EnemyHealthBar healthBar;  // 체력바 스크립트 (캔버스 자식에 붙여두고 연결)

    private NavMeshAgent agent;
    private Animator animator;
    private Renderer[] renderers;

    private bool isAttacking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        animator = GetComponent<Animator>();

        renderers = GetComponentsInChildren<Renderer>();

        // 자식 오브젝트에서 EnemyHealthBar 자동 할당 (방법2)
        if (healthBar == null)
            healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // 플레이어 태그 기반 자동 참조
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (player == null || agent == null)
            return;

        // NavMeshAgent가 정상적으로 NavMesh 위에 있는지 체크해서 에러 방지
        bool canControlAgent = agent.enabled && agent.isOnNavMesh;

        if (isAttacking)
        {
            if (canControlAgent)
                agent.isStopped = true;

            if (animator != null)
                animator.SetBool("IsAttacking", true);
        }
        else
        {
            if (canControlAgent)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }

            if (animator != null)
                animator.SetBool("IsAttacking", false);
        }
    }

    public void SetAttacking(bool isAttack)
    {
        isAttacking = isAttack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        StartCoroutine(FadeOutAndDestroy(1f));
    }

    private IEnumerator FadeOutAndDestroy(float duration)
    {
        float elapsed = 0f;

        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = originalColors[i];
                c.a = alpha;
                renderers[i].material.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
