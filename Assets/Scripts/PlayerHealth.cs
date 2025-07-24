<<<<<<< HEAD
﻿using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 변수에 저장
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }


    protected override void OnEnable()
    {

        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        if (!dead)
        {
            
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {

        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();
        healthSlider.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if (item != null)
            {
                item.Use(gameObject);
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}
=======
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator animator;
    private Collider playerCollider;

    public Text healthText;
    public Image healthBarFill;

    public MonoBehaviour playerMovementScript;

    // 애니메이터 상태 관리 스크립트 참조 (Inspector 할당 or GetComponent)
    private ThirdPersonController playerController;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        if (playerController == null)
        {
            playerController = GetComponent<ThirdPersonController>();
        }

        UpdateHealthUI();
    }

    void Update()
    {
        Debug.Log(currentHealth);
    }

    void HandleShielding()
    {
        bool shielding = playerController != null && playerController.IsShielding;

        if (playerCollider != null)
            playerCollider.enabled = !shielding;

        if (playerMovementScript != null)
            playerMovementScript.enabled = !shielding;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        HandleShielding();

        if (playerController != null && playerController.IsShielding)
        {
            Debug.Log("쉴드가 활성화되어 피해를 막음");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

        if (animator != null)
            animator.SetBool("Die", true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;
    }
}
>>>>>>> 90dd7007b2e7f1dae1b5324872491f5bfa98f5f0
