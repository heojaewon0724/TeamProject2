using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Skill/LightningStrikeSkill")]
public class LightningStrikeSkill : SkillBase
{
    public GameObject strikeEffectPrefab;
    public float strikeDamage = 100f;
    public float cooldownTime = 8f;
    public float effectDelay = 0.6f;

    public override float cooldown => cooldownTime;

    public override void Activate(GameObject user)
    {
        var animator = user.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("LightningStrike");

        var mono = user.GetComponent<MonoBehaviour>();
        if (mono != null)
            mono.StartCoroutine(DelayedStrike(user));
    }

    private IEnumerator DelayedStrike(GameObject user)
    {
        yield return new WaitForSeconds(effectDelay);

        Vector3 targetPos = user.transform.position + user.transform.forward * 5f;
        if (strikeEffectPrefab != null)
            Object.Instantiate(strikeEffectPrefab, targetPos + Vector3.up * 10f, Quaternion.identity);

        // 데미지 판정 등은 필요에 따라
    }
}
