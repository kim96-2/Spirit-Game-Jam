using System.Collections.Generic;
using UnityEngine;

// ★ [안전장치] 이 스크립트가 붙으면 오디오 소스 컴포넌트도 무조건 같이 붙게 만듭니다.
[RequireComponent(typeof(AudioSource))]
public class Sound_Mgr : G_Singleton<Sound_Mgr>
{
    [HideInInspector] public AudioSource m_AudioSrc;
    Dictionary<string, AudioClip> m_AdClipList = new Dictionary<string, AudioClip>();

    float m_BgmVolume = 1.0f;
    [HideInInspector] public float m_SoundVolume = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        if (m_AudioSrc == null)
        {
            m_AudioSrc = GetComponent<AudioSource>();
        }
    }

    void Start() { }
    void Update() { }

    public void PlayBgm(string filenName, float fVolume = 0.2f)
    {
        AudioClip gAudioClip = null;

        if (m_AdClipList.ContainsKey(filenName) == true)
        {
            gAudioClip = m_AdClipList[filenName];
        }
        else
        {
            // 폴더 경로는 Assets/Resources/Sounds/ 파일명 이어야 합니다. (확장자 제외)
            gAudioClip = Resources.Load<AudioClip>("Sounds/" + filenName);

            // 파일 로드 실패 시 에러 로그를 띄워주는 친절한 예외처리 추가
            if (gAudioClip == null)
            {
                Debug.LogError($"[Sound_Mgr] 'Resources/Sounds/{filenName}' 경로에서 오디오 파일을 찾을 수 없습니다! 대소문자나 폴더명을 확인하세요.");
                return;
            }

            m_AdClipList.Add(filenName, gAudioClip);
        }

        // 이제 에러 없이 안전하게 넘어갑니다!
        if (m_AudioSrc == null)
        {
            Debug.LogError("[Sound_Mgr] AudioSource 컴포넌트가 누락되었습니다!");
            return;
        }

        if (m_AudioSrc.clip != null && m_AudioSrc.clip.name == filenName)
        {
            return; // 이미 같은 노래가 나오고 있다면 패스
        }

        m_AudioSrc.clip = gAudioClip;
        m_BgmVolume = fVolume;

        m_AudioSrc.volume = m_BgmVolume * m_SoundVolume;
        m_AudioSrc.loop = true;
        m_AudioSrc.Play();
    }
}