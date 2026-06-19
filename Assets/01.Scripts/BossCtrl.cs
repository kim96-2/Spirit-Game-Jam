using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossCtrl : MonoBehaviour
{
    [Header("보스 관련")]
    public Text BossTxt;

    public Transform PlayerTransform;

    public CinemachineCamera mainBossCamera;
    public float introZoomSize = 3.5f;
    public float defaultZoomSize = 7f;

    private bool isBattleStarted = false;


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



    IEnumerator BossIntroCo()
    {
        mainBossCamera.Follow = this.transform;
        mainBossCamera.LookAt = this.transform;

        // 2. 줌인 및 텍스트 등장
        DOVirtual.Float(mainBossCamera.Lens.OrthographicSize, introZoomSize, 0.5f, (val) => mainBossCamera.Lens.OrthographicSize = val);

        if (BossTxt != null)
        {
            BossTxt.gameObject.SetActive(true);
            Color c = BossTxt.color;
            c.a = 0; BossTxt.color = c;
            DOTween.To(() => BossTxt.color, x => BossTxt.color = x, new Color(c.r, c.g, c.b, 1f), 0.5f);
        }

        yield return new WaitForSeconds(2.0f);

        // 4. 텍스트 사라짐 & 카메라 플레이어 복귀
        if (BossTxt != null)
        {
            DOTween.To(() => BossTxt.color, x => BossTxt.color = x, new Color(BossTxt.color.r, BossTxt.color.g, BossTxt.color.b, 0f), 0.5f)
                   .OnComplete(() => BossTxt.gameObject.SetActive(false));
        }

        ReturnCameraToPlayer();

        isBattleStarted = true;
    }

    public void ReturnCameraToPlayer()
    {
        // ★ 씬에 있는 플레이어를 미리 알고 있으니 바로 사용!
        if (PlayerTransform != null)
        {
            mainBossCamera.Follow = PlayerTransform;
            mainBossCamera.LookAt = PlayerTransform;

            // 줌아웃
            DOVirtual.Float(mainBossCamera.Lens.OrthographicSize, defaultZoomSize, 0.5f, (val) => mainBossCamera.Lens.OrthographicSize = val);
        }
    }
}
