using UnityEngine;

public class MapComponent_Boss : MapComponent
{
    [SerializeField] Enemy bossPrefab;
    [SerializeField] BossTriggerMgr bossTriggerMgr;

    Enemy boss;

    public override string StageInfo => "FINAL STAGE";

    public override void StartMap(Transform player)
    {
        base.StartMap(player);

        bossTriggerMgr.SetBossTrigger(enemySpawnPosParent, this);

        Sound_Mgr.Inst.PlayBgm("Boss_0", 0.8f);
    }

    public void SpawnBoss()
    {
        Vector3 spawnPos = enemySpawnPosParent.position;
        Enemy boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        this.boss = boss;

        BossCtrl.Inst.SetBoss(this.boss);

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
