using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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

    //IEnumerator BossIntroo()
    //{
    //    mainBossCamera.Follow = this.transform;
    //    mainBossCamera.LookAt = this.transform;

       
    //}
}
