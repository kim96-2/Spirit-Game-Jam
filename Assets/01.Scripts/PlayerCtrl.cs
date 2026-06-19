using System.Collections;
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


    [Header("공격 관련")]
    public GameObject magicPrefab;
    public GameObject SkillPrefab;
    public GameObject BiiimPrefab;

    float ultimateDuration = 5.0f;
    float beamRotationSpeed = 150.0f;
    float ultimateCooldown = 20.0f;

    GameObject activeBeam;
    bool isUltimateActive = false;
    float ultimateCooldownTimer;

    float m_AttackTimer = 0.0f;
    float m_SkillTimer = 0.0f;

    public Transform firePoint;

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
    }//void Update()
    
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
                return;
            }
        }//if (isDashing)

        Vector3 movePos = rb.position + (moveInput * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(movePos);
    }//void FixedUpdate()

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = moveInput != Vector3.zero ? moveInput : lastMoveDirection;
    }// void StartDash()

    public void Attack()
    {
        m_AttackTimer -= Time.deltaTime;
        m_SkillTimer -= Time.deltaTime;

        if (m_AttackTimer <= 0.0f)
        {
            if (magicPrefab != null && firePoint != null)
            {
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 targetPosition = ray.GetPoint(rayDistance);
                    Vector3 launchDirection = (targetPosition - firePoint.position).normalized;
                    Instantiate(magicPrefab, firePoint.position, Quaternion.LookRotation(launchDirection));
                }
            }

            m_AttackTimer = 0.5f; 
        }

        if (Input.GetMouseButton(0) && m_SkillTimer <= 0.0f)
        {
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 targetPosition = ray.GetPoint(rayDistance);
                Vector3 launchDirection = (targetPosition - firePoint.position).normalized;
                Instantiate(SkillPrefab, firePoint.position, Quaternion.LookRotation(launchDirection));
            }

            m_SkillTimer = 7.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && ultimateCooldownTimer <= 0 && !isUltimateActive)
        {
            StartCoroutine(UltimateRoutine());
        }

        if (isUltimateActive && activeBeam != null)
        {
            RotateUltimateBeam();
        }

    }// public void Attack()

    IEnumerator UltimateRoutine()
    {
        isUltimateActive = true;
        ultimateCooldownTimer = ultimateCooldown;

        activeBeam = Instantiate(BiiimPrefab, firePoint.position, firePoint.rotation);
        activeBeam.transform.SetParent(firePoint);
        activeBeam.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(ultimateDuration);

        if (activeBeam != null)
        {
            Destroy(activeBeam);
        }

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
}