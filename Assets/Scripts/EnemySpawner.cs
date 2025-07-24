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
        public int[] enemyCounts;            // Example: {1,2,0} => enemyPrefabs[0] 1마리, [1] 2마리, [2] 0마리
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

            // 웨이브 내 모든 적 종류별로 소환
            for (int prefabIndex = 0; prefabIndex < enemyPrefabs.Length; prefabIndex++)
            {
                int count = 0;
                if (prefabIndex < wave.enemyCounts.Length)
                    count = wave.enemyCounts[prefabIndex];

                for (int i = 0; i < count; i++)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject enemyGO = Instantiate(enemyPrefabs[prefabIndex].gameObject, spawnPoint.position, spawnPoint.rotation);

                    Enemy enemy = enemyGO.GetComponent<Enemy>();
                    if (enemy != null)
                        enemy.spawner = this;

                    aliveEnemies.Add(enemyGO);
                }
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
