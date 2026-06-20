using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class BossCtrl : MonoBehaviour
{

    [Header("���� ����")]
    public Text BossTxt;

    public Transform PlayerTransform;

    public CinemachineCamera mainBossCamera;
    public float introZoomSize = 3.5f;
    public float defaultZoomSize = 7f;

    // public GameObject BossTrigger;

    public static BossCtrl Inst;

    private Transform bossSpawnTransform;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BossTxt != null) 
            BossTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerBossIntro(Transform bossSpawnTransform)
    {
        this.bossSpawnTransform = bossSpawnTransform;

        StartCoroutine(BossIntroCo());
    }

    IEnumerator BossIntroCo()
    {
        mainBossCamera.Follow = this.bossSpawnTransform;
        mainBossCamera.LookAt = this.bossSpawnTransform;

        // 2. ���� �� �ؽ�Ʈ ����
        DOVirtual.Float(mainBossCamera.Lens.OrthographicSize, introZoomSize, 0.5f, (val) => mainBossCamera.Lens.OrthographicSize = val);

        if (BossTxt != null)
        {
            BossTxt.gameObject.SetActive(true);
            Color c = BossTxt.color;
            c.a = 0; BossTxt.color = c;
            DOTween.To(() => BossTxt.color, x => BossTxt.color = x, new Color(c.r, c.g, c.b, 0.1f), 0.5f);
        }

        yield return new WaitForSeconds(2.0f);

        // 4. �ؽ�Ʈ ����� & ī�޶� �÷��̾� ����
        if (BossTxt != null)
        {
            DOTween.To(() => BossTxt.color, x => BossTxt.color = x, new Color(BossTxt.color.r, BossTxt.color.g, BossTxt.color.b, 0f), 0.5f)
                   .OnComplete(() => BossTxt.gameObject.SetActive(false));
        }

        ReturnCameraToPlayer();

        //isBattleStarted = true;
    }

    public void ReturnCameraToPlayer()
    {
        if (PlayerTransform != null)
        {
            mainBossCamera.Follow = PlayerTransform;
            mainBossCamera.LookAt = PlayerTransform;

            // �ܾƿ�
            DOVirtual.Float(mainBossCamera.Lens.OrthographicSize, defaultZoomSize, 0.5f, (val) => mainBossCamera.Lens.OrthographicSize = val);
        }
    }
}
