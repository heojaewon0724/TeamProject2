using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public FadeManager fadeManager;
    public BGMManager bgmManager;

    public AudioClip buttonSFX;  // 버튼 효과음

    public string nextSceneName = "Stage1-Jaewon";  // 씬 이름 직접 입력

    public void OnStartButtonClicked()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        // 효과음 재생
        bgmManager.PlaySFX(buttonSFX);

        // 효과음 재생 잠깐 기다리기 (0.2초 정도)
        yield return new WaitForSeconds(0.2f);

        // 배경음악 페이드 아웃
        yield return StartCoroutine(bgmManager.FadeOutMusic());

        // 화면 페이드 아웃 + 씬 전환
        fadeManager.FadeToScene(nextSceneName);
    }
}
