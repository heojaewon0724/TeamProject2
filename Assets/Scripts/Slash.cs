using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem slashVFX; // Particle System 연결
    public Transform attachPoint;

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
        }
        else
        {
            Debug.LogWarning("SlashEffect: Slash VFX가 할당되지 않았습니다.");
        }
    }
}
