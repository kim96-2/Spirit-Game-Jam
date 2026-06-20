using System.Collections.Generic;
using UnityEngine;

public class Sound_Mgr : G_Singleton<Sound_Mgr>
{
    [HideInInspector] public AudioSource m_AudioSrc;
    Dictionary<string, AudioClip> m_AdClipList = new Dictionary<string, AudioClip>();

    float m_BgmVolume = 0.2f;
    [HideInInspector] public float m_SoundVolume = 1.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(string filenName, float fVolume = 0.2f)
    {
        AudioClip gAudioClip = null;
        if (m_AdClipList.ContainsKey(filenName) == true)
        {
            gAudioClip = m_AdClipList[filenName];
        }
        else
        {
            gAudioClip = Resources.Load<AudioClip>("Sounds/" + filenName) as AudioClip;
            m_AdClipList.Add(filenName, gAudioClip);
        }

        if (m_AudioSrc == null)
            return;

        if (m_AudioSrc != null && m_AudioSrc.clip != null && m_AudioSrc.clip.name == filenName)
        {
            return; // 이미 같은 노래가 나오고 있다면 그냥 나감
        }

        m_AudioSrc.clip = gAudioClip;
        m_BgmVolume = fVolume; // 곡의 기본 볼륨 저장

        // 재생하기 직전에 유저 설정 볼륨(m_SoundVolume)을 반드시 곱해줍니다.
        m_AudioSrc.volume = m_BgmVolume * m_SoundVolume;

        m_AudioSrc.loop = true;
        m_AudioSrc.Play();
    }
}
