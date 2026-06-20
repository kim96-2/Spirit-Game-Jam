using Game.Core;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Potan_Mgr : MonoBehaviour
{
    [Header("����ü ����")]
    public float speed = 15.0f;
    public float damage = 20.0f;

    [SerializeField] GameObject destroyParticle;

    Rigidbody rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3.0f); 

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.Translate(Vector3.forward * speed);
        rigidbody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            //Debug.Log("���� ����)");
            coll.gameObject.GetComponent<Enemy>().Damage(damage);

            if(destroyParticle != null) ObjectPoolManager.Instance.Get(destroyParticle, transform.position, quaternion.identity);

            Destroy(gameObject); 
        }

    }
}
