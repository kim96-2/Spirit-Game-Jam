using Game.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet_Create : EnemyBullet
{
    [SerializeField] GameObject createObject;

    public override void DestroyBullet()
    {
        GameObject obj = ObjectPoolManager.Instance.Get(createObject, transform.position, quaternion.identity);

        base.DestroyBullet();
    }
}
