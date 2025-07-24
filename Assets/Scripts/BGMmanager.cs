using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgmSource;  // 배경음악용
    public AudioSource sfxSource;  // 효과음용

    public float fadeDuration = 1f;

    public IEnumerator FadeOutMusic()
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
            yield return null;
        }
        bgmSource.volume = 0f;
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
}
