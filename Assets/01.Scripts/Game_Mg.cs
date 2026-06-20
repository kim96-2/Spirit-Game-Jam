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

        // if (easterEggInputField.text == "tiger")
        // {
        //     SceneManager.LoadScene("EasterEnding");
        // }
    }// void Update()

    public void GenerateSkillChoices()
    {
        skillselectRoot.gameObject.SetActive(true);

        Time.timeScale = 0f; // ���� �Ͻ�����

        if (skill_Mgrs == null || skill_Mgrs.Length < 3)
        {
            Debug.LogError("skill_Mgrs �迭�� ī�� UI 3���� ����� ������� �ʾҽ��ϴ�.");
        }

        // �̹� �̱⿡�� �ӽ� ���õ� ��ų���� ���� (�ߺ� ������ ����Ʈ)
        List<SkillData> selectedChoices = new List<SkillData>();

        // 0�� ī�� ��ũ��Ʈ�� ���� �����ͺ��̽� ������ �����մϴ�.
        Skill_Mgr mainDB = skill_Mgrs[0];

        // ī�� 3�� �̱� ����
        for (int i = 0; i < 3; i++)
        {
            SkillData picked = mainDB.GetRandomSkillExcept(selectedChoices);

            if (picked != null)
            {
                selectedChoices.Add(picked);
                // ȭ���� i��° ī�� UI�� ������ ���� �� ����
                skill_Mgrs[i].SetSkillUI(picked);
                skill_Mgrs[i].gameObject.SetActive(true);
            }
            else
            {
                // �� �̻� ���� ��ų�� �����ϴٸ� �ش� ī�� UI �ڸ��� ��
                skill_Mgrs[i].gameObject.SetActive(false);
            }
        }
    }// public void GenerateSkillChoices()


    public void CloseSkillChoices()
    {
        skillselectRoot.gameObject.SetActive(false); // UI ��Ʈ ����
        Time.timeScale = 1f;                         // ���� �ð� ����ȭ (�ٽ� ������!)
        Debug.Log("��ų ���� UI ���� - ���� �簳");
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



