using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text hpText;
    public Image hpBarFill;
    public GameObject playerObj;

    private int maxHP = 100;
    private int currentHP = 100;

    public bool isGameover { get; private set; }

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindFirstObjectByType<GameManager>();
            return m_instance;
        }
    }
    private static GameManager m_instance;

    // AudioSource 컴포넌트 저장 변수

    void Start()
    {
        UpdateHPUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        // hpText.text = $"{currentHP} / {maxHP}";
        // hpBarFill.fillAmount = (float)currentHP / maxHP;
    }
}
