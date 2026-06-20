using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Mg : MonoBehaviour
{
    [Header("Skill_Select_UI")]
    public GameObject skillselectRoot;
    public Skill_Mgr[] skill_Mgrs;

    [Header("UI")]
    public Image Skill_0;
    public Image Skill_1;

    public Image m_Player_Hon;

    public Image m_PlayerHp;

    [SerializeField] private PlayerCtrl playerCharacter;

    [Header("Skill_Hon")]
    float m_MaxPlayerHon = 100f;
    float m_CurrentPlayerHon;
    float m_MonsterHon = 10.0f;

    [Header("중앙 스킬 데이터베이스")]
    public List<SkillData> masterSkillDatabase = new List<SkillData>();
    public bool isSkillChoose = false;

    [Header("EasterEgg")]
    public InputField easterEggInputField;

    public static Game_Mg Inst;

    private void Awake()
    {
        Inst = this;
        InitializeMasterSkills(); // 딱 한 번만 메인 DB 구축
    }

    void InitializeMasterSkills()
    {
        // 기존 Skill_Mgr에 있던 그 업그레이드 리스트들을 여기다가 싹 넣어줍니다.
        masterSkillDatabase.Add(new SkillData(SkillID.Normal_FireRate, "더 많은 혼령탄", "혼령탄이 더 자주 나갑니다", SkillRarity.Bronze, 3));
        masterSkillDatabase.Add(new SkillData(SkillID.Normal_MultiShot, "혼령탄 동시에 한발 더", "혼령탄이 1개 추가로 나갑니다", SkillRarity.Bronze, 5));
        masterSkillDatabase.Add(new SkillData(SkillID.Normal_Damage, "더 강력한 혼령탄", "혼령탄 위력이 증가합니다", SkillRarity.Bronze, 3));
        masterSkillDatabase.Add(new SkillData(SkillID.Normal_Size, "큰 혼령탄", "혼령탄 크기가 증가합니다", SkillRarity.Bronze, 1));

        masterSkillDatabase.Add(new SkillData(SkillID.Skill_Cooldown, "더 빠른 영혼절단", "영혼절단 쿨타임이 감소합니다", SkillRarity.Bronze, 3));
        masterSkillDatabase.Add(new SkillData(SkillID.Skill_Damage, "더 강력한 영혼절단", "영혼절단이 더 강력해집니다", SkillRarity.Bronze, 3));
        masterSkillDatabase.Add(new SkillData(SkillID.Skill_Range, "더 큰 영혼절단", "영혼절단 공격범위가 증가합니다", SkillRarity.Bronze, 3));
        masterSkillDatabase.Add(new SkillData(SkillID.Skill_Split, "영혼절단 한번에 두개", "영혼절단이 두갈래로 나갑니다", SkillRarity.Gold, 1));

        masterSkillDatabase.Add(new SkillData(SkillID.Ultimate_Duration, "더 오래가는 천도광", "천도광 지속시간이 증가합니다", SkillRarity.Bronze, 2));
        masterSkillDatabase.Add(new SkillData(SkillID.Ultimate_Damage, "더 강력한 천도광", "천도광 위력이 증가합니다", SkillRarity.Bronze, 1));
        masterSkillDatabase.Add(new SkillData(SkillID.Ultimate_Range, "더 넒은 천도광", "천도광 범위가 증가합니다", SkillRarity.Gold, 1));
        masterSkillDatabase.Add(new SkillData(SkillID.Ultimate_Cooltime, "더 빠른 천도광", "천도광 쿨타임이 감소합니다", SkillRarity.Silver, 1));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()    
    {
        skillselectRoot.gameObject.SetActive(false);
        m_Player_Hon.fillAmount = 0.0f;
        //easterEggInputField.gameObject.SetActive(false);

        if (Sound_Mgr.Inst != null)
        {
            Sound_Mgr.Inst.PlayBgm("Bgm_0", 0.8f);
        }

        if (playerCharacter == null)
        {
            playerCharacter = GameObject.FindAnyObjectByType<PlayerCtrl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSkillUpdate();

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    GenerateSkillChoices();
        //}

        if (Input.GetKeyDown(KeyCode.Numlock)) //&& ))isBossStage == true
        {
            easterEggInputField.gameObject.SetActive(true);
            if (easterEggInputField.text == "tiger")
            {
                SceneManager.LoadScene("EasterEnding");
            }
        }

       
    }// void Update()


    public void PlayerSkillUpdate()
    {
        if (isSkillChoose == true)
            return;

        if (playerCharacter != null)
        {
            playerCharacter.ReduceCooldowns(Time.deltaTime);
        }
    }

    public void OnSkillUpdatee(float value)
    {
       m_CurrentPlayerHon += value;
        m_Player_Hon.fillAmount = m_CurrentPlayerHon / m_MaxPlayerHon;

        if (m_CurrentPlayerHon >= m_MaxPlayerHon)
        {
            GenerateSkillChoices();
        }
    }

    public void GenerateSkillChoices()
    {
        isSkillChoose = true;

        skillselectRoot.gameObject.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지        
        //Time.fixedDeltaTime = 0f;

        List<SkillData> selectedChoices = new List<SkillData>();
        Skill_Mgr mainDB = skill_Mgrs[0];

        // 카드 3장 뽑기 진행
        for (int i = 0; i < 3; i++)
        {
            SkillData picked = mainDB.GetRandomSkillExcept(selectedChoices);

            if (picked != null)
            {
                selectedChoices.Add(picked);
                skill_Mgrs[i].SetSkillUI(picked);
                skill_Mgrs[i].gameObject.SetActive(true);
            }
            else
            {
                skill_Mgrs[i].gameObject.SetActive(false);
            }

        }

        m_CurrentPlayerHon = 0.0f;


    }// public void GenerateSkillChoices()


    public void CloseSkillChoices()
    {
        skillselectRoot.gameObject.SetActive(false); // UI ��Ʈ ����

        m_Player_Hon.fillAmount = 0.0f;
        m_CurrentPlayerHon = 0.0f;                          
        Time.timeScale = 1f;
        //Time.fixedDeltaTime = 1f;
        isSkillChoose = false;
    }// public void CloseSkillChoices()

    public static bool IsPointerOverUIObject() //UGUI�� UI���� ���� ��ŷ�Ǵ��� Ȯ���ϴ� �Լ�
    {
        PointerEventData a_EDCurPos = new PointerEventData(EventSystem.current);

        a_EDCurPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(a_EDCurPos, results);
        return (0 < results.Count);
    }//public bool IsPointerOverUIObject() 
}



