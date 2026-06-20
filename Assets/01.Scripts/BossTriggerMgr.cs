using UnityEngine;

public class BossTriggerMgr : MonoBehaviour
{
    private bool isTriggered = false;

    private Transform bossSpawnTransform;

    MapComponent_Boss map;

    Enemy boss;

    public void SetBossTrigger(Transform bossSpawnTransform, MapComponent_Boss map)
    {
        this.bossSpawnTransform = bossSpawnTransform;

        this.map = map;

        // this.boss = boss;
    }

    // Is Trigger�� üũ�Ǿ� �ִٸ� OnTriggerEnter�� ����ؾ� ��!
    void OnTriggerEnter(Collider other)
    {
        // �÷��̾����� Ȯ��
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // �� ���� �ߵ��ϰ� ����

            // BossCtrl�� �̱���(Inst)�� ���� �ƾ� ȣ��
            if (BossCtrl.Inst != null)
            {
                BossCtrl.Inst.TriggerBossIntro(bossSpawnTransform);
            }

            map.SpawnBoss();

            Destroy(this.gameObject);
        }
    }
}