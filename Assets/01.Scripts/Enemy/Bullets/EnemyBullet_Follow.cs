using Game.Core;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet_Follow : EnemyShooter
{
    [SerializeField] float moveSpeed = 5f;

    PoolObject poolObject;
    Rigidbody rigidbody;

    Vector3 shootDir;

    Enemy enemy;

    float _destroyTime = 0f;

    void Awake()
    {
        poolObject = GetComponent<PoolObject>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveBullet();
    }

    protected virtual void MoveBullet()
    {
        rigidbody.MovePosition(transform.position + shootDir * moveSpeed);

        if(_destroyTime > 10f) DestroyBullet();
        _destroyTime += Time.deltaTime;
    }

    public virtual void Shoot(Vector3 shootDir, Enemy enemy)
    {
        this.shootDir = shootDir;
        this.enemy = enemy;
    }

    public virtual void DestroyBullet()
    {
        poolObject.Release();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DestroyBullet();
        }

    }
}
