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
    public float damageInterval = 0.5f;  // 데미지 입히는 시간 간격 (초 단위)
    public int damageCount = 1;           // 데미지를 입히는 횟수, 1이면 한번만
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
        // 이펙트 위치 계산
        Vector3 center = user.transform.position + user.transform.forward * distance;

        // 1. 이펙트 생성
        if (areaEffectPrefab != null)
        {
            Object.Instantiate(areaEffectPrefab, center, Quaternion.identity);
        }

        // 2. 이펙트 딜레이 동안 대기 (딜레이 시간 설정한 변수 사용)
        yield return new WaitForSeconds(effectDelay);

        // 3. 딜레이 후 데미지 판정 실행
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
