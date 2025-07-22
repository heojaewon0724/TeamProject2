using UnityEngine;

[CreateAssetMenu(menuName = "Skill/LightningSkill")]
public class LightningSkill : SkillBase
{
    public GameObject lightningEffectPrefab;
    public float range = 10f;
    public float damage = 100f;
    public float lightningCooldown = 7f;
    public override float cooldown => lightningCooldown;

    public string animationTrigger = "LightningSkill"; // 애니메이션 트리거 이름
    public float effectDelay = 0.5f; // 애니메이션 후 효과 발동까지의 지연시간

    public override void Activate(GameObject user)
    {
        // 1. 애니메이션 먼저 재생
        var animator = user.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger(animationTrigger);

        // 2. 지연 후 실제 효과 발동
        var mono = user.GetComponent<MonoBehaviour>();
        if (mono != null)
            mono.StartCoroutine(ActivateEffectAfterDelay(user));
    }

    private System.Collections.IEnumerator ActivateEffectAfterDelay(GameObject user)
    {
        yield return new WaitForSeconds(effectDelay);

        // 실제 번개 효과 발동
        Collider[] hits = Physics.OverlapSphere(user.transform.position, range);
        foreach (var hit in hits)
        {
            // 플레이어 자신(및 자식 오브젝트) 제외
            if (hit.transform.root == user.transform.root) continue;

            var target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                Vector3 effectPos = hit.transform.position + Vector3.up * 0.2f;
                if (lightningEffectPrefab != null)
                    Object.Instantiate(lightningEffectPrefab, effectPos, Quaternion.identity);

                target.OnDamage(damage, effectPos, Vector3.down);
            }
        }
    }
}