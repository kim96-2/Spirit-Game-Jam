using UnityEngine;

public class MapComponent_Boss : MapComponent
{
    [SerializeField] Enemy bossPrefab;

    public override string StageInfo => "FINAL STAGE";

    public override void StartMap(Transform player)
    {
        base.StartMap(player);

        Vector3 spawnPos = enemySpawnPosParent.position;
        Enemy boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        boss.SetEnemy(this);
    }

    public override void GoNextMap()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public override void EnemyDestroyed()
    {
        EndMap();
    }
}
