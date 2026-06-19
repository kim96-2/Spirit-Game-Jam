using UnityEngine;

public class Potan_Mgr : MonoBehaviour
{
    [Header("투사체 설정")]
    public float speed = 15.0f;
    public float damage = 20.0f;
    public float lifeTime = 2.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Debug.Log("마법 적중)");
            Destroy(gameObject);
        }

    }
}
