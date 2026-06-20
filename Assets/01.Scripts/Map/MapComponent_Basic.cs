using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent_Basic : MapComponent
{
    [SerializeField] List<Enemy> enemyPrefabs;
    
    Transform[] enemySpawnPos;

    [SerializeField] int enemySpawnCount = 2;
    int currentEnemyCount;

    [SerializeField] float waitSeconds = 3f;

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
            Enemy enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPos, Quaternion.identity);

            enemy.SetEnemy(this);

            yield return new WaitForSeconds(waitSeconds);
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
