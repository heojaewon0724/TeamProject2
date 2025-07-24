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
    public string animationTrigger = "Attack2"; // 퍼블릭으로 설정한 애니메이션 트리거

    public override float cooldown => cooldownTime;

    public override void Activate(GameObject user)
    {
        // 1. 애니메이션 실행
        var animator = user.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger(animationTrigger); // 퍼블릭 변수로 설정한 애니메이션 트리거 사용

        // 2. 이펙트 및 판정 실행
        var mono = user.GetComponent<MonoBehaviour>();
        if (mono != null)
            mono.StartCoroutine(DelayedAreaBlast(user));
    }

    private IEnumerator DelayedAreaBlast(GameObject user)
    {
        yield return new WaitForSeconds(effectDelay);

        Vector3 center = user.transform.position + user.transform.forward * distance;

        // 이펙트 생성
        if (areaEffectPrefab != null)
            Object.Instantiate(areaEffectPrefab, center, Quaternion.identity);

        // 데미지 판정: Enemy 컴포넌트가 붙은 오브젝트에만 데미지 적용
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

    }

}
