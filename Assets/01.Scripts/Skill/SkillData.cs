using UnityEngine;

public enum SkillRarity
{
    Bronze,
    Silver,
    Gold,
    None
}

public enum SkillID
{
    Normal_FireRate = 0,     // 1. 탄환이 더 자주 나감
    Normal_MultiShot,        // 2. 탄환 1개 추가 (부채꼴)
    Normal_Damage,           // 3. 탄환 위력 증가
    Normal_Size,             // 4. 탄환 크기 증가
    Normal_WallBounce,       // 5. 벽 반사 탄환
    Normal_Boomerang,        // 6. 부메랑 탄환
    Normal_Penetration,      // 7. 관통탄으로 변경
    Normal_Guided,           // 8. 20% 확률로 유도탄
    Skill_Cooldown,          // 9. 스킬 쿨타임 감소
    Skill_Damage,            // 10. 스킬 위력 증가
    Skill_Range,             // 11. 스킬 범위 증가
    Skill_WallBounce,        // 12. 벽 반사 스킬
    Skill_Boomerang,         // 13. 부메랑 스킬
    Skill_Split,             // 14. 스킬이 2갈래로 나감
    Ultimate_Duration,       // 15. 궁극기 지속시간 증가
    Ultimate_Damage,         // 16. 궁극기 위력 증가
    Ultimate_Range,          // 17. 궁극기 범위 증가
    Ultimate_Cooltime        // 18. 궁극기 쿨타임 감소
}

[System.Serializable]
public class SkillData
{
    public SkillID id;
    public string skillName;
    public SkillRarity rarity;
    public int maxCount;     // 등장 최대 횟수 (기획서 기준)
    public int currentCount; // 현재 플레이어가 획득한 횟수

    public SkillData(SkillID id, string name, SkillRarity rarity, int maxCount)
    {
        this.id = id;
        this.skillName = name;
        this.rarity = rarity;
        this.maxCount = maxCount;
        this.currentCount = 0;
    }

    // 아직 더 뽑을 수 있는지 확인하는 함수
    public bool CanAvailable => currentCount < maxCount;
}