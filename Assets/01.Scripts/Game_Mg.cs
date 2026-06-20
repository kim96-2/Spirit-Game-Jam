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
    public Image Boss_Img;
    public Sprite Boss_Paze2;

    public Image Skill_0;
    public Image Skill_1;

    public Image Player_HpBar;
    public Image Boss_HpBar;

    public Image m_Player_Hon;

    [Header("EasterEgg")]
    public InputField easterEggInputField;
    [HideInInspector] public static bool isBossStage = false;

    public static Game_Mg Inst;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()    
    {
        skillselectRoot.gameObject.SetActive(false);
        easterEggInputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateSkillChoices();
        }

        if (Input.GetKeyDown(KeyCode.Numlock)) //&& ))isBossStage == true
        {
            easterEggInputField.gameObject.SetActive(true);
        }

        if (easterEggInputField.text == "tiger")
        {
            SceneManager.LoadScene("EasterEnding");
        }
    }// void Update()

    public void GenerateSkillChoices()
    {
        skillselectRoot.gameObject.SetActive(true);

        Time.timeScale = 0f; // 게임 일시정지

        if (skill_Mgrs == null || skill_Mgrs.Length < 3)
        {
            Debug.LogError("skill_Mgrs 배열에 카드 UI 3개가 제대로 연결되지 않았습니다.");
        }

        // 이번 뽑기에서 임시 선택된 스킬들을 저장 (중복 방지용 리스트)
        List<SkillData> selectedChoices = new List<SkillData>();

        // 0번 카드 스크립트가 메인 데이터베이스 역할을 겸임합니다.
        Skill_Mgr mainDB = skill_Mgrs[0];

        // 카드 3장 뽑기 진행
        for (int i = 0; i < 3; i++)
        {
            SkillData picked = mainDB.GetRandomSkillExcept(selectedChoices);

            if (picked != null)
            {
                selectedChoices.Add(picked);
                // 화면의 i번째 카드 UI에 데이터 전달 및 갱신
                skill_Mgrs[i].SetSkillUI(picked);
                skill_Mgrs[i].gameObject.SetActive(true);
            }
            else
            {
                // 더 이상 뽑을 스킬이 부족하다면 해당 카드 UI 자리를 끔
                skill_Mgrs[i].gameObject.SetActive(false);
            }
        }
    }// public void GenerateSkillChoices()


    public void CloseSkillChoices()
    {
        skillselectRoot.gameObject.SetActive(false); // UI 루트 끄기
        Time.timeScale = 1f;                         // 게임 시간 정상화 (다시 움직임!)
        Debug.Log("스킬 선택 UI 닫힘 - 게임 재개");
    }// public void CloseSkillChoices()

    public static bool IsPointerOverUIObject() //UGUI의 UI들이 먼저 피킹되는지 확인하는 함수
    {
        PointerEventData a_EDCurPos = new PointerEventData(EventSystem.current);

        a_EDCurPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(a_EDCurPos, results);
        return (0 < results.Count);
    }//public bool IsPointerOverUIObject() 
}



