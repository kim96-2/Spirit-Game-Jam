using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance {get => _instance;}

    [SerializeField] MapComponent mapPrefab;

    [SerializeField] int stageCount = 10;

    [SerializeField] Vector3 mapOffset = new(100f, 0f ,0f);

    List<MapComponent> maps = new();

    [Space(15f)]
    public Transform testPlayer;



    void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        int i;
        for(i = 0; i < stageCount; i++)
        {
            MapComponent mapObject = Instantiate(mapPrefab);

            maps.Add(mapObject);
        }

        
        for(i = 0; i < stageCount - 1; i++)
        {
            maps[i].SetMap(i, mapOffset * i, maps[i + 1]);
        }
        maps[i].SetMap(i, mapOffset * i, null);
    }

    void Start()
    {
        maps[0].StartMap(testPlayer);
    }

    public void GoNextMap(int nexStageNum)
    {
        maps[nexStageNum].StartMap(testPlayer);
    }

    
}
