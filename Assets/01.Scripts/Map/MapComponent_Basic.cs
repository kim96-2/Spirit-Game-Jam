using System.Collections;
using UnityEngine;

public class MapComponent_Basic : MapComponent
{
    [SerializeField] Enemy enemyPrefab;
    
    Transform[] enemySpawnPos;

    [SerializeField] int enemySpawnCount = 2;
    int currentEnemyCount;

    public override void SetMap(int stageNum, Vector3 offsetPos, MapComponent nextMap)
    {
        base.SetMap(stageNum, offsetPos, nextMap);

        enemySpawnPos = enemySpawnPosParent.GetComponentsInChildren<Transform>();
    }

    public override void StartMap(Transform player)
    {
        base.StartMap(player);

        StartCoroutine(StartEnemySpawn());
    }

    IEnumerator StartEnemySpawn()
    {
        currentEnemyCount = enemySpawnCount;

        for(int i = 0; i < enemySpawnCount; i++)
        {
            Vector3 spawnPos = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)].position;
            Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemy.SetEnemy(this);

            yield return new WaitForSeconds(2f);
        }
    }

    public override void EnemyDestroyed()
    {
        currentEnemyCount -= 1;

        if(currentEnemyCount == 0)
        {
            EndMap();
        }
    }
}
