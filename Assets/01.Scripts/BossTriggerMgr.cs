using UnityEngine;

public class BossTriggerMgr : MonoBehaviour
{
    private bool isTriggered = false;

    // Is Trigger가 체크되어 있다면 OnTriggerEnter를 사용해야 해!
    void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 확인
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // 한 번만 발동하게 방지

            // BossCtrl의 싱글톤(Inst)을 통해 컷씬 호출
            if (BossCtrl.Inst != null)
            {
                BossCtrl.Inst.TriggerBossIntro();
            }
        }
    }
}