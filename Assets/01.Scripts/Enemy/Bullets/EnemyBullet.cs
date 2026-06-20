using Game.Core;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] float damageAmount = 10f;
    [SerializeField] float destroyDuration = 10f;
    [SerializeField] GameObject destroyParticle;

    PoolObject poolObject;
    protected Rigidbody rigidbody;

    protected Vector3 shootDir;

    protected Enemy enemy;

    float _destroyTime = 0f;

    void Awake()
    {
        poolObject = GetComponent<PoolObject>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckDestroyTime();
    }

    void FixedUpdate()
    {
        MoveBullet();

    }

    protected virtual void MoveBullet()
    {
        rigidbody.MovePosition(transform.position + shootDir * moveSpeed * Time.deltaTime);

    }

    void CheckDestroyTime()
    {
        if(_destroyTime > destroyDuration) DestroyBullet();
        _destroyTime += Time.deltaTime;
    }

    public virtual void Shoot(Vector3 shootDir, Enemy enemy)
    {
        this.shootDir = shootDir;
        this.enemy = enemy;
    }

    public virtual void DestroyBullet()
    {
        if(destroyParticle != null)
        {
            ObjectPoolManager.Instance.Get(destroyParticle, transform.position, quaternion.identity);
        }

        poolObject.Release();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCtrl>().Damage(damageAmount);
            DestroyBullet();
        }

    }
}
