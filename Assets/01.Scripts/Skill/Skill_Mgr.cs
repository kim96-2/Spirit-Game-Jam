using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Mgr : MonoBehaviour
{
    public Text m_SkillTitle;
    public Text m_SkillDesc;
    public Image m_SkillIcon;

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
        skillDatabase.Add(new SkillData(SkillID.Normal_FireRate, "탄환 빈도 증가", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Normal_MultiShot, "부채꼴 멀티샷", SkillRarity.Bronze, 5));
        skillDatabase.Add(new SkillData(SkillID.Normal_Damage, "일반 공격 위력 증가", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Normal_Size, "탄환 크기 증가", SkillRarity.Bronze, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_WallBounce, "벽 반사 탄환", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Boomerang, "부메랑 탄환", SkillRarity.Silver, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Penetration, "관통탄 변경", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Normal_Guided, "유도탄", SkillRarity.Bronze, 2));

        // 스킬 공격 업그레이드
        skillDatabase.Add(new SkillData(SkillID.Skill_Cooldown, "스킬 쿨타임 감소", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_Damage, "스킬 위력 증가", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_Range, "스킬 범위 증가", SkillRarity.Bronze, 3));
        skillDatabase.Add(new SkillData(SkillID.Skill_WallBounce, "벽 반사 스킬", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Skill_Boomerang, "부메랑 스킬", SkillRarity.Silver, 1));
        skillDatabase.Add(new SkillData(SkillID.Skill_Split, "스킬 2갈래 발사", SkillRarity.Gold, 1));

        // 궁극기 업그레이드
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Duration, "궁극기 지속 증가", SkillRarity.Bronze, 2));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Damage, "궁극기 위력 증가", SkillRarity.Bronze, 1));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_Range, "궁극기 범위 증가", SkillRarity.Gold, 1));
        skillDatabase.Add(new SkillData(SkillID.Ultimate_SoulCost, "필요 혼 게이지 감소", SkillRarity.Silver, 1));
    }

    // 하나의 스킬 데이터를 UI 오브젝트에 꽂아주는 함수
    public void SetSkillUI(SkillData data)
    {
        if (data == null) return;

        currentCardData = data;

        if (m_SkillTitle != null) m_SkillTitle.text = data.skillName;
        if (m_SkillDesc != null) m_SkillDesc.text = $"{data.rarity} 등급 능력 업그레이드";

        if (m_SkillIcon != null)
        {
            if (data.rarity == SkillRarity.Bronze) m_SkillIcon.color = new Color32(139, 115, 85, 255); // 브론즈색
            else if (data.rarity == SkillRarity.Silver) m_SkillIcon.color = new Color32(192, 192, 192, 255); // 실버색
            else if (data.rarity == SkillRarity.Gold) m_SkillIcon.color = new Color32(212, 175, 55, 255); // 골드색
        }
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

        // 2. 싱글톤 매니저를 호출해 UI 창을 닫고 시간 재생(Time.timeScale = 1)
        if (Game_Mg.Inst != null)
        {
            Game_Mg.Inst.CloseSkillChoices();
        }
        else
        {
            Debug.LogError("Game_Mg 싱글톤 인스턴스(Inst)를 찾을 수 없습니다.");
        }
    }
}