using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("이동 및 대시 설정")]
    public float moveSpeed = 5.0f;
    private float defaultSpeed;       // 원래 속도 기억용
    public float dashSpeedMultiplier = 20.0f;
    public float dashDuration = 2.0f; // 대시 유지 시간
    //public float dashCooldown = 2.0f; // 대시 쿨타임

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

        // 대시 입력 처리 (Space 키)
        if (Input.GetKeyDown(KeyCode.Space)) //&& !isDashing)
        {
            //if (Time.time >= lastDashTime + dashCooldown)
            //{
            //    StartCoroutine(DashRoutine());
            //}
            StartCoroutine(DashRoutine());
        }

        // 이동 적용
        transform.position += moveInput * moveSpeed * Time.deltaTime;
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