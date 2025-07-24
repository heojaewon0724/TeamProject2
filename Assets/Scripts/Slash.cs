using UnityEngine;
using System.Collections;
using StarterAssets;
using NUnit.Framework;

public class SlashEffect : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem slashVFX; // Particle System 연결
    public Transform attachPoint;
    public float SlashingTime = 1.0f;

    public bool isSkill = false;
    public bool IsSlashing { get; private set; } = false;

    public float damage = 0f;

    public void Play()
    {
        if (slashVFX != null)
        {
            if (attachPoint != null)
            {
                slashVFX.transform.position = attachPoint.position;
                slashVFX.transform.rotation = attachPoint.rotation;
            }

            slashVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // 리셋
            slashVFX.Play();

            // 스킬 시전 상태 시작
            if (!IsSlashing)
            {
                StartCoroutine(SlashRoutine());
            }
        }
        else
        {
            Debug.LogWarning("SlashEffect: Slash VFX가 할당되지 않았습니다.");
        }
    }

    private IEnumerator SlashRoutine()
    {
        IsSlashing = true;
        // 스킬 시전 시간만큼 대기
        yield return new WaitForSeconds(SlashingTime);
        IsSlashing = false;
    }
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        // Enemy 스크립트를 가져와서 데미지 입히기
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

}
