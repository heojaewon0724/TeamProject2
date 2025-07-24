using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using StarterAssets;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemySpawner spawner;  // 스포너 참조 (없으면 null)

    [Header("Enemy Status")]
    public float maxHealth = 100f;
    private float currentHealth;

    public float damage = 10f;
    public float moveSpeed = 3.5f;

    [Header("References")]
    public Transform player;
    public EnemyHealthBar healthBar;  // 체력바 스크립트 (캔버스 자식에 붙여두고 연결)

    private float lastHitTime = 0f;           // 마지막으로 데미지를 받은 시간
    public float damageCooldown = 1f;       // 데미지 입은 후 재입력 가능 쿨다운(초)
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
            if (playerHealth != null&&animator.GetBool("Block") == false)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        // 쿨다운 체크: 마지막 피격 시점에서 damageCooldown 초 지났는지 확인
        if (Time.time - lastHitTime < damageCooldown)
            return;  // 아직 쿨다운 중이라 데미지를 무시

        lastHitTime = Time.time;  // 데미지 받은 시간 갱신

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
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

        if (spawner != null)
            spawner.OnEnemyDie(gameObject);  // 사망 사실을 스포너에 알림

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
    private void LateUpdate()
    {
        if (healthBar != null && Camera.main != null)
        {
            // 체력바의 부모(캔버스)가 있다면 그 Transform을 사용
            Transform barTransform = healthBar.transform;
            if (healthBar.transform.parent != null)
                barTransform = healthBar.transform.parent;

            // 카메라의 정면 방향을 따라가게 함 (월드 기준)
            barTransform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            // 필요하다면 180도 회전
            barTransform.Rotate(0, 180f, 0);
        }
    }
}
