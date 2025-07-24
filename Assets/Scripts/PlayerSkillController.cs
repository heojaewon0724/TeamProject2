// 플레이어 스킬 컨트롤러 예시
using UnityEngine;
using UnityEngine.Playables;
using StarterAssets;

public class PlayerSkillController : MonoBehaviour
{
    public PlayableDirector skillCutsceneDirector;
    private StarterAssetsInputs _input;
    public Animator playerAnimator;


    public SkillBase[] skills; // 여러 스킬을 배열로 관리
    private float[] skillCooldownTimers;
    //public PlayerMovement playerMovement; // 추가
    
    private bool isUsingSkill = false; // 스킬 사용 중인지 추적

    void Start()
    {
        // 스킬 쿨타임 배열 초기화
        skillCooldownTimers = new float[skills.Length];
        
        // StarterAssetsInputs 컴포넌트 가져오기
        _input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        // 게임오버 상태에서는 스킬 사용 불가
        if (GameManager.instance != null && GameManager.instance.isGameover)
            return;

        // 스킬 쿨타임 업데이트
        for (int i = 0; i < skills.Length; i++)
        {
            if (skillCooldownTimers[i] > 0f)
            {
                skillCooldownTimers[i] -= Time.deltaTime;
                // 쿨타임 UI 갱신
                UIManager.instance.UpdateSkillCooldownImage(i, skillCooldownTimers[i], skills[i].cooldown);
            }
        }

        // 스킬 키 입력 감지 (완전한 선입력 방지)
        if (_input != null && !isUsingSkill && !IsPlayingSkillAnimation())
        {
            // Q키 - 첫 번째 스킬 (GetKeyDown으로만 처리, 중복 방지)
            if (Input.GetKeyDown(KeyCode.Q) && skills.Length > 0 && skillCooldownTimers[0] <= 0f)
            {
                OnSkillButton(0);
            }

            // E키 - 두 번째 스킬  
            if (Input.GetKeyDown(KeyCode.E) && skills.Length > 1 && skillCooldownTimers[1] <= 0f)
            {
                OnSkillButton(1);
            }

            // R키 - 세 번째 스킬
            if (Input.GetKeyDown(KeyCode.R) && skills.Length > 2 && skillCooldownTimers[2] <= 0f)
            {
                OnSkillButton(2);
            }
        }
    }

    // 스킬 애니메이션이 재생 중인지 확인하는 메서드
    private bool IsPlayingSkillAnimation()
    {
        if (playerAnimator == null) return false;

        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        
        // Attack1(평타), Attack2, Attack3, Attack4 애니메이션이 재생 중인지 확인
        return stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3") || stateInfo.IsName("Attack4");
    }

    // 스킬 인덱스별로 발동
    public void OnSkillButton(int skillIndex)
    {
        // 기본 유효성 검사
        if (skillIndex < 0 || skillIndex >= skills.Length) return;
        if (skills[skillIndex] == null) return;
        
        // 쿨타임 중이면 완전히 차단
        if (skillCooldownTimers[skillIndex] > 0f) 
        {
            Debug.Log($"스킬 {skillIndex}는 쿨타임 중입니다. 남은 시간: {skillCooldownTimers[skillIndex]:F1}초");
            return;
        }
        
        // 다른 스킬 사용 중이면 차단
        if (isUsingSkill) 
        {
            Debug.Log("다른 스킬 사용 중입니다.");
            return;
        }
        
        var currentSkill = skills[skillIndex];

        // 스킬 사용 상태 시작 (다른 스킬 차단)
        isUsingSkill = true;

        // 쿨타임 먼저 적용 (중복 실행 방지)
        skillCooldownTimers[skillIndex] = currentSkill.cooldown;
        UIManager.instance.UpdateSkillCooldownImage(skillIndex, skillCooldownTimers[skillIndex], currentSkill.cooldown);

        Debug.Log($"스킬 {skillIndex} 발동! 쿨타임: {currentSkill.cooldown}초");

        // 스킬별로 이동 제어 - StarterAssetsInputs는 입력 차단 방식이 다름
        // 필요시 ThirdPersonController에서 스킬 사용 중 플래그를 확인하도록 구현
       // if (playerMovement != null) playerMovement.enabled = currentSkill.allowMoveWhileCasting;

     //   Debug.Log(playerMovement.enabled ? "이동 가능" : "이동 불가능");

        // 스킬 발동 (실제 효과 - 애니메이션 포함)
        currentSkill.Activate(gameObject);

        // 애니메이션은 각 스킬의 Activate() 메서드에서 처리하므로 여기서는 제거

        // 이름으로 컷씬 오브젝트 찾기 (연출용)
        /*if (!string.IsNullOrEmpty(currentSkill.cutsceneObjectName))
        {
            GameObject cutsceneObj = GameObject.Find(currentSkill.cutsceneObjectName);
            if (cutsceneObj != null)
            {
                var director = cutsceneObj.GetComponent<PlayableDirector>();
                if (director != null)
                {
                    director.Play();
                    director.stopped += OnCutsceneEnd;
                }
            }
            else
            {
                // 컷씬이 없으면 바로 입력 복구
                RestorePlayerControl();
            }
        }
        else
        {
            // 컷씬이 없으면 바로 입력 복구
            RestorePlayerControl();
        }*/
        
        // 스킬 사용 후 일정 시간 후에 입력 복구 (스킬 지속시간 고려)
        StartCoroutine(RestorePlayerControlAfterDelay(1.5f)); // 1.5초 후 복구 (애니메이션 완료 대기)
    }

    private void OnCutsceneEnd(PlayableDirector director)
    {
        // 컷씬이 끝나면 플레이어 제어 복구
        RestorePlayerControl();
        
        director.stopped -= OnCutsceneEnd;
    }

    // 플레이어 제어 복구 메서드
    private void RestorePlayerControl()
    {
        // StarterAssetsInputs는 컴포넌트를 비활성화하지 않고 사용
        // 필요시 입력 값들을 리셋하거나 다른 방식으로 제어
 //       if (playerMovement != null) playerMovement.enabled = true;
        
        // 스킬 사용 상태 해제
        isUsingSkill = false;
    }

    // 딜레이 후 플레이어 제어 복구 코루틴
    private System.Collections.IEnumerator RestorePlayerControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestorePlayerControl();
    }
}