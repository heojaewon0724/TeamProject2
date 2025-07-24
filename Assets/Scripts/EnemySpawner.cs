using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;            // 여러 적 프리팹 배열
    public Transform[] spawnPoints;         // 소환 위치 배열

    public Text waveUIText;
    public int currentWave = 0;

    // 웨이브별 몇 마리, 그리고 어떤 프리팹 인덱스인지 배열로 관리 (각 웨이브마다 여러 마리 가능)
    [System.Serializable]
    public class Wave
    {
        public int prefabIndex;   // 웨이브에서 사용할 프리팹 인덱스
        public int count;         // 웨이브에서 소환할 개수
    }


    public Wave[] waves;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        waveUIText.gameObject.SetActive(false);
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave < waves.Length)
        {
            waveUIText.text = $"WAVE {currentWave + 1}";
            waveUIText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            waveUIText.gameObject.SetActive(false);

            aliveEnemies.Clear();

            Wave wave = waves[currentWave];

            for (int i = 0; i < wave.count; i++)
            {
                if (wave.prefabIndex < 0 || wave.prefabIndex >= enemyPrefabs.Length)
                {
                    Debug.LogError($"Invalid prefabIndex {wave.prefabIndex} in wave {currentWave + 1}");
                    break;
                }

                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemyGO = Instantiate(enemyPrefabs[wave.prefabIndex].gameObject, spawnPoint.position, spawnPoint.rotation);

                Enemy enemy = enemyGO.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.spawner = this;

                aliveEnemies.Add(enemyGO);
            }

            yield return new WaitUntil(() => aliveEnemies.TrueForAll(e => e == null));

            currentWave++;
            yield return new WaitForSeconds(2f);
        }

        waveUIText.text = "GAME CLEAR!";
        waveUIText.gameObject.SetActive(true);
    }


    public void OnEnemyDie(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
    }
}
