using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInputTest : MonoBehaviour {
    // public string moveVerticalAxisName = "Vertical"; // 상하 이동
    // public string moveHorizontalAxisName = "Horizontal"; // 좌우 이동
    // public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    // public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름
    
    // 스킬 버튼 추가
    public string attack2Name = "attack2"; // 스킬 1 입력 버튼 이름
    public string attack3Name = "attack3"; // 스킬 2 입력 버튼 이름
    public string attack4Name = "attack4"; // 스킬 2 입력 버튼 이름

    // // 값 할당은 내부에서만 가능
    // public float moveVertical { get; private set; } // 상하 이동 입력값
    // public float moveHorizontal { get; private set; } // 좌우 이동 입력값
    // public bool fire { get; private set; } // 감지된 발사 입력값
    // public bool reload { get; private set; } // 감지된 재장전 입력값

    // 스킬 입력값 추가
    public bool attack2 { get; private set; }
    public bool attack3 { get; private set; }
    public bool attack4 { get; private set; }

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            // moveVertical = 0;
            // moveHorizontal = 0;
            // fire = false;
            // reload = false;
            attack2 = false;
            attack3 = false;
            attack4 = false;
            return;

        }
        // 상하 이동 (W/S, ↑/↓)
        // moveVertical = Input.GetAxis(moveVerticalAxisName);
        // // 좌우 이동 (A/D, ←/→)
        // moveHorizontal = Input.GetAxis(moveHorizontalAxisName);
        // fire = Input.GetButton(fireButtonName);
        // reload = Input.GetButtonDown(reloadButtonName);

        // 스킬 입력 감지
        attack2 = Input.GetButtonDown(attack2Name); // attack2Name = "attack2"
        attack2 = Input.GetKey(attack2Name);
        attack3 = Input.GetKey(attack3Name);
        attack4 = Input.GetButtonDown(attack4Name);
    }
}