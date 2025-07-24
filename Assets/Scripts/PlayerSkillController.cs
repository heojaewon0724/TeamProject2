// 플레이어 스킬 컨트롤러 예시
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSkillController : MonoBehaviour
{
    public PlayableDirector skillCutsceneDirector;
    public PlayerInputTest playerInput;
    public Animator playerAnimator;


    public SkillBase[] skills; // 여러 스킬을 배열로 관리
    private float[] skillCooldownTimers;
    //public PlayerMovement playerMovement; // 추가
    
    private bool isUsingSkill = false; // 스킬 사용 중인지 추적

    void Start()
    {
        // 스킬 쿨타임 배열 초기화
        skillCooldownTimers = new float[skills.Length];
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

        // 스킬 키 입력 감지 (스킬 사용 중이 아닐 때만)
        if (playerInput != null && !isUsingSkill)
        {
            if (playerInput.attack2)
                OnSkillButton(0); // 첫 번째 스킬

            if (playerInput.attack3)
                OnSkillButton(1); // 두 번째 스킬
        }
    }

    // 스킬 인덱스별로 발동
    public void OnSkillButton(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Length) return;
        if (skillCooldownTimers[skillIndex] > 0f) return;
        if (isUsingSkill) return; // 이미 스킬 사용 중이면 리턴
        
        var currentSkill = skills[skillIndex];
        if (currentSkill == null) return;

        // 스킬 사용 상태 시작
        isUsingSkill = true;

        // 스킬별로 이동 제어
        if (playerInput != null) playerInput.enabled = false;
       // if (playerMovement != null) playerMovement.enabled = currentSkill.allowMoveWhileCasting;

     //   Debug.Log(playerMovement.enabled ? "이동 가능" : "이동 불가능");

        // 먼저 스킬 발동 (실제 효과)
        currentSkill.Activate(gameObject);

        // 이름으로 컷씬 오브젝트 찾기 (연출용)
        if (!string.IsNullOrEmpty(currentSkill.cutsceneObjectName))
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
        }

        // 쿨타임 적용
        skillCooldownTimers[skillIndex] = currentSkill.cooldown;
        UIManager.instance.UpdateSkillCooldownImage(skillIndex, skillCooldownTimers[skillIndex], currentSkill.cooldown);
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
        if (playerInput != null) playerInput.enabled = true;
 //       if (playerMovement != null) playerMovement.enabled = true;
        
        // 스킬 사용 상태 해제
        isUsingSkill = false;
    }
}