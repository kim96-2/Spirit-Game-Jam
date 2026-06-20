using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Mgr : MonoBehaviour
{
    public Text m_SkillTitle;
    public Text m_SkillDesc;
    public Image m_SkillBG;
    //public Image m_SkillIcon;


    public Sprite BronseImg;
    public Sprite SilverImg;
    public Sprite GoldImg;

    //public Sprite Skill_Img;
    //public Sprite Cheondo_Img;

    // 현재 카드 UI가 들고 있는 스킬 데이터 (선택 시 활용)
    private SkillData currentCardData;

    public List<SkillData> skillDatabase = new List<SkillData>();

    void Awake()
    {
        InitializeSkills();
    }

    void InitializeSkills()
    {
        // 일반 공격 업그레이드
        skillDatabase.Add(new SkillData(SkillID.Normal_FireRate, "더 많은 혼령탄", "혼령탄이 더 자주 나갑니다 (최대 3회)", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Normal_MultiShot, "혼령탄 동시에 한발 더", "혼령탄이 1개 추가로 나갑니다 (최대 5개)", SkillRarity.Bronze, 5));
        skillDatabase.Add(new SkillData(SkillID.Normal_Damage, "더 강력한 혼령탄", "혼령탄 위력이 증가합니다", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Normal_Size, "큰 혼령탄", "혼령탄 크기가 증가합니다", SkillRarity.Bronze, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_WallBounce, "반사 혼령탄", "혼령탄이 벽에 1회 반사됩니다", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Boomerang, "부메랑 혼령탄", "부메랑탄환으로 변경됩니다", SkillRarity.Silver, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Penetration, "혼령탄 관통", "관통탄으로 변경됩니다", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Guided, "유도혼령탄", "20%확률로 유도탄이 나갑니다", SkillRarity.Bronze, 2));

        // 스킬 공격 업그레이드
        skillDatabase.Add(new SkillData(SkillID.Skill_Cooldown, "더 빠른 영혼절단", "영혼절단 쿨타임이 감소합니다", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_Damage, "더 강력한 영혼절단", "영혼절단이 더 강력해집니다", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_Range, "더 큰 영혼절단", "영혼절단 공격범위가 증가합니다", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_WallBounce, "영혼절단 반사", "영혼절단이 벽에 1회 반사됩니다", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Skill_Boomerang, "영혼절단 부메랑", "영혼절단이 부메랑처럼 돌아옵니다", SkillRarity.Silver, 1));
        skillDatabase.Add(new SkillData(SkillID.Skill_Split, "영혼절단 한번에 두개", "영혼절단이 두갈래로 나갑니다", SkillRarity.Gold, 1));

        // 궁극기 업그레이드
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Duration, "더 오래가는 천도광", "천도광 지속시간이 증가합니다", SkillRarity.Bronze, 2));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Damage, "더 강력한 천도광", "천도광 위력이 증가합니다", SkillRarity.Bronze, 1));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Range, "더 넒은 천도광", "천도광 범위가 증가합니다", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Cooltime, "더 빠른 천도광", "천도광 쿨타임이 감소합니다", SkillRarity.Silver, 1));
    }

    // 하나의 스킬 데이터를 UI 오브젝트에 꽂아주는 함수
    public void SetSkillUI(SkillData data)
    {
        if (data == null) return;

        currentCardData = data;

        if (m_SkillTitle != null) m_SkillTitle.text = data.skillName;
        if (m_SkillDesc != null) m_SkillDesc.text = data.skillDesc;

        if (m_SkillBG != null)
        {
            if (data.rarity == SkillRarity.Bronze) m_SkillBG.sprite = BronseImg;
            else if (data.rarity == SkillRarity.Silver) m_SkillBG.sprite = SilverImg;
            else if (data.rarity == SkillRarity.Gold) m_SkillBG.sprite = GoldImg;
        }

        //if (m_SkillIcon != null)
        //{
        //    if (data.id.ToString().Contains("Normal")) m_SkillIcon.gameObject.SetActive(false);
        //    if (data.id.ToString().Contains("Ultimate")) m_SkillIcon.sprite = Cheondo_Img;
        //    if (data.id.ToString().Contains("Skill")) m_SkillIcon.sprite = Skill_Img; // 기본 아이콘
        //}
    }

    public SkillData GetRandomSkillExcept(List<SkillData> excludeList)
    {
        List<SkillData> availableSkills = new List<SkillData>();
        int totalWeight = 0;

        foreach (var skill in skillDatabase)
        {
            if (skill.CanAvailable && !excludeList.Contains(skill))
            {
                availableSkills.Add(skill);
                totalWeight += (skill.maxCount - skill.currentCount);
            }
        }

        if (availableSkills.Count == 0 || totalWeight == 0) return null;

        int randomNumber = Random.Range(0, totalWeight);
        int currentWeightSum = 0;

        foreach (var skill in availableSkills)
        {
            int remainingCount = skill.maxCount - skill.currentCount;
            currentWeightSum += remainingCount;

            if (randomNumber < currentWeightSum)
            {
                return skill;
            }
        }
        return null;
    }

  public void OnClickSkillCard()
    {
        if (currentCardData == null)
        {
            Debug.LogError($"{gameObject.name} 카드의 스킬 데이터가 비어있습니다!");
            return;
        }

        // 1. 기획서 규칙: 해당 스킬 획득 카운트 1 증가
        currentCardData.currentCount++;
        Debug.Log($"[스킬 선택 완료] {currentCardData.skillName} (현재 {currentCardData.currentCount}/{currentCardData.maxCount}개 적용됨)");

        if (PlayerCtrl.Inst != null)
        {
            PlayerCtrl.Inst.ApplySkillUpgrade(currentCardData.id);
        }

        // 2. 싱글톤 매니저를 호출해 UI 창을 닫고 시간 재생
        if (Game_Mg.Inst != null)
        {
            Game_Mg.Inst.CloseSkillChoices();
        }
    }
}