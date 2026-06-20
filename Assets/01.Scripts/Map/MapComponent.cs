using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
    int stageNum;

    public virtual string StageInfo {get => $"STAGE {stageNum}"; }

    MapComponent nextMap;

    [SerializeField] Transform startPos;

    [SerializeField] MapExit exit;

    [Space(15f)]
    [SerializeField] protected Transform enemySpawnPosParent;

    public virtual void SetMap(int stageNum, Vector3 offsetPos, MapComponent nextMap)
    {
        transform.position = offsetPos;

        this.stageNum = stageNum;

        this.nextMap = nextMap;

    }

    public virtual void StartMap(Transform player)
    {
        player.position = startPos.position;

        exit.SetExit(this);
        exit.DeactivateExit();

    }

    public void EndMap()
    {
        exit.ActivateExit();

    }

    public virtual void GoNextMap()
    {
        MapManager.Instance.GoNextMap(stageNum + 1);
    }

    public virtual void EnemyDestroyed()
    {
        
    }
}
