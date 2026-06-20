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
    public Image bossHpBar;
    public GameObject bossHpBarObject;

    public Transform PlayerTransform;

    public CinemachineCamera mainBossCamera;
    public float introZoomSize = 3.5f;
    public float defaultZoomSize = 7f;

    // public GameObject BossTrigger;

    public static BossCtrl Inst;

    private Transform bossSpawnTransform;

    private Enemy boss;
    float bossFullHp;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BossTxt != null) 
            BossTxt.gameObject.SetActive(false);

        bossHpBarObject.SetActive(false);
    }

    void OnBossHpDamage()
    {
        bossHpBar.fillAmount = boss.Hp / bossFullHp;

        if(boss.Hp <= 0f) bossHpBarObject.SetActive(false);
    }

    public void SetBoss(Enemy boss)
    {
        this.boss = boss;
    }

    public void TriggerBossIntro(Transform bossSpawnTransform)
    {
        this.bossSpawnTransform = bossSpawnTransform;

        bossHpBarObject.SetActive(true);

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

        bossFullHp = boss.Hp;
        boss.OnHpDamage += OnBossHpDamage;

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
