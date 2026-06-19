using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
    int stageNum;

    MapComponent nextMap;

    [SerializeField] Transform startPos;

    [SerializeField] MapExit exit;

    [Space(15f)]
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Transform enemySpawnPosParent;
    Transform[] enemySpawnPos;

    [SerializeField] int enemySpawnCount = 2;
    int currentEnemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMap(int stageNum, Vector3 offsetPos, MapComponent nextMap)
    {
        transform.position = offsetPos;

        this.stageNum = stageNum;

        this.nextMap = nextMap;

        enemySpawnPos = enemySpawnPosParent.GetComponentsInChildren<Transform>();

    }

    public void StartMap(Transform player)
    {
        player.position = startPos.position;

        exit.SetExit(this);
        exit.DeactivateExit();

        StartCoroutine(StartEnemySpawn());

    }

    public void EndMap()
    {
        exit.ActivateExit();

    }

    public void GoNextMap()
    {
        MapManager.Instance.GoNextMap(stageNum + 1);
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

    public void EnemyDestroyed()
    {
        currentEnemyCount -= 1;

        if(currentEnemyCount == 0)
        {
            EndMap();
        }
    }
}
