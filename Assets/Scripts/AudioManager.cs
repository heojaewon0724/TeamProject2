using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;        // ������ǿ� AudioSource
    public AudioSource sfxSource;        // �Ϲ� ȿ������ AudioSource
    public AudioSource skillSfxSource;   // ��ų ȿ������ AudioSource

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

            // �ʱ� ����
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

    // ������� ���
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

    // �Ϲ� ȿ���� ���
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip �Ǵ� sfxSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // ��ų ȿ���� ��� (��ų ȿ������ �Ϲ� ȿ������ �и��� ���)
    public void PlaySkillSFX(AudioClip clip)
    {
        if (clip != null && skillSfxSource != null)
        {
            skillSfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip �Ǵ� skillSfxSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
