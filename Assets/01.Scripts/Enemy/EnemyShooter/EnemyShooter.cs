using Unity.Mathematics;
using UnityEngine;
using Game.Core;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPos;

    Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public virtual void Shoot(Vector3 dir)
    {
        // GameObject bullet = Instantiate(bulletPrefab,shootPos.position, Quaternion.identity);
        GameObject bullet = ObjectPoolManager.Instance.Get(bulletPrefab, shootPos.position, Quaternion.identity);

        bullet.GetComponent<EnemyBullet>().Shoot(dir, enemy);
    }
}
