using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public Transform[] spawnPoints;

    private int prefabIndex = 0; // 어떤 프리팹 차례인지

    // 한 번 호출로 3마리 소환
    public void SpawnThreeEnemies()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        Enemy prefab = enemyPrefabs[prefabIndex];

        for (int i = 0; i < 3; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Enemy enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            // 필요하다면 관리 코드 (onDeath 등)
            // enemy.onDeath += ...;
        }

        // 다음 프리팹으로 인덱스 이동 (끝나면 처음으로)
        prefabIndex++;
        if (prefabIndex >= enemyPrefabs.Length)
            prefabIndex = 0;
    }

    // 예시: 스페이스바로 3마리씩 소환
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnThreeEnemies();
        }
    }
}
