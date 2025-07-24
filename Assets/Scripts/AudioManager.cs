using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;        // 배경음악용 AudioSource
    public AudioSource sfxSource;        // 일반 효과음용 AudioSource
    public AudioSource skillSfxSource;   // 스킬 효과음용 AudioSource

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgmSource == null)
                bgmSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();
            if (skillSfxSource == null)
                skillSfxSource = gameObject.AddComponent<AudioSource>();

            // 초기 세팅
            bgmSource.playOnAwake = false;
            bgmSource.loop = true;

            sfxSource.playOnAwake = false;
            sfxSource.loop = false;

            skillSfxSource.playOnAwake = false;
            skillSfxSource.loop = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 배경음악 재생
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // 일반 효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip 또는 sfxSource가 할당되지 않았습니다.");
        }
    }

    // 스킬 효과음 재생 (스킬 효과음과 일반 효과음을 분리할 경우)
    public void PlaySkillSFX(AudioClip clip)
    {
        if (clip != null && skillSfxSource != null)
        {
            skillSfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip 또는 skillSfxSource가 할당되지 않았습니다.");
        }
    }
}
