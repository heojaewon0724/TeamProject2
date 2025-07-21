using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; }

    public Transform fireTransform; // 총알 나갈 위치
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    private Animator animator;

    public GunData gunData;

    private float fireDistance = 50f;
    public int ammoRemain = 100;
    public int magAmmo;

    private float lastFireTime;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        state = State.Ready;
        lastFireTime = 0f;
    }

    // 공격 명령 (트리거만 설정)
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
        }
    }

    // 애니메이션 이벤트로 호출됨
    public void ShootEvent()
    {
        if (state != State.Ready) return;
        Shot(fireTransform.position, fireTransform.forward);
    }

    private void Shot(Vector3 firePos, Vector3 fireDir)
    {
        RaycastHit hit;
        Vector3 hitPosition = firePos + fireDir * fireDistance;

        if (Physics.Raycast(firePos, fireDir, out hit, fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
                target.OnDamage(gunData.damage, hit.point, hit.normal);

            hitPosition = hit.point;
        }

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        // shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
            return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;
        ammoToFill = Mathf.Min(ammoToFill, ammoRemain);

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }
}
