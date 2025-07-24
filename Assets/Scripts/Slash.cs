using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem slashVFX; // Particle System 연결
    public Transform attachPoint;

    [Header("Audio")]
    public AudioSource audioSource;  // AudioSource 연결
    public AudioClip slashSound;     // 재생할 사운드 클립

    public void Play()
    {
        if (slashVFX != null)
        {
            if (attachPoint != null)
            {
                slashVFX.transform.position = attachPoint.position;
                slashVFX.transform.rotation = attachPoint.rotation;
            }

            slashVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            slashVFX.Play();
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(slashSound);
        }

    }
}
