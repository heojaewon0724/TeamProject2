using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Skill/AreaBlastSkill")]
public class AreaBlastSkill : SkillBase
{
    public GameObject areaEffectPrefab;
    public float range = 8f;
    public float distance = 10f;
    public float damage = 80f;
    public float cooldownTime = 6f;
    public float effectDelay = 0.5f;
    public float attackStartDelay = 0.5f;    // 공격 판정 시작하는 지연시간
    public float attackActiveTime = 2.0f;    // 공격 판정이 활성화되는 지속 시간 (예: 광역 지속 데미지 유지 시간)
    public float damageInterval = 0.5f;      // 데미지 입히는 간격 (공격 활성화 중 반복 적용하는 속도)
    public int damageCount = 1;               // 데미지 반복 횟수 (1이면 한번만)

    public string animationTrigger = "Attack2";

    public override float cooldown => cooldownTime;

    public override void Activate(GameObject user)
    {
        var animator = user.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger(animationTrigger);

        var mono = user.GetComponent<MonoBehaviour>();
        if (mono != null)
            mono.StartCoroutine(DelayedAreaBlast(user));
    }

    private IEnumerator DelayedAreaBlast(GameObject user)
    {
        Vector3 center = user.transform.position + user.transform.forward * distance;

        // 이펙트 생성
        if (areaEffectPrefab != null)
            Object.Instantiate(areaEffectPrefab, center, Quaternion.identity);

        // 공격 시작 전 대기
        yield return new WaitForSeconds(attackStartDelay);

        for (int i = 0; i < damageCount; i++)
        {
            Collider[] hits = Physics.OverlapSphere(center, range);
            foreach (var hit in hits)
            {
                if (hit.transform.root == user.transform.root)
                    continue;

                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }

            if (i < damageCount - 1)
                yield return new WaitForSeconds(damageInterval);
        }
    }

}
