using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveInput;

    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("대시 설정")]
    public float dashSpeed = 100.0f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;

    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;
    private Vector3 lastMoveDirection = Vector3.forward;
    private bool isDashing = false;


    [Header("공격 프리랩 셋업")]
    public GameObject magicPrefab;
    public GameObject SkillPrefab;
    public GameObject BiiimPrefab;
    public Transform firePoint;

    // --- 기획서 연동을 위한 실시간 변수들 ---
    [Header("[기획 연동 스펙 수치]")]
    public float normalAttackCooldown = 0.5f;   // 1. 일반공격 빈도 주기
    public int normalMultiShotCount = 1;         // 2. 멀티샷 탄환 개수
    public float normalDamageModifier = 1.0f;    // 3. 일반공격 위력 배율
    public float normalSizeModifier = 1.0f;      // 4. 일반공격 투사체 크기 배율
    public bool isNormalWallBounce = false;      // 5. 일반공격 벽 반사 활성화 여부
    public bool isNormalBoomerang = false;       // 6. 일반공격 부메랑 활성화 여부
    public bool isNormalPenetration = false;     // 7. 일반공격 관통탄 여부
    public float normalGuidedChance = 0f;        // 8. 일반공격 유도탄 확률

    public float skillCooldown = 7.0f;           // 9. 스킬 쿨타임 주기
    public float skillDamageModifier = 1.0f;     // 10. 스킬 위력 배율
    public float skillSizeModifier = 1.0f;       // 11. 스킬 범위 배율
    public bool isSkillWallBounce = false;       // 12. 스킬 벽 반사 여부
    public bool isSkillBoomerang = false;        // 13. 스킬 부메랑 여부
    public bool isSkillSplit = false;            // 14. 스킬 2갈래 발사 여부

    public float ultimateDuration = 5.0f;        // 15. 궁극기 지속 시간
    public float ultimateDamageModifier = 1.0f;  // 16. 궁극기 위력 배율
    public float ultimateSizeModifier = 1.0f;    // 17. 궁극기 범위 배율
    public float ultimateCooldown = 20.0f;       // 18. ★ [변경] 궁극기 쿨타임 (기본 20초)
    // ----------------------------------------

    GameObject activeBeam;
    bool isUltimateActive = false;
    float ultimateCooldownTimer;

    float m_AttackTimer = 0.0f;
    float m_SkillTimer = 0.0f;

    public static PlayerCtrl Inst;

    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        if (moveInput != Vector3.zero) lastMoveDirection = moveInput;

        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }

        if (ultimateCooldownTimer > 0)
            ultimateCooldownTimer -= Time.deltaTime;

        Attack();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
            else
            {
                Vector3 targetPos = rb.position + (dashDirection * dashSpeed * Time.fixedDeltaTime);
                rb.MovePosition(targetPos);

                if (dashDirection != Vector3.zero)
                {
                    rb.MoveRotation(Quaternion.LookRotation(dashDirection));
                }
                return;
            }
        }

        Vector3 movePos = rb.position + (moveInput * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(movePos);

        if (moveInput != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(moveInput));
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = moveInput != Vector3.zero ? moveInput : lastMoveDirection;
    }

    public void Attack()
    {
        m_AttackTimer -= Time.deltaTime;
        m_SkillTimer -= Time.deltaTime;

        // [일반 공격]
        if (m_AttackTimer <= 0.0f && Game_Mg.IsPointerOverUIObject() == false)
        {
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 targetPosition = ray.GetPoint(rayDistance);
                Vector3 launchDirection = (targetPosition - firePoint.position).normalized;
                launchDirection.y = 0;

                SpawnNormalProjectiles(launchDirection);
            }

            m_AttackTimer = normalAttackCooldown;
        }

        // [스킬 공격]
        if (Input.GetMouseButton(0) && m_SkillTimer <= 0.0f && Game_Mg.IsPointerOverUIObject() == false)
        {
            if (SkillPrefab != null && firePoint != null)
            {
                Vector3 launchDirection = transform.forward;
                launchDirection.y = 0;
                launchDirection.Normalize();

                SpawnSkillProjectiles(launchDirection);
            }

            m_SkillTimer = skillCooldown;
        }

        // [궁극기 공격]
        if (Input.GetKeyDown(KeyCode.Space) && ultimateCooldownTimer <= 0 && !isUltimateActive && Game_Mg.IsPointerOverUIObject() == false)
        {
            StartCoroutine(UltimateRoutine());
        }

        if (isUltimateActive && activeBeam != null)
        {
            RotateUltimateBeam();
        }
    }

    void SpawnNormalProjectiles(Vector3 baseDirection)
    {
        if (magicPrefab == null || firePoint == null) return;

        int count = normalMultiShotCount;
        float angleStep = 15f;
        float startAngle = -((count - 1) * angleStep) / 2f;

        for (int i = 0; i < count; i++)
        {
            float targetAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            Vector3 finalDir = rotation * baseDirection;

            GameObject go = Instantiate(magicPrefab, firePoint.position, Quaternion.LookRotation(finalDir));
            go.transform.localScale *= normalSizeModifier;
        }
    }

    void SpawnSkillProjectiles(Vector3 baseDirection)
    {
        if (SkillPrefab == null) return;

        if (isSkillSplit)
        {
            Vector3 leftDir = Quaternion.Euler(0, -20, 0) * baseDirection;
            Vector3 rightDir = Quaternion.Euler(0, 20, 0) * baseDirection;

            GameObject go1 = Instantiate(SkillPrefab, firePoint.position, Quaternion.LookRotation(leftDir));
            GameObject go2 = Instantiate(SkillPrefab, firePoint.position, Quaternion.LookRotation(rightDir));

            go1.transform.localScale *= skillSizeModifier;
            go2.transform.localScale *= skillSizeModifier;
        }
        else
        {
            GameObject go = Instantiate(SkillPrefab, firePoint.position, Quaternion.LookRotation(baseDirection));
            go.transform.localScale *= skillSizeModifier;
        }
    }

    IEnumerator UltimateRoutine()
    {
        isUltimateActive = true;

        // ★ 하드코딩 20초 대신, 카드 선택으로 줄어든 실시간 ultimateCooldown 변수가 반영되도록 수정!
        ultimateCooldownTimer = ultimateCooldown;

        activeBeam = Instantiate(BiiimPrefab, firePoint.position, firePoint.rotation);
        activeBeam.transform.SetParent(firePoint);
        activeBeam.transform.localPosition = Vector3.zero;

        activeBeam.transform.localScale = new Vector3(ultimateSizeModifier, ultimateSizeModifier, 1f);

        yield return new WaitForSeconds(ultimateDuration);

        if (activeBeam != null) Destroy(activeBeam);
        isUltimateActive = false;
    }

    void RotateUltimateBeam()
    {
        if (activeBeam == null || firePoint == null) return;

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 targetPosition = ray.GetPoint(rayDistance);
            Vector3 launchDirection = (targetPosition - firePoint.position).normalized;
            launchDirection.y = 0;

            if (launchDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(launchDirection);
                activeBeam.transform.rotation = targetRotation;
            }
        }
    }

    public void Damage(float amount) { }


    public void ApplySkillUpgrade(SkillID id)
    {
        switch (id)
        {
            // === 일반 공격 업그레이드 ===
            case SkillID.Normal_FireRate:
                normalAttackCooldown -= 0.12f;
                if (normalAttackCooldown < 0.15f) normalAttackCooldown = 0.15f;
                break;

            case SkillID.Normal_MultiShot:
                normalMultiShotCount += 1;
                break;

            case SkillID.Normal_Damage:
                normalDamageModifier += 0.25f;
                break;

            case SkillID.Normal_Size:
                normalSizeModifier += 0.4f;
                break;

            case SkillID.Normal_WallBounce:
                isNormalWallBounce = true;
                break;

            case SkillID.Normal_Boomerang:
                isNormalBoomerang = true;
                break;

            case SkillID.Normal_Penetration:
                isNormalPenetration = true;
                break;

            case SkillID.Normal_Guided:
                normalGuidedChance += 0.1f;
                break;

            // === 스킬 공격 업그레이드 ===
            case SkillID.Skill_Cooldown:
                skillCooldown -= 1.5f;
                if (skillCooldown < 1.0f) skillCooldown = 1.0f;
                break;

            case SkillID.Skill_Damage:
                skillDamageModifier += 0.3f;
                break;

            case SkillID.Skill_Range:
                skillSizeModifier += 0.35f;
                break;

            case SkillID.Skill_WallBounce:
                isSkillWallBounce = true;
                break;

            case SkillID.Skill_Boomerang:
                isSkillBoomerang = true;
                break;

            case SkillID.Skill_Split:
                isSkillSplit = true;
                break;

            // === 궁극기 업그레이드 ===
            case SkillID.Ultimate_Duration:
                ultimateDuration += 1.5f;
                break;

            case SkillID.Ultimate_Damage:
                ultimateDamageModifier += 0.4f;
                break;

            case SkillID.Ultimate_Range:
                ultimateSizeModifier += 0.5f;
                break;
                
            case SkillID.Ultimate_Cooltime:
                ultimateCooldown -= 4.0f; // 실버 카드 선택 시 쿨타임 4초씩 감소 예시
                if (ultimateCooldown < 5.0f) ultimateCooldown = 5.0f; // 최소 무한 발사 방지 마지노선 5초
                break;
        }

        Debug.Log($"<color=lime>[스펙업] {id} 장착 완료! 현재 변경 스펙 적용됨.</color>");
    }

}