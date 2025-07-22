using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    private int previousWave = 1;
    private String TEXT2;
    public Text ammoText; // 탄약 표시용 텍스트
    public Text TimeText;
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public Text waveText2;

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore) {
        scoreText.text = "Score : " + newScore;
    }
    public void ShowWaveText2(string message)
    {
        waveText2.text = message;
        waveText2.gameObject.SetActive(true);
        StopCoroutine("AnimateAndHideWaveText2"); // 중복 실행 방지
        StartCoroutine(AnimateAndHideWaveText2());
    }
    public void ShowTimeLeft(float waveTimeLeft)
{
    if (TimeText == null)
    {
        Debug.LogError("TimeText is not assigned in the inspector!");
        return;
    }

    TimeText.text = "Time Left: " + waveTimeLeft.ToString("F1") + "s";
}

private IEnumerator AnimateAndHideWaveText2()
{
    RectTransform rect = waveText2.GetComponent<RectTransform>();

    // 시작 위치와 종료 위치 설정
    Vector3 startPos = new Vector3(-500f, rect.anchoredPosition.y, 0); // 왼쪽 바깥
    Vector3 endPos = new Vector3(500f, rect.anchoredPosition.y, 0);    // 오른쪽 바깥

    rect.anchoredPosition = startPos;

    float duration = 2f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        float t = elapsed / duration;
        rect.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
        elapsed += Time.deltaTime;
        yield return null;
    }

    rect.anchoredPosition = endPos;
    waveText2.gameObject.SetActive(false);
}


    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
{
    waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    TEXT2 = "Wave " + waves + " is coming!";
    if (waves != previousWave)
    {
        previousWave = waves;
        ShowWaveText2(TEXT2);
    }
}



    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}