using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;  // 채워지는 체력바 이미지 (Image Type을 Filled로 설정)

    /// <summary>
    /// 체력 비율(0~1)로 fillAmount를 업데이트합니다.
    /// </summary>
    /// <param name="currentHealth">현재 체력</param>
    /// <param name="maxHealth">최대 체력</param>
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (fillImage == null) return;

        float fillValue = Mathf.Clamp01(currentHealth / maxHealth);
        fillImage.fillAmount = fillValue;
    }
}
