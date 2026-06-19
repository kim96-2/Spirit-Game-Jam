using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("캐릭터 이동 설정")]
    private Vector3 moveInput;
    public float moveSpeed = 5.0f;

    [Header("공격 관련")]
    float fireRate = 0.3f;
    float nextFireTime;
    public GameObject magicPrefab;
    public Transform firePoint;
    float m_PlayerAttackDistance = 10.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        //Attack();
    }

    public void PlayerMove()
    {
        moveInput = Vector3.zero;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            moveInput = new Vector3(h, 0.0f, v);
            if (1.0f < moveInput.magnitude)
                moveInput.Normalize();

            transform.position += moveInput * moveSpeed * Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
