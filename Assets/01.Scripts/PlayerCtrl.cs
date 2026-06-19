using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("이동 및 대시 설정")]
    public float moveSpeed = 5.0f;
    private float defaultSpeed;       // 원래 속도 기억용
    public float dashSpeedMultiplier = 20.0f;
    //public float dashCooldown = 2.0f; // 대시 쿨타임

    [Header("대시 설정")]
    public float dashSpeed = 100.0f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;

    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;
    private Vector3 lastMoveDirection; // 마지막 이동 방향 기억용

    //private float lastDashTime = -10f; // 마지막 대시 시간 저장
    private bool isDashing = false;

    [Header("공격 관련")]
    public GameObject magicPrefab;
    public Transform firePoint;

    void Start()
    {
        defaultSpeed = moveSpeed; // 게임 시작 시 기본 속도 저장
    }

    void Update()
    {
        PlayerMove();
        Attack();
    }

    public void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveInput = new Vector3(h, 0, v).normalized;

        // 1. 이동 방향 기록 (가만히 있을 때를 대비)
        if (moveInput != Vector3.zero)
            lastMoveDirection = moveInput;

        // 2. 대시 쿨타임 감소
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        // 3. 대시 입력
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash(moveInput);
        }

        // 4. 대시 중 이동 처리
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
            else
            {
                // 속도감을 위해 moveSpeed가 아니라 dashSpeed를 즉시 적용
                transform.position += dashDirection * dashSpeed * Time.deltaTime;
                return;
            }
        }

        // 일반 이동
        transform.position += moveInput * moveSpeed * Time.deltaTime;
    }

    void StartDash(Vector3 moveInput)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown; // 쿨타임 시작

        // 입력 방향이 있으면 그쪽으로, 아니면 보던 방향으로!
        dashDirection = moveInput != Vector3.zero ? moveInput : lastMoveDirection;
        dashDirection.Normalize();
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        //lastDashTime = Time.time;

        // 대시 방향으로 속도 고정 (이동 입력 무시)
        Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (dashDirection == Vector3.zero) dashDirection = transform.forward; // 입력이 없으면 앞방향

        moveSpeed *= dashSpeedMultiplier;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = defaultSpeed;
        isDashing = false;
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (magicPrefab != null && firePoint != null)
            {
                Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            }
        }
    }
}