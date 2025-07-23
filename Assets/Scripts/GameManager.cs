using UnityEngine;
using UnityEngine.UI; // 혹은 using TMPro; (TextMeshPro 사용시)

public class GameManager : MonoBehaviour
{
    public Text hpText; // 텍스트 컴포넌트 (TextMeshPro라면 TMP_Text로 변경)
    public Image hpBarFill; // 체력바의 Filled Image
    public GameObject playerObj; // 플레이어나 몬스터 등 대상 오브젝트

    private int maxHP = 100;
    private int currentHP = 100;

    void Start()
    {
        // 예시로 기본값 세팅
        UpdateHPUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        // 텍스트에 체력값 표시
        hpText.text = $"{currentHP} / {maxHP}";
        // UI 채우기 값 업데이트 (0~1로 변환)
        hpBarFill.fillAmount = (float)currentHP / maxHP;
    }
}
