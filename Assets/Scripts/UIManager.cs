using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드
//using TMPro;
//using NUnit.Framework.Internal; // TextMeshPro 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindFirstObjectByType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

  //  public Text ammoText; // 탄약 표시용 텍스트
    //public Text scoreText; // 점수 표시용 텍스트
    //public Text waveText; // 적 웨이브 표시용 텍스트
 //   public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public Image[] skillCooldownImages; // 여러 스킬 쿨타임용 이미지 배열------------------------------------------------------------
 //   public Text timeText; // Inspector에서 할당

    // 탄약 텍스트 갱신
    // public void UpdateAmmoText(int magAmmo, int remainAmmo) {
    //     ammoText.text = magAmmo + "/" + remainAmmo;
    // }

    // // 점수 텍스트 갱신
    // public void UpdateScoreText(int newScore) {
    //     scoreText.text = "Score : " + newScore;
    // }

    // // 적 웨이브 텍스트 갱신
    // public void UpdateWaveText(int waves, int count) {
    //     waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    // }

    // 게임 오버 UI 활성화
    // public void SetActiveGameoverUI(bool active) {
    //     gameoverUI.SetActive(active);
    // }

    // 게임 재시작
    public void GameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 스킬 쿨타임 이미지 갱신
    public void UpdateSkillCooldownImage(int skillIndex, float current, float max)//-------------------------------------------------------------------
    {
        if (skillCooldownImages != null && skillIndex >= 0 && skillIndex < skillCooldownImages.Length)
            skillCooldownImages[skillIndex].fillAmount = current / max;
    }

    // 스킬 쿨타임 이미지 활성/비활성
    public void SetSkillCooldownImageActive(int skillIndex, bool active)//----------------------------------------------------------------------------
    {
        if (skillCooldownImages != null && skillIndex >= 0 && skillIndex < skillCooldownImages.Length)
            skillCooldownImages[skillIndex].gameObject.SetActive(active);
    }

    public void SetSkillIcons(SkillBase[] skills)//--------------------------------------------------------------------------
    {
        if (skillCooldownImages == null) return;
        for (int i = 0; i < skillCooldownImages.Length && i < skills.Length; i++)
        {
            if (skills[i] != null && skills[i].icon != null)
                skillCooldownImages[i].sprite = skills[i].icon;
        }
    }

    // 시간 텍스트 갱신
    // public void UpdateTimeText(float time)//------------------------------------------------------------
    // {
    //     if (timeText != null)
    //     {
    //         int minutes = (int)(time / 60);
    //         int seconds = (int)(time % 60);
    //         timeText.text = $"Time: {minutes:00}:{seconds:00}";
    //     }
    // }
}